using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Tools;

namespace NWDHub.Models
{
    public class NWDProjectRole : NWDProjectSubObject
    {
        [NWDWebPropertyDescription("Name", NWDWebEditionStyle.Text,false, "", "my Title", "my Placeholder", sUseAsSortBy: true, sUseAsColumn: true, sIsPrimaryColumn: true)]
        public string Name { get; set; } = string.Empty;
        
        [NWDWebPropertyDescription("Description", NWDWebEditionStyle.Text,false, "", "my Title", "my Placeholder", sUseAsSortBy: true, sUseAsColumn: true, sIsPrimaryColumn: false)]
        public string Description { get; set; } = string.Empty;
        
        [NWDWebPropertyDescription("PublicToken", NWDWebEditionStyle.Text,false, "", "my Title", "my Placeholder", sUseAsSortBy: true, sUseAsColumn: false, sIsPrimaryColumn: false)]
        public string PublicToken { set; get; } = NWDToolBox.RandomString(128);
        
        [NWDWebPropertyDescription("SecretToken", NWDWebEditionStyle.Text,false, "", "my Title", "my Placeholder", sUseAsSortBy: false, sUseAsColumn: false, sIsPrimaryColumn: false)]
        public string SecretToken { set; get; } = NWDToolBox.RandomString(128);
        
        [NWDWebPropertyDescription("CanEditMetaDataInfos", NWDWebEditionStyle.Bool,false, "", "my Title", "my Placeholder", sUseAsSortBy: false, sUseAsColumn: false, sIsPrimaryColumn: false)]
        public bool CanEditMetaDataInfos { set; get; } = false;
        
        [NWDWebPropertyDescription("CanCreateMetaData", NWDWebEditionStyle.Bool,false, "", "my Title", "my Placeholder", sUseAsSortBy: false, sUseAsColumn: false, sIsPrimaryColumn: false)]
        public bool CanCreateMetaData { set; get; } = false;
        public NWDProjectRole()
        {
        }
        
        public NWDProjectRole(NWDProject sProject)
        {
            this.AssociateToProject(sProject);
        }
    }
}