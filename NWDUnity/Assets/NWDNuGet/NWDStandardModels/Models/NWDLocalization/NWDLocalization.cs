using NWDFoundation.Models;

namespace NWDStandardModels.Models
{
    public class NWDLocalization : NWDStudioData
    {
        public NWDLocalizableText Text { get; set; }
        public string Key { get; set; }
    }
}