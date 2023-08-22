using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDServerBack.MySql.ByReflexion
{
    public class NWDAccountServiceDao_MySqlByReflexion : NWDBackMySqlByReflexion, INWDAccountServiceDao, INWDDao
    {
        public NWDAccountServiceDao_MySqlByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDAccountService>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDAccountService>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDAccountService>(sEnvironment);
        }
        public NWDAccountService Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountService sModel)
        {
            return Create<NWDAccountService>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccountService Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountService sModel)
        {
            return Update<NWDAccountService>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDAccountService>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccountService> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDAccountService>(sEnvironment,sProjectId);
        }

        public List<NWDAccountService> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDAccountService>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccountService InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountService sModel)
        {
            return InsertOrUpdate<NWDAccountService>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDAccountService>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDAccountService>(sEnvironment);
        }

        public List<NWDAccountService> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDAccountService>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }

    }
}