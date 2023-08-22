using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDServerBack.MariaDb.ByReflexion
{
    public class NWDAccountTokenDao_MariaDbByReflexion : NWDBackMariaDbByReflexion, INWDAccountTokenDao, INWDDao
    {
        public NWDAccountTokenDao_MariaDbByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDAccountToken>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDAccountToken>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDAccountToken>(sEnvironment);
        }
        public NWDAccountToken Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountToken sModel)
        {
            return Create<NWDAccountToken>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccountToken Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountToken sModel)
        {
            return Update<NWDAccountToken>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDAccountToken>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccountToken> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDAccountToken>(sEnvironment,sProjectId);
        }

        public List<NWDAccountToken> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDAccountToken>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccountToken InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountToken sModel)
        {
            return InsertOrUpdate<NWDAccountToken>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDAccountToken>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDAccountToken>(sEnvironment);
        }

        public List<NWDAccountToken> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDAccountToken>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}