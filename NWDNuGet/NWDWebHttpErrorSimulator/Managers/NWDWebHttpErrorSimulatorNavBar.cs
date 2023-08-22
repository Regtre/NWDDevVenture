using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDWebHttpErrorSimulator.Controllers;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebHttpErrorSimulator.Services;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Configuration;
using NWDWebStandard.Controllers;

namespace NWDWebHttpErrorSimulator.Managers
{
    public class NWDWebHttpErrorSimulatorNavBar : INWDNavBar
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
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarApp(HttpContext sHttpContext)
        {
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarDebug(HttpContext sHttpContext)
        {
            NWDNavBarCategory tReturnOne = new NWDNavBarCategory()
            {
                Name = "Http Simulator",
                IconStyle = "fas fa-bug",
                Elements = new List<NWDNavBarElement>()
                {
                    new NWDNavBarElement()
                    {
                        Name = "List controllers", ActionName = nameof(NWDWebsiteMapController.Index), ControllerName = NWDWebsiteMapController.ASP_Controller(), UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "All status code", ActionName = nameof(NWDWebsiteMapController.All), ControllerName = NWDWebsiteMapController.ASP_Controller(), UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "Unit status code", ActionName = nameof(NWDWebsiteMapController.UnitTest), ControllerName = NWDWebsiteMapController.ASP_Controller(), UrlParameter = "sControllerName="+NWDWebsiteMapController.ASP_Controller(),
                    },
                }
            };
            return new[] {tReturnOne };
        }
    }
}