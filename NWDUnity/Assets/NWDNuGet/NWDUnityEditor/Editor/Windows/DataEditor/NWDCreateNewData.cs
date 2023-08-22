using NWDUnityEditor.Tools;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    public class NWDCreateNewData : NWDUnityEditorWindowBasis
    {
        static NWDCreateNewData kShareInstance = null;

        NWDUnityEditorWindowData Window;
        int TypeSelected;
        string[] TypeSelection;

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDCreateNewData>("Create a new data");
        }

        public static void SharedInstanceFocus(NWDUnityEditorWindowData sWindow)
        {
            if (kShareInstance == null)
            {
                kShareInstance = (NWDCreateNewData)ScriptableObject.CreateInstance(typeof(NWDCreateNewData));
            }

            kShareInstance.Window = sWindow;
            kShareInstance.TypeSelected = 0;
            kShareInstance.TypeSelection = sWindow.TabSelectionSelected.TypeFilter.Skip(1).ToArray();

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
            TypeSelected = EditorGUILayout.Popup("Type", TypeSelected, TypeSelection);

            GUILayout.EndVertical();
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
            {
                Close();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create data"))
            {
                Window.TabSelectionSelected.NewData(Window, TypeSelected);
                Close();
            }
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