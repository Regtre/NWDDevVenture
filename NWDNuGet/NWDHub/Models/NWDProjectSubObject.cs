using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDHub.Models
{
    [Serializable]
    public abstract class  NWDProjectSubObject : NWDPlayerData
    {
        [NWDWebPropertyHidden]
        public ulong Project { set; get; }
        [NWDWebPropertyHidden]
        public ulong ProjectUniqueId { set; get; }
        // [NWDWebPropertyHiddenDebugable]
        // public NWDReference<NWDProject> ProjectReference { set; get; } = new NWDReference<NWDProject>();

        [NWDWebPropertyDescription("Publish", NWDWebEditionStyle.Hidden,false, "", "Publish", "Publish", true, true)]
        public NWDProjectPartStatus Publish { set; get; } = NWDProjectPartStatus.New;
        public bool Deletable { set; get; } = true;
        
        public void AssociateToProject(NWDProject sProject)
        {
            Project = sProject.Reference;
            ProjectUniqueId = sProject.ProjectUniqueId;
            // ProjectReference = new NWDReference<NWDProject>(sProject.Reference);
        }
    }
}