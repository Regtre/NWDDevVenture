//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Runtime.InteropServices;
using System.Text;

//=====================================================================================================================
using Sqlite3DatabaseHandle = System.IntPtr;
//=====================================================================================================================
namespace NWDUnityShared.SQLite
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class SQLite3
    {
        [DllImport(DLL_NAME, EntryPoint = "sqlite3_open", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Open([MarshalAs(UnmanagedType.LPStr)] string filename,
            out Sqlite3DatabaseHandle db);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_open_v2", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Open([MarshalAs(UnmanagedType.LPStr)] string filename, out Sqlite3DatabaseHandle db,
            int flags, Sqlite3DatabaseHandle zvfs);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_open_v2", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Open(byte[] filename, out Sqlite3DatabaseHandle db, int flags,
            Sqlite3DatabaseHandle zvfs);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_key", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Key(Sqlite3DatabaseHandle db, [MarshalAs(UnmanagedType.LPStr)] string key,
            int keylen);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_open16", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Open16([MarshalAs(UnmanagedType.LPWStr)] string filename,
            out Sqlite3DatabaseHandle db);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_enable_load_extension", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result EnableLoadExtension(Sqlite3DatabaseHandle db, int onoff);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Close(Sqlite3DatabaseHandle db);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_initialize", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Initialize();

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_shutdown", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Shutdown();

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Config(ConfigOption option);

        //[DllImport(DLL_NAME, EntryPoint = "sqlite3_win32_set_directory", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Unicode)]
        //public static extern int SetDirectory (uint directoryType, string directoryPath);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_busy_timeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result BusyTimeout(Sqlite3DatabaseHandle db, int milliseconds);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_changes", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Changes(Sqlite3DatabaseHandle db);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_prepare_v2", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Prepare2(Sqlite3DatabaseHandle db, [MarshalAs(UnmanagedType.LPStr)] string sql,
            int numBytes, out Sqlite3DatabaseHandle stmt, Sqlite3DatabaseHandle pzTail);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_step", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Step(Sqlite3DatabaseHandle stmt);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Reset(Sqlite3DatabaseHandle stmt);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_finalize", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Finalize(Sqlite3DatabaseHandle stmt);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_last_insert_rowid", CallingConvention = CallingConvention.Cdecl)]
        public static extern long LastInsertRowid(Sqlite3DatabaseHandle db);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_errmsg16", CallingConvention = CallingConvention.Cdecl)]
        public static extern Sqlite3DatabaseHandle Errmsg(Sqlite3DatabaseHandle db);

        public static string GetErrmsg(Sqlite3DatabaseHandle db)
        {
            return Marshal.PtrToStringUni(Errmsg(db));
        }

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_bind_parameter_index", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindParameterIndex(Sqlite3DatabaseHandle stmt,
            [MarshalAs(UnmanagedType.LPStr)] string name);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_bind_null", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindNull(Sqlite3DatabaseHandle stmt, int index);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_bind_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindInt(Sqlite3DatabaseHandle stmt, int index, int val);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_bind_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindInt64(Sqlite3DatabaseHandle stmt, int index, long val);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_bind_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindDouble(Sqlite3DatabaseHandle stmt, int index, double val);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_bind_text16", CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Unicode)]
        public static extern int BindText(Sqlite3DatabaseHandle stmt, int index,
            [MarshalAs(UnmanagedType.LPWStr)] string val, int n, Sqlite3DatabaseHandle free);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_bind_blob", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindBlob(Sqlite3DatabaseHandle stmt, int index, byte[] val, int n,
            Sqlite3DatabaseHandle free);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_count", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ColumnCount(Sqlite3DatabaseHandle stmt);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_name", CallingConvention = CallingConvention.Cdecl)]
        public static extern Sqlite3DatabaseHandle ColumnName(Sqlite3DatabaseHandle stmt, int index);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_name16", CallingConvention = CallingConvention.Cdecl)]
        private static extern Sqlite3DatabaseHandle ColumnName16Internal(Sqlite3DatabaseHandle stmt, int index);

        public static string ColumnName16(Sqlite3DatabaseHandle stmt, int index)
        {
            return Marshal.PtrToStringUni(ColumnName16Internal(stmt, index));
        }

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_type", CallingConvention = CallingConvention.Cdecl)]
        public static extern ColType ColumnType(Sqlite3DatabaseHandle stmt, int index);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ColumnInt(Sqlite3DatabaseHandle stmt, int index);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern long ColumnInt64(Sqlite3DatabaseHandle stmt, int index);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ColumnDouble(Sqlite3DatabaseHandle stmt, int index);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_text", CallingConvention = CallingConvention.Cdecl)]
        public static extern Sqlite3DatabaseHandle ColumnText(Sqlite3DatabaseHandle stmt, int index);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_text16", CallingConvention = CallingConvention.Cdecl)]
        public static extern Sqlite3DatabaseHandle ColumnText16(Sqlite3DatabaseHandle stmt, int index);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_blob", CallingConvention = CallingConvention.Cdecl)]
        public static extern Sqlite3DatabaseHandle ColumnBlob(Sqlite3DatabaseHandle stmt, int index);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_column_bytes", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ColumnBytes(Sqlite3DatabaseHandle stmt, int index);

        public static string ColumnString(Sqlite3DatabaseHandle stmt, int index)
        {
            return Marshal.PtrToStringUni(ColumnText16(stmt, index));
        }

        public static byte[] ColumnByteArray(Sqlite3DatabaseHandle stmt, int index)
        {
            int length = ColumnBytes(stmt, index);
            byte[] result = new byte[length];
            if (length > 0)
                Marshal.Copy(ColumnBlob(stmt, index), result, 0, length);
            return result;
        }

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_extended_errcode", CallingConvention = CallingConvention.Cdecl)]
        public static extern ExtendedResult ExtendedErrCode(Sqlite3DatabaseHandle db);

        [DllImport(DLL_NAME, EntryPoint = "sqlite3_libversion_number", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LibVersionNumber();

        //-------------------------------------------------------------------------------------------------------------
        public static Sqlite3DatabaseHandle Prepare2(Sqlite3DatabaseHandle db, string query)
        {
            Sqlite3DatabaseHandle stmt;
            Result r = Prepare2(db, query, Encoding.UTF8.GetByteCount(query), out stmt, Sqlite3DatabaseHandle.Zero);
            if (r != Result.OK) throw SQLiteException.New(r, GetErrmsg(db));
            return stmt;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================