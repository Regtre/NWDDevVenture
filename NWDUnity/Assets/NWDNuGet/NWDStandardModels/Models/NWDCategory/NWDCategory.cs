using NWDFoundation.Models;

namespace NWDStandardModels.Models
{
    public class NWDCategory : NWDStudioData
    {
        public NWDLocalizableText Name { get; set; }
        public NWDReference<NWDItem> ItemDescription { get; set; }

        public NWDReferences<NWDCategory> Parents { get; set; }
        public NWDReferences<NWDCategory> Children { get; set; }
        public NWDReferences<NWDCategory> Cascade { get; set; }
    }
}