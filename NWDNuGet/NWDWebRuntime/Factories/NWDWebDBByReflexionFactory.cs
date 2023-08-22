using NWDFoundation.Configuration.Databases;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDWebRuntime.Tools;

namespace NWDWebRuntime.Factories;

public class NWDWebDbByReflexionFactory
{
    public static INWDWebDBByReflexion? GetNWDWebDBByReflexion(NWDDatabaseKind sDatabaseKind)
    {
        //NWDLogger.Information("Generate "+nameof(INWDWebDBByReflexion) +" for " + sDatabaseKind.ToString());
        INWDWebDBByReflexion? rReturn = null;
        switch (sDatabaseKind)
        {
            case NWDDatabaseKind.Memory:
                break;
            case NWDDatabaseKind.None:
                break;
            case NWDDatabaseKind.MySql:
                // don't use specific DAO for Web Database
                rReturn = new NWDWebMySqlByReflexion();
                break;
            case NWDDatabaseKind.NoSql:
                break;
            case NWDDatabaseKind.SqLite:
                break;
            case NWDDatabaseKind.MariaDb:
                // don't use specific DAO for Web Database
                rReturn = new NWDWebMariaByReflexion();
                break;
            case NWDDatabaseKind.ReflexionMariaDb:
                rReturn = new NWDWebMariaByReflexion();
                break;
            case NWDDatabaseKind.ReflexionMysql:
                rReturn = new NWDWebMySqlByReflexion();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return rReturn;
    }
}