using Microsoft.AspNetCore.Http;
using NWDFoundation.Models.Enums;
using NWDFoundation.WebEdition.Enums;
using NWDWebEditor.Controllers;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Controllers;

namespace NWDWebEditor.Managers
{
    public class NWDWebEditorNavBar : INWDNavBar
    {
        public NWDNavBarMenu[]? AddNavBarMenu(NWDNavBarKind sNavBarKind, HttpContext sHttpContext)
        {
            return null;
            NWDNavBarMenu tMenu = new NWDNavBarMenu()
            {
                Name ="Un",
                Categories = new List<NWDNavBarCategory>()
                {
                    new NWDNavBarCategory()
                    {
                        Name = "Dashboard",
                        IconStyle = "far fa-comment-alt",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Information", ActionName = "Show", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Instructions", ActionName = "Instructions", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Roles", ActionName = "Roles", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Environments", ActionName = "Environments", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Custom classes", ActionName = "CustomClasses", ControllerName = "NWDProject"
                            }
                        }
                    },
                }
            };
            NWDNavBarMenu tMenuB = new NWDNavBarMenu()
            {
                Name ="test",
                Description = "Essi de description pour voir ce que cela donne",
                Categories = new List<NWDNavBarCategory>()
                {
                    new NWDNavBarCategory()
                    {
                        Name = "Dashboard",
                        IconStyle = "far fa-comment-alt",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Information", ActionName = "Show", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Instructions", ActionName = "Instructions", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Roles", ActionName = "Roles", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Environments", ActionName = "Environments", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Custom classes", ActionName = "CustomClasses", ControllerName = "NWDProject"
                            }
                        }
                    },
                    new NWDNavBarCategory()
                    {
                        Name = "Billing",
                        IconStyle = "far fa-file-alt", BadgeText = "test",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Estimates", ActionName = "Estimates", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Billings", ActionName = "Billings", ControllerName = "NWDProject"
                            },
                        }
                    },
                    new NWDNavBarCategory()
                    {
                        Name = "Other actions",
                        IconStyle = "fas fa-tools",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Publish", ActionName = "Publish", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "DeleteInstructions", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                        }
                    },
                    new NWDNavBarCategory()
                    {
                        Name = "Your Website",
                        IconStyle = "fas fa-tools",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Generate CSProj", ActionName = "Publish", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Install your website", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Use our Yaml (gitlab)", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                        }
                    },
                    new NWDNavBarCategory()
                    {
                        Name = "Your Website",
                        IconStyle = "fas fa-tools",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Generate CSProj", ActionName = "Publish", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Install your website", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Use our Yaml (gitlab)", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                        }
                    },
                    new NWDNavBarCategory()
                    {
                        Name = "Your Website",
                        IconStyle = "fas fa-tools",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Generate CSProj", ActionName = "Publish", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Install your website", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Use our Yaml (gitlab)", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                        }
                    },
                    new NWDNavBarCategory()
                    {
                        Name = "Your Website",
                        IconStyle = "fas fa-tools",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Generate CSProj", ActionName = "Publish", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Install your website", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Use our Yaml (gitlab)", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                        }
                    },
                    new NWDNavBarCategory()
                    {
                        Name = "Your Website",
                        IconStyle = "fas fa-tools",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Generate CSProj", ActionName = "Publish", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Install your website", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Use our Yaml (gitlab)", ActionName = "Delete", ControllerName = "NWDProject"
                            },
                        }
                    }
                }
            };
            NWDNavBarMenu tMenuC = new NWDNavBarMenu()
            {
                Name ="Whaooo",
                Categories = new List<NWDNavBarCategory>()
                {
                    new NWDNavBarCategory()
                    {
                        Name = "Dashboard",
                        IconStyle = "far fa-comment-alt",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Information", ActionName = "Show", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Instructions", ActionName = "Instructions", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Roles", ActionName = "Roles", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Environments", ActionName = "Environments", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Custom classes", ActionName = "CustomClasses", ControllerName = "NWDProject"
                            }
                        }
                    },
                    new NWDNavBarCategory()
                    {
                        Name = "Billing",
                        IconStyle = "far fa-file-alt", BadgeText = "test",
                        Elements = new List<NWDNavBarElement>()
                        {
                            new NWDNavBarElement()
                            {
                                Name = "Estimates", ActionName = "Estimates", ControllerName = "NWDProject"
                            },
                            new NWDNavBarElement()
                            {
                                Name = "Billings", ActionName = "Billings", ControllerName = "NWDProject"
                            },
                        }
                    },
                }
            };
            return new[] { tMenu, tMenuB, tMenuC };
        }
        public NWDNavBarMenu[]? AddNavBarAccount(HttpContext sHttpContext)
        {
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarAdmin(HttpContext sHttpContext)
        {
            string? tController = sHttpContext.Request.RouteValues["controller"]?.ToString();
            string? tAction = sHttpContext.Request.RouteValues["action"]?.ToString();
            return new []{new NWDNavBarCategory()
            {
                Name = "CMSystem",
                IconStyle = "fas fa-tools",
                Elements = new List<NWDNavBarElement>()
                {
                    new NWDNavBarElement()
                    {
                        Name = "Pages management",Icon="fas fa-shield-alt", ActionName = nameof(NWDPageEditorController.Index), ControllerName = "NWDPageEditor"
                    },
                    new NWDNavBarElement()
                    {
                        Name = "FAQ management",Icon="fas fa-database", ActionName = nameof(NWDFrequentlyAskedQuestionController.Index), ControllerName = "NWDFrequentlyAskedQuestion"
                    },
                    new NWDNavBarElement()
                    {
                        Name = "FAQ add one here",Icon="fas fa-cookie", ActionName = nameof(NWDFrequentlyAskedQuestionController.NewHere), ControllerName = "NWDFrequentlyAskedQuestion", UrlParameter = "sController="+tController+"&sAction="+tAction
                    },
                }
            }
            };
        }

        public NWDNavBarCategory[]? AddNavBarApp(HttpContext sHttpContext)
        {
            return null;
            return new []{new NWDNavBarCategory()
                {
                    Name = "Dev app",
                    IconStyle = "fas fa-tools",
                    Elements = new List<NWDNavBarElement>()
                    {
                        new NWDNavBarElement()
                        {
                            Name = "Privacy policy",Icon="fas fa-shield-alt", ActionName = nameof(NWDPrivacyController.PrivacyPolicy), ControllerName = NWDPrivacyController.ASP_Controller()
                        },
                        new NWDNavBarElement()
                        {
                            Name = "GDPR",Icon="fas fa-database", ActionName = nameof(NWDPrivacyController.GeneralDataProtectionRegulation), ControllerName = NWDPrivacyController.ASP_Controller()
                        },
                        new NWDNavBarElement()
                        {
                            Name = "Cookies management",Icon="fas fa-cookie", ActionName = nameof(NWDPrivacyController.CookiesManagement), ControllerName = NWDPrivacyController.ASP_Controller()
                        },
                    }
                }
            };
        }

        public NWDNavBarCategory[]? AddNavBarDebug(HttpContext sHttpContext)
        {
            return null;
            NWDNavBarCategory tReturn = new NWDNavBarCategory()
            {
                Name = "Net-Worked-Data",
                Description = "Version " + NWDWebRuntime.NWDVersionDll.Version + ", Project Id " + NWDWebRuntimeConfiguration.KConfig.GetProjectId()+", Environment " + NWDWebRuntimeConfiguration.KConfig.MyEnvironment.ToString()+ ", Debug mode is " +NWDWebRuntimeConfiguration.KConfig.IsDevelopment.ToString(),
                IconStyle = "fas fa-cube", Elements = new List<NWDNavBarElement>()
                {
                    new NWDNavBarElement()
                                     {
                                         Name = "Go to hub", ActionName = "", ControllerName = "", UrlParameter = "https://"+NWDWebRuntimeConfiguration.KConfig.GetHubDnsHttps(),
                                     },
                    new NWDNavBarElement()
                    {
                        Name = "Show modules", ActionName = nameof(NWDWebRuntimeController.Index), ControllerName = "NWDWebRuntime", UrlParameter = "",
                    },
                }
                
            };
            if (NWDAuthorizeAdminOnly.ValidFor(sHttpContext))
            {
                tReturn.Elements.Add(new NWDNavBarElement()
                {
                    Name = "You're admin", ActionName = nameof(NWDAccountController.ServicesList), ControllerName = NWDAccountController.ASP_Controller(), UrlParameter = "",
                });
            }
            
            tReturn.Elements.Add(new NWDNavBarElement()
            {
                Name = "Project license is "+NWDWebRuntimeConfiguration.KConfig.LicenseValid.ToString().ToLower(), ActionName = "", ControllerName = "", UrlParameter = "",
            });
            switch(NWDWebRuntimeConfiguration.KConfig.NeedUpdate)
            {
                case NWDNeedUpdate.Unknow :
                    tReturn.Elements.Add(new NWDNavBarElement()
                    {
                        Name = "Update is "+NWDNeedUpdate.Unknow.ToString().ToLower(), ActionName = "", ControllerName = "", UrlParameter = "",
                    });
                break;
                case NWDNeedUpdate.Update :
                    tReturn.Elements.Add(new NWDNavBarElement()
                        {
                            Name = "Up to date"+NWDNeedUpdate.Unknow.ToString().ToLower(), ActionName = "", ControllerName = "", UrlParameter = "", BadgeText = "ok", BadgeStyle = NWDBootstrapKindOfStyle.Success
                        });
                break;
                case NWDNeedUpdate.Upgrade :
                    tReturn.Elements.Add(new NWDNavBarElement()
                        {
                            Name = "Upgrade"+NWDNeedUpdate.Unknow.ToString().ToLower(), ActionName = "", ControllerName = "", UrlParameter = "", BadgeText = "available", BadgeStyle = NWDBootstrapKindOfStyle.Warning
                        });
                break;
                case NWDNeedUpdate.UpgradeNow :
                    tReturn.Elements.Add(new NWDNavBarElement()
                    {
                        Name = "Upgrade"+NWDNeedUpdate.Unknow.ToString().ToLower(), ActionName = "", ControllerName = "", UrlParameter = "", BadgeText = "now!", BadgeStyle = NWDBootstrapKindOfStyle.Danger
                    });
                break;
            }
            return new[] { tReturn };
        }
    }
}