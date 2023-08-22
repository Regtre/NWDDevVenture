using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDClassConstruction : NWDProjectSubObject
    {
        public string Name { set; get; } = string.Empty;
        public NWDClassConstruction()
        {
        }
        
        public NWDClassConstruction(NWDProject sProject)
        {
            this.AssociateToProject(sProject);
        }
    }
}