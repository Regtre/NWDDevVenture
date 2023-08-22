using NWDEditor;
using NWDUnityEditor.Coroutine;
using NWDUnityEditor.SQLite;
using NWDUnityEditor.Windows;
using NWDUnityShared.SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace NWDUnityEditor.Tools
{

    public static class NWDVirtualDataManager
    {
        static bool StartRequestorActive = false;
        static NWDEditorCoroutine EditorCoroutine;
        //static int Counter = 0;

        static private List<NWDUnityEditorWindowData> AllWindowsList = new List<NWDUnityEditorWindowData>();

        static NWDVirtualDataManager()
        {
            //Debug.Log(nameof(NWDVirtualDataManager) + " class daemon start!");
        }

        public static void AddWindow(NWDUnityEditorWindowData sWindow)
        {
            if (AllWindowsList.Contains(sWindow) == false)
            {
                AllWindowsList.Add(sWindow);
            }
        }

        public static void RemoveWindow(NWDUnityEditorWindowData sWindow)
        {
            if (AllWindowsList.Contains(sWindow) == true)
            {
                AllWindowsList.Remove(sWindow);
            }
        }

        public static string GetVirtualDatabasPath()
        {
            return SQLiteConnector.GetVirtualDatabasPath();
        }

        [Obsolete("Use NWDUnityEngine.Instance.NWDUnityEditorLocalDBManager.DeleteDatabase")]
        public static void DeleteDatabase()
        {
            string tOldPath = GetVirtualDatabasPath();
            if (File.Exists(tOldPath))
            {
                File.Delete(tOldPath);
            }
        }

        public static void FlusAllWindows()
        {
            foreach (NWDUnityEditorWindowData tWindows in AllWindowsList)
            {
                foreach (NWDBasisWindowTabSelected tTab in tWindows.TabSelection)
                {
                    tTab.ClearAll();
                }
                tWindows.RepaintMe();
            }
        }

        public static int GetLastDataTimeStamp()
        {
            int rReturn = 0;
            SQLiteConnector.UseDatabase(delegate (IntPtr sSQLiteDeviceHandle)
            {
                string tRequest = "SELECT MAX(" + nameof(NWDMetaData.ModificationDate) + ") FROM " + nameof(NWDMetaData) + ";";
                //Debug.Log("tRequest = " + tRequest);
                IntPtr stmt = SQLite3.Prepare2(sSQLiteDeviceHandle, tRequest);
                while (SQLite3.Step(stmt) == SQLite3.Result.Row)
                {
                    rReturn = SQLite3.ColumnInt(stmt, 0);
                }
                SQLite3.Finalize(stmt);
            });

            return rReturn;
        }

        public static NWDUnityEditorWindowData[] FindWindowForClass(Type tType)
        {
            List<NWDUnityEditorWindowData> rReturn = new List<NWDUnityEditorWindowData>();
            foreach (NWDUnityEditorWindowData tWindows in AllWindowsList)
            {
                foreach (NWDBasisWindowTabSelected tTab in tWindows.TabSelection)
                {
                    if (tTab.RepresentationOfType() == tType)
                    {
                        rReturn.Add(tWindows);
                        //Debug.Log(" for " + tType.Name + " need use window : " + tWindows.TitleMulti.GetContent().text);
                        break;
                    }
                }
            }
            return rReturn.ToArray();
        }

        public static NWDBasisWindowTabSelected[] FindTabForClass(Type tType)
        {
            List<NWDBasisWindowTabSelected> rReturn = new List<NWDBasisWindowTabSelected>();
            foreach (NWDUnityEditorWindowData tWindows in AllWindowsList)
            {
                foreach (NWDBasisWindowTabSelected tTab in tWindows.TabSelection)
                {
                    if (tTab.RepresentationOfType() == tType)
                    {
                        rReturn.Add(tTab);
                        //Debug.Log(" for " + tType.Name + " need use window  " + tWindows.TitleMulti.GetContent().text + " for its tab ");
                    }
                }
            }
            return rReturn.ToArray();
        }
    }
}