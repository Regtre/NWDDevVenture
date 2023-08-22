using System;

namespace NWDFoundation.Configuration.Databases
{
    [Serializable]
    public enum NWDDatabaseKind
    {
        Memory = -1,
        None = 0,
        MySql = 10,
        ReflexionMysql = 11,
        MariaDb = 20,
        ReflexionMariaDb = 21,
        NoSql = 30,
        SqLite = 40,
        PostgreSql = 50,
    }
}