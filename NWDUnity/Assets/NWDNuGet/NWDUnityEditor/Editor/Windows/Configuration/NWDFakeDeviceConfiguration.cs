using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Localization;
using NWDUnityEditor.Device;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Managers;
using NWDUnityEditor.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    public class NWDFakeDeviceConfiguration : NWDUnityEditorWindowBasis
    {
        static List<string> LanguageKey = null;
        static List<string> LanguageCode = null;

        static List<string> CountryKey = null;
        static List<string> CountryCode = null;

        public Vector2 ScrollPosition;

        public static void SharedInstanceFocus()
        {
            NWDFakeDeviceConfiguration rReturn = GetWindow<NWDFakeDeviceConfiguration>();
            rReturn.ShowMe();
            rReturn.Focus();
        }

        static public void Init ()
        {
            if (LanguageKey == null)
            {
                LanguageKey = new List<string>();
                LanguageCode = new List<string>();

                CountryKey = new List<string>();
                CountryCode = new List<string>();

                foreach (NWDLocalizationISO tLangage in NWDLocalizationISO.LanguageDico)
                {
                    LanguageKey.Add(tLangage.Name);
                    LanguageCode.Add(tLangage.ShortCode);
                }

                foreach (NWDCountryISO tCountry in NWDCountryISO.List)
                {
                    CountryKey.Add(tCountry.Name);
                    CountryCode.Add(tCountry.TwoLetterCode);
                }
            }
        }

        public void GUITitle()
        {
            NWDGUILayout.Title("Fake devices");
        }

        public override void OnPreventGUI_InCompileTime()
        {
            GUITitle();
            base.OnPreventGUI_InCompileTime();
        }

        public override void OnPreventGUI_InPlayingMode()
        {
            GUITitle();

            NWDDataTrackDescription tDataTrack = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironment();

            if (tDataTrack != null)
            {
                GUILayout.Label(tDataTrack.GetGUIContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4));
                NWDGUILayout.Line();
                //NWDLauncherUnityEditor.GetActiveEnvironment().LayoutFakeAccountForm(State,ref ScrollPosition);
            }
            else
            {
                base.OnPreventGUI_InPlayingMode();
            }
        }

        public override void OnPreventGUI_InEditorMode()
        {
            GUITitle();
            DrawInEditor(ref ScrollPosition);
        }

        static public void DrawInEditor(ref Vector2 sScrollPosition)
        {
            Init();
            NWDUnityEditorDeviceManager tDeviceManager = NWDUnityEngineEditor.Instance.DeviceManager as NWDUnityEditorDeviceManager;
            List<NWDFakeDevice> tDevices = tDeviceManager.GetDevices();

            EditorGUI.BeginChangeCheck();
            sScrollPosition = GUILayout.BeginScrollView(sScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            int tToDelete = -1;
            for (int i = 0; i < tDevices.Count; i++)
            {
                NWDGUILayout.LineWhite();
                NWDGUILayout.SubSection(tDevices[i].InternalName);
                GUILayout.BeginHorizontal();
                //---------
                GUILayout.BeginVertical();
                EditorGUI.BeginDisabledGroup(i == 0);
                GUILayout.Label("Device", NWDGUI.KTableSearchTitle);
                EditorGUILayout.LabelField("Device", tDevices[i].DeviceID);
                tDevices[i].InternalName = EditorGUILayout.TextField("Internal name", tDevices[i].InternalName);
                NWDGUILayout.BigSpace();
                EditorGUI.EndDisabledGroup();
                GUILayout.EndVertical();
                //---------
                GUILayout.BeginVertical();

                //EditorGUILayout.LabelField("Country", tAccount.Country);
                int tCountry = CountryCode.IndexOf(tDevices[i].Country);
                if (tCountry < 0) { tCountry = 0; }
                tDevices[i].Country = CountryCode[EditorGUILayout.Popup("Country", tCountry, CountryKey.ToArray())];

                //EditorGUILayout.LabelField("Language", tAccount.Language);
                int tLanguage = LanguageCode.IndexOf(tDevices[i].Language);
                if (tLanguage < 0) { tLanguage = 0; }
                tDevices[i].Language = LanguageCode[EditorGUILayout.Popup("Language", tLanguage, LanguageKey.ToArray())];

                tDevices[i].NetworkQuality = (NWDDeviceNetworkQuality)EditorGUILayout.EnumPopup("Network", tDevices[i].NetworkQuality);
                GUILayout.EndVertical();
                //---------
                GUILayout.BeginVertical(GUILayout.Width(120));
                GUILayout.Label("Actions", NWDGUI.KTableSearchTitle, GUILayout.Width(120));

                bool tDisabled = i == 0;
                EditorGUI.BeginDisabledGroup(tDisabled);
                if (GUILayout.Button(tDisabled ? "Default device" : "Remove this device", GUILayout.Width(120)))
                {
                    tToDelete = i;
                }
                EditorGUI.EndDisabledGroup();

                tDisabled = tDevices[i].DeviceID == tDeviceManager.GetDeviceId();
                EditorGUI.BeginDisabledGroup(tDisabled);
                if (GUILayout.Button(tDisabled ? "Current" : "Select", GUILayout.Width(120)))
                {
                    tDeviceManager.SetCurrentDevice(i);
                }
                EditorGUI.EndDisabledGroup();

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                NWDGUILayout.Line();
            }
            if (tToDelete > 0)
            {
                tDeviceManager.DeleteFakeDevice(tToDelete);
            }
            GUILayout.EndScrollView();
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("New device"))
            {
                tDeviceManager.AddFakeDevice();
            }
            GUILayout.EndHorizontal();
            NWDGUILayout.LittleSpace();
            if (EditorGUI.EndChangeCheck())
            {
                tDeviceManager.Save();
                NWDUnityEditorWindowReimport.RepaintAll();
            }
        }

        public override void OnEnableFromConstructor()
        {
        }

        public override void OnEnableFromSerialization()
        {
        }

        public override void OnDisableWindow()
        {
        }

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDFakeDeviceConfiguration>("Fake Devices");
        }
    }
}
