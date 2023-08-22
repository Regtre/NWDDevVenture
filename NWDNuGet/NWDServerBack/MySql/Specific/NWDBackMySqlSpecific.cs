using System;
using System.Data;
using MySql.Data.MySqlClient;
using NWDDatabaseAccess;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Tools;

namespace NWDServerBack.MySql.Specific
{
    public class NWDBackMySqlSpecific
    {
        private readonly NWDDatabaseConnector _DBConnector;
        private readonly ushort _Range;
        private readonly string _Infos;

        protected NWDBackMySqlSpecific(NWDDatabaseCredentials sDatabaseCredentials)
        {
            _Infos = /*sDatabaseCredentials.Server + " " +*/ sDatabaseCredentials.Database;
            _DBConnector = new NWDDatabaseConnector(sDatabaseCredentials);
            _Range = sDatabaseCredentials.Range;
        }
        public string GetInfos()
        {
            return _Infos;
        }

        public ushort GetRange()
        {
            return _Range;
        }

        public bool TestConnexion()
        {
            bool rReturn = false;
            MySqlConnection tConn = _DBConnector.Connection();
            _DBConnector.Open(tConn);
            if (tConn.State == ConnectionState.Open)
            {
                rReturn = true;
            }

            _DBConnector.Close(tConn);
            return rReturn;
        }
    }
}