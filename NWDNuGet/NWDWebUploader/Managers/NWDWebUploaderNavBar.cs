using Microsoft.AspNetCore.Http;
using NWDWebDownloader.Configuration;
using NWDWebUploader.Controllers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models.Enums;

namespace NWDWebUploader.Managers
{
    public class NWDWebUploaderNavBar : INWDNavBar
    {
        public NWDNavBarMenu[]? AddNavBarMenu(NWDNavBarKind sNavBarKind, HttpContext sHttpContext)
        {
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
                                ActionName = nameof(NWDWebUploaderController.Upload),
                                ControllerName = NWDWebUploaderController.ASP_Controller(),
                                UrlParameter = "sDownloadPageNameFor=" + tConfig.PageName,
                            });
                        }
                        else
                        {
                            tElements.Add(new NWDNavBarElement()
                            {
                                Name = tConfig.PageName,
                                ActionName = nameof(NWDWebUploaderController.Upload),
                                ControllerName = NWDWebUploaderController.ASP_Controller(),
                                UrlParameter = "sDownloadPageNameFor=" + tConfig.PageName,
                            });
                        }
                    }
                }

                if (tElements.Count > 0 || tElementsAllowAnonymous.Count > 0)
                {
                    NWDNavBarMenu tMenu = new NWDNavBarMenu()
                    {
                        Name = "Uploads",
                        Categories = new List<NWDNavBarCategory>()
                    };
                    if (tElements.Count > 0)
                    {
                        tMenu.Categories.Add(new NWDNavBarCategory()
                        {
                            Name = "Uploads by service",
                            IconStyle = "fas fa-file-upload",
                            Elements = tElements
                        });
                    }

                    if (tElementsAllowAnonymous.Count > 0)
                    {
                        tMenu.Categories.Add(new NWDNavBarCategory()
                        {
                            Name = "Uploads for all",
                            IconStyle = "fas fa-file-upload",
                            Elements = tElementsAllowAnonymous
                        });
                    }

                    return tMenu.Categories.ToArray();
                }

                return null;
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