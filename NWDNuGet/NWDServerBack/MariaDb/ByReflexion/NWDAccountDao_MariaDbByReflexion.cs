using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDServerBack.MariaDb.ByReflexion
{
    public class NWDAccountDao_MariaDbByReflexion : NWDBackMariaDbByReflexion, INWDAccountDao, INWDDao
    {
        public NWDAccountDao_MariaDbByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDAccount>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDAccount>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDAccount>(sEnvironment);
        }
        public NWDAccount Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccount sModel)
        {
            return Create<NWDAccount>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccount Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccount sModel)
        {
            return Update<NWDAccount>(sEnvironment, sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDAccount>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccount> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDAccount>(sEnvironment,sProjectId);
        }

        public List<NWDAccount> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDAccount>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccount InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccount sModel)
        {
            return InsertOrUpdate<NWDAccount>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDAccount>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDAccount>(sEnvironment);
        }

        public List<NWDAccount> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDAccount>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}