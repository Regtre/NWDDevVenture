using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDProjectName : NWDProjectSubObject
    {
        [NWDWebPropertyDescription("Name", NWDWebEditionStyle.Text,false, "", "", "", true)]
        public string Name { set; get; } = string.Empty;
        [NWDWebPropertyDescription("Description", NWDWebEditionStyle.RichText,false, "", "", "", true)]
        public string Description { set; get; } = string.Empty;
        
        public NWDProjectName()
        {
        }
        
        public NWDProjectName(NWDProject sProject)
        {
            this.AssociateToProject(sProject);
        }
    }
}