using NWDUnityEditor.Windows;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;

namespace NWDUnityEditor.Tools
{
    public class NWDUnityEditorWindowReimport : AssetPostprocessor
    {
        static private List<NWDUnityEditorWindowBasis> AllWindowsList = new List<NWDUnityEditorWindowBasis>();

        public static void AddWindow(NWDUnityEditorWindowBasis sWindow)
        {
            if (AllWindowsList.Contains(sWindow) == false)
            {
                AllWindowsList.Add(sWindow);
            }
        }

        public static void RemoveWindow(NWDUnityEditorWindowBasis sWindow)
        {
            if (AllWindowsList.Contains(sWindow) == true)
            {
                AllWindowsList.Remove(sWindow);
            }
        }

        static public void ConfigUpdate()
        {
            if (AllWindowsList != null)
            {
                foreach (NWDUnityEditorWindowBasis tWindow in AllWindowsList)
                {
                    if (tWindow != null)
                    {
                        tWindow.OnConfigUpdate();
                        tWindow.Repaint();
                    }
                }
            }
        }

        static public void RepaintAll()
        {
            if (AllWindowsList != null)
            {
                foreach (NWDUnityEditorWindowBasis tWindow in AllWindowsList)
                {
                    if (tWindow != null)
                    {
                        tWindow.Repaint();
                    }
                }
            }
        }

        void OnPreprocessAsset()
        {
            if (AllWindowsList != null)
            {
                foreach (NWDUnityEditorWindowBasis tWindow in AllWindowsList)
                {
                    if (tWindow != null)
                    {
                        tWindow.Repaint();
                    }
                }
            }
        }

        void OnPostprocessAsset()
        {
            if (AllWindowsList != null)
            {
                foreach (NWDUnityEditorWindowBasis tWindow in AllWindowsList)
                {
                    if (tWindow != null)
                    {
                        tWindow.Repaint();
                    }
                }
            }
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (AllWindowsList != null)
            {
                foreach (NWDUnityEditorWindowBasis tWindow in AllWindowsList)
                {
                    if (tWindow != null)
                    {
                        tWindow.Repaint();
                    }
                }
            }
        }
    }
}