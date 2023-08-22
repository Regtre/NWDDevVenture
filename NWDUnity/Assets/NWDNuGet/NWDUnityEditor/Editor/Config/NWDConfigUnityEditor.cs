using NWDFoundation.Config;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Facades;
using NWDFoundation.Models;
using NWDFoundation.Plans;
using NWDFoundation.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NWDUnityEditor.Config
{
    [Serializable]
    public partial class NWDConfigUnityEditor : INWDConfig
    {
        #region Constants
        const string kWebEditor = "https://www.net-worked-data.com";

        const string kPrivateRoleTokenKey = nameof(NWDConfigUnityEditor) + "_" + nameof(kPrivateRoleTokenKey);
        const string kPublicRoleTokenKey = nameof(NWDConfigUnityEditor) + "_" + nameof(kPublicRoleTokenKey);
        const string kRoleTokenKeyResponse = "NWDUserSecretTokenResponse_dsqejhfbyvrztjkhtsrtegvbehkrbvkuer";

        const string kRoleNameKey = nameof(NWDConfigUnityEditor) + "_" + nameof(kRoleNameKey);
        const string kCanEditMetaDataInfosKey = nameof(NWDConfigUnityEditor) + "_" + nameof(kCanEditMetaDataInfosKey);
        const string kCanCreateMetaDataKey = nameof(NWDConfigUnityEditor) + "_" + nameof(kCanCreateMetaDataKey);
        const string kNicknameKey = nameof(NWDConfigUnityEditor) + "_" + nameof(kNicknameKey);

        const string KEnvironementSelectedKey = "KEnvironementSelectedKey";

        private readonly string[] DefaultTagList = new string[] {
            "No Tag",
            "tag 0",
            "tag 1",
            "tag 2",
            "tag 3",
            "tag 4",
            "tag 5",
            "tag 6",
            "tag 7",
            "tag 8",
            "tag 9",
            "tag 10",
            };
        #endregion

        #region Properties
        public string Commit { set; get; } = string.Empty;
        public string ConfigName { set; get; } = string.Empty;
        public NWDPlan Plan { set; get; } = NWDPlan.Standard;
        public string WebEditorUrl { set; get; } = string.Empty;
        public string WebServerUrlFormat { set; get; } = string.Empty;
        public ulong ProjectId { set; get; } = NWDRandom.UnsignedLongNumeric(32);
        public NWDProjectCredentials[] ProjectCredentials = new NWDProjectCredentials[0];

        public NWDDataTrackDescription[] DataTracks = new NWDDataTrackDescription[0];
        public Dictionary<int, NWDTrackRights> DevRights { get; set; }
        public Dictionary<int, NWDTrackRights> PreprodRights { get; set; }
        public Dictionary<int, NWDTrackRights> QualificationRights { get; set; }

        public string[] TagList { get; set; } = null;
        #endregion

        #region Cache
        private bool? IsConfigReady = null;
        private NWDDataTrackDescription SelectedDataTrack = null;
        static private Dictionary<NWDEnvironmentKind, NWDDataTrackDescription[]> kDataTrackByEnvironment = null;

        #endregion

        #region constructors

        public NWDConfigUnityEditor()
        {
            ConfigName = "Config UnityEditor" + "<" + Application.platform.ToString() + ">";
        }

        #endregion
    }
}