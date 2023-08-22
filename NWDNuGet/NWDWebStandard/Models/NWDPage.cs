using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models;

namespace NWDWebStandard.Models
{

    [Serializable]
    public class NWDPage : NWDDatabaseWebBasicModel
    {       
        [NWDWebPropertyDescription("Title", NWDWebEditionStyle.Text,false, "",  "",  "", sUseAsSortBy: true, sUseAsColumn: true, sIsPrimaryColumn: true)]
        public string Title { set; get; } = string.Empty;
        
        [NWDWebPropertyDescription("Use as navbar menu", NWDWebEditionStyle.Bool,false, "",  "",  "", sUseAsSortBy: true, sUseAsColumn: true, sIsPrimaryColumn: false)]
        public bool UseAsNavBarMenu { set; get; }
        [NWDWebPropertyDescription("NavBar replace title", NWDWebEditionStyle.Text,false, "",  "",  "", sUseAsSortBy: true, sUseAsColumn: true, sIsPrimaryColumn: false)]
        public string NavBarTitle { set; get; } = string.Empty;
        [NWDWebPropertyDescription("NavBar icon class", NWDWebEditionStyle.Text,false, "",  "",  "", sUseAsSortBy: false, sUseAsColumn: false, sIsPrimaryColumn: false)]
        public string NavBarIcon { set; get; } = string.Empty;
        [NWDWebPropertyDescription("MetaDescription", NWDWebEditionStyle.Text,false, "",  "",  "", sUseAsSortBy: false, sUseAsColumn: false, sIsPrimaryColumn: false)]
        public string MetaDescription { set; get; } = string.Empty;
        [NWDWebPropertyDescription("MetaKeywords", NWDWebEditionStyle.Text,false, "",  "",  "", sUseAsSortBy: false, sUseAsColumn: false, sIsPrimaryColumn: false)]
        public string MetaKeywords { set; get; } = string.Empty;
        
        
        [NWDWebPropertyDescription("Header", NWDWebEditionStyle.Text,false, "",  "",  "", sUseAsSortBy: false, sUseAsColumn: false, sIsPrimaryColumn: false)]
        public string Header { set; get; } = string.Empty;
        [NWDWebPropertyDescription("Paragraphe", NWDWebEditionStyle.Text,false, "",  "",  "", sUseAsSortBy: false, sUseAsColumn: false, sIsPrimaryColumn: false)]
        public string Paragraphe { set; get; } = string.Empty;


        public string GetNavBarMenu()
        {
            if (string.IsNullOrEmpty(NavBarTitle) == true)
            {
                return Title;
            }
            else
            {
                return NavBarTitle;
            }
        }
    }
}