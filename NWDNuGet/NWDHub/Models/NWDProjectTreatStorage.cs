using NWDCrucial.Models;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;

namespace NWDHub.Models
{
    public class NWDProjectTreatStorage : NWDDatabaseWebBasicModel
    {
        public NWDEnvironmentKind Environment { get; set; } = NWDEnvironmentKind.Production;
        public ulong ProjectReference { get; set; }
        public ulong ProjectUniqueId { get; set; }
        public string ProjectKey { set; get; } = string.Empty;
        public string SecretKey { set; get; } = string.Empty;
        public string TreatKey { set; get; } = string.Empty;
        public NWDProjectCredentials ConvertToProjectCredentials()
        {
            return new NWDProjectCredentials()
            {
                Creation = NWDTimestamp.ToTimestamp(DateTime.UtcNow),
                Modification = NWDTimestamp.ToTimestamp(DateTime.UtcNow),
                Active = true,
                Trashed = false,
                
                ProjectId = this.ProjectUniqueId,
                
                EnvironmentKind = this.Environment,
                Status = NWDEnvironmentStatus.Active,
                TreatKey = this.TreatKey,
                ProjectKey = this.ProjectKey,
                SecretKey = this.SecretKey,
                
            };
        }
    }
}