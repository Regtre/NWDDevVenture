using System.IO.Compression;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWDCrucial.Exchanges;
using NWDCrucial.Exchanges.Payloads;
using NWDCrucial.Models;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.WebEdition.Enums;
using NWDHub.Configuration;
using NWDHub.Managers;
using NWDHub.Models;
using NWDTreat.Exchanges;
using NWDWebEditor.Controllers;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools.Cookies;
using NWDWebStandard.Configuration;
using NWDWebTreat.Configuration;

namespace NWDHub.Controllers
{
    public class NWDProjectController : NWDModelEditionAsyncController<NWDProject>
    {
        public override NWDNavBarMenu[]? GetNavBarMenu(NWDProject? sObject)
        {
            if (sObject != null)
            {
                NWDNavBarMenu tMenu = new NWDNavBarMenu()
                {
                    Name = sObject.Name,
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
                                    Name = "Information", ActionName = "Show", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "Instructions", ActionName = "Instructions", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "Roles", ActionName = "Roles", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "Environments", ActionName = "Environments", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "Custom classes", ActionName = "CustomClasses", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
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
                                    Name = "Estimates", ActionName = "Estimates", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "Billings", ActionName = "Billings", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
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
                                    Name = "Publish", ActionName = "Publish", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "DeleteInstructions", ActionName = "Delete", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference, BadgeText = "test"
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
                                    Name = "Generate CSProj", ActionName = "Publish", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "Install your website", ActionName = "Delete", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
                                },
                                new NWDNavBarElement()
                                {
                                    Name = "Use our Yaml (gitlab)", ActionName = "Delete", ControllerName = "NWDProject", UrlParameter = "sReference=" + sObject.Reference
                                },
                            }
                        }
                    }
                };
                return new[] { tMenu };
            }
            return null;
        }

        public override NWDSideBarBlock[] GetSideBarBlock(NWDProject? sProject)
        {
            if (sProject!=null)
            {
            NWDSideBarBlock tBlock = null;
            if (sProject != null)
            {
                Console.WriteLine("genertaion de sidebar");
                tBlock = new NWDSideBarBlock()
                {
                    Name = sProject.Name,
                    Categories = new List<NWDSideBarCategory>()
                    {
                        new NWDSideBarCategory()
                        {
                            Name = "Dashboard",
                            IconStyle = "far fa-comment-alt", BadgeText = "test",
                            Elements = new List<NWDSideBarElement>()
                            {
                                new NWDSideBarElement()
                                {
                                    Name = "Information", ActionName = "Show", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference, BadgeText = "test",
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "Instructions", ActionName = "Instructions", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "Roles", ActionName = "Roles", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "Services", ActionName = "Services", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "Environments", ActionName = "Environments", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "Keys", ActionName = "Keys", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "WebSetting", ActionName = "WebSetting", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "Custom classes", ActionName = "CustomClasses", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                }
                            }
                        },
                        new NWDSideBarCategory()
                        {
                            Name = "Billing",
                            IconStyle = "far fa-file-alt",
                            Elements = new List<NWDSideBarElement>()
                            {
                                new NWDSideBarElement()
                                {
                                    Name = "Estimates", ActionName = "Estimates", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "Billings", ActionName = "Billings", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                            }
                        },
                        new NWDSideBarCategory()
                        {
                            Name = "Other actions",
                            IconStyle = "fas fa-tools",
                            Elements = new List<NWDSideBarElement>()
                            {
                                new NWDSideBarElement()
                                {
                                    Name = "Publish", ActionName = "Publish", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "DeleteInstructions", ActionName = "Delete", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                            }
                        },
                        new NWDSideBarCategory()
                        {
                            Name = "Your Website",
                            IconStyle = "fas fa-tools",
                            Elements = new List<NWDSideBarElement>()
                            {
                                new NWDSideBarElement()
                                {
                                    Name = "Generate CSProj", ActionName = "Publish", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "Install your website", ActionName = "Delete", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference, BadgeText = "test"
                                },
                                new NWDSideBarElement()
                                {
                                    Name = "Use our Yaml (gitlab)", ActionName = "Delete", ControllerName = "NWDProject", UrlParameter = "sReference=" + sProject.Reference
                                },
                            }
                        }
                    }
                };
            }

            return new[] { tBlock };
        }
        return null;
        }

        public override NWDSideBarAnnexe[]? GetSideBarAnnexe(NWDProject? sObject)
        {
            if (sObject!=null)
            {
            NWDSideBarAnnexe tBlock = null;
            if (sObject != null)
            {
                if (sObject.NeedToBePublish == true)
                {
                    Console.WriteLine("GetSideBarAnnexe");
                    tBlock = new NWDSideBarAnnexe()
                    {
                        Style = NWDBootstrapKindOfStyle.Danger,
                        Name = "ALERT",
                        Description = " MUST BE PUBLISH!"
                    };
                }
            }

            return new[] { tBlock };
        }
        return null;
        }

        static public NWDCookieEnum<NWDEnvironmentKind> kEnvironmentCookie = new NWDCookieEnum<NWDEnvironmentKind>("Env", "env", "", NWDCookieDefinitionGroup.Functional, NWDEnvironmentKind.Dev);

        public override void Before(string sReference)
        {
            NWDLogger.TraceSuccess(nameof(Before));
            // TODO  PageInformation.SideBarStyle = NWDSideBarKind.Tools;
            // PageInformation.SideBarPartialList.Add(new NWDPartialView() { PartialPath = "/Views/NWDProject/SideBar.cshtml" });
            // PageInformation.NavBarStyle = NWDNavBarKind.Tools;
            // PageInformation.NavBarPartialList.Add(new NWDPartialView() { PartialPath = "/Views/NWDProject/NavBar.cshtml" });
        }

        public override void NewAddon(NWDProject sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
        {
            NWDLogger.TraceSuccess(nameof(NewAddon));
            sObject.Check();
            sObject.Development = new NWDReference<NWDProjectByEnvironment>(new NWDProjectByEnvironment() { EnvironmentKind = NWDEnvironmentKind.Dev });
            sObject.PlayTest = new NWDReference<NWDProjectByEnvironment>(new NWDProjectByEnvironment() { EnvironmentKind = NWDEnvironmentKind.PlayTest });
            sObject.Qualification = new NWDReference<NWDProjectByEnvironment>(new NWDProjectByEnvironment() { EnvironmentKind = NWDEnvironmentKind.Qualification });
            sObject.PreProduction = new NWDReference<NWDProjectByEnvironment>(new NWDProjectByEnvironment() { EnvironmentKind = NWDEnvironmentKind.PreProduction });
            sObject.Production = new NWDReference<NWDProjectByEnvironment>(new NWDProjectByEnvironment() { EnvironmentKind = NWDEnvironmentKind.Production });
            sObject.PostProduction = new NWDReference<NWDProjectByEnvironment>(new NWDProjectByEnvironment() { EnvironmentKind = NWDEnvironmentKind.PostProduction });
            // NWDProjectDataTrack tDev = new NWDProjectDataTrack()
            // {
            //     ProjectId = sObject.Reference,
            //     // ProjectReference = new NWDReference<NWDProject>(sObject.Reference),
            //     Kind = NWDEnvironmentKind.Dev,
            //     UsualName = "dev standard",
            //     Color = NWDRandom.RandomHexadecimalColor(),
            //     Track = 1,
            // };
            // NWDProjectDataTrack tPlaytest = new NWDProjectDataTrack()
            // {
            //     ProjectId = sObject.ProjectUniqueId,
            //     // ProjectReference = new NWDReference<NWDProject>(sObject.Reference),
            //     Kind = NWDEnvironmentKind.PlayTest,
            //     UsualName = "playtest standard",
            //     Color = NWDRandom.RandomHexadecimalColor(),
            //     Track = 1,
            // };
            // NWDProjectDataTrack tQualification = new NWDProjectDataTrack()
            // {
            //     ProjectId = sObject.ProjectUniqueId,
            //     // ProjectReference = new NWDReference<NWDProject>(sObject.Reference),
            //     Kind = NWDEnvironmentKind.Qualification,
            //     UsualName = "standard",
            //     Color = NWDRandom.RandomHexadecimalColor(),
            //     Track = 1,
            // };
            // NWDProjectDataTrack tPreprod = new NWDProjectDataTrack()
            // {
            //     ProjectId = sObject.ProjectUniqueId,
            //     // ProjectReference = new NWDReference<NWDProject>(sObject.Reference),
            //     Kind = NWDEnvironmentKind.PreProduction,
            //     UsualName = tQualification.UsualName,
            //     Color = tQualification.Color,
            //     Track = tQualification.Track,
            // };
            // NWDProjectDataTrack tProd = new NWDProjectDataTrack()
            // {
            //     ProjectId = sObject.ProjectUniqueId,
            //     // ProjectReference = new NWDReference<NWDProject>(sObject.Reference),
            //     Kind = NWDEnvironmentKind.Production,
            //     UsualName = tQualification.UsualName,
            //     Color = tQualification.Color,
            //     Track = tQualification.Track,
            // };
            // NWDProjectDataTrack tPostprod = new NWDProjectDataTrack()
            // {
            //     ProjectId = sObject.ProjectUniqueId,
            //     // ProjectReference = new NWDReference<NWDProject>(sObject.Reference),
            //     Kind = NWDEnvironmentKind.PostProduction,
            //     UsualName = tQualification.UsualName,
            //     Color = tQualification.Color,
            //     Track = tQualification.Track,
            // };
            // sObject.Tracks.AddValues(new NWDProjectDataTrack[]
            // {
            //     tDev,
            //     tPlaytest,
            //     tQualification,
            //     tPreprod,
            //     tProd,
            //     tPostprod,
            // });
        }

        public override void UpdateAddon(NWDProject sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
        {
            NWDLogger.TraceSuccess(nameof(UpdateAddon));
            sObject.Check();
            NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, sObject.Reference);
            string tOldJson = JsonConvert.SerializeObject(tProject);
            string tNewJson = JsonConvert.SerializeObject(sObject);
            if (tOldJson != tNewJson)
            {
                sObject.NeedToBePublish = true;
                sObject.TestTrackFollow(sHttpContext);
                NWDWebDataManager.SaveData(sHttpContext, sObject);
            }
        }

        // public override string PathToModifyView()
        // {
        //     return "/Views/NWDProject/" + nameof(Modify) + ".cshtml";
        // }

        public override string PathToShowView()
        {
            return "/Views/NWDProject/" + nameof(Show) + ".cshtml";
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult Instructions(ulong sReference)
        {
           
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);
                
                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/Instructions.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult Roles(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);
                
                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/Roles.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult Services(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);

                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/Services.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult Environments(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);

                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                ViewData.Add(typeof(NWDProjectGlobalSettings).Name, NWDProjectManager.GetProjectGlobalSettingsFor(tItem, HttpContext));
                After(sReference.ToString());
                
                return View("/Views/NWDProject/Environments.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult Keys(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);

                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/Keys.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        public IActionResult WebSetting(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);

                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/WebSetting.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        public IActionResult WebProjectDownload(NWDWebsiteProjectCreationOption sModel)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sModel.Reference);
            if (tItem != null)
            {
                Before(sModel.Reference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sModel.Reference.ToString());
                MemoryStream tStream = NWDWebsiteProjectCreationManager.GenerateWebsiteProject(sModel, tItem);
                tStream.Seek(0, SeekOrigin.Begin);
                return File(tStream, "application/zip", tItem.Name + "_Website.zip");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult CustomClasses(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);

                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/CustomClasses.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult Estimates(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);

                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/Estimates.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult Billings(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);

                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/Billings.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult StatisticsCurrent(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);

                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/StatisticsCurrent.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult StatisticsOld(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);

                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/StatisticsOld.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult Published(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);

                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());

                Dictionary<int, NWDEnvironmentKind> tTrackEnvironment = new Dictionary<int, NWDEnvironmentKind>();
                Dictionary<int, ulong> tTrackReference = new Dictionary<int, ulong>();
                List<NWDProjectCredentials> tProjectCredentials = new List<NWDProjectCredentials>();

                foreach (NWDProjectByEnvironment tObject in NWDWebDataManager.GetAllData<NWDProjectByEnvironment>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        NWDProjectCredentials tCredential = tObject.ConvertToProjectCredentials();
                        tProjectCredentials.Add(tCredential);
                    }
                }

                tTrackEnvironment.Add(0, NWDEnvironmentKind.Dev);
                tTrackReference.Add(0, 0);

                foreach (NWDProjectDevDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectDevDataTrack>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tTrackEnvironment.TryAdd(tObject.Track, tObject.Kind);
                        tTrackReference.TryAdd(tObject.Track, tObject.Reference);
                    }
                }

                foreach (NWDProjectPlayTestDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectPlayTestDataTrack>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tTrackEnvironment.TryAdd(tObject.Track, tObject.Kind);
                        tTrackReference.TryAdd(tObject.Track, tObject.Reference);
                    }
                }

                foreach (NWDProjectQualificationDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectQualificationDataTrack>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tTrackEnvironment.TryAdd(tObject.Track, tObject.Kind);
                        tTrackReference.TryAdd(tObject.Track, tObject.Reference);
                    }
                }

                foreach (NWDProjectPublishDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectPublishDataTrack>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tTrackEnvironment.TryAdd(tObject.Track, tObject.Kind);
                        tTrackReference.TryAdd(tObject.Track, tObject.Reference);
                    }
                }

                //TODO publish environment to NWDServer services
                List<NWDDataTrackDescription> tTrackList = new List<NWDDataTrackDescription>();
                foreach (NWDProjectDevDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectDevDataTrack>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tObject.Publish = NWDProjectPartStatus.Active;
                        NWDWebDataManager.SaveData(HttpContext, tObject);
                        tTrackList.Add(tObject.ToDataTrackDescription(tTrackEnvironment, tTrackReference));
                    }
                }

                foreach (NWDProjectPlayTestDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectPlayTestDataTrack>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tObject.Publish = NWDProjectPartStatus.Active;
                        NWDWebDataManager.SaveData(HttpContext, tObject);
                        tTrackList.Add(tObject.ToDataTrackDescription(tTrackEnvironment, tTrackReference));
                    }
                }

                foreach (NWDProjectQualificationDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectQualificationDataTrack>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tObject.Publish = NWDProjectPartStatus.Active;
                        NWDWebDataManager.SaveData(HttpContext, tObject);
                        tTrackList.Add(tObject.ToDataTrackDescription(tTrackEnvironment, tTrackReference));
                    }
                }

                foreach (NWDProjectPublishDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectPublishDataTrack>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tObject.Publish = NWDProjectPartStatus.Active;
                        NWDWebDataManager.SaveData(HttpContext, tObject);
                        tTrackList.Add(tObject.ToDataTrackDescription(tTrackEnvironment, tTrackReference));
                    }
                }

                foreach (NWDProjectService tObject in NWDWebDataManager.GetAllData<NWDProjectService>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tObject.Publish = NWDProjectPartStatus.Active;
                        NWDWebDataManager.SaveData(HttpContext, tObject);
                    }
                }

                foreach (NWDProjectRole tObject in NWDWebDataManager.GetAllData<NWDProjectRole>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tObject.Publish = NWDProjectPartStatus.Active;
                        NWDWebDataManager.SaveData(HttpContext, tObject);
                    }
                }

                foreach (NWDPlayerClassConstruction tObject in NWDWebDataManager.GetAllData<NWDPlayerClassConstruction>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tObject.Publish = NWDProjectPartStatus.Active;
                        NWDWebDataManager.SaveData(HttpContext, tObject);
                    }
                }

                foreach (NWDStudioClassConstruction tObject in NWDWebDataManager.GetAllData<NWDStudioClassConstruction>(HttpContext))
                {
                    if (tObject.Project == tItem.Reference && tObject.Publish != NWDProjectPartStatus.Inactive)
                    {
                        tObject.Publish = NWDProjectPartStatus.Active;
                        NWDWebDataManager.SaveData(HttpContext, tObject);
                    }
                }

                // Create Role project description cache
                NWDProjectDescriptionManager.RemoveAllCache(tItem);
                foreach (NWDProjectRole tRole in NWDWebDataManager.GetAllData<NWDProjectRole>(HttpContext))
                {
                    if (tRole.Project == tItem.Reference && tRole.Publish != NWDProjectPartStatus.Inactive)
                    {
                        NWDProjectDescriptionManager.CreateInCacheFor(tItem, tTrackList, tProjectCredentials, tRole);
                    }
                }

                // Create credentials to use in hub
                // NWDProjectTreatManager.RemoveAllCache(tProject);
                foreach (NWDEnvironmentKind tEnvironmentKind in Enum.GetValues<NWDEnvironmentKind>())
                {
                    NWDProjectTreatManager.CreateInCacheFor(tItem, tEnvironmentKind);
                }

                NWDUpPayloadProjectPublish tUpPayloadProjectPublish = new NWDUpPayloadProjectPublish();
                {
                    foreach (NWDEnvironmentKind tEnvironmentKind in Enum.GetValues<NWDEnvironmentKind>())
                    {
                        NWDProjectTreatStorage tCredentials = NWDProjectTreatManager.GetByProjectUniqueId(tItem.ProjectUniqueId, tEnvironmentKind);
                        tUpPayloadProjectPublish.ProjectCredentialsList.Add(tCredentials.ConvertToProjectCredentials());
                    }
                }
                // push on server ... 
                NWDRequestCrucial tRequest = new NWDRequestCrucial(NWDHubConfiguration.KConfig, NWDExchangeCrucialKind.ProjectPublish, tUpPayloadProjectPublish, NWDExchangeOrigin.Web, NWDExchangeDevice.Unknown);
                NWDResponseCrucial? tResponseCrucial = NWDWebCrucialCallbackServers.PostRequest(tRequest).Result;

                tItem.NeedToBePublish = false;
                NWDWebDataManager.SaveData(HttpContext, tItem);
                return View("/Views/NWDProject/Published.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult Publish(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);
                
                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/Publish.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }

        [HttpGet]
        [NWDAuthorizeByAuthentication(true)]
        public IActionResult DeleteInstructions(ulong sReference)
        {
            NWDProject? tItem = NWDWebDataManager.GetDataByReference<NWDProject>(HttpContext, sReference);
            if (tItem != null)
            {
                PageInformation.SetSideBarKind(NWDSideBarKind.Tools, GetSideBarBlock(tItem), GetSideBarAnnexe(tItem),HttpContext);
                PageInformation.SetNavBarKind(NWDNavBarKind.Tools, GetNavBarMenu(tItem),HttpContext);
                PageInformation.SetNavFooter(null,HttpContext);
                
                Before(sReference.ToString());
                ViewData.Add(KControllerName, this.GetType().Name.Replace("Controller", ""));
                ViewData.Add(KResultToUse, tItem);
                ViewData.Add(KModelToUse, typeof(NWDProject).AssemblyQualifiedName);
                ViewData.Add(typeof(NWDProject).Name, tItem);
                After(sReference.ToString());
                
                return View("/Views/NWDProject/DeleteInstructions.cshtml");
            }
            else
            {
                return View("/Views/Shared/_Error.cshtml");
            }
        }
    }
}