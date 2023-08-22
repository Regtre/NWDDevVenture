using NWDUnityEditor.Engine;
using NWDUnityEditor.Tools;
using NWDUnityShared.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NWDFoundation.Configuration.Permissions
{
    static class NWDDataTrackDescriptionExtension
    {
        static private readonly Color kSelectedTrackColor = new Color32(180, 179, 255, 255);
        static Texture2D Wagon = null;
        static Texture2D Locomotive = null;
        static Texture2D Tunnel = null;
        static Texture2D Railway = null;

        static private Dictionary<NWDDataTrackDescription, NWDUnityEditorMultiGUIContent> kGUIContent = new Dictionary<NWDDataTrackDescription, NWDUnityEditorMultiGUIContent>();
        static private Dictionary<ulong, List<NWDDataTrackDescription>> kStalkers = new Dictionary<ulong, List<NWDDataTrackDescription>>();

        static public void RefereshGUIContent(this NWDDataTrackDescription sObject)
        {
            if (kGUIContent.ContainsKey(sObject) == true)
            {
                kGUIContent.Remove(sObject);
            }
        }

        static public GUIContent GetGUIContent(this NWDDataTrackDescription sObject)
        {
            GUIContent rReturn = null;
            if (kGUIContent.ContainsKey(sObject) == true)
            {
                rReturn = kGUIContent[sObject].GetContent();
            }
            else
            {
                NWDUnityEditorMultiGUIContent tReturn = NWDUnityEditorMultiGUIContent.NewContent("NWDEnvironment" + sObject.Kind.ToString(), sObject.Name, "Environment " + sObject.Name + " (type " + sObject.Kind.ToString() + ")");
                kGUIContent.Add(sObject, tReturn);
                rReturn = tReturn.GetContent();
            }
            return rReturn;
        }

        static public void DrawTunnel(this NWDDataTrackDescription sObject, Rect sRect, bool sCurrentTrack)
        {
            if (Tunnel == null)
            {
                string[] tGUIDs = AssetDatabase.FindAssets("Tunnel t:texture");
                if (tGUIDs.Length > 0)
                {
                    Tunnel = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(tGUIDs[0])) as Texture2D;
                }
            }

            DrawImage(sRect, Texture2D.whiteTexture, Tunnel, NWDConverter.ColorFromString(sObject.Color), sCurrentTrack ? kSelectedTrackColor : Color.white);
        }

        static public void DrawRailway (this NWDDataTrackDescription sObject, Rect sRect, bool sCurrentTrack)
        {
            if (Railway == null)
            {
                string[] tGUIDs = AssetDatabase.FindAssets("Railway t:texture");
                if (tGUIDs.Length > 0)
                {
                    Railway = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(tGUIDs[0])) as Texture2D;
                }
            }

            GUI.DrawTexture(sRect, Railway, ScaleMode.StretchToFill, true, 1, sCurrentTrack ? kSelectedTrackColor : Color.white, 0, 0);
        }

        static public void DrawLocomotive(this NWDDataTrackDescription sObject, Rect sRect, bool sCurrentTrack)
        {
            if (Locomotive == null)
            {
                string[] tGUIDs = AssetDatabase.FindAssets("Locomotive t:texture");
                if (tGUIDs.Length > 0)
                {
                    Locomotive = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(tGUIDs[0])) as Texture2D;
                }
            }

            DrawImage(sRect, Texture2D.whiteTexture, Locomotive, NWDConverter.ColorFromString(sObject.Color), sCurrentTrack ? kSelectedTrackColor : Color.white);
        }

        static public void DrawWagon (this NWDDataTrackDescription sObject, Rect sRect, bool sCurrentTrack)
        {
            if (Wagon == null)
            {
                string[] tGUIDs = AssetDatabase.FindAssets("Wagon t:texture");
                if (tGUIDs.Length > 0)
                {
                    Wagon = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(tGUIDs[0])) as Texture2D;
                }
            }

            DrawImage(sRect, Texture2D.whiteTexture, Wagon, NWDConverter.ColorFromString(sObject.Color), sCurrentTrack ? kSelectedTrackColor : Color.white);
        }

        static private void DrawImage (Rect sRect, Texture sBackground, Texture sForeground, Color sBackgroundColor, Color sForegroundColor)
        {

            GUI.DrawTexture(sRect, sBackground, ScaleMode.ScaleToFit, true, 1, sBackgroundColor, 0, 0);
            GUI.DrawTexture(sRect, sForeground, ScaleMode.ScaleToFit, true, 1, sForegroundColor, 0, 0);
        }

        static public int GetHashCode (this NWDDataTrackDescription sObject)
        {
            return (sObject.Kind.ToString() + sObject.Track.ToString()).GetHashCode();
        }

        static public void ResolveStalkers (NWDDataTrackDescription[] sDataTracks)
        {
            kStalkers = sDataTracks.ToDictionary(x => x.Reference, y => new List<NWDDataTrackDescription>());

            foreach (NWDDataTrackDescription sDataTrack in sDataTracks)
            {
                NWDDataTrackDescription tStalkedTrack = NWDUnityEngineEditor.Instance.GetConfig().GetEnvironment(sDataTrack.StalkedReference);

                if (tStalkedTrack != null)
                {
                    kStalkers[tStalkedTrack.Reference].Add(sDataTrack);
                }
            }
        }

        static public List<NWDDataTrackDescription> GetStalkers (this NWDDataTrackDescription sObject)
        {

            if (NWDUnityEngineEditor.Instance.GetConfig().GetEnvironments().Length != kStalkers.Count)
            {
                ResolveStalkers(NWDUnityEngineEditor.Instance.GetConfig().GetEnvironments());
            }

            List<NWDDataTrackDescription> rResult;
            if (!kStalkers.TryGetValue(sObject.Reference, out rResult))
            {
                rResult = new List<NWDDataTrackDescription> ();
            }

            return rResult;
        }

        //static public void GUILayoutLabel(this NWDEnvironmentUnityEditor sObject)
        //{
        //    GUIStyle KenvStyle = new GUIStyle(EditorStyles.helpBox);
        //    KenvStyle.fixedHeight = 24;
        //    KenvStyle.alignment = TextAnchor.MiddleCenter;

        //    NWDEnvironmentDefinition tEnvSelectedDefinition = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironmentDefinition();
        //    if (sObject.EnvName == tEnvSelectedDefinition)
        //    {
        //        NWDGUI.BeginRedArea();
        //        GUILayout.Label(sObject.GetGUIContent(), KenvStyle);
        //        NWDGUI.EndRedArea();
        //    }
        //    else
        //    {
        //        GUILayout.Label(sObject.GetGUIContent(), KenvStyle);
        //    }
        //}

        //static public NWDEnvironmentRights GetAccessRights(this NWDEnvironmentUnityEditor sObject)
        //{
        //    return NWDEnvironmentAccessExtension.GetEnvironmentAccess(sObject.EnvName).Rights;
        //}

        //static public void SetAccessRights(this NWDEnvironmentUnityEditor sObject, NWDEnvironmentRights sRights)
        //{
        //    NWDEnvironmentAccess tAccess = NWDEnvironmentAccessExtension.GetEnvironmentAccess(sObject.EnvName);
        //    tAccess.Rights = sRights;
        //    tAccess.Save();
        //}

        //static public void LayoutInfosForm(this NWDEnvironmentUnityEditor sEnvironment)
        //{
        //    if (sEnvironment.GetAccessRights() == NWDEnvironmentRights.None)
        //    {
        //        NWDGUILayout.DrawMessageNoRights();
        //    }
        //    else
        //    {

        //        EditorGUI.EndDisabledGroup();
        //        EditorGUI.BeginDisabledGroup(sEnvironment.GetAccessRights() == NWDEnvironmentRights.Read);

        //        NWDGUILayout.Section("User rights");
        //        EditorGUILayout.LabelField("Rights", sEnvironment.GetAccessRights().ToString());

        //        NWDGUILayout.Section("Activation for " + sEnvironment.EnvName.ToString() + " environment");

        //        EditorGUILayout.LabelField("Environment type", sEnvironment.EnvType.ToString());
        //        EditorGUILayout.LabelField("Environment name", sEnvironment.EnvName.ToString());

        //        EditorGUILayout.LabelField("Environment little name", sEnvironment.InternalName);

        //        //bool tEnable = EditorGUILayout.Toggle("Enable", sEnvironment.Enable);
        //        //{
        //        //    if (tEnable != sEnvironment.Enable)
        //        //    {
        //        //        sEnvironment.Enable = tEnable;
        //        //        if (sEnvironment.Enable == false && sEnvironment.Selected == true)
        //        //        {
        //        //            sEnvironment.Selected = false;
        //        //            NWDUnityEngineEditor.Instance.GetConfig().ActiveEnvironement(NWDEnvironmentDefinition.Preview);
        //        //        }
        //        //    }
        //        //}

        //        NWDGUILayout.Section("Runtime " + sEnvironment.EnvName.ToString());

        //        //EditorGUI.BeginDisabledGroup(NWDEnvironmentToolbox.CanBeUseForRuntime(sEnvironment.EnvName) == false);
        //        //bool tSelected = EditorGUILayout.Toggle("Selected to runtime", sEnvironment.Selected);
        //        //if (tSelected == true)
        //        //{
        //        //    if (sEnvironment.Selected == false)
        //        //    {
        //        //        NWDUnityEngineEditor.Instance.GetConfig().ActiveEnvironement(sEnvironment.EnvName);
        //        //    }
        //        //}
        //        //EditorGUI.EndDisabledGroup();

        //        sEnvironment.LauncherFaster = EditorGUILayout.IntSlider("Launcher faster", sEnvironment.LauncherFaster, 1, 10);
        //        if (sEnvironment.LauncherFaster < 1)
        //        {
        //            sEnvironment.LauncherFaster = 1;
        //        }
        //        NWDGUILayout.Section("Account");
        //        NWDGUILayout.SubSection("Account anonymous");
        //        sEnvironment.AnonymousDeviceConnected = EditorGUILayout.ToggleLeft("Anonymous account connected from system device!", sEnvironment.AnonymousDeviceConnected);
        //        sEnvironment.PurgeOldAccountDatabase = EditorGUILayout.ToggleLeft("Purge old account", sEnvironment.PurgeOldAccountDatabase);

        //        NWDGUILayout.SubSection("App identity " + sEnvironment.EnvName.ToString());
        //        sEnvironment.AppName = EditorGUILayout.TextField("AppName", sEnvironment.AppName);
        //        sEnvironment.PreProdTimeFormat = EditorGUILayout.TextField("Preprod time format", sEnvironment.PreProdTimeFormat);

        //        NWDGUILayout.SubSection("IP Ban " + sEnvironment.EnvName.ToString());
        //        sEnvironment.IPBanActive = EditorGUILayout.Toggle("IPBan active", sEnvironment.IPBanActive);
        //        EditorGUI.BeginDisabledGroup(!sEnvironment.IPBanActive);
        //        sEnvironment.IPBanMaxTentative = EditorGUILayout.IntField("Max tentative", sEnvironment.IPBanMaxTentative);
        //        sEnvironment.IPBanTimer = EditorGUILayout.IntField("Timer", sEnvironment.IPBanTimer);
        //        EditorGUI.EndDisabledGroup();

        //        NWDGUILayout.SubSection("Security of datas" + sEnvironment.EnvName.ToString());
        //        sEnvironment.DataSHAPassword = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("SHA password", sEnvironment.DataSHAPassword));
        //        sEnvironment.DataSHAVector = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("SHA vector", sEnvironment.DataSHAVector));

        //        NWDGUILayout.SubSection("Hash of datas" + sEnvironment.EnvName.ToString());
        //        sEnvironment.SaltStart = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt start", sEnvironment.SaltStart));
        //        sEnvironment.SaltEnd = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt end", sEnvironment.SaltEnd));
        //        sEnvironment.SaltFrequency = EditorGUILayout.IntField("Salt frequency", sEnvironment.SaltFrequency);

        //        NWDGUILayout.SubSection("Log mode " + sEnvironment.EnvName.ToString());
        //        sEnvironment.LogMode = (NWDLogMode)EditorGUILayout.EnumPopup("Log mode", sEnvironment.LogMode);


        //        NWDGUILayout.SubSection("Time multiplicator for " + sEnvironment.EnvName.ToString());
        //        sEnvironment.SpeedOfGameTime = EditorGUILayout.FloatField("Speed of game time", sEnvironment.SpeedOfGameTime);


        //        NWDGUILayout.SubSection("Last build infos " + sEnvironment.EnvName.ToString());


        //        sEnvironment.LayoutWebHookForm();

        //        EditorGUI.EndDisabledGroup();
        //    }
        //}

        //static public void LayoutClusterForm(this NWDEnvironmentUnityEditor sEnvironment)
        //{
        //    if (sEnvironment.GetAccessRights() == NWDEnvironmentRights.None)
        //    {
        //        NWDGUILayout.DrawMessageNoRights();
        //    }
        //    else
        //    {
        //        EditorGUI.BeginDisabledGroup(sEnvironment.GetAccessRights() == NWDEnvironmentRights.Read);
        //        NWDGUILayout.Section("Cluster mode for " + sEnvironment.EnvName.ToString() + " environment");

        //        sEnvironment.Cluster = (NWDEnvironmentClusterEnum)EditorGUILayout.EnumPopup("Cluster mode", sEnvironment.Cluster);
        //        if (sEnvironment.Cluster == NWDEnvironmentClusterEnum.Localhost)
        //        {
        //            sEnvironment.DatabaseClusterSimulate = EditorGUILayout.IntSlider("Simulate database", sEnvironment.DatabaseClusterSimulate, 1, 5);
        //            GUILayout.BeginHorizontal();
        //            //if (GUILayout.Button("test on localhost"))
        //            //{
        //            //    //NWDRequest tRequest = new NWDRequest();
        //            //    //tRequest.SynchronizeServerAsync(delegate (string sResponse)
        //            //    //{

        //            //    //    Debug.LogWarning("sResponse = " + sResponse);
        //            //    //});
        //            //}
        //            GUILayout.EndHorizontal();
        //        }

        //        NWDGUILayout.SubSection("Network ping tester " + sEnvironment.EnvName.ToString());
        //        sEnvironment.AddressPing = EditorGUILayout.TextField("Address ping (8.8.8.8)", sEnvironment.AddressPing);

        //        NWDGUILayout.SubSection("Server params for " + sEnvironment.EnvName.ToString());
        //        sEnvironment.AlwaysSecureData = EditorGUILayout.Toggle("Always secure data", sEnvironment.AlwaysSecureData);
        //        //sAppEnvironment.ServerLanguage = (NWDServerLanguage)EditorGUILayout.EnumPopup("Server language", sAppEnvironment.ServerLanguage);
        //        sEnvironment.WebTimeOut = EditorGUILayout.IntField("Timeout request", sEnvironment.WebTimeOut);
        //        sEnvironment.EditorWebTimeOut = EditorGUILayout.IntField("Editor timeout request", sEnvironment.EditorWebTimeOut);
        //        sEnvironment.LoadBalancingLimit = EditorGUILayout.IntField("Balance load", sEnvironment.LoadBalancingLimit);

        //        NWDGUILayout.SubSection("Token historic limit for " + sEnvironment.EnvName.ToString());
        //        sEnvironment.TokenHistoric = EditorGUILayout.IntSlider("Token number", sEnvironment.TokenHistoric, 1, 10);
        //        EditorGUI.EndDisabledGroup();
        //    }
        //}


        //static public void LayoutLocalizationForm(this NWDEnvironmentUnityEditor sEnvironment)
        //{
        //    if (sEnvironment.GetAccessRights() == NWDEnvironmentRights.None)
        //    {
        //        NWDGUILayout.DrawMessageNoRights();
        //    }
        //    else
        //    {
        //        EditorGUI.BeginDisabledGroup(sEnvironment.GetAccessRights() == NWDEnvironmentRights.Read);
        //        NWDGUILayout.Section("Languages activation");

        //        Dictionary<string, string> tLanguageDico = NWDLocalizationISO.LanguageDico;
        //        GUILayout.BeginHorizontal();
        //        List<string> tResult = new List<string>();
        //        int tColunm = 0;
        //        int tColunmMax = 3;
        //        foreach (KeyValuePair<string, string> tKeyValue in tLanguageDico)
        //        {
        //            if (tColunmMax <= tColunm)
        //            {
        //                tColunm = 0;
        //                GUILayout.EndHorizontal();
        //                GUILayout.BeginHorizontal();
        //            }
        //            tColunm++;
        //            bool tContains = Array.IndexOf(sEnvironment.DataLocalizationManager.LanguagesString, tKeyValue.Value) > -1;
        //            tContains = EditorGUILayout.ToggleLeft(tKeyValue.Key, tContains, GUILayout.MaxWidth(160), GUILayout.MinWidth(100));
        //            if (tContains == true)
        //            {
        //                tResult.Add(tKeyValue.Value);
        //            }
        //        }
        //        GUILayout.EndHorizontal();
        //        if (tResult.Count == 0)
        //        {
        //            tResult.Add(sEnvironment.DataLocalizationManager.Default);
        //        }

        //        tResult.Add(NWDLocalizationISO.kBaseDev);
        //        tResult.Sort();

        //        NWDGUILayout.Section(NWDConstantsUnityEditor.K_APP_CONFIGURATION_DEV_LOCALALIZATION_AREA);
        //        NWDGUILayout.Label("Select the default language of the app.");
        //        int tIndex = tResult.IndexOf(sEnvironment.DataLocalizationManager.Default);
        //        if (tIndex < 0)
        //        {
        //            tIndex = 0;
        //        }
        //        int tSelect = EditorGUILayout.Popup(NWDConstantsUnityEditor.K_APP_CONFIGURATION_DEV_LOCALALIZATION_CHOOSE, tIndex, tResult.ToArray());
        //        sEnvironment.DataLocalizationManager.Default = tResult[tSelect];
        //        if (
        //        tResult.Contains(sEnvironment.DataLocalizationManager.Default) == false)
        //        {
        //            tResult.Add(sEnvironment.DataLocalizationManager.Default);
        //        }

        //        sEnvironment.DataLocalizationManager.LanguagesString = tResult.ToArray();

        //        NWDGUILayout.Section("Special localizations operations");
        //        NWDGUILayout.Label("Reorder all localizations for all datas (to see the same order in all datas).");
        //        if (GUILayout.Button("Reorder all localizations"))
        //        {
        //            // TODO: ReOrderAllLocalizations
        //            //NWDAppConfiguration.SharedInstance().DataLocalizationManager.ReOrderAllLocalizations();
        //            if (EditorUtility.DisplayDialog("ALERTE", "must reactive ReOrderAllLocalizations", "Cancel")) { }
        //        }

        //        NWDGUILayout.Label("Export all localizations in CSV's file to send to translate.");
        //        if (GUILayout.Button("Export localizations in CSV's file"))
        //        {
        //            // TODO: ExportToCSV
        //            //NWDAppConfiguration.SharedInstance().DataLocalizationManager.ExportToCSV();
        //            if (EditorUtility.DisplayDialog("ALERTE", "must reactive ExportToCSV", "Cancel")) { }
        //        }

        //        NWDGUILayout.Label("Import all localizations translated from CSV's file.");
        //        if (GUILayout.Button("Import localizations from CSV's file"))
        //        {
        //            // TODO: ImportFromCSV
        //            //NWDAppConfiguration.SharedInstance().DataLocalizationManager.ImportFromCSV();
        //            if (EditorUtility.DisplayDialog("ALERTE", "must reactive ImportFromCSV", "Cancel")) { }
        //        }


        //        NWDGUILayout.Section("Copy localization configuration to another environment");
        //        GUIContent tButtonTitleHere = new GUIContent(sEnvironment.GetGUIContent());
        //        tButtonTitleHere.text = "Copy to everywhere ";
        //        NWDGUILayout.BigSpace();
        //        if (GUILayout.Button(tButtonTitleHere, GUILayout.Height(30), GUILayout.Width(300)))
        //        {
        //            foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnvironments())
        //            {
        //                if (sEnvironment != tEnv)
        //                {
        //                    tEnv.DataLocalizationManager.LanguagesString = (string[])sEnvironment.DataLocalizationManager.LanguagesString.Clone();
        //                    tEnv.DataLocalizationManager.Default = string.Copy(sEnvironment.DataLocalizationManager.Default);
        //                }
        //            }
        //        }
        //        NWDGUILayout.BigSpace();
        //        NWDGUILayout.Line();
        //        NWDGUILayout.BigSpace();
        //        foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnvironments())
        //        {
        //            if (sEnvironment != tEnv)
        //            {
        //                GUIContent tButtonTitle = new GUIContent(tEnv.GetGUIContent());
        //                tButtonTitle.text = "Copy to " + tEnv.InternalName;
        //                if (GUILayout.Button(tButtonTitle, GUILayout.Height(30), GUILayout.Width(300)))
        //                {
        //                    tEnv.DataLocalizationManager.LanguagesString = (string[])sEnvironment.DataLocalizationManager.LanguagesString.Clone();
        //                    tEnv.DataLocalizationManager.Default = string.Copy(sEnvironment.DataLocalizationManager.Default);
        //                }
        //            }
        //        }
        //        NWDGUILayout.BigSpace();
        //        EditorGUI.EndDisabledGroup();
        //    }
        //}

        //static public void LayoutDatabaseForm(this NWDEnvironmentUnityEditor sEnvironment)
        //{

        //    if (sEnvironment.GetAccessRights() == NWDEnvironmentRights.None)
        //    {
        //        NWDGUILayout.DrawMessageNoRights();
        //    }
        //    else
        //    {
        //        EditorGUI.BeginDisabledGroup(sEnvironment.GetAccessRights() == NWDEnvironmentRights.Read);
        //        NWDGUILayout.SubSection("Databases used for " + sEnvironment.EnvName.ToString());

        //        NWDGUILayout.Label("Always copy the database in asset streaming folder on build");

        //        string tStudioDatabaseFolder = NWDDatabasePathTools.GetDatabaseName(NWDToolboxUnityEditor.GetDatabaseEditorPath() + "/" + NWDToolbox.UnixCleaner(sEnvironment.EnvName.ToString()), NWDDatabaseDestination.StudioDatabase);
        //        NWDGUILayout.Label("StudioDatabase for " + sEnvironment.EnvName.ToString());
        //        RevealDatabase(tStudioDatabaseFolder, "StudioDatabase");

        //        string tPlayerDatabaseFolder = NWDDatabasePathTools.GetDatabaseName(NWDToolboxUnityEditor.GetDatabaseEditorPath() + "/" + NWDToolbox.UnixCleaner(sEnvironment.EnvName.ToString()), NWDDatabaseDestination.PlayerDatabase);
        //        NWDGUILayout.Label("PlayerDatabase for " + sEnvironment.EnvName.ToString());
        //        RevealDatabase(tPlayerDatabaseFolder, "PlayerDatabase");

        //        NWDGUILayout.SubSection("SQL thread Activation " + sEnvironment.EnvName.ToString());
        //        sEnvironment.ThreadPoolForce = EditorGUILayout.Toggle("SQL Thread", sEnvironment.ThreadPoolForce);
        //        sEnvironment.WritingModeLocal = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing local", (NWDWritingModeConfig)sEnvironment.WritingModeLocal);
        //        sEnvironment.WritingModeWebService = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing webservice", (NWDWritingModeConfig)sEnvironment.WritingModeWebService);
        //        sEnvironment.WritingModeEditor = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing editor", (NWDWritingModeConfig)sEnvironment.WritingModeEditor);


        //        NWDGUILayout.SubSection("Action");

        //        if (sEnvironment.EnvType == NWDEnvironmentType.Playtest)
        //        {
        //            NWDGUILayout.Label("Reset all database Editor (TODO)");
        //            NWDGUILayout.Label("Reset all database Player (TODO)");
        //            NWDGUILayout.Label("replace by data from preview (TODO ... limit by user rights)");
        //            NWDGUILayout.Label("replace by data from another playtest (TODO ... limit by user rights)");

        //            NWDGUILayout.Label("insert all unknow data from preview (TODO ... limit by user rights)");
        //            NWDGUILayout.Label("insert all unknow data another playtest (TODO ... limit by user rights)");



        //        }
        //        EditorGUI.EndDisabledGroup();
        //    }
        //}

        //static public void LayoutFakeAccountForm(this NWDEnvironmentUnityEditor sEnvironment, NWDUnityEditorWindowBasisState sState, ref Vector2 sScrollPosition)
        //{
        //    if (sEnvironment.GetAccessRights() == NWDEnvironmentRights.None)
        //    {
        //        NWDGUILayout.DrawMessageNoRights();
        //    }
        //    else
        //    {
        //        EditorGUI.BeginDisabledGroup(sEnvironment.GetAccessRights() == NWDEnvironmentRights.Read);
        //        NWDGUILayout.Section("Fake accounts");
        //        if (sState == NWDUnityEditorWindowBasisState.InEditorMode)
        //        {
        //            List<string> tLanguageKey = new List<string>();
        //            List<string> tLanguageCode = new List<string>();
        //            foreach (KeyValuePair<string, string> tKV in NWDLocalizationISO.LanguageDico)
        //            {
        //                tLanguageKey.Add(tKV.Key);
        //                tLanguageCode.Add(tKV.Value);
        //            }

        //            List<string> tCountryKey = new List<string>();
        //            List<string> tCountryCode = new List<string>();
        //            foreach (NWDCountryISO tCountry in NWDCountryISO.List)
        //            {
        //                tCountryKey.Add(tCountry.Name);
        //                tCountryCode.Add(tCountry.TwoLetterCode);
        //            }

        //            if (NWDPlanResumeGUI.GetPlanResumeGUI(NWDUnityEngineEditor.Instance.GetConfig().Plan).DrawIfConnected())
        //            {
        //                EditorGUI.BeginChangeCheck();
        //                if (NWDEnvironmentToolbox.CanBeUseForRuntime(sEnvironment.EnvName))
        //                {
        //                    sScrollPosition = GUILayout.BeginScrollView(sScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        //                    NWDFakeAccount tToDelete = null;
        //                    foreach (NWDFakeAccount tAccount in sEnvironment.AccountsToTest)
        //                    {
        //                        NWDGUILayout.LineWhite();
        //                        NWDGUILayout.SubSection(tAccount.InternaleName);
        //                        GUILayout.BeginHorizontal();
        //                        //---------
        //                        GUILayout.BeginVertical();
        //                        GUILayout.Label("Device", NWDGUI.KTableSearchTitle);
        //                        EditorGUILayout.LabelField("Device " + tAccount.DeviceID);
        //                        EditorGUILayout.LabelField("Account", tAccount.Account.ToString());
        //                        tAccount.InternaleName = EditorGUILayout.TextField("Internal name", tAccount.InternaleName);
        //                        NWDGUILayout.BigSpace();
        //                        GUILayout.EndVertical();
        //                        //---------
        //                        GUILayout.BeginVertical();
        //                        GUILayout.Label("Options", NWDGUI.KTableSearchTitle);
        //                        tAccount.Email = EditorGUILayout.TextField("Email", tAccount.Email);
        //                        tAccount.Password = EditorGUILayout.TextField("Password", tAccount.Password);

        //                        //EditorGUILayout.LabelField("Country", tAccount.Country);
        //                        int tCountry = tCountryCode.IndexOf(tAccount.Country);
        //                        if (tCountry < 0) { tCountry = 0; }
        //                        tAccount.Country = tCountryCode[EditorGUILayout.Popup("Country", tCountry, tCountryKey.ToArray())];

        //                        //EditorGUILayout.LabelField("Language", tAccount.Language);
        //                        int tLanguage = tLanguageCode.IndexOf(tAccount.Language);
        //                        if (tLanguage < 0) { tLanguage = 0; }
        //                        tAccount.Language = tLanguageCode[EditorGUILayout.Popup("Language", tLanguage, tLanguageKey.ToArray())];

        //                        tAccount.NetworkQuality = (NWDFakeAccountNetworkQuality)EditorGUILayout.EnumPopup("Network", tAccount.NetworkQuality);
        //                        GUILayout.EndVertical();
        //                        //---------
        //                        GUILayout.BeginVertical(GUILayout.Width(120));
        //                        GUILayout.Label("Actions", NWDGUI.KTableSearchTitle, GUILayout.Width(120));
        //                        if (GUILayout.Button("Remove this device", GUILayout.Width(120)))
        //                        {
        //                            tToDelete = tAccount;
        //                        }
        //                        GUILayout.EndVertical();
        //                        GUILayout.EndHorizontal();
        //                        NWDGUILayout.Line();
        //                    }
        //                    if (tToDelete != null)
        //                    {
        //                        sEnvironment.AccountsToTest.Remove(tToDelete);
        //                    }
        //                    GUILayout.EndScrollView();
        //                    NWDGUILayout.Line();
        //                    NWDGUILayout.LittleSpace();
        //                    GUILayout.BeginHorizontal();
        //                    GUILayout.FlexibleSpace();
        //                    if (GUILayout.Button("New device"))
        //                    {
        //                        NWDFakeAccount tAccount = new NWDFakeAccount();
        //                        sEnvironment.AccountsToTest.Add(tAccount);
        //                    }
        //                    GUILayout.EndHorizontal();
        //                    NWDGUILayout.LittleSpace();
        //                }
        //                else
        //                {
        //                    EditorGUILayout.HelpBox("Not available in this environment", MessageType.Info);
        //                }
        //                if (EditorGUI.EndChangeCheck())
        //                {
        //                    NWDUnityEngineEditor.Instance.GetConfig().Save();
        //                    NWDUnityEditorWindowReimport.RepaintAll();
        //                }
        //            }
        //        }
        //        else if (sState == NWDUnityEditorWindowBasisState.InPlayingMode)
        //        {
        //            if (NWDPlanResumeGUI.GetPlanResumeGUI(NWDUnityEngineEditor.Instance.GetConfig().Plan).DrawIfConnected())
        //            {
        //                if (NWDEnvironmentToolbox.CanBeUseForRuntime(sEnvironment.EnvName))
        //                {
        //                    sScrollPosition = GUILayout.BeginScrollView(sScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        //                    foreach (NWDFakeAccount tAccount in sEnvironment.AccountsToTest)
        //                    {
        //                        NWDGUILayout.LineWhite();
        //                        NWDGUILayout.SubSection(tAccount.InternaleName);
        //                        GUILayout.BeginHorizontal();
        //                        //---------
        //                        GUILayout.BeginVertical();
        //                        GUILayout.Label("Device", NWDGUI.KTableSearchTitle);
        //                        EditorGUILayout.LabelField("Device " + tAccount.DeviceID);
        //                        EditorGUILayout.LabelField("Account", tAccount.Account.ToString());
        //                        EditorGUILayout.LabelField("Internal name", tAccount.InternaleName);
        //                        NWDGUILayout.BigSpace();
        //                        GUILayout.EndVertical();
        //                        //---------
        //                        GUILayout.BeginVertical();
        //                        GUILayout.Label("Options", NWDGUI.KTableSearchTitle);
        //                        EditorGUILayout.LabelField("Email", tAccount.Email);
        //                        EditorGUILayout.LabelField("Password", tAccount.Password);

        //                        EditorGUILayout.LabelField("Country", tAccount.Country);

        //                        EditorGUILayout.LabelField("Language", tAccount.Language);

        //                        EditorGUILayout.LabelField("Network", tAccount.NetworkQuality.ToString());
        //                        GUILayout.EndVertical();
        //                        //---------
        //                        GUILayout.BeginVertical(GUILayout.Width(120));
        //                        GUILayout.Label("Actions", NWDGUI.KTableSearchTitle, GUILayout.Width(120));

        //                        if (GUILayout.Button("Use this Device", GUILayout.Width(120)))
        //                        {
        //                            Debug.LogWarning("NEED CODE USE THIS DEVICE ID IN Runtime");
        //                        }
        //                        EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(tAccount.Email) || string.IsNullOrEmpty(tAccount.Password));
        //                        if (GUILayout.Button("Signup", GUILayout.Width(120)))
        //                        {
        //                            Debug.LogWarning("NEED CODE TO SIGN THIS RUNTIME ON SERVER");
        //                        }
        //                        if (GUILayout.Button("Signin", GUILayout.Width(120)))
        //                        {
        //                            Debug.LogWarning("NEED CODE TO SIGN THIS RUNTIME ON SERVER");
        //                        }
        //                        if (GUILayout.Button("Signout", GUILayout.Width(120)))
        //                        {
        //                            Debug.LogWarning("NEED CODE TO SIGN THIS RUNTIME ON SERVER");
        //                        }
        //                        if (GUILayout.Button("Lost password", GUILayout.Width(120)))
        //                        {
        //                            Debug.LogWarning("NEED CODE TO SIGN THIS RUNTIME ON SERVER");
        //                        }
        //                        EditorGUI.EndDisabledGroup();

        //                        GUILayout.EndVertical();
        //                        GUILayout.EndHorizontal();
        //                        NWDGUILayout.Line();
        //                    }
        //                    GUILayout.EndScrollView();
        //                }
        //                else
        //                {
        //                    EditorGUILayout.HelpBox("Not available in this environment", MessageType.Info);
        //                }
        //            }
        //        }
        //        EditorGUI.EndDisabledGroup();
        //    }
        //}

        //static public void LayoutClassesForm(this NWDEnvironmentUnityEditor sEnvironment, Dictionary<string, List<NWDTypeImplement>> K_ModuleTypeDico)
        //{
        //    if (sEnvironment.GetAccessRights() == NWDEnvironmentRights.None)
        //    {
        //        NWDGUILayout.DrawMessageNoRights();
        //    }
        //    else
        //    {
        //        EditorGUI.BeginDisabledGroup(sEnvironment.GetAccessRights() == NWDEnvironmentRights.Read);
        //        NWDGUILayout.Section("Standard classes and models");
        //        NWDGUILayout.Informations("Standard classes and models can be used in your project. You can disable unused models.");
        //        List<string> tTypeActivateList = new List<string>();
        //        List<string> tTypeInactivateList = new List<string>();
        //        List<string> tModuleActivateList = new List<string>();
        //        List<string> tModuleInactivateList = new List<string>();

        //        NWDTypeModelsStyle AnalyzeStyle = NWDTypeModelsStyle.Standard;


        //        if (K_ModuleTypeDico.ContainsKey(NWDModelOptionsAttribute.Mandatory))
        //        {
        //            NWDGUILayout.SubSection("Mandatory classes");
        //            int tCounterTypes = 0;
        //            if (K_ModuleTypeDico.ContainsKey(NWDModelOptionsAttribute.Mandatory))
        //            {
        //                foreach (NWDTypeImplement tTypeImplement in K_ModuleTypeDico[NWDModelOptionsAttribute.Mandatory])
        //                {
        //                    if (AnalyzeStyle == tTypeImplement.ModelStyle)
        //                    {
        //                        tCounterTypes++;
        //                        EditorGUI.BeginDisabledGroup(true);
        //                        EditorGUILayout.ToggleLeft(tTypeImplement.ClassType.Name, Array.IndexOf(sEnvironment.TypeActivateList, tTypeImplement.ClassType.AssemblyQualifiedName) > -1);
        //                        EditorGUI.EndDisabledGroup();
        //                        tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                    }
        //                }
        //            }
        //            if (tCounterTypes == 0)
        //            {
        //                GUILayout.Label("No mandatory class!", NWDGUI.kInspectorReferenceCenter);
        //            }
        //        }
        //        if (K_ModuleTypeDico.ContainsKey(NWDModelOptionsAttribute.None))
        //        {
        //            NWDGUILayout.SubSection("Optional classes");
        //            int tCounterOptionalTypes = 0;
        //            foreach (NWDTypeImplement tTypeImplement in K_ModuleTypeDico[NWDModelOptionsAttribute.None])
        //            {
        //                if (AnalyzeStyle == tTypeImplement.ModelStyle)
        //                {
        //                    tCounterOptionalTypes++;
        //                    if (EditorGUILayout.ToggleLeft(tTypeImplement.ClassType.Name, Array.IndexOf(sEnvironment.TypeActivateList, tTypeImplement.ClassType.AssemblyQualifiedName) > -1))
        //                    {
        //                        tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                    }
        //                    else
        //                    {
        //                        tTypeInactivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                    }
        //                }
        //            }
        //            if (tCounterOptionalTypes == 0)
        //            {
        //                GUILayout.Label("No optional class!", NWDGUI.kInspectorReferenceCenter);
        //            }
        //        }
        //        NWDGUILayout.SubSection("Optional modules of classes");

        //        int tCounterModule = 0;
        //        foreach (KeyValuePair<string, List<NWDTypeImplement>> tKV in K_ModuleTypeDico)
        //        {
        //            if (tKV.Key != NWDModelOptionsAttribute.None && tKV.Key != NWDModelOptionsAttribute.Mandatory)
        //            {
        //                bool tInclu = false;
        //                List<string> tTypeName = new List<string>();
        //                foreach (NWDTypeImplement tTypeImplement in tKV.Value)
        //                {
        //                    tTypeName.Add(tTypeImplement.ClassType.Name);
        //                    if (AnalyzeStyle == tTypeImplement.ModelStyle)
        //                    {
        //                        tInclu = true;
        //                    }
        //                }

        //                if (tInclu)
        //                {
        //                    tCounterModule++;
        //                    if (EditorGUILayout.ToggleLeft("Module " + tKV.Key + " (" + string.Join(", ", tTypeName) + ")", Array.IndexOf(sEnvironment.ModuleActivateList, tKV.Key) > -1))
        //                    {
        //                        tModuleActivateList.Add(tKV.Key);
        //                        foreach (NWDTypeImplement tTypeImplement in tKV.Value)
        //                        {
        //                            tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        tModuleInactivateList.Add(tKV.Key);
        //                        foreach (NWDTypeImplement tTypeImplement in tKV.Value)
        //                        {
        //                            tTypeInactivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (tCounterModule == 0)
        //        {
        //            GUILayout.Label("No optional modules of classes!", NWDGUI.kInspectorReferenceCenter);
        //        }
        //        NWDGUILayout.Section("Custom classes and models");
        //        NWDGUILayout.Informations("Custom classes and models can be used in your project. You can disable unused models.");
        //        AnalyzeStyle = NWDTypeModelsStyle.Custom;
        //        if (NWDPlanResumeGUI.GetPlanResumeGUI(NWDUnityEngineEditor.Instance.GetConfig().Plan).DrawIfCustomClasses())
        //        {

        //            if (K_ModuleTypeDico.ContainsKey(NWDModelOptionsAttribute.Mandatory))
        //            {
        //                NWDGUILayout.SubSection("Mandatory custom clasess");
        //                int tCounterCustomTypes = 0;
        //                if (K_ModuleTypeDico.ContainsKey(NWDModelOptionsAttribute.Mandatory))
        //                {
        //                    foreach (NWDTypeImplement tTypeImplement in K_ModuleTypeDico[NWDModelOptionsAttribute.Mandatory])
        //                    {
        //                        if (AnalyzeStyle == tTypeImplement.ModelStyle)
        //                        {
        //                            tCounterCustomTypes++;
        //                            EditorGUI.BeginDisabledGroup(true);
        //                            EditorGUILayout.ToggleLeft(tTypeImplement.ClassType.Name, Array.IndexOf(sEnvironment.TypeActivateList, tTypeImplement.ClassType.AssemblyQualifiedName) > -1);
        //                            EditorGUI.EndDisabledGroup();
        //                            tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                        }
        //                    }
        //                }
        //                if (tCounterCustomTypes == 0)
        //                {
        //                    GUILayout.Label("No custom mandatory class!", NWDGUI.kInspectorReferenceCenter);
        //                }
        //            }
        //            if (K_ModuleTypeDico.ContainsKey(NWDModelOptionsAttribute.None))
        //            {
        //                NWDGUILayout.SubSection("Optional custom clasess");
        //                int tCounterCustomOptionalTypes = 0;
        //                foreach (NWDTypeImplement tTypeImplement in K_ModuleTypeDico[NWDModelOptionsAttribute.None])
        //                {
        //                    if (AnalyzeStyle == tTypeImplement.ModelStyle)
        //                    {
        //                        tCounterCustomOptionalTypes++;
        //                        if (EditorGUILayout.ToggleLeft(tTypeImplement.ClassType.Name, Array.IndexOf(sEnvironment.TypeActivateList, tTypeImplement.ClassType.AssemblyQualifiedName) > -1))
        //                        {
        //                            tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                        }
        //                        else
        //                        {
        //                            tTypeInactivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                        }
        //                    }
        //                }
        //                if (tCounterCustomOptionalTypes == 0)
        //                {
        //                    GUILayout.Label("No custom optional class!", NWDGUI.kInspectorReferenceCenter);
        //                }
        //            }
        //            NWDGUILayout.SubSection("Optional custom modules of classes");
        //            int tCounterCustomModule = 0;
        //            foreach (KeyValuePair<string, List<NWDTypeImplement>> tKV in K_ModuleTypeDico)
        //            {
        //                if (tKV.Key != NWDModelOptionsAttribute.None && tKV.Key != NWDModelOptionsAttribute.Mandatory)
        //                {
        //                    bool tInclu = false;
        //                    List<string> tTypeName = new List<string>();
        //                    foreach (NWDTypeImplement tTypeImplement in tKV.Value)
        //                    {
        //                        tTypeName.Add(tTypeImplement.ClassType.Name);
        //                        if (AnalyzeStyle == tTypeImplement.ModelStyle)
        //                        {
        //                            tInclu = true;
        //                        }
        //                    }

        //                    if (tInclu)
        //                    {
        //                        tCounterCustomModule++;
        //                        if (EditorGUILayout.ToggleLeft("Module " + tKV.Key + " (" + string.Join(", ", tTypeName) + ")", Array.IndexOf(sEnvironment.ModuleActivateList, tKV.Key) > -1))
        //                        {
        //                            tModuleActivateList.Add(tKV.Key);
        //                            foreach (NWDTypeImplement tTypeImplement in tKV.Value)
        //                            {
        //                                tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            tModuleInactivateList.Add(tKV.Key);
        //                            foreach (NWDTypeImplement tTypeImplement in tKV.Value)
        //                            {
        //                                tTypeInactivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (tCounterCustomModule == 0)
        //            {
        //                NWDGUILayout.Label("No custom optional modules of classes!");
        //            }
        //        }
        //        else
        //        {
        //            if (K_ModuleTypeDico.ContainsKey(NWDModelOptionsAttribute.Mandatory))
        //            {
        //                if (K_ModuleTypeDico.ContainsKey(NWDModelOptionsAttribute.Mandatory))
        //                {
        //                    foreach (NWDTypeImplement tTypeImplement in K_ModuleTypeDico[NWDModelOptionsAttribute.Mandatory])
        //                    {
        //                        if (AnalyzeStyle == tTypeImplement.ModelStyle)
        //                        {
        //                            tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                        }
        //                    }
        //                }
        //            }
        //            if (K_ModuleTypeDico.ContainsKey(NWDModelOptionsAttribute.None))
        //            {
        //                foreach (NWDTypeImplement tTypeImplement in K_ModuleTypeDico[NWDModelOptionsAttribute.None])
        //                {
        //                    if (AnalyzeStyle == tTypeImplement.ModelStyle)
        //                    {
        //                        if (Array.IndexOf(sEnvironment.TypeActivateList, tTypeImplement.ClassType.AssemblyQualifiedName) > -1)
        //                        {
        //                            tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                        }
        //                        else
        //                        {
        //                            tTypeInactivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                        }
        //                    }
        //                }
        //            }
        //            foreach (KeyValuePair<string, List<NWDTypeImplement>> tKV in K_ModuleTypeDico)
        //            {
        //                if (tKV.Key != NWDModelOptionsAttribute.None && tKV.Key != NWDModelOptionsAttribute.Mandatory)
        //                {
        //                    bool tInclu = false;
        //                    List<string> tTypeName = new List<string>();
        //                    foreach (NWDTypeImplement tTypeImplement in tKV.Value)
        //                    {
        //                        tTypeName.Add(tTypeImplement.ClassType.Name);
        //                        if (AnalyzeStyle == tTypeImplement.ModelStyle)
        //                        {
        //                            tInclu = true;
        //                        }
        //                    }
        //                    if (tInclu)
        //                    {
        //                        if (Array.IndexOf(sEnvironment.ModuleActivateList, tKV.Key) > -1)
        //                        {
        //                            tModuleActivateList.Add(tKV.Key);
        //                            foreach (NWDTypeImplement tTypeImplement in tKV.Value)
        //                            {
        //                                tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            tModuleInactivateList.Add(tKV.Key);
        //                            foreach (NWDTypeImplement tTypeImplement in tKV.Value)
        //                            {
        //                                tTypeInactivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        tTypeActivateList.Sort();
        //        tTypeInactivateList.Sort();
        //        tModuleActivateList.Sort();
        //        tModuleInactivateList.Sort();

        //        sEnvironment.TypeActivateList = tTypeActivateList.ToArray();
        //        sEnvironment.TypeInactivateList = tTypeInactivateList.ToArray();
        //        sEnvironment.ModuleActivateList = tModuleActivateList.ToArray();
        //        sEnvironment.ModuleInactivateList = tModuleInactivateList.ToArray();

        //        NWDGUILayout.Section("Actions");
        //        NWDGUILayout.LittleSpace();
        //        if (GUILayout.Button("Reset to default"))
        //        {
        //            NWDTypeImplement.SetDefault(sEnvironment);
        //        }
        //        NWDGUILayout.LittleSpace();


        //        NWDGUILayout.Section("Copy classes configuration to another environment");
        //        GUIContent tButtonTitleHere = new GUIContent(sEnvironment.GetGUIContent());
        //        tButtonTitleHere.text = "Copy to everywhere ";
        //        NWDGUILayout.BigSpace();
        //        if (GUILayout.Button(tButtonTitleHere, GUILayout.Height(30), GUILayout.Width(300)))
        //        {
        //            foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnvironments())
        //            {
        //                if (sEnvironment != tEnv)
        //                {
        //                    tEnv.TypeActivateList = (string[])sEnvironment.TypeActivateList.Clone();
        //                    tEnv.TypeInactivateList = (string[])sEnvironment.TypeInactivateList.Clone();
        //                    tEnv.ModuleActivateList = (string[])sEnvironment.ModuleActivateList.Clone();
        //                    tEnv.ModuleInactivateList = (string[])sEnvironment.ModuleInactivateList.Clone();
        //                }
        //            }
        //        }
        //        NWDGUILayout.BigSpace();
        //        NWDGUILayout.Line();
        //        NWDGUILayout.BigSpace();
        //        foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnvironments())
        //        {
        //            if (sEnvironment != tEnv)
        //            {
        //                GUIContent tButtonTitle = new GUIContent(tEnv.GetGUIContent());
        //                tButtonTitle.text = "Copy to " + tEnv.InternalName;
        //                if (GUILayout.Button(tButtonTitle, GUILayout.Height(30), GUILayout.Width(300)))
        //                {
        //                    tEnv.TypeActivateList = (string[])sEnvironment.TypeActivateList.Clone();
        //                    tEnv.TypeInactivateList = (string[])sEnvironment.TypeInactivateList.Clone();
        //                    tEnv.ModuleActivateList = (string[])sEnvironment.ModuleActivateList.Clone();
        //                    tEnv.ModuleInactivateList = (string[])sEnvironment.ModuleInactivateList.Clone();
        //                }
        //            }
        //        }
        //        NWDGUILayout.BigSpace();
        //        EditorGUI.EndDisabledGroup();
        //    }
        //}

        //static void RevealDatabase(string sPath, string sDatabaseTitle)
        //{
        //    GUILayout.Label("Database path : " + sPath);
        //    EditorGUILayout.BeginHorizontal();
        //    if (File.Exists(sPath))
        //    {
        //        if (GUILayout.Button("Reveal " + sDatabaseTitle + ""))
        //        {
        //            EditorUtility.RevealInFinder(sPath);
        //            EditorUtility.OpenWithDefaultApp(Path.GetDirectoryName(sPath));
        //            UnityEngine.Debug.Log(sPath);
        //        }
        //    }
        //    else
        //    {
        //        GUILayout.Label("Database " + sDatabaseTitle + " doesn't exist");
        //    }
        //    if (File.Exists(sPath))
        //    {
        //        if (GUILayout.Button("Open " + sDatabaseTitle + ""))
        //        {
        //            EditorUtility.OpenWithDefaultApp(sPath);
        //            UnityEngine.Debug.Log(sPath);
        //        }
        //    }
        //    else
        //    {
        //        GUILayout.Label("Database " + sDatabaseTitle + " doesn't exist");
        //    }
        //    EditorGUILayout.EndHorizontal();
        //}
    }
}