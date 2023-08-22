using NWDFoundation.Logger;
using NWDUnityShared.Tools;
using System;
using System.Text;
using UnityEngine;

namespace NWDUnityShared.SQLite
{
    public class NWDSQLiteDbRequest : INWDDBRequest, INWDPoolElement<NWDSQLiteDbRequest, NWDSQLiteDbConnection>
    {
        NWDPool<NWDSQLiteDbRequest, NWDSQLiteDbConnection> Pool;
        private NWDSQLiteDbConnection Connection;
        private IntPtr Handle = IntPtr.Zero;
        private SQLite3.Result Result;

        public void Use(NWDPool<NWDSQLiteDbRequest, NWDSQLiteDbConnection> sPool, NWDSQLiteDbConnection sValue)
        {
            Pool = sPool;
            Connection = sValue;
        }

        public INWDDBRequest Open(string sQuery)
        {
            Result = SQLite3.Result.OK;
            if (Handle == IntPtr.Zero)
            {
                Result = SQLite3.Prepare2(Connection, sQuery, Encoding.UTF8.GetByteCount(sQuery), out Handle, IntPtr.Zero);
                if (Result != SQLite3.Result.OK)
                {
                    Handle = IntPtr.Zero;
                    Debug.LogError("Couldn't prepare statement: " + sQuery + "!\nRequest result: " + Result);
                }
                else
                {
                    Connection.OpenedRequests++;
                }
            }
            return this;
        }

        public SQLite3.Result Step()
        {
            Result = SQLite3.Result.OK;
            if (Handle != IntPtr.Zero)
            {
                Result = SQLite3.Step(Handle);
                if (Result != SQLite3.Result.OK && Result != SQLite3.Result.Done && Result != SQLite3.Result.Row)
                {
                    Debug.LogError("Error while steping query!\nRequest result: " + Result);
                }
            }
            return Result;
        }

        public SQLite3.Result Close()
        {
            Result = SQLite3.Result.OK;
            if (Handle != IntPtr.Zero)
            {
                Result = SQLite3.Finalize(Handle);
                if (Result == SQLite3.Result.OK)
                {
                    Handle = IntPtr.Zero;
                    Connection.OpenedRequests--;
                }
                else
                {
                    Debug.LogError("Couldn't close database!\nRequest result: " + Result);
                }
            }
            return Result;
        }

        public SQLite3.Result Exec(string sQuery)
        {
            Result = SQLite3.Result.OK;
            try
            {
                Open(sQuery);
                if (Result == SQLite3.Result.OK)
                {
                    Result = Step();
                }
            }
            finally
            {
                Result = Close();
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
            Pool.SetAvailable(this);
        }

        ~NWDSQLiteDbRequest()
        {
            Close();
        }

        static public implicit operator IntPtr(NWDSQLiteDbRequest sRequest)
        {
            return sRequest.Handle;
        }

        static public implicit operator SQLite3.Result(NWDSQLiteDbRequest sRequest)
        {
            return sRequest.Result;
        }
    }
}
