using System.Collections.Generic;
using NWDCrucial.Facades;
using NWDCrucial.Models;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDServerBack.MariaDb.ByReflexion
{
    public class NWDProjectServiceKeyDao_MariaDbByReflexion : NWDBackMariaDbByReflexion, INWDProjectServiceKeyDao
    {
        public NWDProjectServiceKeyDao_MariaDbByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDProjectServiceKey>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDProjectServiceKey>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDProjectServiceKey>(sEnvironment);
        }
        public NWDProjectServiceKey Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectServiceKey sModel)
        {
            return Create<NWDProjectServiceKey>(sEnvironment,sProjectId, sModel, true, false);
        }

        public NWDProjectServiceKey Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectServiceKey sModel)
        {
            return Update<NWDProjectServiceKey>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDProjectServiceKey>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDProjectServiceKey> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDProjectServiceKey>(sEnvironment,sProjectId);
        }

        public List<NWDProjectServiceKey> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDProjectServiceKey>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDProjectServiceKey InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectServiceKey sModel)
        {
            return InsertOrUpdate<NWDProjectServiceKey>(sEnvironment,sProjectId, sModel, true, false);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDProjectServiceKey>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDProjectServiceKey>(sEnvironment);
        }

        public List<NWDProjectServiceKey> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDProjectServiceKey>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}