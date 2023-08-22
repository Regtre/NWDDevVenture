namespace NWDHub.Models
{
    [Serializable]
    public class NWDPlayerClassConstruction : NWDClassConstruction
    {
        public NWDPlayerClassConstruction()
        {
        }
        
        public NWDPlayerClassConstruction(NWDProject sProject)
        {
            this.AssociateToProject(sProject);
        }
    }
}