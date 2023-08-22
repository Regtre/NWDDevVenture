using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDProjectPlan : NWDProjectSubObject
    {
        [NWDWebPropertyDescription("Plan", NWDWebEditionStyle.Dropdown,false, "", "", "", true)]
        public NWDPlan Plan { set; get; } = NWDPlan.Disconnected;
        
        public NWDProjectPlan()
        {
        }
        
        public NWDProjectPlan(NWDProject sProject)
        {
            this.AssociateToProject(sProject);
        }
    }
}