using NWDFoundation.Logger;
using NWDUnityShared.Tools;
using System;
using System.Text;
using UnityEngine;

namespace NWDUnityShared.SQLite
{
    public class NWDSQLiteDbConnection : INWDDBConnection
    {
        private NWDPool<NWDSQLiteDbTransaction, NWDSQLiteDbConnection> TransactionPool = new NWDPool<NWDSQLiteDbTransaction, NWDSQLiteDbConnection> ();
        private NWDPool<NWDSQLiteDbRequest, NWDSQLiteDbConnection> RequestPool = new NWDPool<NWDSQLiteDbRequest, NWDSQLiteDbConnection> ();

        private byte[] Path;
        private IntPtr Handle = IntPtr.Zero;
        internal ulong OpenedRequests = 0;
        public SQLite3.Result Result;

        public NWDSQLiteDbConnection (string sDatabasePath)
        {
            int utf8Length = Encoding.UTF8.GetByteCount(sDatabasePath);
            Path = new byte[utf8Length + 1];
            Encoding.UTF8.GetBytes(sDatabasePath, 0, sDatabasePath.Length, Path, 0);
        }

        public INWDDBConnection Open()
        {
            Result = SQLite3.Result.OK;
            if (Handle == IntPtr.Zero)
            {
                Result = SQLite3.Open(Path, out Handle, (int)(SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create), IntPtr.Zero);
                if (Result != SQLite3.Result.OK)
                {
                    Handle = IntPtr.Zero;
                    Debug.LogError("Couldn't open database!\nRequest result: " + Result);
                }
                else
                {
                    Exec("PRAGMA encoding = \"UTF-8\";");
                }
            }
            return this;
        }

        public INWDDBRequest GetRequest()
        {
            NWDSQLiteDbRequest rResult = null;
            if (Handle != IntPtr.Zero)
            {
                rResult = RequestPool.Get(this);
            }
            return rResult;
        }

        public INWDDBRequest StartRequest(string sQuery)
        {
            INWDDBRequest rResult = GetRequest();
            return rResult.Open(sQuery);
        }

        public INWDDBTransaction GetTransaction()
        {
            NWDSQLiteDbTransaction rResult = null;
            if (Handle != IntPtr.Zero)
            {
                rResult = TransactionPool.Get(this);
            }
            return rResult;
        }

        public INWDDBTransaction StartTransaction()
        {
            INWDDBTransaction rResult = GetTransaction();
            return rResult.Open();
        }

        public SQLite3.Result Exec(string sQuery)
        {
            Result = SQLite3.Result.OK;
            if (Handle != IntPtr.Zero)
            {
                using (INWDDBRequest tRequest = GetRequest())
                {
                    Result = tRequest.Exec(sQuery);
                }
            }
            return Result;
        }

        public SQLite3.Result Close()
        {
            Result = SQLite3.Result.OK;
            if (Handle != IntPtr.Zero)
            {
                if (OpenedRequests != 0)
                {
                    Debug.LogWarning("You are trying to close a database connection with active requests! This might lead to unwanted behaviours...\nRequests still active: " + OpenedRequests);
                }

                Result = SQLite3.Close(Handle);
                if (Result == SQLite3.Result.OK)
                {
                    Handle = IntPtr.Zero;
                }
                else
                {
                    Debug.LogError("Couldn't close database!\nRequest result: " + Result);
                }
            }
            return Result;
        }

        public SQLite3.Result GetResult()
        {
            return Result;
        }

        public void Dispose()
        {
            Close();
        }

        static public implicit operator IntPtr(NWDSQLiteDbConnection sConnection)
        {
            return sConnection.Handle;
        }

        static public implicit operator SQLite3.Result(NWDSQLiteDbConnection sConnection)
        {
            return sConnection.Result;
        }
    }
}
