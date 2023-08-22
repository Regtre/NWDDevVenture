using NWDFoundation.Configuration.Environments;

namespace NWDUnityShared.Facades
{
    public interface INWDUnityEnvironmentManager
    {
        public NWDEnvironmentKind GetCurrentEnvironment();
        public int GetCurrentDataTrack();
    }
}
