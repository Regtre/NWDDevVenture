using System;
using MySql.Data.MySqlClient;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Logger;

namespace NWDDatabaseAccess
{
    public class NWDDatabaseConnector
    {
        private readonly NWDDatabaseCredentials _Credentials;
        private readonly string _ConnStr;

        public NWDDatabaseConnector(NWDDatabaseCredentials sCredentials)
        {
            _Credentials = sCredentials;
            _ConnStr = string.Empty;
            switch (_Credentials.Kind)
            {
                case NWDDatabaseKind.Memory:
                {
                }
                    break;
                case NWDDatabaseKind.None:
                {
                }
                    break;
                case NWDDatabaseKind.SqLite:
                {
                    _ConnStr = "/tmp/" + _Credentials.Server + _Credentials.Database + ".nwdb";
                }
                    break;
                case NWDDatabaseKind.NoSql:
                {
                    _ConnStr =
                        "server=" + _Credentials.Server +
                        ";user=" + _Credentials.User +
                        ";database=" + _Credentials.Database +
                        ";port=" + _Credentials.Port +
                        ";password=" + _Credentials.Password +
                        ";SSL Mode=" + _Credentials.Secure.ToString();
                }
                    break;
                case NWDDatabaseKind.MySql:
                case NWDDatabaseKind.MariaDb:
                case NWDDatabaseKind.ReflexionMysql:
                case NWDDatabaseKind.ReflexionMariaDb:
                {
                    _ConnStr =
                        "server=" + _Credentials.Server +
                        ";user=" + _Credentials.User +
                        ";database=" + _Credentials.Database +
                        ";port=" + _Credentials.Port +
                        ";password=" + _Credentials.Password +
                        ";SSL Mode=" + _Credentials.Secure.ToString();
                }
                    break;
            }
        }

        public string GetTableNamePrefix()
        {
            return _Credentials.TablePrefix;
        }

        public string GetDatabaseName()
        {
            return _Credentials.Database;
        }

        private MySqlConnection NewConnection()
        {
            MySqlConnection rReturn = new MySqlConnection(_ConnStr);
            return rReturn;
        }

        private void DeleteConnection(MySqlConnection sConnection)
        {
            try
            {
                sConnection.Close();
            }
            catch (Exception tEx)
            {
                NWDLogger.Critical(tEx.ToString());
            }

            sConnection.Dispose();
        }

        public MySqlConnection Connection()
        {
            MySqlConnection rReturn = NewConnection();
            return rReturn;
        }

        public void Open(MySqlConnection sConn)
        {
            try
            {
                sConn.Open();
            }
            catch (Exception tEx)
            {
                NWDLogger.TraceAttention("Connection failed with : " + _ConnStr);
                switch (_Credentials.Kind)
                {
                    case NWDDatabaseKind.None:
                        NWDLogger.TraceAttention("NO STORAGE : VERY DANGEROUS!");
                        break;
                    case NWDDatabaseKind.Memory:
                        NWDLogger.TraceAttention("Data storage in memory ... flush when restart services : VERY DANGEROUS!");
                        break;
                    case NWDDatabaseKind.NoSql:
                        NWDLogger.TraceAttention("try install Docker MariaDB >#");
                        NWDLogger.TraceAttention("docker run -p 127.0.0.1:" + _Credentials.Port + ":27017 --name NWDDatabaseMongoDB -e MONGO_INITDB_ROOT_USERNAME=root -e MONGO_INITDB_ROOT_PASSWORD=HYgetrdfderdQdgfaezfde -d mongo ");
                        break;
                    case NWDDatabaseKind.SqLite:
                        NWDLogger.TraceAttention("Try SqLite create ");
                        NWDLogger.TraceAttention("" + _Credentials.Database + ".sqlite");
                        break;
                    case NWDDatabaseKind.MariaDb:
                    case NWDDatabaseKind.ReflexionMariaDb:
                        NWDLogger.TraceAttention("Try install Docker MariaDB >#");
                        NWDLogger.TraceAttention("docker run -p 127.0.0.1:" + _Credentials.Port + ":3306 --name NWDDatabaseMariaDB -e MYSQL_ROOT_PASSWORD=HYgetrdfderdQdgfaezfde -d mariadb:10.9.5 ");
                        NWDLogger.TraceAttention("Connect as root and execute:");
                        NWDLogger.TraceAttention("CREATE DATABASE " + _Credentials.Database + " CHARACTER SET utf8 COLLATE utf8_unicode_ci; ");
                        NWDLogger.TraceAttention("CREATE USER '" + _Credentials.User + "'@'%' IDENTIFIED BY '" + _Credentials.Password + "'; ");
                        NWDLogger.TraceAttention("GRANT ALL ON *.* TO '" + _Credentials.User + "'@'%' WITH GRANT OPTION;");
                        NWDLogger.TraceAttention("FLUSH PRIVILEGES; ");
                        NWDLogger.TraceAttention("SELECT * FROM mysql.user; ");
                        break;
                    case NWDDatabaseKind.MySql:
                    case NWDDatabaseKind.ReflexionMysql:
                        NWDLogger.TraceAttention("Try install Docker MySql >#");
                        NWDLogger.TraceAttention("docker run -p 127.0.0.1:" + _Credentials.Port +":3306 --name NWDDatabaseMysql -e MYSQL_ROOT_PASSWORD=HYgetrdfderdQdgfaezfde -d mysql ");
                        NWDLogger.TraceAttention("Connect as root and execute:");
                        NWDLogger.TraceAttention("CREATE DATABASE " + _Credentials.Database + " CHARACTER SET utf8 COLLATE utf8_unicode_ci; ");
                        NWDLogger.TraceAttention("CREATE USER '" + _Credentials.User + "'@'%' IDENTIFIED BY '" + _Credentials.Password + "'; ");
                        NWDLogger.TraceAttention("GRANT ALL ON *.* TO '" + _Credentials.User + "'@'%' WITH GRANT OPTION;");
                        NWDLogger.TraceAttention("FLUSH PRIVILEGES; ");
                        NWDLogger.TraceAttention("SELECT * FROM mysql.user; ");
                        break;
                }
                NWDLogger.TraceAttention(tEx.ToString());
            }
        }

        public void Close(MySqlConnection sConn)
        {
            DeleteConnection(sConn);
        }

        public bool TestConnection()
        {
            bool rReturn = false;
            MySqlConnection tConn = Connection();
            Open(tConn);
            try
            {
                MySqlCommand tCmd = new MySqlCommand("SELECT 1", tConn);
                MySqlDataReader tRdr = tCmd.ExecuteReader();
                while (tRdr.Read())
                {
                    if (tRdr.GetInt64(0) == 1)
                    {
                        rReturn = true;
                    }
                }

                tRdr.Close();
                tRdr.Dispose();
                tCmd.Dispose();
            }
            catch (Exception tEx)
            {
                NWDLogger.Critical(tEx.ToString());
            }

            Close(tConn);
            return rReturn;
        }
    }
}