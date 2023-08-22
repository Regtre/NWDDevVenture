using NWDFoundation.Configuration.Environments;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDProjectEnvironmentSettings : NWDProjectGlobalSettings
    {
        public NWDEnvironmentKind EnvironmentKind { set; get; } = NWDEnvironmentKind.Dev;

    }
}