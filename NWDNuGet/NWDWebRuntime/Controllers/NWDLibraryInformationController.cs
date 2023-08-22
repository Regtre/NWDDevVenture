using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Configuration;
using NWDFoundation.Exchanges;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Services;

namespace NWDWebRuntime.Controllers
{
    // [NWDAuthorizeAdminOnly()]
    public class NWDLibraryInformationController : NWDRawController
    {
        public ActionResult Index()
        {
            NWDWebRuntimeRecursiveService.TestConfiguration();
            foreach (KeyValuePair<string, NWDRequestStatus> tResult in NWDWebRuntimeConfiguration.KConfig.ConfigurationStatus)
            {
                PageInformation.AddActualToastAlert(NWDBootstrapKindOfStyle.Primary, tResult.Key, tResult.Value.ToString(), null);
            }
            List<NWDLibraryInfos> tFileVersionInfoList = (List<NWDLibraryInfos>)NWDLibrariesInstalled.GetFileVersionInfoList();
            tFileVersionInfoList.Sort(delegate(NWDLibraryInfos x, NWDLibraryInfos y)
            {
                if (x.Information == null && y.Information == null) return 0;
                else if (x.Information == null) return -1;
                else if (y.Information == null) return 1;
                else return x.Information.InternalName.CompareTo(y.Information.InternalName);
            });
            ViewData.Add(nameof(NWDLibraryInfos), tFileVersionInfoList.ToArray());
            NWDSideBar tSideBar = new NWDSideBar();
            NWDSideBarBlock tSideBarBlock = new NWDSideBarBlock();
            tSideBarBlock.Name = "Modules installed";
            tSideBar.Blocks.Add(tSideBarBlock);
            
            // NWDSideBarBlock tSideBarBlockProject = new NWDSideBarBlock();
            // tSideBarBlockProject.Name = "General information";
            // tSideBar.Blocks.Add(tSideBarBlockProject);
            // tSideBarBlockProject.Categories.Add(new NWDSideBarCategory(){Name = "Project", IconStyle = "far fa-folder", AlwaysShow = false, Elements = new List<NWDSideBarElement>()
            // {
            //     new NWDSideBarElement(){Name = NWDWebRuntimeConfiguration.KConfig.GetProjectId().ToString(), ActionName = string.Empty, ControllerName = string.Empty, UrlParameter = "#top"},
            // }});
            // tSideBarBlockProject.Categories.Add(new NWDSideBarCategory(){Name = "Environment", IconStyle = "far fa-compass", AlwaysShow = false, Elements = new List<NWDSideBarElement>()
            // {
            //     new NWDSideBarElement(){Name = NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment().ToString(), ActionName = string.Empty, ControllerName = string.Empty, UrlParameter = "#top"},
            // }});

            NWDSideBarCategory tSideBarNuget = new NWDSideBarCategory();
            tSideBarNuget.Name = "Nuget";
            tSideBarNuget.IconStyle = "bi puzzle";
            tSideBarNuget.AlwaysShow = true;
            NWDSideBarCategory tSideBarProject = new NWDSideBarCategory();
            tSideBarProject.Name = "CSProj";
            tSideBarNuget.IconStyle = "bi box-seam";
            tSideBarProject.AlwaysShow = true;
            foreach (NWDLibraryInfos tLibInfos in tFileVersionInfoList)
            {
                NWDSideBarElement tElement = new NWDSideBarElement();
                if (tLibInfos.Information != null)
                {
                    if (tLibInfos.Information.InternalName != null)
                    {
                        tElement.Name = tLibInfos.Information.InternalName.Replace(".dll","").Replace("NWD","");;
                        tElement.ControllerName = "";
                        tElement.ActionName = "";
                        //tElement.BadgeText = tLibInfos.Information.FileVersion;
                        tElement.UrlParameter="#"+tLibInfos.Information.InternalName.Replace(".dll","");
                        if (tLibInfos.Nuget == true)
                        {
                            tSideBarNuget.Elements.Add(tElement);
                        }
                        else
                        {
                            tSideBarProject.Elements.Add(tElement);
                        }
                    }
                }
            }
            if (tSideBarNuget.Elements.Count > 0)
            {
            tSideBarBlock.Categories.Add(tSideBarNuget);
            }
            if (tSideBarProject.Elements.Count > 0)
            {
                tSideBarBlock.Categories.Add(tSideBarProject);
            }
            PageInformation.SetSideBarKind(NWDSideBarKind.Tools, tSideBar.Blocks.ToArray(), null,HttpContext);
            PageInformation.SetNavBarKind(NWDNavBarKind.Home,null,HttpContext);
            PageInformation.SetNavFooter(null,HttpContext);
            AddViewDataPageStandard(PageInformation);
            return View("/Views/NWDLibraryInformation/Index.cshtml");
        }
    }
}