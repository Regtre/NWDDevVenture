using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWDFoundation.WebEdition.Attributes;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Controllers;
using NWDWebStandard.Models;
using NWDWebStandard.Models.Enums;

namespace NWDWebDevelopment.Controllers;

public class NWDWebHtmlLayoutController : NWDBasicController<NWDWebHtmlLayoutController>
{
    private static NWDSideBarBlock[]? KSideBarBlocks;
    public NWDSideBarBlock[]? AddSideBar()
    {
        if (KSideBarBlocks == null)
        {
            NWDSideBarBlock tBlock = new NWDSideBarBlock()
            {
                Name = "Layout",
                BadgeText = "Important",
                Categories = new List<NWDSideBarCategory>()
                {
                    new NWDSideBarCategory()
                    {
                        Name = "Layout by grid",
                        IconStyle = "fas fa-th",
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "Grid", ActionName = nameof(Index), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                        }
                    },

                    new NWDSideBarCategory()
                    {
                        Name = "Title",
                        IconStyle = "fas fa-heading",
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "Title", ActionName = nameof(TitleBasic), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Title with take away", ActionName = nameof(TitleTakeAway), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Title with menu", ActionName = nameof(TitleWithMenu), ControllerName = ASP_Controller(), UrlParameter = ""
                            },
                        }
                    },

                    new NWDSideBarCategory()
                    {
                        Name = "Card",
                        IconStyle = "fas fa-th-list",
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "Card", ActionName = nameof(CardBasic), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Card with take away", ActionName = nameof(CardTakeAway), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Card with menu", ActionName = nameof(CardWithMenu), ControllerName = ASP_Controller(), UrlParameter = ""
                            },
                        }
                    },
                    
                    new NWDSideBarCategory()
                    {
                        Name = "Form",
                        IconStyle = "fas fa-th-list",
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "Sign form", ActionName = nameof(SignForm), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            // new NWDSideBarElement()
                            // {
                            //     Name = "Card with take away", ActionName = nameof(CardTakeAway), ControllerName = ASP_Controller(), UrlParameter = "",
                            // },
                            // new NWDSideBarElement()
                            // {
                            //     Name = "Card with menu", ActionName = nameof(CardWithMenu), ControllerName = ASP_Controller(), UrlParameter = ""
                            // },
                        }
                    },
                    new NWDSideBarCategory()
                    {
                        Name = "Dynamic",
                        IconStyle = "fas fa-fire",
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "TextEffects", ActionName = nameof(TextEffects), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Graphs", ActionName = nameof(Graphs), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            // new NWDSideBarElement()
                            // {
                            //     Name = "Material Icons", ActionName = nameof(IconMaterialIcons), ControllerName = ASP_Controller(), UrlParameter = ""
                            // },
                        }
                    },
                    new NWDSideBarCategory()
                    {
                        Name = "Shop",
                        IconStyle = "fas fa-shopping-cart",
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "Shop services", ActionName = nameof(ShopServices), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Shop cart", ActionName = nameof(ShopCart), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Shop paiment", ActionName = nameof(ShopPayment), ControllerName = ASP_Controller(), UrlParameter = ""
                            },
                        }
                    },
                    new NWDSideBarCategory()
                    {
                        Name = "Icons",
                        IconStyle = "far fa-dizzy",
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "Bootstrap", ActionName = nameof(IconBootstrap), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Font Awesome", ActionName = nameof(IconFontAwesome), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Material Icons", ActionName = nameof(IconMaterialIcons), ControllerName = ASP_Controller(), UrlParameter = ""
                            },
                        }
                    },
                    new NWDSideBarCategory()
                    {
                        Name = "Shareable",
                        IconStyle = "fas fa-share-alt",
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "Example", ActionName = nameof(Shareable), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                        }
                    },
                }
            };

            NWDSideBarBlock tBlockSideBar = new NWDSideBarBlock()
            {
                Name = "SideBar",
                Categories = new List<NWDSideBarCategory>()
                {
                    new NWDSideBarCategory()
                    {
                        Name = "SideBar menus", 
                        IconStyle = "fas fa-bars",
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "General", ActionName = nameof(SideBarGeneral), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Configuration", ActionName = nameof(SideBarConfiguration), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Controller", ActionName = nameof(SideBarController), ControllerName = ASP_Controller(), UrlParameter = ""
                            },
                        }
                    },
                    new NWDSideBarCategory()
                    {
                        Name = "NavBar menu",
                        IconStyle = "fas fa-tv", //fas fa-map-signs
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "General", ActionName = nameof(NavBarGeneral), ControllerName = ASP_Controller(), UrlParameter = ""
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Configuration", ActionName = nameof(NavBarConfiguration), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Controller", ActionName = nameof(NavBarController), ControllerName = ASP_Controller(), UrlParameter = ""
                            },
                        }
                    },
                    new NWDSideBarCategory()
                    {
                        Name = "NavFooter area",
                        IconStyle = "fas fa-university",
                        Elements = new List<NWDSideBarElement>()
                        {
                            new NWDSideBarElement()
                            {
                                Name = "General", ActionName = nameof(NavFooterGeneral), ControllerName = ASP_Controller(), UrlParameter = ""
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Configuration", ActionName = nameof(NavFooterConfiguration), ControllerName = ASP_Controller(), UrlParameter = "",
                            },
                            new NWDSideBarElement()
                            {
                                Name = "Controller", ActionName = nameof(NavFooterController), ControllerName = ASP_Controller(), UrlParameter = ""
                            },
                        }
                    }
                }
            };
            KSideBarBlocks = new []{tBlock, tBlockSideBar};
        }
        return KSideBarBlocks;
    }

    public override void OnActionExecuting(ActionExecutingContext sContext)
    {
        base.OnActionExecuting(sContext);
        //PageInformation.SideBarStyle = NWDSideBarKind.Tools;
        PageInformation.SetSideBarKind(NWDSideBarKind.Tools, AddSideBar(), null,HttpContext);
        PageInformation.CssPathAddonList.Add("//cdnjs.cloudflare.com/ajax/libs/highlight.js/11.7.0/styles/monokai-sublime.min.css");
        PageInformation.JavascriptPathAddonList.Add("//cdnjs.cloudflare.com/ajax/libs/highlight.js/11.7.0/highlight.min.js");
        PageInformation.JavascriptCodeAtEndOfPage.Add("hljs.highlightAll();");
    }

    [NWDWebMethodTestExclude()]
    public IActionResult Index()
    {
            return View();
    }
    
    [NWDWebMethodTestExclude()]
    public IActionResult TitleBasic()
    {
            return View();
    }
    
    [NWDWebMethodTestExclude()]
    public IActionResult TitleTakeAway()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult TitleWithMenu()
    {
        return View();
    }
    
    [NWDWebMethodTestExclude()]
    public IActionResult CardBasic()
    {
        return View();
    }
    
    [NWDWebMethodTestExclude()]
    public IActionResult CardTakeAway()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult CardWithMenu()
    {
        return View();
    }
    
    [NWDWebMethodTestExclude()]
    public IActionResult SideBarGeneral()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult SideBarConfiguration()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult SideBarController()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult NavBarGeneral()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult NavBarConfiguration()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult NavBarController()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult NavFooterGeneral()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult NavFooterConfiguration()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult NavFooterController()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult IconBootstrap()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult IconFontAwesome()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult IconMaterialIcons()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult SignForm()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult TextEffects()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult ShopServices()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult ShopCart()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult ShopPayment()
    {
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult Shareable(NWDSocialShareableStyle sStyle = NWDSocialShareableStyle.Inherited)
    {
        PageInformation.SocialShareableUrl = new NWDSocialShareableUrl("Partage cette page", "https://this url", NWDSocialShareableKind.Html, sStyle);
        return View();
    }
    [NWDWebMethodTestExclude()]
    public IActionResult Graphs()
    {PageInformation.JavascriptPathAddonList.Add("/vendors/chart/chart.min.js");
        NWDStatisticsGrid tGrid = new NWDStatisticsGrid();
        AddViewDataObject(tGrid);
        PageInformation.Title = "Statistiques du site web";
        PageInformation.Keywords.AddRange(new List<string>() {
            "Statistics",
        });
        // string tUserAgent = HttpContext.Request.Headers["User-Agent"];
        // Parser tParser = Parser.GetDefault();
        // ClientInfo tClientInfo = tParser.Parse(tUserAgent);
        // ViewData["ClientInfo"] = tClientInfo;
        tGrid.Add("Operating system", "", NWDStatisticsConsolidated.GenerateBarChartWithGroup("Operating system", NWDStatisticsConsolidated.K_OS, DateTime.Now, NWDStatisticsConsolidateRange.ThisDate, -7));
        tGrid.Add("Device", "", NWDStatisticsConsolidated.GenerateBarChartWithGroup("Devices", NWDStatisticsConsolidated.K_DEVICE, DateTime.Now, NWDStatisticsConsolidateRange.Month, -3));
        tGrid.Add("Browser", "",NWDStatisticsConsolidated.GenerateBarChartWithGroup("Browser", NWDStatisticsConsolidated.K_BROWSER, DateTime.Now, NWDStatisticsConsolidateRange.Month, -3));
        tGrid.Add("Sessions this date","",NWDStatisticsConsolidated.GenerateBarChartWithGroup("Distinct users", NWDStatisticsConsolidated.K_SESSION, DateTime.Now, NWDStatisticsConsolidateRange.ThisDate, -7));
        tGrid.Add("Sessions this hour", "", NWDStatisticsConsolidated.GenerateBarChartWithGroup("Distinct users", NWDStatisticsConsolidated.K_SESSION, DateTime.Now, NWDStatisticsConsolidateRange.ThisHour, -24));
        return View();
    }
}