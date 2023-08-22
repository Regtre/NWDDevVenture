using NWDFoundation.Configuration.Environments;
using NWDUnityShared.Facades;

namespace NWDUnityTests.Manager
{
    public class NWDUnityTestsEnvironmentManager : INWDUnityEnvironmentManager
    {
        static private object _lock = new object();

        private NWDEnvironmentKind CurrentEnvironement;
        private int CurrentTrack;

        public NWDEnvironmentKind GetCurrentEnvironment()
        {
            lock (_lock)
            {
                return CurrentEnvironement;
            }
        }
        public int GetCurrentDataTrack()
        {
            lock (_lock)
            {
                return CurrentTrack;
            }
        }

        public NWDUnityTestsEnvironmentManager()
        {
            CurrentEnvironement = NWDEnvironmentKind.Dev;
            CurrentTrack = 0;
        }
        public NWDUnityTestsEnvironmentManager(NWDEnvironmentKind sCurrentEnvironement, int sCurrentTrack)
        {
            CurrentEnvironement = sCurrentEnvironement;
            CurrentTrack = sCurrentTrack;
        }
    }
}
