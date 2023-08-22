using System;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public abstract class NWDGUILayout
    {
        public static void DrawMessageNoRights()
        {
            DrawMessage("No rights", "You have no rights to read or edit this part!");
        }

        public static void DrawMessage(string sTitle, string sMessage)
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label(sTitle, NWDGUI.KTableSearchClassIcon);
            NWDGUILayout.Line();
            GUILayout.Label(sMessage);
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }


        public static void Line(params GUILayoutOption[] sOptions)
        {
            GUILayout.Label(string.Empty, NWDGUI.kLineStyle, sOptions);
        }

        public static void LineWhite()
        {
            GUILayout.Label(string.Empty, NWDGUI.kLineWhiteStyle);
        }
        
        public static void Separator()
        {
            GUILayout.Space(NWDGUI.kFieldMarge);
            GUILayout.Label(string.Empty, NWDGUI.kSeparatorStyle);
            GUILayout.Space(NWDGUI.kFieldMarge);
        }


        public static void Title(string sTitle)
        {
            EditorGUI.indentLevel=0;
            Line();
            GUILayout.Label(sTitle, NWDGUI.kTitleStyle);
            Line();
            EditorGUI.indentLevel=1;
        }


        public static void Title(GUIContent sTitle)
        {
            EditorGUI.indentLevel = 0;
            Line();
            GUILayout.Label(sTitle, NWDGUI.kTitleStyle);
            Line();
            EditorGUI.indentLevel = 1;
        }


        public static void Section(string sTitle)
        {
            EditorGUI.indentLevel = 0;
            //GUILayout.Space(NWDGUI.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSectionStyle);
            EditorGUI.indentLevel = 1;
        }


        public static void Section(GUIContent sTitle)
        {
            EditorGUI.indentLevel = 0;
            //GUILayout.Space(NWDGUI.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSectionStyle);
            EditorGUI.indentLevel = 1;
        }


        public static void SubSection(string sTitle)
        {
            EditorGUI.indentLevel = 0;
            GUILayout.Space(NWDGUI.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSubSectionStyle);
            EditorGUI.indentLevel = 1;
        }


        public static void SubSection(GUIContent sTitle)
        {
            EditorGUI.indentLevel = 0;
            GUILayout.Space(NWDGUI.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSubSectionStyle);
            EditorGUI.indentLevel = 1;
        }


        public static void Label(string sTitle)
        {
                GUILayout.Label(sTitle);
        }


        public static void Informations(string sTitle, bool sIndent = true)
        {
            if (sIndent == true)
            {
                EditorGUILayout.HelpBox(sTitle, MessageType.None);
            }
            else
            {
                GUILayout.Label(sTitle, EditorStyles.helpBox);
            }
        }


        public static void Informations(GUIContent sTitle, bool sIndent = true)
        {
            if (sIndent == true)
            {
                EditorGUILayout.HelpBox(sTitle.text, MessageType.None);
            }
            else
            {
                GUILayout.Label(sTitle, EditorStyles.helpBox);
            }
        }


        public static void HelpBox(string sTitle)
        {
            EditorGUILayout.HelpBox(sTitle, MessageType.Info);
        }


        public static void WarningBox(string sTitle)
        {
            EditorGUILayout.HelpBox(sTitle, MessageType.Warning);
        }


        public static bool WarningBoxButton(string sTitle, string sButtonTitle)
        {
            //LittleSpace();
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            //https://unitylist.com/p/5c3/Unity-editor-icons
           Texture2D tIcon = EditorGUIUtility.FindTexture("console.warnicon");
            GUILayout.Label(new GUIContent(sTitle,tIcon));
            bool rReturn = GUILayout.Button(sButtonTitle, NWDGUI.KTableSearchButton, GUILayout.Width(160.0F));
            EditorGUILayout.EndHorizontal();
            //LittleSpace();
            return rReturn;
        }

        public static bool AlertBoxButton(string sTitle, string sButtonTitle)
        {
            //LittleSpace();
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            //https://unitylist.com/p/5c3/Unity-editor-icons
           Texture2D tIcon = EditorGUIUtility.FindTexture("console.erroricon");
            GUILayout.Label(new GUIContent(sTitle,tIcon));
            bool rReturn = GUILayout.Button(sButtonTitle, NWDGUI.KTableSearchButton, GUILayout.Width(160.0F));
            EditorGUILayout.EndHorizontal();
            //LittleSpace();
            return rReturn;
        }

        public static void ErrorBox(string sTitle)
        {
            //LittleSpace();
            EditorGUILayout.HelpBox(sTitle, MessageType.Error);
            //LittleSpace();
        }
        
        public static void LittleSpace()
        {
            GUILayout.Space(NWDGUI.kFieldMarge);
        }
        
        public static void BigSpace()
        {
            GUILayout.Space(NWDGUI.kFieldMarge*2);
        }

        public static DateTime DateField(GUIContent sLabel, DateTime sData, params GUILayoutOption[] sOptions)
        {
            Rect tRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, sOptions);
            return NWDGUI.DateField(tRect, sLabel, sData);
        }

        public static long DateField(GUIContent sLabel, long sNWDTimestamp, params GUILayoutOption[] sOptions)
        {
            Rect tRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, sOptions);
            return NWDGUI.DateField(tRect, sLabel, sNWDTimestamp);
        }
    }
    
}