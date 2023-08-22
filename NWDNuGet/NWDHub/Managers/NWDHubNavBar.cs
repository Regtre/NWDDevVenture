using Microsoft.AspNetCore.Http;
using NWDHub.Controllers;
using NWDHub.Models;
using NWDWebRuntime.Models;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models.Enums;

namespace NWDHub.Managers
{
    public class NWDHubNavBar : INWDNavBar
    {
        public NWDNavBarMenu[]? AddNavBarMenu(NWDNavBarKind sNavBarKind, HttpContext sHttpContext)
        {
            if (NWDAuthorizeByAuthentication.ValidFor(sHttpContext, true) == true)
            {
                NWDNavBarCategory tNavBarCategoryNew = new NWDNavBarCategory()
                {
                    Name = "New Project",
                    IconStyle = "far fa-folder",
                    Elements = new List<NWDNavBarElement>()
                    {
                        new NWDNavBarElement()
                        {
                            Name = "Create project",
                            ActionName = nameof(NWDProjectController.New),
                            ControllerName = "NWDProject",
                            UrlParameter = "",
                        }
                    }
                };
                NWDNavBarMenu tMenu = new NWDNavBarMenu()
                {
                    Name = "My projects",
                    Categories = new List<NWDNavBarCategory>()
                    {
                        tNavBarCategoryNew,
                    }
                };
                List<NWDProject> tProjectList = NWDWebDataManager.GetAllData<NWDProject>(sHttpContext);
                if (tProjectList.Count > 0)
                {
                    NWDNavBarCategory tNavBarCategory = new NWDNavBarCategory()
                    {
                        Name = "My projects",
                        IconStyle = "far fa-folder",
                        Elements = new List<NWDNavBarElement>()
                    };
                    tMenu.Categories.Add(tNavBarCategory);
                    foreach (NWDProject tProject in tProjectList)
                    {
                        if (tProject.Trashed == false)
                        {
                            string tName = "Project " + tProject.ProjectUniqueId;
                            if (string.IsNullOrEmpty(tProject.Name) == false)
                            {
                                tName = tProject.Name;
                            }

                            tNavBarCategory.Elements.Add(
                                new NWDNavBarElement()
                                {
                                    Name = tName,
                                    ActionName = nameof(NWDProjectController.Show),
                                    ControllerName = "NWDProject",
                                    UrlParameter = "sReference=" + tProject.Reference,
                                });
                        }
                    }
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
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarApp(HttpContext sHttpContext)
        {
            if (NWDAuthorizeByAuthentication.ValidFor(sHttpContext, true) == true)
            {
                NWDNavBarCategory tNavBarCategoryNew = new NWDNavBarCategory()
                {
                    Name = "New Project",
                    IconStyle = "far fa-folder",
                    Elements = new List<NWDNavBarElement>()
                    {
                        new NWDNavBarElement()
                        {
                            Name = "Create project",
                            ActionName = nameof(NWDProjectController.New),
                            ControllerName = "NWDProject",
                            UrlParameter = "",
                        }
                    }
                };
                List<NWDProject> tProjectList = NWDWebDataManager.GetAllData<NWDProject>(sHttpContext);
                if (tProjectList.Count > 0)
                {
                    NWDNavBarCategory tNavBarCategory = new NWDNavBarCategory()
                    {
                        Name = "My projects",
                        IconStyle = "far fa-folder",
                        Elements = new List<NWDNavBarElement>()
                    };
                    foreach (NWDProject tProject in tProjectList)
                    {
                        if (tProject.Trashed == false)
                        {
                            string tName = "Project " + tProject.ProjectUniqueId;
                            if (string.IsNullOrEmpty(tProject.Name) == false)
                            {
                                tName = tProject.Name;
                            }

                            tNavBarCategory.Elements.Add(
                                new NWDNavBarElement()
                                {
                                    Name = tName,
                                    ActionName = nameof(NWDProjectController.Show),
                                    ControllerName = "NWDProject",
                                    UrlParameter = "sReference=" + tProject.Reference,
                                });
                        }
                    }

                    return new[] { tNavBarCategoryNew, tNavBarCategory };
                }

                return new[] { tNavBarCategoryNew };
            }
            return null;
        }

        public NWDNavBarCategory[]? AddNavBarDebug(HttpContext sHttpContext)
        {
            return null;
        }
    }
}