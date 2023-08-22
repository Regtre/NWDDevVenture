using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDUnityShared.Engine;
using NWDUnityShared.Facades;

namespace NWDUnityRuntime.Managers
{
    public class NWDUnityRuntimeEnvironmentManager : INWDUnityEnvironmentManager
    {
        public int GetCurrentDataTrack()
        {
            NWDDataTrackDescription tDescription = NWDUnityEngine.Instance.Config.GetSelectedEnvironment();
            return tDescription.Track;
        }

        public NWDEnvironmentKind GetCurrentEnvironment()
        {
            NWDDataTrackDescription tDescription = NWDUnityEngine.Instance.Config.GetSelectedEnvironment();
            return tDescription.Kind;
        }
    }
}
