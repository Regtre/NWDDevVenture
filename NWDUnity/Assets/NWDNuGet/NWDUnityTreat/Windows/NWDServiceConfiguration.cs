using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Localization;
using NWDFoundation.Logger;
using NWDUnityEditor.Device;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Managers;
using NWDUnityEditor.Models;
using NWDUnityEditor.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    public class NWDServiceConfiguration : NWDUnityEditorWindowBasis
    {
        public Vector2 ScrollPosition;

        public static void SharedInstanceFocus()
        {
            NWDServiceConfiguration rReturn = GetWindow<NWDServiceConfiguration>();
            rReturn.ShowMe();
            rReturn.Focus();
        }

        public void GUITitle()
        {
            NWDGUILayout.Title("Services and Treat");
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
            DrawInEditor(ref ScrollPosition);
        }

        static public void DrawInEditor(ref Vector2 sScrollPosition)
        {
            NWDUnityTreatManager tTreatManager = NWDUnityTreatManager.Instance;
            List<NWDService> tServices = tTreatManager.GetServices();

            EditorGUI.BeginChangeCheck();
            sScrollPosition = GUILayout.BeginScrollView(sScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            int tToDelete = -1;
            int tToAssociate = -1;
            for (int i = 0; i < tServices.Count; i++)
            {
                NWDGUILayout.LineWhite();
                NWDGUILayout.SubSection(tServices[i].Name);
                GUILayout.BeginHorizontal();
                //---------
                GUILayout.BeginVertical();
                GUILayout.Label("Service", NWDGUI.KTableSearchTitle);
                EditorGUILayout.LabelField("Reference", tServices[i].Reference.ToString());
                tServices[i].Name = EditorGUILayout.TextField("Name", tServices[i].Name);
                NWDGUILayout.BigSpace();
                GUILayout.EndVertical();
                //---------
                GUILayout.BeginVertical();
                GUILayout.Label("Message", NWDGUI.KTableSearchTitle);
                tServices[i].Message = EditorGUILayout.TextArea(tServices[i].Message, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2 + 2));
                GUILayout.EndVertical();
                //---------
                GUILayout.BeginVertical(GUILayout.Width(120));
                GUILayout.Label("Actions", NWDGUI.KTableSearchTitle, GUILayout.Width(120));

                if (GUILayout.Button("Remove", GUILayout.Width(120)))
                {
                    tToDelete = i;
                }

                if (GUILayout.Button("Associate", GUILayout.Width(120)))
                {
                    tToAssociate = i;
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                NWDGUILayout.Line();
            }
            if (tToDelete > 0)
            {
                tTreatManager.RemoveServiceAt(tToDelete);
            }
            GUILayout.EndScrollView();
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("New service"))
            {
                tTreatManager.AddService();
            }
            GUILayout.EndHorizontal();
            NWDGUILayout.LittleSpace();
            if (EditorGUI.EndChangeCheck())
            {
                tTreatManager.Save();
                NWDUnityEditorWindowReimport.RepaintAll();
            }

            if (tToAssociate >= 0)
            {
                NWDTreatWindow.SharedInstanceFocus(tToAssociate);
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
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDServiceConfiguration>("Services and Treat");
        }
    }
}
