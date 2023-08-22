using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NWDFoundation.Configuration;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;
using NWDWebStandard.Configuration;
using NWDWebStandard.Extensions;
using NWDWebStandard.Models;

namespace NWDWebStandard.Controllers
{
    public partial class NWDAccountController : NWDBasicController<NWDAccountController>
    {
        #region Constants

        private const string K_KViewAccountFolder = "/Views/Account/";
        private const string K_KViewSharedFolder = "/Views/Shared/";

        public static NWDToastStandard YouAreAlreadyConnected =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Secondary, "You are already signed-in!");
        //public static ToastStandard K_YourAccountNotExists =
        //    ToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your sign-in failed!", "The reasons can be many:", new List<string>() { "You entered wrong credentials", "You entered invisible characters", "Your cookies are disabled", "Your account has been deleted" });

        public const string K_AccountSignUpForm_Embedded = K_KViewSharedFolder + "_AccountSignUpForm_Embedded.cshtml";
        public const string K_AccountSignUpForm = K_KViewSharedFolder + "_AccountSignUpForm.cshtml";
        public const string K_AccountSignUpModalForm = K_KViewSharedFolder + "_AccountSignUpModalForm.cshtml";
        public const string K_AccountSignUpWelcome_Embedded = K_KViewSharedFolder + "_AccountSignUpWelcome_Embedded.cshtml";
        public const string K_AccountSignUpWelcome = K_KViewSharedFolder + "_AccountSignUpWelcome.cshtml";

        public static NWDToastStandard SignUpSuccess =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success, "You are now successfully signed-up!");

        public static NWDToastStandard SignUpFailAlreadyAccount =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your sign-up failed!", "The reasons can be many:",
                new List<string>() { "Your email is used by another account", "You entered wrong credentials", "You entered invisible characters", "Your cookies are disabled" });

        public static NWDToastStandard SignUpFailModelInvalid =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your sign-up failed!", "Please fill the form and retry");

        public const string K_AccountSignInForm_Embedded = K_KViewSharedFolder + "_AccountSignInForm_Embedded.cshtml";
        public const string K_AccountSignInForm = K_KViewSharedFolder + "_AccountSignInForm.cshtml";
        public const string K_AccountSignInMiniForm = K_KViewSharedFolder + "_AccountSignInMiniForm.cshtml";
        public const string K_AccountSignInModalForm = K_KViewSharedFolder + "_AccountSignInModalForm.cshtml";

        public const string K_AccountSignInWelcome_Embedded = K_KViewSharedFolder + "_AccountSignInWelcome_Embedded.cshtml";
        public const string K_AccountSignInWelcome = K_KViewSharedFolder + "_AccountSignInWelcome.cshtml";

        public static NWDToastStandard K_SignInSuccess =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success, "You are now successfully signed-in!");

        public static NWDToastStandard SignInFailUnknownAccount =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your sign-in failed!", "The reasons can be many:",
                new List<string>() { "You entered wrong credentials", "You entered invisible characters", "Your cookies are disabled", "Your account has been deleted" });

        public static NWDToastStandard SignInFailModelInvalid =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your sign-in failed!", "Please fill the form and retry");

        public const string K_AccountSignOutForm_Embedded = K_KViewSharedFolder + "_AccountSignOutForm_Embedded.cshtml";
        public const string K_AccountSignOutForm = K_KViewSharedFolder + "_AccountSignOutForm.cshtml";
        public const string K_AccountSignOutByeBye_Embedded = K_KViewSharedFolder + "_AccountSignOutByeBye_Embedded.cshtml";
        public const string K_AccountSignOutByeBye = K_KViewSharedFolder + "_AccountSignOutByeBye.cshtml";

        public static NWDToastStandard SignOutSuccess =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success, "You are now successfully signed-out!");

        public static NWDToastStandard SignOutFail =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your sign-out failed!", "The reasons can be many:",
                new List<string>() { "Your cookies are disabled", "Your cookies are corrupted" });

        public static NWDToastStandard LogOutSuccess =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success, "You are now successfully logged-out!");

        public static NWDToastStandard LogOutFail =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your log-out failed!", "The reasons can be many:",
                new List<string>() { "Your cookies are disabled", "Your cookies are corrupted" });

        public const string K_AccountSignRescueForm_Embedded = K_KViewSharedFolder + "_AccountSignRescueForm_Embedded.cshtml";

        public const string K_AccountSignRescueForm = K_KViewSharedFolder + "_AccountSignRescueForm.cshtml";
        public const string K_AccountSignRescueFormB_Embedded = K_KViewSharedFolder + "_AccountSignRescueFormB_Embedded.cshtml";

        public const string K_AccountSignRescueFormB = K_KViewSharedFolder + "_AccountSignRescueFormB.cshtml";

        //public const string _AccountSignRescueSuccess_Embedded = KViewSharedFolder + "_AccountSignRescueSuccess_Embedded.cshtml";
        //public const string _AccountSignRescueSuccess = KViewSharedFolder + "_AccountSignRescueSuccess.cshtml";
        public static readonly NWDToastStandard K_SignRescueSuccess =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success, "You are now successfully rescue your password!");

        public static NWDToastStandard SignRescueFail =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your rescue failed!",
                "The information does not match any resettable member account. The reasons may be the following:",
                new List<string>() { "Your reset token has expired", "Your reset token is invalid", "Your reset token has already been used" });

        public static readonly NWDToastStandard SignRescueUnknown =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your rescue failed!",
                "The information does not match any resettable member account. The reasons may be the following:",
                new List<string>() { "Your reset token has expired", "Your reset token is invalid", "Your reset token has already been used" });

        public static readonly NWDToastStandard SignRescueFailModelInvalid =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your rescue failed!", "Please fill the form and retry");


        public const string K_AccountSignLostForm_Embedded = K_KViewSharedFolder + "_AccountSignLostForm_Embedded.cshtml";
        public const string K_AccountSignLostForm = K_KViewSharedFolder + "_AccountSignLostForm.cshtml";
        public const string K_AccountSignLostSuccess_Embedded = K_KViewSharedFolder + "_AccountSignLostSuccess_Embedded.cshtml";
        public const string K_AccountSignLostSuccess = K_KViewSharedFolder + "_AccountSignLostSuccess.cshtml";

        public static NWDToastStandard SignLostSuccess =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success, "We just sent you an email to reset your password!");

        public static NWDToastStandard SignLostFail =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your rescue failed!", "Your email is unknown!", new List<string>(), string.Empty, true);

        public static NWDToastStandard SignLostFailModelInvalid =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your rescue failed!", "Please fill the form and retry");


        public const string K_AccountDeleteForm_Embedded = K_KViewSharedFolder + "_AccountDeleteForm_Embedded.cshtml";
        public const string K_AccountDeleteForm = K_KViewSharedFolder + "_AccountDeleteForm.cshtml";
        public const string K_AccountDeleteSuccess_Embedded = K_KViewSharedFolder + "_AccountDeleteByeBye_Embedded.cshtml";
        public const string K_AccountDeleteSuccess = K_KViewSharedFolder + "_AccountDeleteByeBye.cshtml";

        public static readonly NWDToastStandard AccountDeleteFail =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your deletion failed!", "", new List<string>(), string.Empty, false);

        public static readonly NWDToastStandard AccountDeleteModelInvalid =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your deletion failed!", "Please fill the form and retry");


        public static NWDToastStandard SignAddSuccess =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success, "You add successfully sign!");

        public static readonly NWDToastStandard SignRevokeSuccess =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success, "You revoked successfully sign!");

        public static readonly NWDToastStandard SignRevokeFailModelInvalid =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning, "Your revoke failed!", "Please retry");

        #endregion

        #region Logger

        private readonly ILogger<NWDAccountController> _logger;


        public NWDAccountController(ILogger<NWDAccountController> logger)
        {
            _logger = logger;
        }

        #endregion
        [Obsolete("Must be rewrite without HttpContext.Request.Path")]
        public ActionResult RevokeSign()
        {
            string?[]? tDirectories = HttpContext.Request.Path.Value?.Split("/" + nameof(RevokeSign) + "/");
            if (tDirectories != null && tDirectories.Length>1)
            {
                if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
                {
                    // TODO 
                    NWDAccountRevokeSign tModel = new NWDAccountRevokeSign();
                    tModel.Reference = ulong.Parse(tDirectories?[1] ?? string.Empty);
                    List<NWDAccountSign> tSigns = NWDAccountSignWebManager.GetAccountSigns(HttpContext);
                    bool tFound = false;
                    foreach (NWDAccountSign tSign in tSigns)
                    {
                        if (tSign.Reference == tModel.Reference)
                        {
                            tFound = true;
                            tModel.SignName = tSign.DecryptName();
                            AddViewDataObject(tModel);
                            AddViewDataObject(tSign);
                            break;
                        }
                    }
                    //AddTempDataObject(PageInformation);
                    if (tFound == false)
                    {
                        return RedirectToAction(nameof(Error));
                    }

                    return View(tModel);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevokeSign(NWDAccountRevokeSign sModel)
        {
            if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
            {
                if (ModelState.IsValid)
                {
                    // Delete this sign if is not the last one and register for active account
                    List<NWDAccountSign> tList = NWDAccountSignWebManager.GetAccountSigns(HttpContext);
                    bool tFound = false;
                    foreach (NWDAccountSign tSign in tList)
                    {
                        if (tSign.Reference == sModel.Reference)
                        {
                            tFound = true;
                            NWDAccountSignWebManager.Delete(tSign, HttpContext);
                            AddViewDataObject(tSign);
                            AddActualToast(SignRevokeSuccess);
                            break;
                        }
                    }
                    //AddTempDataObject(PageInformation);
                    if (tFound == false)
                    {
                        return RedirectToAction(nameof(Error));
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var tErrors = ModelState
                        .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                        .Select(x =>
                        {
                            if (x.Value != null) return new { x.Key, x.Value.Errors };
                            return null;
                        })
                        .ToArray();
                    foreach (var tErr in tErrors)
                    {
                        NWDLogger.Warning(" !!=> " + tErr);
                    }

                    AddActualToast(SignRevokeFailModelInvalid);
                    return RedirectToAction(nameof(Error));
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult AddEmailPasswordSign()
        {
            if (NWDWebStandardConfiguration.KConfig.AddAccountSignEmailPassword == true)
            {
                if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
                {
                    NWDAccountSignAddEmailPassword tSign = new NWDAccountSignAddEmailPassword();
                    return View(tSign);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmailPasswordSign(NWDAccountSignAddEmailPassword sModel)
        {
            if (NWDWebStandardConfiguration.KConfig.AddAccountSignEmailPassword == true)
            {
                if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
                {
                    if (ModelState.IsValid)
                    {
                        NWDAccountSign tSign = NWDAccountSign.CreateEmailPassword(sModel.AccountSignAddEmailPasswordEmail, sModel.AccountSignAddEmailPasswordPassword, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                        if (NWDAccountSignWebManager.TryToAdd(tSign, HttpContext).Status == NWDRequestStatus.Ok)
                        {
                            //AddTempDataObject(PageInformation);
                            if (NWDWebStandardConfiguration.KConfig.AddAccountSignEmailPasswordSendEmail)
                            {
                                NWDEmailManager tEmailSend = new NWDEmailManager();
                                tEmailSend.SendFromWebsite(sModel.AccountSignAddEmailPasswordEmail, "Add new sign to your account",
                                    "Hello, you just add new sign to ypur account : " + sModel.AccountSignAddEmailPasswordEmail + ": " + sModel.AccountSignAddEmailPasswordPassword);
                            }

                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            // TODO Add toast information
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        // TODO Add toast information
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    // TODO Add toast information
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                // TODO Add toast information
                return RedirectToAction(nameof(Error));
            }
        }

        public ActionResult AddLoginPasswordEmailSign()
        {
            if (NWDWebStandardConfiguration.KConfig.AddAccountSignLoginPasswordEmail == true)
            {
                if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
                {
                    NWDAccountSignAddLoginPasswordEmail tSign = new NWDAccountSignAddLoginPasswordEmail();
                    return View(tSign);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLoginPasswordEmailSign(NWDAccountSignAddLoginPasswordEmail sModel)
        {
            if (NWDWebStandardConfiguration.KConfig.AddAccountSignLoginPasswordEmail == true)
            {
                if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
                {
                    if (ModelState.IsValid)
                    {
                        NWDAccountSign tSign = NWDAccountSign.CreateLoginEmailPassword(sModel.AccountSignAddLoginPasswordEmailLogin, sModel.AccountSignAddLoginPasswordEmailEmail,
                            sModel.AccountSignAddLoginPasswordEmailPassword, NWDWebRuntimeConfiguration.KConfig.GetProjectId());

                        if (NWDAccountSignWebManager.TryToAdd(tSign, HttpContext).Status == NWDRequestStatus.Ok)
                        {
                            //AddTempDataObject(PageInformation);
                            if (NWDWebStandardConfiguration.KConfig.AddAccountSignLoginPasswordEmailSendEmail)
                            {
                                NWDEmailManager tEmailSend = new NWDEmailManager();
                                tEmailSend.SendFromWebsite(sModel.AccountSignAddLoginPasswordEmailEmail, "Add new sign to your account",
                                    "Hello, you just add new sign to your account : " + sModel.AccountSignAddLoginPasswordEmailEmail + ": " + sModel.AccountSignAddLoginPasswordEmailLogin + ": " +
                                    sModel.AccountSignAddLoginPasswordEmailPassword);
                            }

                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            // TODO Add toast information
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(sModel);
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }

        public ActionResult AddLoginPasswordSign()
        {
            if (NWDWebStandardConfiguration.KConfig.AddAccountSignLoginPassword == true)
            {
                if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
                {
                    NWDAccountSignAddLoginPassword tSign = new NWDAccountSignAddLoginPassword();
                    return View(tSign);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLoginPasswordSign(NWDAccountSignAddLoginPassword sModel)
        {
            if (NWDWebStandardConfiguration.KConfig.AddAccountSignLoginPassword == true)
            {
                if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
                {
                    if (ModelState.IsValid)
                    {
                        NWDAccountSign tSign = NWDAccountSign.CreateLoginPassword(sModel.AccountSignAddLoginPasswordLogin, sModel.AccountSignAddLoginPasswordPassword,
                            NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                        if (NWDAccountSignWebManager.TryToAdd(tSign, HttpContext).Status == NWDRequestStatus.Ok)
                        {
                            //AddTempDataObject(PageInformation);
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            // TODO Add toast information
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(sModel);
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }

        private void UseSocialSign(NWDAccountSign sAccountSign)
        {
            if (NWDAccountWebManager.AccountIsConnected(HttpContext))
            {
                // already connected 
                if (NWDAccountSignWebManager.TryToAdd(sAccountSign, HttpContext).Status == NWDRequestStatus.Ok)
                {
                    // add success
                    AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Primary, "Social sign added", "Social sign was added with success!"));
                }
                else
                {
                    // already used 
                    if (NWDAccountSignWebManager.TrySignIn(sAccountSign, HttpContext, true).Status == NWDRequestStatus.Ok)
                    {
                        // connect by this account
                        AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Primary, "Social used by another account", "You just change account!"));
                    }
                    else
                    {
                        // impossible to use
                        AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Social sign error", "Social sign not working!"));
                    }
                }
            }
            else
            {
                // not connected 
                // try to SignIn
                if (NWDAccountSignWebManager.TrySignIn(sAccountSign, HttpContext, true).Status == NWDRequestStatus.Ok)
                {
                    // connect by this account
                    AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Primary, "Social used", "You just change account!"));
                }
                else
                {
                    if (NWDAccountSignWebManager.TrySignUp(sAccountSign, HttpContext, true).Status == NWDRequestStatus.Ok)
                    {
                        AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Primary, "Social used ", "You just create your account!"));
                    }
                    else
                    {
                        AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Social sign error", "Social sign not working!"));
                    }
                }
            }
        }

        [Route("Account")]
        [Route("NWDAccount")]
        public ActionResult Index()
        {
            PageInformation.Title = "User account";
            PageInformation.Keywords.AddRange(new List<string>()
            {
                "user",
                "account",
            });
            PageInformation.ShowAuthentication = false;
            ViewData.Add("TraceIdentifier", HttpContext.TraceIdentifier.ToString());
            ViewData.Add("RemoteIpAddress", NWDGetIp.GetRemoteIpAddress(HttpContext).ToString());
            //PageInformation.FrequentlyAskedQuestionsList = NWDFrequentlyAskedQuestionExtension.GetList(new string[] { "Website" }, string.Empty, true);
            return View();
        }

        public ActionResult ServicesList()
        {
            PageInformation.Title = "User service";
            PageInformation.Keywords.AddRange(new List<string>()
            {
                "user",
                "service",
            });
            PageInformation.ShowAuthentication = false;
            return View();
        }

        public ActionResult SignsList()
        {
            PageInformation.Title = "User signs";
            PageInformation.Keywords.AddRange(new List<string>()
            {
                "user",
                "signs",
            });
            PageInformation.ShowAuthentication = false;
            return View();
        }

        public ActionResult ModifyLoginPassword()
        {
            if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
            {
                NWDAccountSignModifyLoginPassword tSocial = new NWDAccountSignModifyLoginPassword();
                return View(tSocial);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyLoginPassword(NWDAccountSignModifyLoginPassword sModel)
        {
            if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
            {
                if (ModelState.IsValid)
                {
                    NWDAccountSign tNewSign = NWDAccountSign.CreateLoginPassword(sModel.AccountSignModifyLoginPasswordNewLogin, sModel.AccountSignModifyLoginPasswordNewPassword,
                        NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                    NWDAccountSign tOldSign = NWDAccountSign.CreateLoginPassword(sModel.AccountSignModifyLoginPasswordLogin, sModel.AccountSignModifyLoginPasswordPassword,
                        NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                    if (NWDAccountSignWebManager.TryToModify(HttpContext, tOldSign, tNewSign).Status == NWDRequestStatus.Ok)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(sModel);
                    }
                }
                else
                {
                    var tErrors = ModelState
                        .Where(sX => sX.Value != null && sX.Value.Errors.Count > 0)
                        .Select(sX =>
                        {
                            if (sX.Value != null) return new { sX.Key, sX.Value.Errors };
                            return null;
                        })
                        .ToArray();
                    foreach (var tErr in tErrors)
                    {
                        NWDLogger.Warning(" !!=> " + tErr);
                    }
                }

                return View(sModel);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult ModifyEmailPassword()
        {
            if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
            {
                NWDAccountSignModifyEmailPassword tSocial = new NWDAccountSignModifyEmailPassword();
                return View(tSocial);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyEmailPassword(NWDAccountSignModifyEmailPassword sModel)
        {
            if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
            {
                if (ModelState.IsValid)
                {
                    NWDAccountSign tNewSign = NWDAccountSign.CreateEmailPassword(sModel.AccountSignModifyEmailPasswordNewEmail, sModel.AccountSignModifyEmailPasswordNewPassword,
                        NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                    NWDAccountSign tOldSign = NWDAccountSign.CreateEmailPassword(sModel.AccountSignModifyEmailPasswordEmail, sModel.AccountSignModifyEmailPasswordPassword,
                        NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                    if (NWDAccountSignWebManager.TryToModify(HttpContext, tOldSign, tNewSign).Status == NWDRequestStatus.Ok)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(sModel);
                    }
                }
                else
                {
                    var tErrors = ModelState
                        .Where(sKeyValuePair => sKeyValuePair.Value != null && sKeyValuePair.Value.Errors.Count > 0)
                        .Select(sKeyValuePairSecond =>
                        {
                            if (sKeyValuePairSecond.Value != null) return new { sKeyValuePairSecond.Key, sKeyValuePairSecond.Value.Errors };
                            return null;
                        })
                        .ToArray();
                    foreach (var tErr in tErrors)
                    {
                        NWDLogger.Warning(tErr.ToString());
                    }
                }

                return View(sModel);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult ModifyLoginPasswordEmail()
        {
            if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
            {
                PageInformation.Title = "Modify login";
                PageInformation.ShowAuthentication = false;
                NWDAccountSignModifyLoginPasswordEmail tSocial = new NWDAccountSignModifyLoginPasswordEmail();
                return View(tSocial);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyLoginPasswordEmail(NWDAccountSignModifyLoginPasswordEmail sModel)
        {
            if (NWDAccountWebManager.AccountIsConnected(HttpContext) == true)
            {
                if (ModelState.IsValid)
                {
                    NWDAccountSign tNewSign = NWDAccountSign.CreateLoginEmailPassword(sModel.AccountSignModifyLoginPasswordEmailNewLogin, sModel.AccountSignModifyLoginPasswordEmailNewPassword,
                        sModel.AccountSignModifyLoginPasswordEmailNewEmail, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                    NWDAccountSign tOldSign = NWDAccountSign.CreateLoginEmailPassword(sModel.AccountSignModifyLoginPasswordEmailLogin, sModel.AccountSignModifyLoginPasswordEmailPassword,
                        sModel.AccountSignModifyLoginPasswordEmailEmail, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                    if (NWDAccountSignWebManager.TryToModify(HttpContext, tOldSign, tNewSign).Status == NWDRequestStatus.Ok)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(sModel);
                    }
                }
                else
                {
                    var tErrors = ModelState
                        .Where(sKeyValuePair => sKeyValuePair.Value != null && sKeyValuePair.Value.Errors.Count > 0)
                        .Select(sKeyValuePairSecond =>
                        {
                            if (sKeyValuePairSecond.Value != null) return new { sKeyValuePairSecond.Key, sKeyValuePairSecond.Value.Errors };
                            return null;
                        })
                        .ToArray();
                    foreach (var tErr in tErrors)
                    {
                        NWDLogger.Warning(tErr.ToString());
                    }
                }

                return View(sModel);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult AccountFakeModel()
        {
            return View("AccountFakeModel");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModifyAccountInfo(NWDAccountInfo sModel)
        {
            if (ModelState.IsValid)
            {
                // NWDAccountInfo? tModel = NWDWebDataManager.GetDataByReference<NWDAccountInfo>(HttpContext, sModel.Reference);
                NWDAccountInfo? tModel = NWDAccountInfoManager.GetAccountInfo(HttpContext);
                if (tModel == null)
                {
                    tModel = new NWDAccountInfo();
                }
                if (string.IsNullOrEmpty(sModel.GravatarAccount) == false)
                {
                    tModel.GravatarHash = NWDSecurityTools.HashMd5(sModel.GravatarAccount).ToLower();
                    tModel.GravatarAccount = string.Empty;
                }
                tModel.Nickname = sModel.Nickname;
                tModel.Firstname = sModel.Firstname;
                tModel.Lastname = sModel.Lastname;
                
                tModel.Save(HttpContext);
                return RedirectToAction("Index");
            }
            else
            {
                return Error();
            }
        }
    }
}