using System;
using NWDFoundation.Tools;

namespace NWDFoundation.Configuration.Databases
{
    [Serializable]
    public class NWDDatabaseCredentials
    {
        #region properties

        public string Name { set; get; } = "Name's database";
        public ushort Range { set; get; } = 1;
        public NWDDatabaseKind Kind { set; get; } = NWDDatabaseKind.MySql;
        public string Server { set; get; } = "10.10.10.10";
        public string User { set; get; } = "User";
        public string Database { set; get; } = "MyDatabase";
        public string TablePrefix { set; get; } = NWDRandom.RandomStringToken(4);
        public int Port { set; get; } = 3652;
        public string Password { set; get; } = NWDRandom.RandomStringToken(32);
        public NWDDatabaseConnectionSsl Secure { set; get; } = NWDDatabaseConnectionSsl.Required;

        #endregion
    }
}