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
using NWDWebDevelopment.Controllers;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebDevelopment.Services;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Configuration;
using NWDWebStandard.Controllers;

namespace NWDWebDevelopment.Managers
{
    public class NWDWebDevelopmentNavBar : INWDNavBar
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
                Name = "Model edition demo",
                IconStyle = "fas fa-couch",
                Elements = new List<NWDNavBarElement>()
                {
                    new NWDNavBarElement()
                    {
                        Name = "NWD Data Player Demo", ActionName = nameof(NWDNetWorkedDataPlayerDemoEditionController.Index), ControllerName = "NWDNetWorkedDataPlayerDemoEdition", UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "NWD Data Web Demo", ActionName = nameof(NWDDataWebDemoEditionController.Index), ControllerName = "NWDDataWebDemoEdition", UrlParameter = "",
                    },
                }
            };
            NWDNavBarCategory tReturnTwo = new NWDNavBarCategory()
            {
                Name = "Development",
                IconStyle = "fas fa-couch",
                Elements = new List<NWDNavBarElement>()
                {
                    new NWDNavBarElement()
                    {
                        Name = "SetUp", ActionName = nameof(NWDSetUpController.Index), ControllerName = NWDSetUpController.ASP_Controller(), UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "Email test", ActionName = nameof(NWDEmailConfigurationController.Index), ControllerName = NWDEmailConfigurationController.ASP_Controller(), UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "Logger test", ActionName = nameof(NWDWebDevelopmentController.LoggerTest), ControllerName = NWDWebDevelopmentController.ASP_Controller(), UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "Convention", ActionName = nameof(NWDWebDevelopmentController.Convention), ControllerName = NWDWebDevelopmentController.ASP_Controller(), UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "AppSetting", ActionName = nameof(NWDWebDevelopmentController.Appsettings), ControllerName = NWDWebDevelopmentController.ASP_Controller(), UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "HtmlLayout", ActionName = nameof(NWDWebHtmlLayoutController.Index), ControllerName = NWDWebHtmlLayoutController.ASP_Controller(), UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "No Menu", ActionName = nameof(NWDWebDevelopmentController.NoMenu), ControllerName = NWDWebDevelopmentController.ASP_Controller(), UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "Error list", ActionName = "List", ControllerName = "Error", UrlParameter = "",
                    },
                    new NWDNavBarElement()
                    {
                        Name = "Error 404", ActionName = "FakeAction", ControllerName = "Error", UrlParameter = "",
                    },
                }
            };
            return new[] {tReturnOne, tReturnTwo };
        }
    }
}