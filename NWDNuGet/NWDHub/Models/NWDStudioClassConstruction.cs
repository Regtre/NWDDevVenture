using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDStudioClassConstruction : NWDClassConstruction
    {
        public NWDStudioClassConstruction()
        {
        }
        
        public NWDStudioClassConstruction(NWDProject sProject)
        {
            this.AssociateToProject(sProject);
        }
    }
}  