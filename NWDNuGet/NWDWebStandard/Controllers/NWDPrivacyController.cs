using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NWDWebRuntime;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools.Cookies;

namespace NWDWebStandard.Controllers;

public class NWDPrivacyController : NWDBasicController<NWDPrivacyController>
{
    private readonly ILogger<NWDPrivacyController> _logger;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        if (NWDWebRuntimeStartupService.CookieConsent.GetValue(context.HttpContext) == false)
        {
            NWDCookieGlobal.DeleteAllCookie(context.HttpContext);
        }
    }

    public NWDPrivacyController(ILogger<NWDPrivacyController> sLogger)
    {
        _logger = sLogger;
    }

    public IActionResult Index()
    {
        PageInformation.Title = "Privacy";
        PageInformation.Description = "Pagination about privacy policy, terms of service and terms and condition for this website's usage.";
        PageInformation.Keywords = new List<string>() { "privacy", "policy","term", "condition", "service", "GDPR", "RGPD" };
        PageInformation.ShowAuthentication = false;
        // PageInformation.VisitShopNow = ShopShow.No;
        PageInformation.PageStyle = NWDPageContainer.ContainerPage;
        return View();
    }

    /// <summary>
    /// Privacy Policy (Politique de confidentialité)
    /// </summary>
    /// <returns></returns>
    /// 
    public IActionResult PrivacyPolicy()
    {
        PageInformation.Title = "Privacy Policy";
        PageInformation.Description = "Pagination about privacy policy.";
        PageInformation.Keywords = new List<string>() { "privacy", "policy"};
        PageInformation.ShowAuthentication = false;
        //PageInformation.SocialShareableUrl = new NWDSocialShareableUrl() { Title ="Share this", UrlEncoded = HttpContext.Request.GetEncodedUrl(), Kind = NWDSocialShareableKind.Html , Style = NWDSocialShareableStyle.Inline};
        // PageInformation.VisitShopNow = ShopShow.No;
        PageInformation.PageStyle = NWDPageContainer.ContainerPage;
        return View();
    }

    /// <summary>
    /// Terms of service (condition générale d'utilisation)
    /// </summary>
    /// <returns></returns>
    public IActionResult TermsOfService()
    {
        PageInformation.Title = "Terms of service";
        PageInformation.Description = "Pagination about terms of services.";
        PageInformation.Keywords = new List<string>() { "term", "service" };
        PageInformation.ShowAuthentication = false;
        //PageInformation.SocialShareableUrl = new NWDSocialShareableUrl() { Title ="Share this", UrlEncoded = HttpContext.Request.GetEncodedUrl(), Kind = NWDSocialShareableKind.Html , Style = NWDSocialShareableStyle.Carded};
        // PageInformation.VisitShopNow = ShopShow.No;
        PageInformation.PageStyle = NWDPageContainer.ContainerPage;
        return View();
    }

    /// <summary>
    /// Terms and conditions (condition générale de vente)
    /// </summary>
    /// <returns></returns>
    public IActionResult TermsAndConditions()
    {
        PageInformation.Title = "Terms and conditions";
        PageInformation.Description = "Pagination about terms and condition.";
        PageInformation.Keywords = new List<string>() { "term", "condition" };
        PageInformation.ShowAuthentication = false;
        //PageInformation.SocialShareableUrl = new NWDSocialShareableUrl() { Title ="Share this", UrlEncoded = HttpContext.Request.GetEncodedUrl(), Kind = NWDSocialShareableKind.Html , Style = NWDSocialShareableStyle.Toolbar};
        // PageInformation.VisitShopNow = ShopShow.No;
        PageInformation.PageStyle = NWDPageContainer.ContainerPage;
        return View();
    }

    /// <summary>
    /// Legal Notice (mentions légales)
    /// </summary>
    /// <returns></returns>
    public IActionResult LegalNotice()
    {
        PageInformation.Title = "Legal notice";
        PageInformation.Description = "Pagination about legal notice.";
        PageInformation.Keywords = new List<string>() { "legal", "notice" };
        PageInformation.ShowAuthentication = false;
        //PageInformation.SocialShareableUrl = new NWDSocialShareableUrl() { Title ="Share this", UrlEncoded = HttpContext.Request.GetEncodedUrl(), Kind = NWDSocialShareableKind.Html , Style = NWDSocialShareableStyle.MenuDropdown};
        // PageInformation.VisitShopNow = ShopShow.No;
        PageInformation.PageStyle = NWDPageContainer.ContainerPage;
        return View();
    }
    /// <summary>
    /// GDPR (RGPD)
    /// </summary>
    /// <returns></returns>
    public ActionResult GeneralDataProtectionRegulation()
    {
        PageInformation.Title = "General Data Protection Regulation";
        PageInformation.Description = "Pagination about General Data Protection Regulation.";
        PageInformation.Keywords = new List<string>() { "General", "Data", "Protection", "Regulation", "GDPR", "RGPD" };
        PageInformation.ShowAuthentication = false;
        // PageInformation.VisitShopNow = ShopShow.No;
        PageInformation.PageStyle = NWDPageContainer.ContainerPage;

        ViewData.Add("TraceIdentifier", HttpContext.TraceIdentifier.ToString());
        // ViewData.Add("RemoteIpAddress", AccountService.GetRemoteIPAddress(HttpContext).ToString());
        return View();
    }

    public ActionResult CookiesManagement()
    {
        PageInformation.Title = "Cookies management";
        PageInformation.Description = "Pagination about Cookies.";
        PageInformation.Keywords = new List<string>() { "cookies", "cookie", "management", "privacy" };
        PageInformation.ShowAuthentication = false;
        // PageInformation.VisitShopNow = ShopShow.No;
        PageInformation.PageStyle = NWDPageContainer.ContainerPage;

        ViewData.Add("TraceIdentifier", HttpContext.TraceIdentifier.ToString());
        // ViewData.Add("RemoteIpAddress", AccountService.GetRemoteIPAddress(HttpContext).ToString());
        return View();
    }

    // public IActionResult RevokeCookie()
    // {
    //     NWDCookieGlobal.DeleteAllCookie(HttpContext);
    //     return RedirectToAction("Index", "Home");
    // }
}