using NWDFoundation.Models;

namespace NWDHub.Models
{
    public class NWDProjectDescriptionStorage : NWDDatabaseWebBasicModel
    {
        public ulong ProjectReference { get; set; }
        public string PublicToken { set; get; } = string.Empty;
        public string SecretToken { set; get; } = string.Empty;
        public string Json { set; get; } = string.Empty;
    }
}