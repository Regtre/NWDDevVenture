using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;
using NWDRuntime.Exchanges;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Extensions;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;
using NWDWebStandard.Configuration;

namespace NWDWebStandard.Controllers
{
    public abstract class NWDBasicController<T> : NWDRawController where T : Controller
    {
        #region static for generic
        public override void OnActionExecuting(ActionExecutingContext sContext)
        {
            //Console.WriteLine(nameof(NWDBasicController<T>) + " " + nameof(OnActionExecuting) + " /" + sContext.RouteData.Values["controller"]?.ToString() +"/" +sContext.RouteData.Values["action"]?.ToString());
            
            base.OnActionExecuting(sContext);
            
            PageInformation.SetSideBarKind(NWDSideBarKind.None, null, null, HttpContext);
            PageInformation.SetNavBarKind(NWDNavBarKind.None, null, HttpContext);
            PageInformation.SetNavFooter( null, HttpContext);
            AddFrequentlyAskedQuestions();
        }
        
        public void AddActualToast(NWDToastStandard sToast)
        {
            if (PageInformation != null)
            {
                PageInformation.AddActualToast(sToast);
            }
           AddTempDataObject(sToast);
        }

        public override void OnActionExecuted(ActionExecutedContext sContext)
        {
            //Console.WriteLine(nameof(NWDBasicController<T>) + " " + nameof(OnActionExecuted) + " /" + sContext.RouteData.Values["controller"]?.ToString() +"/" +sContext.RouteData.Values["action"]?.ToString());
            
            base.OnActionExecuted(sContext);
            
            if (NWDAccountWebManager.AccountIsConnected(HttpContext))
            {
                PageInformation.Notifications = new NWDNotificationFilteredLists(NWDNotificationManager.GetNotificationForAccountSortByDateDesc(NWDAccountWebManager
                    .GetAccountInContext(HttpContext).Reference)); 
            }
        }

        public static string ASP_Controller()
        {
            return typeof(T).Name.Replace("Controller", "");
        }

        #endregion

        #region Sign-up

        public ActionResult SignUpForm()
        {
            PageInformation.Title = "Sign-up form";
            PageInformation.Description = "Register";
            PageInformation.Keywords = new List<string>() { "sign", "sign-up" };
            return View(NWDAccountController.K_AccountSignUpForm_Embedded);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUpModal(NWDAccountSignUpModal sModel)
        {
            NWDAccountSignUp tModel = new NWDAccountSignUp();
            if (ModelState.IsValid)
            {
                tModel.AccountSignUpEmail = sModel.AccountSignUpModalEmail;
                tModel.AccountSignUpPassword = sModel.AccountSignUpModalPassword;
                tModel.AccountSignUpPasswordConfirm = sModel.AccountSignUpModalPasswordConfirm;
                tModel.AccountSignUpChecked = sModel.AccountSignUpModalChecked;
                tModel.AccountSignUpRememberMe = sModel.AccountSignUpModalRememberMe;
                tModel.AccountSignUpController = sModel.AccountSignUpModalController;
                tModel.AccountSignUpAction = sModel.AccountSignUpModalAction;
            }

            return SignUp(tModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(NWDAccountSignUp sModel)
        {
            AddTempDataObject(sModel);
            if (NWDAccountWebManager.AccountIsConnected(HttpContext) == false)
            {
                if (ModelState.IsValid)
                {
                    NWDAccountSign tAccountSign = NWDAccountSign.CreateEmailPassword(sModel.AccountSignUpEmail, sModel.AccountSignUpPassword, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                    NWDResponseRuntime tResponse = NWDAccountSignWebManager.TrySignUp(tAccountSign, HttpContext, sModel.AccountSignUpRememberMe);
                    // NWDLogger.WriteLine("--------- tResponse >>>>");
                    // NWDLogger.WriteLine(tResponse.Payload);
                    // NWDLogger.WriteLine(tResponse.Status.ToString());
                    if (tResponse.IsValid(NWDWebRuntimeConfiguration.KConfig))
                    {
                        if (tResponse.Status == NWDRequestStatus.Ok)
                        {
                            AddActualToast(NWDAccountController.SignUpSuccess);

                            if (NWDWebStandardConfiguration.KConfig.AccountSignUpSendEmail)
                            {
                                NWDEmailManager tEmailSend = new NWDEmailManager();
                                tEmailSend.SendFromWebsite(sModel.AccountSignUpEmail, "Just sign up",
                                    "Hello, you just sign-up  : " + sModel.AccountSignUpEmail + ": " +
                                    sModel.AccountSignUpPassword);
                            }

                            if (typeof(T).GetMethod(NWDWebStandardConfiguration.KConfig.SignUpSuccessAction) != null)
                            {
                                return RedirectToAction(NWDWebStandardConfiguration.KConfig.SignUpSuccessAction);
                            }
                            else
                            {
                                return RedirectToAction(nameof(SignUpWelcome));
                            }
                        }
                        else
                        {
                            AddActualToast(NWDAccountController.SignUpFailAlreadyAccount);
                            return RedirectToAction(nameof(SignUpForm));
                        }
                    }
                    else
                    {
                        AddActualToast(NWDAccountController.SignUpFailModelInvalid);
                        return RedirectToAction(nameof(SignUpForm));
                    }
                }
                else
                {
                    AddActualToast(NWDAccountController.SignUpFailModelInvalid);
                    return RedirectToAction(nameof(SignUpForm));
                }
            }
            else
            {
                AddActualToast(NWDAccountController.YouAreAlreadyConnected);
                //Check if method exist
                if (typeof(T).GetMethod(NWDWebStandardConfiguration.KConfig.SignInSuccessAction) != null) // yes Sign IN Success
                {
                    return RedirectToAction(NWDWebStandardConfiguration.KConfig.SignInSuccessAction);
                } // yes Sign IN Success
                else
                {
                    return RedirectToAction(nameof(SignInWelcome));
                }
            }
        }

        public ActionResult SignUpWelcome()
        {
            PageInformation.Title = "Authentication";
            PageInformation.Keywords.AddRange(new List<string>()
            {
                "authentication",
            });
            PageInformation.ShowAuthentication = false;
            PageInformation.PageStyle = NWDPageContainer.ContainerPage;
            PageInformation.NavBarStyle = NWDNavBarKind.Tools;
            return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignUpWelcome_Embedded);
        }

        #endregion

        #region Sign-in

        public ActionResult SignInForm()
        {
            PageInformation.Title = "Sign-in form";
            PageInformation.Description = "Authentication";
            PageInformation.Keywords = new List<string>() { "sign", "sign-in" };
            return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignInForm_Embedded);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignInModal(NWDAccountSignInModal sModel)
        {
            NWDAccountSignIn tModel = new NWDAccountSignIn();
            if (ModelState.IsValid)
            {
                tModel.AccountSignInEmail = sModel.AccountSignInModalEmail;
                tModel.AccountSignInRememberMe = sModel.AccountSignInModalRememberMe;
                tModel.AccountSignInPassword = sModel.AccountSignInModalPassword;
                tModel.AccountSignInController = sModel.AccountSignInModalController;
                tModel.AccountSignInAction = sModel.AccountSignInModalAction;
            }

            return SignIn(tModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignInMini(NWDAccountSignInMini sModel)
        {
            NWDAccountSignIn tModel = new NWDAccountSignIn();
            if (ModelState.IsValid)
            {
                tModel.AccountSignInEmail = sModel.AccountSignInMiniEmail;
                tModel.AccountSignInRememberMe = sModel.AccountSignInMiniRememberMe;
                tModel.AccountSignInPassword = sModel.AccountSignInMiniPassword;
                tModel.AccountSignInController = sModel.AccountSignInMiniController;
                tModel.AccountSignInAction = sModel.AccountSignInMiniAction;
            }

            return SignIn(tModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // [NWDWebMethodTestExclude()]
        [NWDWebMethodTestPost("AccountSignInEmail=hjghhgghjghjgjh@ggyffyyyffyfy.com&AccountSignInPassword=789854525687451458754874548&AccountSignInRememberMe=false")]
        public ActionResult SignIn(NWDAccountSignIn sModel)
        {
            PageInformation.Title = "Authentification";
            PageInformation.Description = "Authentification";
            PageInformation.Keywords = new List<string>() { "sign", "sign-in" };
            AddTempDataObject(sModel);
            if (NWDAccountWebManager.AccountIsConnected(HttpContext) == false)
            {
                if (ModelState.IsValid)
                {
                    NWDAccountSign tAccountSign = NWDAccountSign.CreateEmailPassword(sModel.AccountSignInEmail, sModel.AccountSignInPassword, NWDWebRuntimeConfiguration.KConfig.GetProjectId());

                    NWDResponseRuntime tResponse = NWDAccountSignWebManager.TrySignIn(tAccountSign, HttpContext, sModel.AccountSignInRememberMe);

                    if (tResponse.IsValid(NWDWebRuntimeConfiguration.KConfig))
                    {
                        if (tResponse.Status != NWDRequestStatus.Ok)
                        {
                            AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignInFailUnknownAccount);
                            return RedirectToAction(nameof(SignInForm));
                        }
                        else
                        {
                            AddActualToast(NWDWebStandard.Controllers.NWDAccountController.K_SignInSuccess);
                            if (NWDWebStandardConfiguration.KConfig.AccountSignInSendEmail)
                            {
                                NWDEmailManager tEmailSend = new NWDEmailManager();
                                tEmailSend.SendFromWebsite(sModel.AccountSignInEmail, "Just sign-in to your account",
                                    "Hello, you just sign-in to your account");
                            }

                            if (typeof(T).GetMethod(NWDWebStandardConfiguration.KConfig.SignInSuccessAction) != null)
                            {
                                return RedirectToAction(NWDWebStandardConfiguration.KConfig.SignInSuccessAction);
                            }
                            else
                            {
                                return RedirectToAction(nameof(SignInWelcome));
                            }
                        }
                    }
                    else
                    {
                        AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignInFailModelInvalid);
                        return RedirectToAction(nameof(SignInForm));
                    }
                }
                else
                {
                    AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignInFailModelInvalid);
                    return RedirectToAction(nameof(SignInForm));
                }
            }
            else
            {
                AddActualToast(NWDWebStandard.Controllers.NWDAccountController.YouAreAlreadyConnected);
                //Check if method exist
                if (typeof(T).GetMethod(NWDWebStandardConfiguration.KConfig.SignInSuccessAction) != null)
                {
                    return RedirectToAction(NWDWebStandardConfiguration.KConfig.SignInSuccessAction);
                }
                else
                {
                    return RedirectToAction(nameof(SignInWelcome));
                }
            }
        }

        public ActionResult SignInWelcome()
        {
            return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignInWelcome_Embedded);
        }

        #endregion

        #region account delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AccountDeleteForm(NWDAccountDelete sModel)
        {
            AddViewDataObject(sModel);
            if (ModelState.IsValid)
            {
                if (string.Equals(sModel.AccountDeleteSentence, NWDAccountDelete.K_Security))
                {
                    if (NWDAuthorizeAdminOnly.ValidFor(HttpContext) == true)
                    {
                        PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Danger, "Admin restriction!", "Admin can not delete his account.", new List<string>());
                    }
                    else
                    {
                        // TODO :
                        // if (NWDAccountWebManager.DeleteAccount(HttpContext))
                        // {
                        //     PageInformation.AddToast(AccountController._AccountDeleteSuccess);
                        //     //AddTempDataObject(PageInformation);
                        //     AddTempDataObject(AccountController._AccountDeleteSuccess);
                        //     return RedirectToAction(nameof(AccountDeleteSuccess));
                        // }
                        // else
                        // {
                        //     PageInformation.AddToast(AccountController.K_AccountDeleteFail);
                        //     //AddTempDataObject(PageInformation);
                        //     AddTempDataObject(AccountController.K_AccountDeleteFail);
                        // }
                    }
                }
                else
                {
                    AddActualToast(NWDWebStandard.Controllers.NWDAccountController.AccountDeleteFail);
                }
            }
            else
            {
                AddActualToast(NWDWebStandard.Controllers.NWDAccountController.AccountDeleteModelInvalid);
            }

            return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountDeleteForm_Embedded);
        }

        public ActionResult AccountDeleteSuccess()
        {
            return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountDeleteSuccess_Embedded);
        }

        #endregion

        #region Sign-rescue

        [Obsolete("Must be rewrite without HttpContext.Request.Query")]
        public ActionResult SignRescueForm()
        {
            string tTokenSecured = HttpContext.Request.Query["token"].ToString();
            string tTokenLimit = HttpContext.Request.Query["limit"].ToString();
            string tEmailSecured = HttpContext.Request.Query["email"].ToString();
            string tSignTypeString = HttpContext.Request.Query["sign"].ToString();
            if (string.IsNullOrEmpty(tSignTypeString) == false && string.IsNullOrEmpty(tEmailSecured) == false && string.IsNullOrEmpty(tTokenLimit) == false && string.IsNullOrEmpty(tTokenSecured) == false)
            {
                NWDAccountSignType tSignType = (NWDAccountSignType)int.Parse(tSignTypeString);
                int tLimit = int.Parse(tTokenLimit);

                string tEmail = NWDSecurityTools.DecryptSomething(tEmailSecured, NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeConfiguration.KConfig);
                string? tToken = NWDSecurityTools.DecryptAes(tTokenSecured, tEmail, NWDWebRuntimeConfiguration.KConfig.GetProjectId().ToString());
                if (tToken == null)
                {
                    tToken = string.Empty;
                }
#if DEBUG
                // PageInformation.AddToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Danger, "DEBUG", "Email = " + tEmail + " Token = " + tToken + " for sign type = " + tSignType.ToString() + " with limit to " + tLimit));
#endif
                NWDAccountSignRescue tModel = new NWDAccountSignRescue();
                tModel.AccountSignRescueEmail = tEmail;
                tModel.AccountSignRescueToken = tToken;
                tModel.AccountSignRescueLimit = tLimit;
                if (tSignType == NWDAccountSignType.EmailPassword)
                {
                    return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignRescueForm, tModel);
                }
                else
                {
                    AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignRescueUnknown);
                    return RedirectToAction(nameof(SignLostForm));
                }
            }
            else
            {
                AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignRescueUnknown);
                return RedirectToAction(nameof(SignLostForm));
            }
        }

        [Obsolete("Must be rewrite without HttpContext.Request.Query")]
        public ActionResult SignRescueFormB()
        {
            string tTokenSecured = HttpContext.Request.Query["token"].ToString();
            string tTokenLimit = HttpContext.Request.Query["limit"].ToString();
            string tEmailSecured = HttpContext.Request.Query["email"].ToString();
            string tSignTypeString = HttpContext.Request.Query["sign"].ToString();
            if (string.IsNullOrEmpty(tSignTypeString) == false && string.IsNullOrEmpty(tEmailSecured) == false && string.IsNullOrEmpty(tTokenLimit) == false && string.IsNullOrEmpty(tTokenSecured) == false)
            {
                NWDAccountSignType tSignType = (NWDAccountSignType)int.Parse(tSignTypeString);
                int tLimit = int.Parse(tTokenLimit);

                string tEmail = NWDSecurityTools.DecryptSomething(tEmailSecured, NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeConfiguration.KConfig);
                string? tToken = NWDSecurityTools.DecryptAes(tTokenSecured, tEmail, NWDWebRuntimeConfiguration.KConfig.GetProjectId().ToString());
                if (tToken == null)
                {
                    tToken = string.Empty;
                }
#if DEBUG
                // PageInformation.AddToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Danger, "DEBUG", "Email = " + tEmail + " Token = " + tToken + " for sign type = " + tSignType.ToString() + " with limit to " + tLimit));
#endif
                NWDAccountSignRescueLogin tModel = new NWDAccountSignRescueLogin();
                tModel.AccountSignRescueEmail = tEmail;
                tModel.AccountSignRescueToken = tToken;
                tModel.AccountSignRescueLimit = tLimit;
                if (tSignType == NWDAccountSignType.LoginEmailPassword)
                {
                    return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignRescueFormB, tModel);
                }
                else
                {
                    AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignRescueUnknown);
                    return RedirectToAction(nameof(SignLostForm));
                }
            }
            else
            {
                AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignRescueUnknown);
                return RedirectToAction(nameof(SignLostForm));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignRescue(NWDAccountSignRescue sModel)
        {
            AddViewDataObject(sModel);
            if (ModelState.IsValid)
            {
                NWDAccountSign tAccountSign = NWDAccountSign.CreateEmailPassword(sModel.AccountSignRescueEmail, sModel.AccountSignRescuePassword, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                tAccountSign.TokenRescue = sModel.AccountSignRescueToken;
                if (NWDAccountSignWebManager.RescueAccountSignTestToken(tAccountSign, HttpContext) == true)
                {
                    return RedirectToAction(nameof(SignInWelcome));
                }
                else
                {
                    AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignRescueUnknown);
                    return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignRescueForm, sModel);
                }
            }
            else
            {
                AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignRescueFailModelInvalid);
                return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignRescueForm, sModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignRescueB(NWDAccountSignRescueLogin sModel)
        {
            AddViewDataObject(sModel);
            if (ModelState.IsValid)
            {
                NWDAccountSign tAccountSign = NWDAccountSign.CreateLoginEmailPassword(sModel.AccountSignRescueLogin, sModel.AccountSignRescueEmail, sModel.AccountSignRescuePassword, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                tAccountSign.TokenRescue = sModel.AccountSignRescueToken;
                if (NWDAccountSignWebManager.RescueAccountSignTestToken(tAccountSign, HttpContext) == true)
                {
                    return RedirectToAction(nameof(SignInWelcome));
                }
                else
                {
                    AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignRescueUnknown);
                    return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignRescueFormB, sModel);
                }
            }
            else
            {
                AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignRescueFailModelInvalid);
                return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignRescueFormB, sModel);
            }
        }

        #endregion

        #region Sign-lost

        public ActionResult SignLostForm()
        {
            return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignLostForm_Embedded);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignLost(NWDAccountSignLost sModel)
        {
            AddViewDataObject(sModel);
            if (ModelState.IsValid)
            {
                try
                {
                    NWDAccountSign tAccountSign = new NWDAccountSign();
                    string tUrlToClick = NWDAccountSignWebManager.LostAccountSign(sModel.AccountSignLostEmail, HttpContext, NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/" + NWDAccountController.ASP_Controller() + "/" + nameof(SignRescueForm) + "/index.html", NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/" + NWDAccountController.ASP_Controller() + "/" + nameof(SignRescueFormB) + "/index.html");
                    if (string.IsNullOrEmpty(tUrlToClick) == false)
                    {
                        AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignLostSuccess);
#if DEBUG
                        PageInformation.AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Danger, "DEBUG LINK", tUrlToClick));
#endif
                        NWDEmailManager tSender = new NWDEmailManager();
                        tSender.SendFromWebsite(sModel.AccountSignLostEmail, NWDWebStandardConfiguration.KConfig.WebSiteName + " rescue account", "<div>Hello you need rescue your account, click on this link : <a href=\"" + tUrlToClick + "\"> rescue token </a></div>");
                        return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignLostSuccess_Embedded);
                    }
                    else
                    {
                        AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignLostFail);
                    }
                }
                catch (Exception tEx)
                {
                    NWDLogger.Exception(tEx);
                }
            }
            else
            {
                AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignLostFailModelInvalid);
            }

            return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignLostForm_Embedded);
        }

        #endregion

        #region Sign-out

        public ActionResult SignOutForm()
        {
            return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignOutForm_Embedded);
        }

        public ActionResult LogOut()
        {
            NWDAccountWebManager.AccountSignOut(HttpContext);
            if (NWDAccountWebManager.AccountIsConnected(HttpContext) == false)
            {
                AddActualToast(NWDWebStandard.Controllers.NWDAccountController.LogOutSuccess);
            }
            else
            {
                AddActualToast(NWDWebStandard.Controllers.NWDAccountController.LogOutFail);
            }
            //Check if method exist
            if (typeof(T).GetMethod(NWDWebStandardConfiguration.KConfig.SignOutSuccessAction) != null)
            {
                return RedirectToAction(NWDWebStandardConfiguration.KConfig.SignOutSuccessAction);
            }
            else
            {
                return RedirectToAction(nameof(SignOutByeBye));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut(NWDAccountSignOut sModel)
        {
            AddViewDataObject(sModel);
            if (ModelState.IsValid)
            {
                NWDAccountWebManager.AccountSignOut(HttpContext);
                if (NWDAccountWebManager.AccountIsConnected(HttpContext) == false)
                {
                    AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignOutSuccess);
                }
                else
                {
                    AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignOutFail);
                }
            }
            else
            {
                AddActualToast(NWDWebStandard.Controllers.NWDAccountController.SignOutFail);
            }
            //AddTempDataObject(PageInformation);
            //Check if method exist
            if (typeof(T).GetMethod(NWDWebStandardConfiguration.KConfig.SignOutSuccessAction) != null)
            {
                return RedirectToAction(NWDWebStandardConfiguration.KConfig.SignOutSuccessAction);
            }
            else
            {
                return RedirectToAction(nameof(SignOutByeBye));
            }
        }

        public ActionResult SignOutByeBye()
        {
            return View(NWDWebStandard.Controllers.NWDAccountController.K_AccountSignOutByeBye_Embedded);
        }

        #endregion

        #region Contact us

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(NWDContactUsModel sModel)
        {
            if (ModelState.IsValid)
            {
                if (sModel.Message.Contains("http://") || sModel.Message.Contains("https://"))
                {
                    sModel.CaptchaValue = string.Empty;
                    TryValidateModel(sModel);
                }
                else
                {
                    if (NWDCaptcha.TestCaptcha(HttpContext, sModel) == true)
                    {
                        if (string.IsNullOrWhiteSpace(sModel.Subject))
                        {
                            sModel.Subject = "Contact";
                        }

                        try
                        {
                            NWDEmailManager tEmailer = new NWDEmailManager();
                            tEmailer.SendEmailFromContact(sModel, HttpContext);
                            TempData.PutObject<NWDContactUsModel>(sModel);
                            return RedirectToAction(nameof(ContactUsSuccess));
                        }
                        catch (Exception tEx)
                        {
                            NWDLogger.Exception(tEx);
                        }
                    }
                    else
                    {
                        PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Warning, "Captcha error", "Captcha failed!", null);
                        sModel.CaptchaValue = string.Empty;
                        TryValidateModel(sModel);
                    }
                }
            }
            else
            {
                foreach (var tError in ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }))
                {
                    NWDLogger.TraceError(tError.ToString());
                }

                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                NWDLogger.TraceError(message);

                sModel.CaptchaValue = string.Empty;
                TryValidateModel(sModel);
            }

            PageInformation.Title = "contact-us";
            PageInformation.Keywords.AddRange(new List<string>()
            {
                "contact-us",
            });
            PageInformation.ShowAuthentication = false;
            PageInformation.NewCaptcha(HttpContext);
            return View(sModel);
        }

        public IActionResult ContactUs()
        {
            PageInformation.Title = "contact-us";
            PageInformation.Keywords.AddRange(new List<string>()
            {
                "contact-us",
            });
            PageInformation.ShowAuthentication = false;
            PageInformation.NewCaptcha(HttpContext);
            return View(new NWDContactUsModel());
        }

        public IActionResult ContactUsSuccess()
        {
            ViewData[nameof(NWDContactUsModel)] = TempData.GetObject<NWDContactUsModel>();
            return View();
        }

        #endregion
    }
}