using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWDFoundation.Logger;
using NWDFoundation.WebEdition.Enums;
using NWDWebEditor.Managers;
using NWDWebTreat.Managers;
using NWDWebTreat.Models;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools.Sessions;
using NWDWebStandard.Configuration;
using NWDWebStandard.Controllers;
using NWDWebTreat.Configuration;

namespace NWDWebTreat.Controllers;

public class NWDVoucherController : NWDBasicController<NWDVoucherController>
{
    private static NWDSessionString SessionVoucherToken = new NWDSessionString("Voucher Token", "Token record", "Voucher operation in progress ", NWDSessionDefinitionGroup.Functional, "");
    public readonly NWDWebDBEditionDataManager<NWDTokenVoucher> TokenVoucherManager = new NWDWebDBEditionDataManager<NWDTokenVoucher>();

    private static NWDToastStandard SendEmailSuccessful =
        NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success,
            "Send successful", "Your voucher has been send successfully");

    private static NWDToastStandard SendEmailFailed =
        NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Danger,
            "Something went wrong", "Your voucher has not been send");

    public static NWDToastStandard CosumeTokenSuccessful =
        NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success,
            "The Voucher is consumed", "Your voucher has been properly consumed, your services has been updated");

    private static NWDToastStandard CosumeTokenFailed =
        NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Danger,
            "Something went wrong", "Your voucher has not been properly consumed");

    private static NWDToastStandard SignBeforeVoucherConsuming =
        NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Primary,
            "Information", "You need to sign in or sign up before consuming your voucher");

    static NWDVoucherController()
    {
        ListOnActionExecuting.Add((HttpContext sHttpContext, NWDRawController sController) =>
        {
            if (NWDAccountWebManager.AccountIsConnected(sHttpContext) && !string.IsNullOrEmpty(SessionVoucherToken.GetValue(sHttpContext)))
            {
                if (NWDVoucherTokenManager.ConsumeVoucherToken(sHttpContext, SessionVoucherToken.GetValue(sHttpContext)))
                {
                    sController.PageInformation.AddActualToast(CosumeTokenSuccessful);
                }
                else
                {
                    sController.PageInformation.AddActualToast(CosumeTokenFailed);
                }

                SessionVoucherToken.DeleteFrom(sHttpContext);
            }
        });
        ListOnActionExecuted.Add((HttpContext sHttpContext, NWDRawController sController) => { });
    }

    private List<NWDVoucherServiceName> VoucherList()
    {
        List<NWDVoucherServiceName> tVoucherList = new List<NWDVoucherServiceName>();
        if (NWDAuthorizeByAllOfServices.ValidFor(HttpContext, NWDGenericServiceEnum.Admin))
        {
            tVoucherList.AddRange(NWDWebTreatConfiguration.KConfig.VoucherServiceList);
            tVoucherList.Add(new NWDVoucherServiceName(NWDGenericServiceEnum.Admin.Name, NWDGenericServiceEnum.Admin.Value));
            if (NWDAuthorizeByAllOfServices.ValidFor(HttpContext,NWDGenericServiceEnum.Marketing))
            {
                tVoucherList.AddRange(NWDWebTreatConfiguration.KConfig.VoucherServiceMarketingList);
            }
        }
        else if (NWDAuthorizeByAllOfServices.ValidFor(HttpContext,NWDGenericServiceEnum.Marketing))
        {
            tVoucherList.AddRange(NWDWebTreatConfiguration.KConfig.VoucherServiceMarketingList);
        }

        return tVoucherList;
    }

    private void AddVoucherList()
    {
        ViewData.Add("VoucherList", VoucherList());
    }

    [NWDAuthorizeByAuthentication(true)]
    public IActionResult Index()
    {
        PageInformation.JavascriptPathAddonList.Add("/vendors/tinymce/tinymce.min.js");
        PageInformation.JavascriptPathAddonList.Add("/assets/js/flatpickr.js");
        PageInformation.CssPathAddonList.Add("/vendors/flatpickr/flatpickr.min.css");
        //PageInformation.SideBarStyle = NWDSideBarKind.None;
        List<NWDVoucherServiceName> tVoucherList = new List<NWDVoucherServiceName>();
        if (NWDAuthorizeByOneOfService.ValidFor(HttpContext, new NWDGenericServiceEnum[] { NWDGenericServiceEnum.Marketing,NWDGenericServiceEnum.Admin}))
        {
            AddVoucherList();
            PageInformation.NewCaptcha(HttpContext);
            return View(nameof(Index), new NWDVoucher() { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7) });
        }
        else
        {
            return RedirectToAction(nameof(ServiceOnly));
        }
    }

    public override void OnActionExecuting(ActionExecutingContext sContext)
    {
        base.OnActionExecuting(sContext);
        ConsumeTokenFromSession();
    }

    public void ConsumeTokenFromSession()
    {
        if (NWDAccountWebManager.AccountIsConnected(HttpContext) && !string.IsNullOrEmpty(SessionVoucherToken.GetValue(HttpContext)))
        {
            NWDVoucherTokenManager.ConsumeVoucherToken(HttpContext, SessionVoucherToken.GetValue(HttpContext));
            PageInformation.AddActualToast(CosumeTokenSuccessful);
        }
    }

    [HttpPost()]
    [NWDAuthorizeByAuthentication(true)]
    [ValidateAntiForgeryToken]
    public IActionResult SendVoucher(NWDVoucher sVoucher)
    {
        PageInformation.JavascriptPathAddonList.Add("/vendors/tinymce/tinymce.min.js");
        PageInformation.JavascriptPathAddonList.Add("/assets/js/flatpickr.js");
        PageInformation.CssPathAddonList.Add("/vendors/flatpickr/flatpickr.min.css");
        //PageInformation.SideBarStyle = NWDSideBarKind.None;
        if (ModelState.IsValid)
        {
            if (NWDAuthorizeByOneOfService.ValidFor(HttpContext, new NWDGenericServiceEnum[] { NWDGenericServiceEnum.Marketing,NWDGenericServiceEnum.Admin}))
            {
                bool tTest = false;
                string tVoucherName = string.Empty;
                foreach (NWDVoucherServiceName tVoucherItem in VoucherList())
                {
                    if (sVoucher.Service == tVoucherItem.Id)
                    {
                        tTest = true;
                        tVoucherName = tVoucherItem.Name;
                        break;
                    }
                }
                if (NWDCaptcha.TestCaptcha(HttpContext, sVoucher) == true && tTest == true)
                {
                    foreach (NWDVoucher tVoucher in sVoucher.IndividualVoucher())
                    {
                        NWDTokenVoucher tTokenVoucher = new NWDTokenVoucher(tVoucher);
                        tTokenVoucher.Name = tVoucherName;
                        TokenVoucherManager.Add(tTokenVoucher);
                        try
                        {
                            string tUrlForVoucher = NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/NWDVoucher/" + nameof(ConsumeVoucher) + "?token=" + tTokenVoucher.Token;
                            NWDLogger.Information(tUrlForVoucher);
                            if (string.IsNullOrEmpty(tVoucher.Message))
                            {
                                tVoucher.Message = string.Empty;
                            }

                            new NWDEmailManager().SendFromWebsite(tVoucher.Email,
                                "You receive a free trial for " + NWDWebStandardConfiguration.KConfig.WebSiteName,
                                tVoucher.Message.Replace("\n", "\r").Replace("\r", "<br />") +
                                "You've been invited to join " + NWDWebStandardConfiguration.KConfig.WebSiteName +
                                ", just click the link to redeem your free trial of " + tTokenVoucher.GetDurationInDays() +
                                " days! <br/> " + tUrlForVoucher + " <br/><br/> best regards, <br/><br/>The teams of " + NWDWebStandardConfiguration.KConfig.WebSiteName);
                            AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success,
                                "Send successful", "Your voucher has been send successfully to " + tVoucher.Email));
                        }
                        catch (Exception tE)
                        {
                            NWDLogger.Exception(tE);
                            AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Warning,
                                "Send failed", "Your voucher has not been send to " + tVoucher.Email));
                            AddActualToast(SendEmailFailed);
                        }
                    }

                    AddVoucherList();
                    // AddTempDataObject(PageInformation);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Warning, "Captcha error", "Captcha failed!", null);
                    AddVoucherList();
                    sVoucher.CaptchaValue = string.Empty;
                    PageInformation.NewCaptcha(HttpContext);
                    return View(nameof(Index), sVoucher);
                }
            }
            else
            {
                return RedirectToAction(nameof(ServiceOnly));
            }
        }
        else
        {
            // foreach (var tError in ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }))
            // {
            //     NWDLogger.TraceError(tError.ToString());
            // }
            return View(nameof(Index), sVoucher);
        }
    }

    public IActionResult ConsumeVoucher(string token) // DON'T Change this name ... it's use in URL
    {
        if (NWDAccountWebManager.AccountIsConnected(HttpContext))
        {
            if (NWDVoucherTokenManager.ConsumeVoucherToken(HttpContext, token))
            {
                AddActualToast(CosumeTokenSuccessful);
            }
            else
            {
                AddActualToast(CosumeTokenFailed);
            }

            SessionVoucherToken.DeleteFrom(HttpContext);
        }
        else
        {
            AddActualToast(SignBeforeVoucherConsuming);
            SessionVoucherToken.SetValue(HttpContext, token);
            return RedirectToAction(nameof(NWDAccountController.SignInForm), NWDAccountController.ASP_Controller());
        }
        // AddTempDataObject(PageInformation);
        return RedirectToAction(nameof(NWDAccountController.Index), NWDAccountController.ASP_Controller());
    }
}