using NWDFoundation.Config;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Exchanges;

namespace NWDUnityTests.Config
{
    public class NWDConfigUnityTests : INWDConfig
    {
        static private object _lock = new object();

        private string DefaultWebEditor;
        private string WebEditor;
        private NWDExchangeDevice DeviceOs;
        private ulong ProjectId;
        private string ProjectKeyName;
        private string ProjectKey;
        private NWDDataTrackDescription SelectedEnvironment;

        public string GetDefaultWebEditor()
        {
            lock (_lock)
            {
                return DefaultWebEditor;
            }
        }

        public NWDExchangeDevice GetDeviceOS()
        {
            lock (_lock)
            {
                return DeviceOs;
            }
        }

        public ulong GetProjectId()
        {
            lock (_lock)
            {
                return ProjectId;
            }
        }

        public string GetProjectKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            lock (_lock)
            {
                return ProjectKey;
            }
        }

        public string GetProjectKeyInstanceName()
        {
            lock (_lock)
            {
                return ProjectKeyName;
            }
        }

        public NWDDataTrackDescription GetSelectedEnvironment()
        {
            lock (_lock)
            {
                return SelectedEnvironment;
            }
        }

        public string WebEditorURL()
        {
            lock (_lock)
            {
                return WebEditor;
            }
        }

        public NWDConfigUnityTests(string sDefaultWebEditor, string sWebEditor, NWDExchangeDevice sDeviceOs, ulong sProjectId, string sProjectKeyName, string sProjectKey, NWDDataTrackDescription sSelectedEnvironment)
        {
            DefaultWebEditor = sDefaultWebEditor;
            WebEditor = sWebEditor;
            DeviceOs = sDeviceOs;
            ProjectId = sProjectId;
            ProjectKeyName = sProjectKeyName;
            ProjectKey = sProjectKey;
            SelectedEnvironment = sSelectedEnvironment;
        }
    }
}

