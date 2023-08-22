using NWDFoundation.Logger;
using NWDUnityEditor.Engine;
using NWDUnityShared.SQLite;
using System;
using System.IO;
using System.Text;

namespace NWDUnityEditor.SQLite
{
    public delegate void SQLiteConnectorDelegate(IntPtr sSQLiteDeviceHandle);
    public static class SQLiteConnector
    {
        static private byte[] DatabasePathAsBytes;
        static public IntPtr SQLiteDeviceHandle = IntPtr.Zero;

        public static string GetTempPath()
        {
            return Directory.GetParent(Path.GetTempPath()).FullName + "/Net-Worked-Data/";
        }

        public static string GetDatabaseEditorPath()
        {
            string rReturn = GetTempPath() + NWDUnityEngineEditor.Instance.GetConfig().GetProjectId() + "/";
            rReturn = rReturn.Replace("//", "/");
            if (Directory.Exists(rReturn) == false)
            {
                Directory.CreateDirectory(rReturn);
            }
            return rReturn;
        }

        public static string GetVirtualDatabasPath()
        {
            if (NWDUnityEngineEditor.Instance.GetConfig().IsReady())
            {
                return GetDatabaseEditorPath() + "VirtualDatabase.sqlite";
            }
            else
            {
                return GetDatabaseEditorPath() + "/error.sqlite";
            }
        }

        public static byte[] GetNullTerminatedUtf8(string s)
        {
            int utf8Length = Encoding.UTF8.GetByteCount(s);
            byte[] bytes = new byte[utf8Length + 1];
            utf8Length = Encoding.UTF8.GetBytes(s, 0, s.Length, bytes, 0);
            return bytes;
        }

        public static void InitDatabase()
        {
            OpenDatabase();
            CloseDatabase();
        }

        public static bool UseDatabase(SQLiteConnectorDelegate sDelegate)
        {
            bool rReturn = false; 
            OpenDatabase();
            if (SQLiteDeviceHandle != IntPtr.Zero)
            {
                try
                {
                    sDelegate(SQLiteDeviceHandle);
                }
                catch (Exception e)
                {
                    NWDLogger.Error(e.Message, e);
                }
                finally
                {
                    CloseDatabase();
                }
                rReturn = true;
            }
            else
            {
                NWDLogger.Warning("Database not opened!");
            }
            return rReturn;
        }

        public static void OpenDatabase()
        {
            if (NWDUnityEngineEditor.Instance.GetConfig().IsReady())
            {
                //Debug.Log("open data base : " + SQLiteConnector.GetVirtualDatabasPath());
                DatabasePathAsBytes = GetNullTerminatedUtf8(GetVirtualDatabasPath());
                SQLite3.Result tResult = SQLite3.Open(DatabasePathAsBytes, out SQLiteDeviceHandle, (int)(SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create), IntPtr.Zero);
                if (tResult != SQLite3.Result.OK)
                {
                    SQLiteDeviceHandle = IntPtr.Zero;
                    throw SQLiteException.New(tResult, string.Format("Could not open database file: {0} ({1})", SQLiteConnector.GetVirtualDatabasPath(), tResult));
                }
            }
            else
            {
                CloseDatabase();
            }
        }

        public static void CloseDatabase()
        {
            if (SQLiteDeviceHandle != IntPtr.Zero)
            {
                SQLite3.Close(SQLiteDeviceHandle);
                SQLiteDeviceHandle = IntPtr.Zero;
            }
        }
    }
}