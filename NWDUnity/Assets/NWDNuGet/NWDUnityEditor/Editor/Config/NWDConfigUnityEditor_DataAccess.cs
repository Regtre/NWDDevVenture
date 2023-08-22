using Newtonsoft.Json;
using NWDEditor;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Plans;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Tools;
using NWDUnityEditor.UserSettings;
using NWDUnityShared.Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace NWDUnityEditor.Config
{
    public partial class NWDConfigUnityEditor
    {
        #region Lock
        public static readonly object _lock = new object();
        #endregion

        #region Cache
        private NWDRoleUserSettings RoleData = null;
        private NWDEnvironmentUserSettings EnvironmentData = null;
        private string ConfigCache = null;
        private string DeviceUniqueId = null;
        #endregion

        public bool IsReady()
        {
            lock (_lock)
            {
                if (!IsConfigReady.HasValue)
                {
                    IsConfigReady = IsConfigurationReady();
                }
                return IsConfigReady.Value;
            }
        }

        public string GetDefaultWebEditor()
        {
            lock (_lock)
            {
                return kWebEditor;
            }
        }
        public string WebEditorURL()
        {
            lock (_lock)
            {
                string tURL = WebEditorUrl;
                if (string.IsNullOrEmpty(tURL))
                {
                    tURL = kWebEditor;
                }
                return tURL.TrimEnd(new char[] { '/' });
            }
        }

        public string WebServerURLFormat()
        {
            lock (_lock)
            {
                return WebServerUrlFormat;
            }
        }

        public void SetEditorURL(string sURL)
        {
            lock (_lock)
            {
                WebEditorUrl = sURL.TrimEnd(new char[] { '/' });
            }
        }

        public ulong GetProjectId ()
        {
            lock (_lock)
            {
                return ProjectId;
            }
        }

        public string GetConfigName()
        {
            lock (_lock)
            {
                return ConfigName;
            }
        }

        public NWDPlan GetPlan()
        {
            lock (_lock)
            {
                return Plan;
            }
        }

        public string[] GetTagList()
        {
            lock (_lock)
            {
                return TagList;
            }
        }

        #region instance method
        public NWDDataTrackDescription GetSelectedEnvironment()
        {
            lock (_lock)
            {
                if (SelectedDataTrack == null)
                {
                    SelectedDataTrack = DataTracks.FirstOrDefault(x => x.Reference == EnvironmentData.SeletectedEnvironment);

                    if (SelectedDataTrack == null && DataTracks.Length > 0)
                    {
                        SetSelectedEnvironment(DataTracks[0]);
                    }
                }

                return SelectedDataTrack;
            }
        }

        public void SetSelectedEnvironment(NWDDataTrackDescription sDataTrack)
        {
            lock (_lock)
            {
                SelectedDataTrack = null;

                if (sDataTrack != null)
                {
                    SelectedDataTrack = DataTracks.FirstOrDefault(x => x.Reference == sDataTrack.Reference);
                }

                NWDLogger.Debug("Set Data Track: " + SelectedDataTrack?.Reference);
                EnvironmentData.SeletectedEnvironment = SelectedDataTrack?.Reference ?? 0;
                NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(SaveEnvironmentData);
            }
        }

        #region file save
        private void SaveData()
        {
            string tJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(ConfigUnityEditorPath(), tJson);
        }

        private void SaveEnvironmentData ()
        {
            lock (_lock)
            {
                NWDUserSettings.Save(EnvironmentData);
            }
        }

        private void SaveRoleData()
        {
            lock (_lock)
            {
                NWDUserSettings.Save(RoleData);
            }
        }

        #endregion

        public NWDDataTrackDescription[] GetEnvironments()
        {
            lock(_lock)
            {
                return DataTracks;
            }
        }
        public NWDDataTrackDescription[] GetEnvironments(NWDEnvironmentKind sKind)
        {
            lock (_lock)
            {
                return DataTracks.Where(x => x.Kind == sKind).ToArray();
            }
        }
        public NWDDataTrackDescription[] GetEnabledEnvironments()
        {
            lock (_lock)
            {
                return DataTracks;
            }
        }
        public NWDDataTrackDescription[] GetAccessibleEnvironments()
        {
            lock (_lock)
            {
                List<NWDDataTrackDescription> rReturn = new List<NWDDataTrackDescription>();
                NWDTrackRights tRights = NWDTrackRights.None;
                bool tFound = false;
                foreach (NWDDataTrackDescription tEnv in DataTracks)
                {
                    switch (tEnv.Kind)
                    {
                        case NWDEnvironmentKind.Dev:
                            tFound = DevRights.TryGetValue(tEnv.Track, out tRights);
                            break;
                        case NWDEnvironmentKind.PlayTest:
                            break;
                        case NWDEnvironmentKind.Qualification:
                            tFound = QualificationRights.TryGetValue(tEnv.Track, out tRights);
                            break;
                        case NWDEnvironmentKind.PreProduction:
                            tFound = PreprodRights.TryGetValue(tEnv.Track, out tRights);
                            break;
                        case NWDEnvironmentKind.Production:
                            break;
                        case NWDEnvironmentKind.PostProduction:
                            break;
                    }

                    if (tFound && tRights != NWDTrackRights.None)
                    {
                        rReturn.Add(tEnv);
                    }
                }
                return rReturn.ToArray();
            }
        }
        public NWDDataTrackDescription[] GetDisabledEnvironments()
        {
            lock (_lock)
            {
                List<NWDDataTrackDescription> rReturn = new List<NWDDataTrackDescription>();
                //foreach (NWDDataTrackDescription tEnv in AllEnvironment)
                //{
                //    if (tEnv.Enable == false)
                //    {
                //        rReturn.Add(tEnv);
                //    }
                //}
                return rReturn.ToArray();
            }
        }
        public NWDDataTrackDescription GetEnvironment (ulong sReference)
        {
            lock (_lock)
            {
                return DataTracks.FirstOrDefault(x => x.Reference == sReference);
            }
        }
        public Dictionary<NWDEnvironmentKind, NWDDataTrackDescription[]> GetEnvironmentsByKind ()
        {
            lock (_lock)
            {
                if (kDataTrackByEnvironment == null)
                {
                    kDataTrackByEnvironment = new Dictionary<NWDEnvironmentKind, NWDDataTrackDescription[]>();
                }

                if (kDataTrackByEnvironment.Count == 0)
                {
                    kDataTrackByEnvironment = new Dictionary<NWDEnvironmentKind, NWDDataTrackDescription[]>()
                {
                    { NWDEnvironmentKind.Dev, GetEnvironments(NWDEnvironmentKind.Dev) },
                    { NWDEnvironmentKind.PlayTest, GetEnvironments(NWDEnvironmentKind.PlayTest) },
                    { NWDEnvironmentKind.Qualification, GetEnvironments(NWDEnvironmentKind.Qualification) },
                    { NWDEnvironmentKind.PreProduction, GetEnvironments(NWDEnvironmentKind.PreProduction) },
                    { NWDEnvironmentKind.Production, GetEnvironments(NWDEnvironmentKind.Production) },
                    { NWDEnvironmentKind.PostProduction, GetEnvironments(NWDEnvironmentKind.PostProduction) },
                };
                }
                return kDataTrackByEnvironment;
            }
        }

        #endregion

        #region UserSecretToken

        public string GetPrivateRoleToken()
        {
            lock (_lock)
            {
                return RoleData.PrivateToken;
            }
        }
        public string GetPublicRoleToken()
        {
            lock (_lock)
            {
                return RoleData.PublicToken;
            }
        }

        public void SetPrivateRoleToken(string sPrivateToken)
        {
            lock (_lock)
            {
                RoleData.PrivateToken = sPrivateToken;
                NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(SaveRoleData);
            }
        }
        public void SetPublicRoleToken(string sPublicToken)
        {
            lock (_lock)
            {
                RoleData.PublicToken = sPublicToken;
                NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(SaveRoleData);
            }
        }

        public string GetRoleName()
        {
            lock (_lock)
            {
                return RoleData.RoleName;
            }
        }
        public void SetRoleName(string sValue)
        {
            lock (_lock)
            {
                RoleData.RoleName = sValue;
                NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(SaveRoleData);
            }
        }

        public bool GetCanEditMetaDataInfos()
        {
            lock (_lock)
            {
                return RoleData.CanEditMetaData;
            }
        }
        public void SetCanEditMetaDataInfos(bool sValue)
        {
            lock (_lock)
            {
                RoleData.CanEditMetaData = sValue;
                NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(SaveRoleData);
            }
        }

        public bool GetCanCreateMetaData()
        {
            lock (_lock)
            {
                return RoleData.CanCreateMetaData;
            }
        }
        public void SetCanCreateMetaData(bool sValue)
        {
            lock (_lock)
            {
                RoleData.CanCreateMetaData = sValue;
                NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(SaveRoleData);
            }
        }

        private string GetConfigCache()
        {
            lock (_lock)
            {
                return ConfigCache;
            }
        }
        private void SetConfigCache(string sConfigCache)
        {
            lock (_lock)
            {
                ConfigCache = sConfigCache;
            }
        }

        public string GetNickname()
        {
            lock (_lock)
            {
                return RoleData.Nickname;
            }
        }
        public void SetNickname(string sLocalUsername)
        {
            lock (_lock)
            {
                RoleData.Nickname = sLocalUsername;
                NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(SaveRoleData);
            }
        }

        public string GetDeviceUniqueId()
        {
            lock(_lock)
            {
                if (DeviceUniqueId == null)
                {
                    DeviceUniqueId = SystemInfo.deviceUniqueIdentifier;
                }
                return DeviceUniqueId;
            }
        }

        public void ResetRoleTokens()
        {
            UpdateConfig(new NWDDownPayloadGetProjectSettings(), string.Empty, string.Empty, true);
        }

        public string GetProjectKeyInstanceName()
        {
            return GetConfigName();
        }
        public NWDProjectCredentials GetCredentials(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            lock (_lock)
            {
                NWDProjectCredentials rResult = null;
                if (sProjectId == ProjectId)
                {
                    rResult = ProjectCredentials.FirstOrDefault(x => x.EnvironmentKind == sEnvironmentKind);
                }
                return rResult;
            }
        }
        public string GetProjectKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            string rResult = string.Empty;

            NWDProjectCredentials tCredentials = GetCredentials(sProjectId, sEnvironmentKind);
            if (tCredentials != null)
            {
                rResult = tCredentials.ProjectKey;
            }

            return rResult;
        }

        public void UpdateConfig(NWDDownPayloadGetProjectSettings sResponse, string sPublicToken, string sPrivateToken, bool sForceUpdate = false)
        {
            lock (_lock)
            {
                string tOldResponse = GetConfigCache();
                string tNewResponse = JsonConvert.SerializeObject(sResponse);

                if (string.IsNullOrEmpty(tOldResponse))
                {
                    tOldResponse = tNewResponse;
                    ConfigCache = tNewResponse;
                }

                if (sForceUpdate || tOldResponse != tNewResponse)
                {
                    RoleData.PrivateToken = string.Empty;
                    RoleData.PublicToken = string.Empty;

                    RoleData.CanCreateMetaData = false;
                    RoleData.CanEditMetaData = false;

                    ConfigCache = string.Empty;

                    // important set Reference affect NWDProjectPrefs secret key
                    ProjectId = sResponse.ProjectDescription?.ProjectId ?? 0;
                    ProjectCredentials = sResponse.ProjectDescription?.Keys ?? new NWDProjectCredentials[0];
                    if (sResponse.ProjectDescription?.Track != null)
                    {
                        DataTracks = sResponse.ProjectDescription?.Track.OrderBy(x => x.Kind).ToArray();
                    }
                    else
                    {
                        DataTracks = new NWDDataTrackDescription[0];
                    }

                    WebServerUrlFormat = sResponse.ProjectDescription?.ServerDnsHttpsFormat ?? string.Empty;

                    kDataTrackByEnvironment = null;

                    RoleData.PrivateToken = sPrivateToken;
                    RoleData.PublicToken = sPublicToken;

                    RoleData.CanCreateMetaData = true;
                    RoleData.CanEditMetaData = true;

                    ConfigName = sResponse.ProjectDescription?.Name;

                    ConfigCache = tNewResponse;

                    TagList = DefaultTagList;

                    IsConfigReady = IsConfigurationReady();
                    NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(SaveConfiguration);
                }
            }
        }

        public void SaveConfiguration ()
        {
            lock (_lock)
            {
                NWDUnityEngineEditor.Instance.GetLocalDBManager().DeleteDatabase();
                NWDVirtualDataManager.FlusAllWindows();

                SaveData();
                SaveRoleData();
                SaveEnvironmentData();

                NWDUnityEditorWindowReimport.RepaintAll();
                if (IsConfigReady.Value)
                {
                    NWDUnityEngineEditor.Instance.Start();
                }
                else
                {
                    NWDUnityEngineEditor.Instance.Stop();
                }
            }
        }
        #endregion
    }
}