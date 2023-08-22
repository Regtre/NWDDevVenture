using Microsoft.AspNetCore.Http;
using NWDWebDownloader.Configuration;
using NWDWebDownloader.Controllers;
using NWDWebDownloader.Models;
using NWDWebRuntime.Models;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models.Enums;

namespace NWDWebDownloader.Managers
{
    public class NWDWebDownloaderNavBar : INWDNavBar
    {
        public NWDNavBarMenu[]? AddNavBarMenu(NWDNavBarKind sNavBarKind, HttpContext sHttpContext)
        {
            List<NWDNavBarElement> tElements = new List<NWDNavBarElement>();
            List<NWDNavBarElement> tElementsAllowAnonymous = new List<NWDNavBarElement>();

            foreach (NWDDownloadConfig tConfig in NWDWebDownloaderConfiguration.KConfig.Downloads)
            {
                if (tConfig.AllowAnonymous || (tConfig.AllowConnected && NWDAuthorizeByAuthentication.ValidFor(sHttpContext, true) || (NWDAuthorizeByAllOfServices.ValidFor(sHttpContext, tConfig.Service ))))
                {
                    if (tConfig.AllowAnonymous)
                    {
                        tElementsAllowAnonymous.Add(new NWDNavBarElement()
                        {
                            Name = tConfig.PageName,
                            ActionName = nameof(NWDWebDownloaderController.Download),
                            ControllerName = NWDWebDownloaderController.ASP_Controller(),
                            UrlParameter = "sName=" + tConfig.PageName,
                        });
                    }
                    else
                    {
                        tElements.Add(new NWDNavBarElement()
                        {
                            Name = tConfig.PageName,
                            ActionName = nameof(NWDWebDownloaderController.Download),
                            ControllerName = NWDWebDownloaderController.ASP_Controller(),
                            UrlParameter = "sName=" + tConfig.PageName,
                        });
                    }
                }
            }

            if (tElements.Count > 0 || tElementsAllowAnonymous.Count > 0)
            {
                NWDNavBarMenu tMenu = new NWDNavBarMenu()
                {
                    Name = "Downloads",
                    Categories = new List<NWDNavBarCategory>()
                };
                if (tElements.Count > 0)
                {
                    tMenu.Categories.Add(new NWDNavBarCategory()
                    {
                        Name = "Downloads available",
                        IconStyle = "fas fa-file-download",
                        Elements = tElements
                    });
                }

                if (tElementsAllowAnonymous.Count > 0)
                {
                    tMenu.Categories.Add(new NWDNavBarCategory()
                    {
                        Name = "Downloads for all",
                        IconStyle = "fas fa-file-download",
                        Elements = tElementsAllowAnonymous
                    });
                }

                return new[] { tMenu };
            }

            return null;
        }

        public NWDNavBarMenu[]? AddNavBarAccount(HttpContext sHttpContext)
        {
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarAdmin(HttpContext sHttpContext)
        {
            if (NWDAuthorizeAdminOnly.ValidFor(sHttpContext))
            {
                NWDNavBarCategory tNavBarCategoryNew = new NWDNavBarCategory()
                {
                    Name = "Downloads",
                    IconStyle = "fas fa-file-download",
                    Elements = new List<NWDNavBarElement>()
                    {
                        new NWDNavBarElement()
                        {
                            Name = "Statistics",
                            ActionName = nameof(NWDWebDownloaderController.Statistics),
                            ControllerName = NWDWebDownloaderController.ASP_Controller(),
                            UrlParameter = "",
                        }
                    }
                };
            return new[] { tNavBarCategoryNew };
            }

            return null;
        }

        public NWDNavBarCategory[]? AddNavBarApp(HttpContext sHttpContext)
        {
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarDebug(HttpContext sHttpContext)
        {
            return null;
        }
    }
}