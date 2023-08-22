using NWDFoundation.Logger;
using NWDUnityShared.Tools;
using System;
using System.Text;

namespace NWDUnityShared.SQLite
{
    public class NWDSQLiteDbTransaction : INWDDBTransaction, INWDPoolElement<NWDSQLiteDbTransaction, NWDSQLiteDbConnection>
    {
        private NWDPool<NWDSQLiteDbTransaction, NWDSQLiteDbConnection> Pool;
        private NWDSQLiteDbConnection Connection;
        private SQLite3.Result Result;

        public void Use(NWDPool<NWDSQLiteDbTransaction, NWDSQLiteDbConnection> sPool, NWDSQLiteDbConnection sValue)
        {
            Pool = sPool;
            Connection = sValue;
        }

        public INWDDBTransaction Open()
        {
            Result = Connection.Exec("BEGIN;");
            return this;
        }

        public SQLite3.Result Rollback()
        {
            return Connection.Exec("ROLLBACK;");
        }

        public SQLite3.Result Commit()
        {
            return Connection.Exec("COMMIT;");
        }

        public SQLite3.Result GetResult()
        {
            return Result;
        }

        public void Dispose()
        {
            Commit();
            Pool.SetAvailable(this);
        }
    }
}
