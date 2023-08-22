using NWDFoundation.Benchmark;
using NWDUnityEditor.Benchmark;
using NWDUnityEditor.Constants;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Tools;
using NWDUnityShared.Tools;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    public class NWDUserConfigurationManager : NWDUnityEditorWindowBasis
    {
        public Vector2 ScrollPosition;
        private NWDBenchmarkParametersUnityEditor BenchmarkParametersUnityEditor;
        bool ShowLogoContent;
        NWDUnityEditorWindowStyle EditorWindowStyle;

        public static void SharedInstanceFocus()
        {
            NWDUserConfigurationManager rReturn = GetWindow<NWDUserConfigurationManager>();
            rReturn.ShowMe();
            rReturn.Focus();
        }

        public void GUITitle()
        {
            NWDGUILayout.Title(NWDConstantsUnityEditor.K_USER_CONFIGURATION_TITLE);
        }

        public override void OnPreventGUI_InCompileTime()
        {
            GUITitle();
            base.OnPreventGUI_InCompileTime();
        }

        public override void OnPreventGUI_InPlayingMode()
        {
            GUITitle();
            base.OnPreventGUI_InPlayingMode();
        }

        public override void OnPreventGUI_InEditorMode()
        {
            GUITitle();
            // start scroll
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUI.BeginChangeCheck();
            NWDGUILayout.SubSection("General configurations");
            EditorWindowStyle = (NWDUnityEditorWindowStyle)EditorGUILayout.EnumPopup("Window style", EditorWindowStyle);
            ShowLogoContent = EditorGUILayout.Toggle("Show logo content", ShowLogoContent);
            NWDGUILayout.SubSection("Build configurations");

            NWDGUILayout.SubSection("Benchmark");
            BenchmarkParametersUnityEditor.IsEnable = EditorGUILayout.ToggleLeft("Benchmark is enable", BenchmarkParametersUnityEditor.IsEnable);
            BenchmarkParametersUnityEditor.BenchmarkShowStart = EditorGUILayout.ToggleLeft("Benchmark show start", BenchmarkParametersUnityEditor.BenchmarkShowStart);
            BenchmarkParametersUnityEditor.BenchmarkLimit = EditorGUILayout.Slider("Benchmark limit", BenchmarkParametersUnityEditor.BenchmarkLimit, 0F, 1.5F);
            if (EditorGUI.EndChangeCheck() == true)
            {
                NWDUnityEditorWindowReimport.RepaintAll();
                Save();
            }
            // Change color
            EditorGUI.BeginChangeCheck();
            BenchmarkParametersUnityEditor.Green = NWDConverter.ColorToString(EditorGUILayout.ColorField("Green highlight", NWDConverter.ColorFromString(BenchmarkParametersUnityEditor.Green)));
            BenchmarkParametersUnityEditor.Orange = NWDConverter.ColorToString(EditorGUILayout.ColorField("Orange highlight", NWDConverter.ColorFromString(BenchmarkParametersUnityEditor.Orange)));
            BenchmarkParametersUnityEditor.Red = NWDConverter.ColorToString(EditorGUILayout.ColorField("Red highlight", NWDConverter.ColorFromString(BenchmarkParametersUnityEditor.Red)));
            BenchmarkParametersUnityEditor.Blue = NWDConverter.ColorToString(EditorGUILayout.ColorField("Blue highlight", NWDConverter.ColorFromString(BenchmarkParametersUnityEditor.Blue)));
            if (GUILayout.Button("Reset color"))
            {
                BenchmarkParametersUnityEditor.ResetColor();
            }
            // end scroll
            GUILayout.EndScrollView();

            if (EditorGUI.EndChangeCheck() == true)
            {
                NWDUnityEditorWindowReimport.RepaintAll();
                Save();
            }
        }

        public override void OnEnableFromConstructor()
        {
            Load();
        }

        public override void OnEnableFromSerialization()
        {
            Load();
        }

        public override void OnDisableWindow()
        {
        }

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDUserConfigurationManager>(NWDConstantsUnityEditor.K_USER_CONFIGURATION_TITLE);
        }

        //public static NWDEnvironmentDefinition GetEditoBuildEnvironment()
        //{
        //    return (NWDEnvironmentDefinition)NWDProjectPrefs.GetInt(NWDConstantsUnityEditor.K_EDITOR_BUILD_ENVIRONMENT, (int)NWDEnvironmentDefinition.None);
        //}

        //public static void SetEditorBuildEnvironment(NWDEnvironmentDefinition sValue)
        //{
        //    NWDProjectPrefs.SetInt(NWDConstantsUnityEditor.K_EDITOR_BUILD_ENVIRONMENT, (int)sValue);
        //}

        public void Load()
        {
            BenchmarkParametersUnityEditor = (NWDBenchmarkParametersUnityEditor)NWDBenchmark.GetParameters();
            BenchmarkParametersUnityEditor.PrefReload();
            ShowLogoContent = NWDUnityEngineEditor.Instance.GetConfig().GetShowLogo();
            EditorWindowStyle = NWDUnityEngineEditor.Instance.GetConfig().GetWindowStyle();
        }

        public void Save()
        {
            BenchmarkParametersUnityEditor.Save();
            if (ShowLogoContent != NWDUnityEngineEditor.Instance.GetConfig().GetShowLogo())
            {
                NWDUnityEditorWindowReimport.RepaintAll();
            }
            NWDUnityEngineEditor.Instance.GetConfig().SetShowLogo(ShowLogoContent);
            NWDUnityEngineEditor.Instance.GetConfig().SetWindowStyle(EditorWindowStyle);
        }
    }
}