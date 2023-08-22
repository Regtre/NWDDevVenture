using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Tools;
using NWDUnityEditor.UserSettings;
using NWDUnityShared.Constants;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Config
{
    public partial class NWDConfigUnityEditor
    {
        #region Cache
        private NWDWindowUserSettings WindowData;
        #endregion

        #region file paths

        private static string ConfigUnityEditorPath()
        {
            return NWDConstants.K_NWDConfigUnityEditorPath;
        }

        private static string ConfigUnityRuntimePath(bool sForBuild)
        {
            string rReturn = "";
            if (sForBuild == true)
            {
                CreateStreamingAssetsFolder();
                rReturn = Application.streamingAssetsPath + "/" + NWDConstants.K_NWDConfigUnityRuntimePath;
            }
            else
            {

                rReturn = Application.temporaryCachePath + "/" + NWDConstants.K_NWDConfigUnityRuntimePath;
            }
            return rReturn;
        }

        static private void CreateStreamingAssetsFolder()
        {
            if (Directory.Exists(Application.streamingAssetsPath) == false)
            {
                AssetDatabase.CreateFolder(NWDConstantsUnityRuntime.K_Assets, NWDConstantsUnityRuntime.K_StreamingAssets);
                AssetDatabase.ImportAsset(Application.streamingAssetsPath);
                AssetDatabase.Refresh();
            }
        }

        static string JSONLoadConfig()
        {
            string rReturn = string.Empty;
            string tPath = ConfigUnityEditorPath();
            if (File.Exists(tPath) == true)
            {
                rReturn = File.ReadAllText(tPath);
            }
            else
            {
                NWDLogger.Warning("FILE NOT EXISTS AT " + tPath);
            }
            return rReturn;
        }

        static public NWDConfigUnityEditor LoadConfig()
        {
            CreateStreamingAssetsFolder();
            NWDConfigUnityEditor rReturn = null;
            string tJSON = JSONLoadConfig();
            rReturn = (NWDConfigUnityEditor)JsonConvert.DeserializeObject(tJSON, typeof(NWDConfigUnityEditor));
            if (rReturn == null)
            {
                NWDLogger.Warning(typeof(NWDConfigUnityEditor).Name + " LoadConfig() rReturn is null");
                rReturn = new NWDConfigUnityEditor();
            }

            rReturn.RoleData = NWDUserSettings.LoadOrDefault(new NWDRoleUserSettings());
            rReturn.EnvironmentData = NWDUserSettings.LoadOrDefault(new NWDEnvironmentUserSettings());
            rReturn.WindowData = NWDUserSettings.LoadOrDefault(new NWDWindowUserSettings());

            return rReturn;
        }

        #endregion


        public NWDUnityEditorWindowStyle GetWindowStyle()
        {
            return WindowData.Style;
        }

        public void SetWindowStyle(NWDUnityEditorWindowStyle sValue)
        {
            WindowData.Style = sValue;
            NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(() => NWDUserSettings.Save(WindowData));
        }

        public bool GetShowLogo()
        {
            return WindowData.ShowLogo;
        }

        public void SetShowLogo(bool sValue)
        {
            WindowData.ShowLogo = sValue;
            NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(() => NWDUserSettings.Save(WindowData));
        }
        public void ManagePlanByWebsite()
        {
            Application.OpenURL(WebEditorURL());
        }
        private bool IsConfigurationReady()
        {
            lock (_lock)
            {
                bool rResult = true;

                if (string.IsNullOrWhiteSpace(RoleData.PublicToken))
                {
                    rResult = false;
                }
                if (string.IsNullOrWhiteSpace(RoleData.PrivateToken))
                {
                    rResult = false;
                }
                if (string.IsNullOrWhiteSpace(RoleData.Nickname))
                {
                    rResult = false;
                }

                return rResult;
            }
        }

        public NWDExchangeDevice GetDeviceOS()
        {
            lock(_lock)
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
    }
}