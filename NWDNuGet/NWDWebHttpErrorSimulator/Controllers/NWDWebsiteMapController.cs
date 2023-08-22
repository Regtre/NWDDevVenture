using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWDFoundation.Configuration;
using NWDFoundation.WebEdition.Attributes;
using NWDWebHttpErrorSimulator.Models;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using Microsoft.AspNetCore.Http;
using NWDFoundation.Logger;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Configuration;
using NWDWebStandard.Controllers;

namespace NWDWebHttpErrorSimulator.Controllers
{
    public class NWDWebsiteMapController : NWDBasicController<NWDWebsiteMapController>
    {
        static List<NWDWebsiteMapLibrary> WebsiteMapControllerList = new List<NWDWebsiteMapLibrary>();
        private static bool Installed = false;

        static void Install(HttpContext sHttpContext)
        {
            if (Installed == false)
            {
                Installed = true;
                foreach (NWDLibraryInfos tLibrary in NWDLibrariesInstalled.LibrariesInfoList)
                {
                    if (tLibrary.Information != null)
                    {
                        if (tLibrary.AssemblyDll != null)
                        {
                            NWDWebsiteMapLibrary tWebsiteMapLibrary = new NWDWebsiteMapLibrary();
                            if (string.IsNullOrEmpty(tLibrary.Information.ProductName) == false)
                            {
                                tWebsiteMapLibrary.LibraryName = tLibrary.Information.ProductName;
                            }

                            foreach (Type tType in tLibrary.AssemblyDll.GetTypes().Where(sType => sType.IsClass && !sType.IsAbstract && sType.IsSubclassOf(typeof(NWDRawController))))
                            {
                                NWDWebsiteMapClass tWebsiteMapClass = new NWDWebsiteMapClass();
                                string tControllerName = tType.Name.Replace("Controller", "");
                                tWebsiteMapClass.ControllerName = tControllerName;

                                // Did you really mean to prohibit public methods? I assume not
                                var tMethodInfo = tType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                                IEnumerable<MethodInfo> tMethodList = tMethodInfo.Where(sMethodInfo => sMethodInfo.ReturnType.IsAssignableFrom(typeof(ActionResult)));
                                foreach (MethodInfo tMethodName in tMethodList)
                                {
                                    bool tExclude = false;
                                    foreach (NWDWebMethodTestExcludeAttribute tAttributExclude in tMethodName.GetCustomAttributes<NWDWebMethodTestExcludeAttribute>())
                                    {
                                        tExclude = true;
                                    }

                                    NWDWebsiteMapAction tWebsiteMapAction = new NWDWebsiteMapAction();
                                    if (tExclude == false)
                                    {
                                        bool tHasAlreadyOne = false;
                                                foreach (NWDWebMethodTestAttribute tAttribut in tMethodName.GetCustomAttributes<NWDWebMethodTestAttribute>())
                                                {
                                                    tHasAlreadyOne = true;
                                                    tWebsiteMapAction.RequestList.Add(new NWDWebsiteMapRequest()
                                                    {
                                                        ControllerName = tControllerName,
                                                        ActionName = tMethodName.Name,
                                                        ReturnTypeName = nameof(ActionResult),
                                                        GetParams = string.Empty,
                                                        PostLinearized = string.Empty,
                                                        Expected = tAttribut.GetExpected(),
                                                        StatusTag = tAttribut.GetTag(),
                                                    });
                                                }
                                        if (tMethodName.Name != "get_ViewBag" && tMethodName.Name != "ValidationProblem")
                                        {
                                            if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment && tHasAlreadyOne == false)
                                            {
                                                tWebsiteMapAction.RequestList.Add(new NWDWebsiteMapRequest()
                                                {
                                                    ControllerName = tControllerName,
                                                    ActionName = tMethodName.Name,
                                                    ReturnTypeName = nameof(ActionResult),
                                                    GetParams = string.Empty,
                                                    PostLinearized = string.Empty,
                                                    Expected = 200,
                                                    StatusTag = NWDPageStandardStatusTag.None,
                                                });
                                            }
                                        }
                                    }

                                    foreach (NWDWebMethodTestGetAttribute tAttribut in tMethodName.GetCustomAttributes<NWDWebMethodTestGetAttribute>())
                                    {
                                        tWebsiteMapAction.RequestList.Add(new NWDWebsiteMapRequest()
                                        {
                                            ControllerName = tControllerName,
                                            ActionName = tMethodName.Name,
                                            ReturnTypeName = nameof(ActionResult),
                                            GetParams = tAttribut.GetLinearizedParams(),
                                            PostLinearized = string.Empty,
                                            Expected = tAttribut.GetExpected(),
                                            StatusTag = tAttribut.GetTag(),
                                        });
                                    }

                                    foreach (NWDWebMethodTestPostAttribute tAttribut in tMethodName.GetCustomAttributes<NWDWebMethodTestPostAttribute>())
                                    {
                                        tWebsiteMapAction.RequestList.Add(new NWDWebsiteMapRequest()
                                        {
                                            ControllerName = tControllerName,
                                            ActionName = tMethodName.Name,
                                            ReturnTypeName = nameof(ActionResult),
                                            GetParams = string.Empty,
                                            PostLinearized = tAttribut.PostLinearizedJson(),
                                            Expected = tAttribut.GetExpected(),
                                            StatusTag = tAttribut.GetTag(),
                                        });
                                    }

                                    if (tWebsiteMapAction.RequestList.Count() > 0)
                                    {
                                        tWebsiteMapAction.RequestList.Sort((sElementA, sElementB) => String.Compare(sElementA.ActionName, sElementB.ActionName, StringComparison.Ordinal));
                                        tWebsiteMapClass.ActionList.Add(tWebsiteMapAction);
                                    }
                                }

                                if (tWebsiteMapClass.ActionList.Count() > 0)
                                {
                                    tWebsiteMapClass.ActionList.Sort((sElementA, sElementB) => String.Compare(sElementA.ActionName, sElementB.ActionName, StringComparison.Ordinal));
                                    tWebsiteMapLibrary.ClassList.Add(tWebsiteMapClass);
                                }
                            }

                            if (tWebsiteMapLibrary.ClassList.Count() > 0)
                            {
                                WebsiteMapControllerList.Add(tWebsiteMapLibrary);
                            }
                        }
                        else
                        {
                            NWDLogger.Warning("error : AssemblyDll is null!");
                        }
                    }
                }
            }
        }

        [NWDAuthorizeAdminOnlyInRelease()]
        public IActionResult Index()
        {
            Install(HttpContext);
            NWDSideBar tSideBar = new NWDSideBar();
            NWDSideBarBlock tSideBarBlock = new NWDSideBarBlock();
            tSideBarBlock.Name = "Controller in project";
            tSideBar.Blocks.Add(tSideBarBlock);
            foreach (NWDWebsiteMapLibrary tWebsiteMapLibrary in WebsiteMapControllerList)
            {
                NWDSideBarCategory tSideBarCategory = new NWDSideBarCategory();
                tSideBarCategory.Name = tWebsiteMapLibrary.LibraryName;
                tSideBarCategory.AlwaysShow = true;
                tSideBarBlock.Categories.Add(tSideBarCategory);
                foreach (NWDWebsiteMapClass tWebsiteMap in tWebsiteMapLibrary.ClassList)
                {
                    NWDSideBarElement tElement = new NWDSideBarElement();
                    tElement.Name = tWebsiteMap.ControllerName;
                    tElement.ControllerName = NWDWebsiteMapController.ASP_Controller();
                    tElement.ActionName = nameof(NWDWebsiteMapController.UnitTest);
                    tElement.UrlParameter = "sControllerName=" + tWebsiteMap.ControllerName;
                    tSideBarCategory.Elements.Add(tElement);
                }
                // tSideBarCategory.Elements.Sort((sSideBarElement, sElement) => String.Compare(sElement.Name, sSideBarElement.Name, StringComparison.Ordinal));
            }
            PageInformation.AddSideBar(tSideBar.Blocks.ToArray(), null, HttpContext);
            return View();
        }
        
        [NWDAuthorizeAdminOnlyInRelease()]
        public IActionResult All()
        {
            Install(HttpContext);
            //PageInformation.SideBarStyle = NWDSideBarKind.Tools;
            List<NWDLibraryInfos> tFileVersionInfoList = NWDLibrariesInstalled.GetFileVersionInfoList();
            ViewData.Add(nameof(NWDLibraryInfos), tFileVersionInfoList.ToArray());
            NWDSideBar tSideBar = new NWDSideBar();
            NWDSideBarBlock tSideBarBlock = new NWDSideBarBlock();
            tSideBarBlock.Name = "Controller in project";
            tSideBar.Blocks.Add(tSideBarBlock);
            foreach (NWDWebsiteMapLibrary tWebsiteMapLibrary in WebsiteMapControllerList)
            {
                NWDSideBarCategory tSideBarCategory = new NWDSideBarCategory();
                tSideBarCategory.Name = tWebsiteMapLibrary.LibraryName;
                tSideBarCategory.AlwaysShow = true;
                tSideBarBlock.Categories.Add(tSideBarCategory);
                foreach (NWDWebsiteMapClass tWebsiteMap in tWebsiteMapLibrary.ClassList)
                {
                    NWDSideBarElement tElement = new NWDSideBarElement();
                    tElement.Name = tWebsiteMap.ControllerName;
                    tElement.ControllerName = "";
                    tElement.ActionName = "";
                    tElement.UrlParameter = "#" + tWebsiteMap.ControllerName;
                    tSideBarCategory.Elements.Add(tElement);
                }
                // tSideBarCategory.Elements.Sort((sSideBarElement, sElement) => String.Compare(sElement.Name, sSideBarElement.Name, StringComparison.Ordinal));
            }
            PageInformation.AddSideBar(tSideBar.Blocks.ToArray(), null, HttpContext);
            ViewData.Add("WebsiteMapControllerList", WebsiteMapControllerList);
            return View();
        }

        [NWDWebMethodTest(200, NWDPageStandardStatusTag.NotInRelease)]
        [NWDWebMethodTestGet("?ide=lll", 200, NWDPageStandardStatusTag.NotInRelease)]
        [NWDWebMethodTestGet("?sControllerName=ERROR", 500, NWDPageStandardStatusTag.None)]
        [NWDWebMethodTestPost("{\"ide\":\"lll\"}", 200, NWDPageStandardStatusTag.NotInRelease)]
        [NWDWebMethodTestPost("{\"sControllerName\":\"ERROR\"}", 200, NWDPageStandardStatusTag.NotInRelease)]
        [NWDWebMethodTestPost("sControllerName=ERROR", 500, NWDPageStandardStatusTag.None)]
        [NWDWebMethodTestPost("sControllerName=WARNING", 400, NWDPageStandardStatusTag.None)]
        [NWDAuthorizeAdminOnlyInRelease()]
        [HttpGet]
        [HttpPost]
        public IActionResult UnitTest(string sControllerName)
        {
            
            Install(HttpContext);
            NWDSideBar tSideBar = new NWDSideBar();
            NWDSideBarBlock tSideBarBlock = new NWDSideBarBlock();
            tSideBarBlock.Name = "Controller in project";
            tSideBar.Blocks.Add(tSideBarBlock);
            foreach (NWDWebsiteMapLibrary tWebsiteMapLibrary in WebsiteMapControllerList)
            {
                NWDSideBarCategory tSideBarCategory = new NWDSideBarCategory();
                tSideBarCategory.Name = tWebsiteMapLibrary.LibraryName;
                tSideBarCategory.AlwaysShow = true;
                tSideBarBlock.Categories.Add(tSideBarCategory);
                foreach (NWDWebsiteMapClass tWebsiteMap in tWebsiteMapLibrary.ClassList)
                {
                    NWDSideBarElement tElement = new NWDSideBarElement();
                    tElement.Name = tWebsiteMap.ControllerName;
                    tElement.ControllerName = NWDWebsiteMapController.ASP_Controller();
                    tElement.ActionName = nameof(NWDWebsiteMapController.UnitTest);
                    tElement.UrlParameter = "sControllerName=" + tWebsiteMap.ControllerName;
                    tSideBarCategory.Elements.Add(tElement);
                }
                // tSideBarCategory.Elements.Sort((sSideBarElement, sElement) => String.Compare(sElement.Name, sSideBarElement.Name, StringComparison.Ordinal));
            }
            PageInformation.AddSideBar(tSideBar.Blocks.ToArray(), null, HttpContext);
            
            PageInformation.StatusTag = NWDPageStandardStatusTag.NotInRelease;
            //PageInformation.SideBarStyle = NWDSideBarKind.None;
            string tControllerName = string.Empty;
            if (string.IsNullOrEmpty(sControllerName) == false)
            {
                tControllerName = sControllerName.Replace("Controller", "");
            }

            if (tControllerName == "ERROR")
            {
                return StatusCode(500);
            }
            if (tControllerName == "WARNING")
            {
                return StatusCode(400);
            }

            List<NWDWebsiteMapLibrary> tWebsiteMapControllerList = new List<NWDWebsiteMapLibrary>();
            NWDWebsiteMapLibrary tWebsiteMapLibraryAdd = new NWDWebsiteMapLibrary();
            tWebsiteMapControllerList.Add(tWebsiteMapLibraryAdd);

            foreach (NWDWebsiteMapLibrary tWebsiteMapLibrary in WebsiteMapControllerList)
            {
                foreach (NWDWebsiteMapClass tWebsiteMapClass in tWebsiteMapLibrary.ClassList)
                {
                    if (tWebsiteMapClass.ControllerName == tControllerName)
                    {
                        tWebsiteMapLibraryAdd.LibraryName = tWebsiteMapLibrary.LibraryName + " " + tWebsiteMapClass.ControllerName;
                        tWebsiteMapLibraryAdd.ClassList.Add(tWebsiteMapClass);
                    }
                }
            }

            ViewData.Add("WebsiteMapControllerList", tWebsiteMapControllerList);
            ViewData.Add("ViewAllButton", "yes");
            return View("/Views/NWDWebsiteMap/All.cshtml");
        }
    }
}