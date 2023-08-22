using NWDFoundation.Config;
using NWDUnityShared.Engine;
using NWDUnityShared.Enumerations;
using NWDUnityShared.Facades;
using NWDUnityShared.Tools;

namespace NWDUnityTests.Engine
{
    public class NWDEngineUnityTests : INWDUnityEngine
    {
        static private readonly object _lock = new object();

        public NWDConnectionState connectionState = NWDConnectionState.Offline;
        public NWDConnectionState ConnectionState
        {
            get
            {
                lock (_lock)
                {
                    return connectionState;
                }
            }
            set
            {
                lock (_lock)
                {
                    connectionState = value;
                }
            }
        }

        private INWDConfig config;
        private INWDUnityAccountManager accountManager;
        private INWDUnityDataManager dataManager;
        private INWDUnityDeviceManager deviceManager;
        private INWDUnityEnvironmentManager environmentManager;
        private INWDUnityThreadManager asyncManager;

        public INWDConfig Config => config;

        public INWDUnityAccountManager AccountManager => accountManager;

        public INWDUnityDataManager DataManager => dataManager;

        public INWDUnityDeviceManager DeviceManager => deviceManager;

        public INWDUnityEnvironmentManager EnvironmentManager => environmentManager;

        public INWDUnityThreadManager ThreadManager => asyncManager;

        public INWDUnityLocalDBManager LocalDBManager => throw new System.NotImplementedException();

        public NWDAsyncOperation LaunchOperation
        {
            get
            {
                return null;
            }
        }

        public NWDEngineUnityTests(INWDConfig sConfig, INWDUnityAccountManager sAccountManager, INWDUnityDataManager sDataManager, INWDUnityDeviceManager sDeviceManager, INWDUnityEnvironmentManager sEnvironmentManager, INWDUnityThreadManager sAsyncManager)
        {
            config = sConfig;
            accountManager = sAccountManager;
            dataManager = sDataManager;
            deviceManager = sDeviceManager;
            environmentManager = sEnvironmentManager;
            asyncManager = sAsyncManager;
        }

        public void Launch()
        {

        }
    }
}
