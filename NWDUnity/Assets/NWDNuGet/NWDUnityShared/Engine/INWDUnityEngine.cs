using NWDFoundation.Config;
using NWDUnityShared.Enumerations;
using NWDUnityShared.Facades;
using NWDUnityShared.Tools;

namespace NWDUnityShared.Engine
{
    public interface INWDUnityEngine
    {
        #region State Data
        public NWDAsyncOperation LaunchOperation { get; }
        public NWDConnectionState ConnectionState { get; set; }
        #endregion

        #region Configuration
        public INWDConfig Config { get; }
        #endregion

        #region Managers
        public INWDUnityAccountManager AccountManager { get; }
        public INWDUnityDataManager DataManager { get; }
        public INWDUnityDeviceManager DeviceManager { get; }
        public INWDUnityEnvironmentManager EnvironmentManager { get; }
        public INWDUnityThreadManager ThreadManager { get; }
        public INWDUnityLocalDBManager LocalDBManager { get; }
        #endregion

        #region Launch
        public void Launch();
        #endregion
    }
}
