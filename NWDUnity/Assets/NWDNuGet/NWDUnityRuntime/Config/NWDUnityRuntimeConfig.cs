using NWDFoundation.Config;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Exchanges;
using System;
using UnityEngine;

namespace NWDUnityRuntime.Config
{
    [Serializable]
    public class NWDUnityRuntimeConfig : INWDConfig
    {
        #region Lock
        private static readonly object _lock = new object();
        #endregion

        public string WebEditor;
        public ulong ProjectId;
        public string ProjectKey;

        public NWDDataTrackDescription Track;

        public string GetDefaultWebEditor()
        {
            return WebEditorURL();
        }

        public NWDExchangeDevice GetDeviceOS()
        {
            lock (_lock)
            {
                RuntimePlatform tPlatform = Application.platform;

                switch (tPlatform)
                {
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                    case RuntimePlatform.OSXServer:
                        return NWDExchangeDevice.Macos;
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.WindowsServer:
                        return NWDExchangeDevice.Windows;
                    case RuntimePlatform.LinuxEditor:
                    case RuntimePlatform.LinuxPlayer:
                    case RuntimePlatform.LinuxServer:
                    case RuntimePlatform.EmbeddedLinuxArm32:
                    case RuntimePlatform.EmbeddedLinuxArm64:
                    case RuntimePlatform.EmbeddedLinuxX64:
                    case RuntimePlatform.EmbeddedLinuxX86:
                        return NWDExchangeDevice.Linux;
                    case RuntimePlatform.Android:
                        return NWDExchangeDevice.Android;
                    case RuntimePlatform.IPhonePlayer:
                        return NWDExchangeDevice.Ios;
                    case RuntimePlatform.WebGLPlayer:
                        return NWDExchangeDevice.Web;
                    default:
                        return NWDExchangeDevice.Unknown;

                }
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
                string rResult = null;
                if (sProjectId == ProjectId && Track.Kind == sEnvironmentKind)
                {
                    rResult = ProjectKey;
                }

                return rResult;
            }
        }

        public string GetProjectKeyInstanceName()
        {
            return "Runtime Config";
        }

        public string WebEditorURL()
        {
            lock (_lock)
            {
                return WebEditor;
            }
        }

        public NWDDataTrackDescription GetSelectedEnvironment()
        {
            lock (_lock)
            {
                return Track;
            }
        }
    }
}
