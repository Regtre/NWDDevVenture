using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDWebsiteProjectCreationOption
    {
        [NWDWebPropertyHidden] public ulong Reference { set; get; }

        [NWDWebPropertyDescription("Include Treat Module", NWDWebEditionStyle.Bool,false, "", "", "", true)]
        public bool TreatModule { set; get; } = false;

        public static NWDWebsiteProjectCreationOption ForProject(NWDProject sProject)
        {
            return new NWDWebsiteProjectCreationOption()
            {
                Reference = sProject.Reference,
            };
        }
    }
}