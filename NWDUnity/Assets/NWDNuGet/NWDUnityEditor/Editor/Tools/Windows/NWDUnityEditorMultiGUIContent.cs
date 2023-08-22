using NWDFoundation.Logger;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    [Serializable]
    public class NWDUnityEditorMultiGUIContent
    {
        private static NWDUnityEditorMultiGUIContent _LogoWindows;
        public static NWDUnityEditorMultiGUIContent LogoWindows { get { if (_LogoWindows == null) { _LogoWindows = NewContent("NWD3WindowLogo", null); } return _LogoWindows; } }
        private static NWDUnityEditorMultiGUIContent _LogoEditor;
        public static NWDUnityEditorMultiGUIContent LogoEditor { get { if (_LogoEditor == null) { _LogoEditor = NewContent("NWDEditorWindow", "Editor",null); } return _LogoEditor; } }

        public GUIContent Pro;
        public GUIContent NotPro;

        static public NWDUnityEditorMultiGUIContent NewTitle<T>( string sText = "")
        {
            return NewContent(LogoEditor, typeof(T).Name, sText, null);
        }

        static public NWDUnityEditorMultiGUIContent NewContent(Type sType, string sText = "")
        {
            NWDUnityEditorMultiGUIContent rReturn = new NWDUnityEditorMultiGUIContent();
            rReturn.Init(sType, sText, "");
            return rReturn;
        }

        static public NWDUnityEditorMultiGUIContent NewContent(NWDUnityEditorMultiGUIContent sOrigin, string sIcon, string sText , string sTooltip)
        {
            NWDUnityEditorMultiGUIContent rReturn = new NWDUnityEditorMultiGUIContent();
            rReturn.Init(sIcon, sText, sTooltip);
            if (rReturn.Pro.image == null)
            {
                rReturn.Pro.image = sOrigin.Pro.image;
            }
            if (rReturn.NotPro.image == null)
            {
                rReturn.NotPro.image = sOrigin.NotPro.image;
            }
            return rReturn;
        }

        static public NWDUnityEditorMultiGUIContent NewContent(string sIcon, string sText = "")
        {
            NWDUnityEditorMultiGUIContent rReturn = new NWDUnityEditorMultiGUIContent();
            rReturn.Init(sIcon, sText, "");
            return rReturn;
        }

        static public NWDUnityEditorMultiGUIContent NewContent(Type sType, string sText , string sTooltip)
        {
            NWDUnityEditorMultiGUIContent rReturn = new NWDUnityEditorMultiGUIContent();
            rReturn.Init(sType, sText, sTooltip);
            return rReturn;
        }

        static public NWDUnityEditorMultiGUIContent NewContent(string sIcon, string sText , string sTooltip)
        {
            NWDUnityEditorMultiGUIContent rReturn = new NWDUnityEditorMultiGUIContent();
            rReturn.Init(sIcon, sText, sTooltip);
            return rReturn;
        }

        private void Init(string sIcon, string sText , string sTooltip)
        {
            Pro = new GUIContent();
            Pro.text = sText;
            Pro.tooltip = sTooltip;
            NotPro = new GUIContent();
            NotPro.text = sText;
            NotPro.tooltip = sTooltip;

            if (string.IsNullOrEmpty(sIcon) == false)
            {
                string tIconNameCompileNotPro = sIcon;
                string tIconNameCompilePro = sIcon + "_pro";

                string[] sGUIDCompilesNotPro = AssetDatabase.FindAssets(tIconNameCompileNotPro + " t:texture");
                foreach (string tGUID in sGUIDCompilesNotPro)
                {
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    if (tPathFilename.Equals(tIconNameCompileNotPro))
                    {
                        NotPro.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        break;
                    }
                }
                if (NotPro.image  == null)
                {
                    NWDLogger.Warning(tIconNameCompileNotPro+" not found !");
                }

                string[] sGUIDCompilesPro = AssetDatabase.FindAssets(tIconNameCompilePro + " t:texture");
                foreach (string tGUID in sGUIDCompilesPro)
                {
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    if (tPathFilename.Equals(tIconNameCompilePro))
                    {
                        Pro.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        break;
                    }
                }
                if (Pro.image == null)
                {
                    NWDLogger.Warning(tIconNameCompilePro + " not found !");
                }
            }
        }

        private void Init(Type sType, string sText, string sTooltip)
        {
            Init(sType.Name, sText, sTooltip);
        }

        public GUIContent GetContent()
        {
            if (EditorGUIUtility.isProSkin == true)
            {
                return Pro;
            }
            else
            {
                return NotPro;
            }
        }
    }

}