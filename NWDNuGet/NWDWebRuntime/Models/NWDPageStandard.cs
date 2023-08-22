using Microsoft.AspNetCore.Http;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Models
{
    [Serializable]
    public class NWDPageStandard
    {
        public NWDBootstrapKindOfStyle EditorValidationStatus { set; get; } = NWDBootstrapKindOfStyle.Secondary;
        public NWDPageContainer PageStyle { set; get; } = NWDPageContainer.ContainerPage;
        public NWDNavBarKind NavBarStyle { set; get; } = NWDNavBarKind.Home;
        public NWDPageStandardStatusTag StatusTag { set; get; } = NWDPageStandardStatusTag.None;
        public string ControllerName { set; get; } = string.Empty;
        public string ActionName { set; get; } = string.Empty;
        public NWDSocialShareableUrl SocialShareableUrl { set; get; } = new NWDSocialShareableUrl();
        public string Title { set; get; } = string.Empty;
        public string SubTitle { set; get; } = string.Empty;
        public string Description { set; get; } = string.Empty;
        public List<string> Keywords { set; get; } = new List<string>();
        public List<string> CssPathAddonList { set; get; } = new List<string>();
        public List<string> JavascriptPathAddonList { set; get; } = new List<string>();
        public List<string> JavascriptCodeAtEndOfPage { set; get; } = new List<string>();
        public List<NWDToastStandard> ActualToastList { set; get; } = new List<NWDToastStandard>();
        
        public List<NWDModal> ModalList { set; get; } = new List<NWDModal>();
        public  NWDNotificationFilteredLists Notifications { set; get; } = new NWDNotificationFilteredLists();

        // public List<NWDPartialView> NavBarPartialList { set; get; } = new List<NWDPartialView>();
        // public List<NWDPartialView> AdminNavBarPartialList { set; get; } = new List<NWDPartialView>();
        // public List<NWDPartialView> AppNavBarPartialList { set; get; } = new List<NWDPartialView>();
        // public List<NWDPartialView> DebugNavBarPartialList { set; get; } = new List<NWDPartialView>();
        // public List<NWDPartialView> SideBarPartialList { set; get; } = new List<NWDPartialView>();
        public List<NWDSideBarBlock> SideBarBlockList { set; get; } = new List<NWDSideBarBlock>();
        public List<NWDSideBarAnnexe> SideBarAnnexeList { set; get; } = new List<NWDSideBarAnnexe>();
        public List<NWDNavBarMenu> NavBarMenuList { set; get; } = new List<NWDNavBarMenu>();
        public NWDNavBarMenu NavFooterMenu { set; get; } = new NWDNavBarMenu();
        public NWDNavBarMenu NavBarApp { set; get; } = new NWDNavBarMenu(){Name = "Applications", NavBar = false, IconStyle = "fas fa-pencil-ruler",Categories = new List<NWDNavBarCategory>()};
        public NWDNavBarMenu NavBarAdmin { set; get; } = new NWDNavBarMenu(){Name = "Administration",NavBar = false, IconStyle = "fas fa-tools",Categories = new List<NWDNavBarCategory>()};
        public NWDNavBarMenu NavBarDebug { set; get; } = new NWDNavBarMenu(){Name = "Debug",NavBar = false, IconStyle = "fas fa-bug",
            // Description = "Project Id " + NWDWebRuntimeConfiguration.KConfig.GetProjectId()+" Environment " + NWDWebRuntimeConfiguration.KConfig.MyEnvironment.ToString()+ " Debug mode is " +NWDWebRuntimeConfiguration.KConfig.IsDevelopment.ToString(),
            Categories = new List<NWDNavBarCategory>()};
        public bool ShowAuthentication { set; get; } = true;
        private Dictionary<string, string> _Debug = new Dictionary<string, string>();
        public NWDFrequentlyAskedQuestionsList? FrequentlyAskedQuestionsList = null;
        public int ItemsInCartCount { set; get; } = 0;
        public string CaptchaSixtyFour { set; get; } = string.Empty;

        public NWDPageStandard()
        {
        }

        public void SetSideBarKind(NWDSideBarKind sSideBarStyle, NWDSideBarBlock[]? sBlocks, NWDSideBarAnnexe[]? sAnnexes, HttpContext sHttpContext)
        {
            //Console.WriteLine(nameof(SetSideBarKind) +"()");
            SideBarBlockList.Clear();
            SideBarAnnexeList.Clear();
            foreach (INWDSideBar tSideBar in NWDWebRuntimeConfiguration.KConfig.GetSideBarInterface())
            {
                //Console.WriteLine(nameof(SetSideBarKind) +" use " + tSideBar.GetType().Name + "");
                NWDSideBarBlock[]? tBlocks = tSideBar.AddSideBarBlock(sSideBarStyle, sHttpContext);
                if (tBlocks != null)
                {
                    foreach (NWDSideBarBlock tBlock in tBlocks)
                    {
                    SideBarBlockList.Add(tBlock);
                    }
                }
                NWDSideBarAnnexe[]? tAnnexes = tSideBar.AddSideBarAnnexe(sSideBarStyle, sHttpContext);
                if (tAnnexes != null)
                {
                    foreach (NWDSideBarAnnexe tAnnexe in tAnnexes)
                    {
                        SideBarAnnexeList.Add(tAnnexe);
                    }
                }
            }
            AddSideBar(sBlocks, sAnnexes,sHttpContext);
        }
        public void SetNavBarKind(NWDNavBarKind sNavBarStyle, NWDNavBarMenu[]? sMenus, HttpContext sHttpContext)
        {
            //Console.WriteLine(nameof(SetNavBarKind) +"()");
            NavBarMenuList.Clear();
            foreach (INWDNavBar tNavBar in NWDWebRuntimeConfiguration.KConfig.GetNavBarInterface())
            {
                //Console.WriteLine(nameof(SetNavBarKind) +" use " + tNavBar.GetType().Name + "");
                NWDNavBarMenu[]? tBlocks = tNavBar.AddNavBarMenu(sNavBarStyle, sHttpContext);
                if (tBlocks != null)
                {
                    foreach (NWDNavBarMenu tBlock in tBlocks)
                    {
                        NavBarMenuList.Add(tBlock);
                    }
                }

                NWDNavBarCategory[]? tApp = tNavBar.AddNavBarApp(sHttpContext);
                if (tApp != null)
                { 
                    NavBarApp.Categories.AddRange(tApp);
                }

                NWDNavBarCategory[]? tAdmin = tNavBar.AddNavBarAdmin(sHttpContext);
                if (tAdmin != null)
                {
                    NavBarAdmin.Categories.AddRange(tAdmin);
                }

                NWDNavBarCategory[]? tDebug = tNavBar.AddNavBarDebug(sHttpContext);
                if (tDebug != null)
                {
                    NavBarDebug.Categories.AddRange(tDebug);
                }

            }
            AddNavBar(sMenus,sHttpContext);
        }
        public void SetNavFooter(NWDNavBarCategory[]? sCategories, HttpContext sHttpContext)
        {
            //Console.WriteLine(nameof(SetNavFooterKind) +"()");
            NavFooterMenu.Categories.Clear();
            foreach (INWDNavFooter tNavFooter in NWDWebRuntimeConfiguration.KConfig.GetNavFooterInterface())
            {
                //Console.WriteLine(nameof(SetNavFooterKind) +" use " + tNavFooter.GetType().Name + "");
                NWDNavBarCategory[]? tBlocks = tNavFooter.AddNavFooterMenu(sHttpContext);
                if (tBlocks != null)
                {
                    foreach (NWDNavBarCategory tBlock in tBlocks)
                    {
                        NavFooterMenu.Categories.Add(tBlock);
                    }
                }
            }
            AddNavFooter(sCategories,sHttpContext);
        }
        
        public void AddSideBar(NWDSideBarBlock[]? sBlocks, NWDSideBarAnnexe[]? sAnnexes, HttpContext sHttpContext)
        {
            //Console.WriteLine(nameof(AddSideBar) +"()");
            if (sBlocks != null)
            {
                foreach (NWDSideBarBlock tBlock in sBlocks)
                {
                    SideBarBlockList.Add(tBlock);
                }
            }
            if (sAnnexes != null)
            {
                foreach (NWDSideBarAnnexe tAnnexe in sAnnexes)
                {
                    SideBarAnnexeList.Add(tAnnexe);
                }
            }
        }
        
        public void AddNavBar( NWDNavBarMenu[]? sMenus, HttpContext sHttpContext)
        {
            //Console.WriteLine(nameof(AddNavBar) +"()");
            if (sMenus != null)
            {
                foreach (NWDNavBarMenu tBlock in sMenus)
                {
                    NavBarMenuList.Add(tBlock);
                }
            }
        }
        
        public void AddNavFooter(NWDNavBarCategory[]? sBlocks, HttpContext sHttpContext)
        {
            //Console.WriteLine(nameof(AddNavFooter) +"()");
            if (sBlocks != null)
            {
                foreach (NWDNavBarCategory tBlock in sBlocks)
                {
                    NavFooterMenu.Categories.Add(tBlock);
                }
            }
        }
        
        public void NewCaptcha(HttpContext sHttpContext)
        {
            CaptchaSixtyFour = NWDCaptcha.RandomCaptchaToImage(sHttpContext, NWDWebRuntimeConfiguration.KConfig.CaptchaParameters);
        }
        
        public void AddTinymce()
        {
            if (JavascriptPathAddonList.Contains("/vendors/tinymce/tinymce.min.js") == false)
            {
                JavascriptPathAddonList.Add("/vendors/tinymce/tinymce.min.js");
            }
        }
        
        public void AddActualToast(NWDToastStandard sToast)
        {
            ActualToastList.Add(sToast);
        }

        public void AddActualToastAlert(NWDBootstrapKindOfStyle sStyle, string sTitle, string sMessage, List<string>? sListOfString, string sSubtitle = "", bool sContactUs = false, string sPrimaryUrl = "",
            string sPrimaryText = "", string sSecondaryUrl = "", string sSecondaryText = "")
        {
            ActualToastList.Add(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, sStyle, sTitle, sMessage, sListOfString, sSubtitle, sContactUs, sPrimaryUrl, sPrimaryText, sSecondaryUrl, sSecondaryText));
        }

        public void AddActualToastPopup(NWDBootstrapKindOfStyle sStyle, string sTitle, string sMessage, List<string>? sListOfString, string sSubtitle = "", bool sContactUs = false, string sPrimaryUrl = "",
            string sPrimaryText = "", string sSecondaryUrl = "", string sSecondaryText = "")
        {
            ActualToastList.Add(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, sStyle, sTitle, sMessage, sListOfString, sSubtitle, sContactUs, sPrimaryUrl, sPrimaryText, sSecondaryUrl, sSecondaryText));
        }

        public Dictionary<string, string> Debug()
        {
            return _Debug;
        }

        public void AddDebug(string sKey, string sValue)
        {
#if DEBUG
            string tKey = DateTime.Now.ToShortTimeString() + " : " + sKey;
            if (_Debug.ContainsKey(tKey) == false)
            {
                _Debug.Add(tKey, sValue);
            }
            else
            {
                AddDebug(sKey + " ", sValue);
            }
#endif
        }

        public string GetKeywordsMeta()
        {
            Keywords.Remove("");
            string rReturn = string.Join(", ", Keywords);
            return rReturn;
        }
    }
}