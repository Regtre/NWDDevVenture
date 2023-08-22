using NWDUnityEditor.Engine;
using NWDUnityEditor.Requestors;
using NWDUnityEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    public class NWDChangeWebsiteURL : NWDUnityEditorWindowBasis
    {
        static NWDChangeWebsiteURL kShareInstance = null;

        private string NewWebsiteURL = string.Empty;
        private NWDRequestorTask TestConnection = null;

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDChangeWebsiteURL>("Change Website URL");
        }

        public static void SharedInstanceFocus()
        {
            if (kShareInstance == null)
            {
                kShareInstance = (NWDChangeWebsiteURL)ScriptableObject.CreateInstance(typeof(NWDChangeWebsiteURL));
            }

            kShareInstance.NewWebsiteURL = NWDUnityEngineEditor.Instance.GetConfig().WebEditorURL();
            kShareInstance.TestConnection = null;

            kShareInstance.ShowModal();
            kShareInstance.Focus();
        }

        public void GUITitle()
        {
            NWDGUILayout.Title("Change of the Net-Worked-Data management URL");
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
            EditorGUI.BeginChangeCheck();
            // Start scrollview

            GUILayout.BeginVertical();
            //NWDGUILayout.Title("I have an account on Net-Worked-Data");
            GUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(TestConnection?.IsProcessing() ?? false);

            EditorGUI.BeginChangeCheck();
            NewWebsiteURL = EditorGUILayout.TextField("Enter the new Website url:", NewWebsiteURL);
            if (EditorGUI.EndChangeCheck())
            {
                TestConnection = null;
            }

            if (GUILayout.Button("Test"))
            {
                TestConnection = NWDSettingsRequestor.GetCurrent().EnqueueTask(new NWDTestConnectionRequestor(NewWebsiteURL, null));
            }

            EditorGUI.EndDisabledGroup();

            GUILayout.EndHorizontal();
            if (TestConnection != null && TestConnection.State == NWDRequestorTaskState.Error)
            {
                EditorGUILayout.HelpBox(TestConnection.exception.Message, MessageType.Error);
            }

            if (TestConnection != null && TestConnection.State == NWDRequestorTaskState.Success)
            {
                EditorGUILayout.HelpBox("Successfully reached management server!", MessageType.Info);
            }

            GUILayout.EndVertical();
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
            {
                Close();
            }
            EditorGUI.BeginDisabledGroup(TestConnection == null || TestConnection.State != NWDRequestorTaskState.Success);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Ok"))
            {
                NWDUnityEngineEditor.Instance.GetConfig().SetEditorURL(NewWebsiteURL);
                Close();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
        }

        public override void OnDisableWindow()
        {

        }

        public override void OnEnableFromConstructor()
        {

        }

        public override void OnEnableFromSerialization()
        {

        }
    }

}