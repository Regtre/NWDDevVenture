using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NWDWebRuntime.Models;
using NWDWebStandard.Resources;

namespace NWDWebStandard.Controllers
{
    public class NWDHomeController : NWDBasicController<NWDHomeController>
    {
        private readonly ILogger<NWDHomeController> _logger;


        public NWDHomeController(ILogger<NWDHomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult About()
        {
            PageInformation.SocialShareableUrl = new NWDSocialShareableUrl(" essai", "https://www.dicolatin.com", NWDSocialShareableKind.Html, NWDSocialShareableStyle.MenuDropdown);
            PageInformation.Title = "Logo";
            PageInformation.Keywords.AddRange(new List<string>() {
                "logo",
            });
            PageInformation.ShowAuthentication = false;
            // PageInformation.VisitShopNow = ShopShow.No;
            return View();
        }
        public IActionResult Logos()
        {
            PageInformation.SocialShareableUrl = new NWDSocialShareableUrl(" Logo", "https://www.dicolatin.com", NWDSocialShareableKind.Html, NWDSocialShareableStyle.Toolbar);
            PageInformation.Title = "Logo";
            PageInformation.Keywords.AddRange(new List<string>() {
                "logo",
            });
            PageInformation.ShowAuthentication = false;
            // PageInformation.VisitShopNow = ShopShow.No;
            return View();
        }
    }
}
