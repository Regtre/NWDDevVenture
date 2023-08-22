using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDCrucial.Facades;
using NWDCrucial.Models;

namespace NWDServerBack.MySql.ByReflexion
{
    public class NWDProjectCredentialsDaoMySqlByReflexion : NWDBackMySqlByReflexion, INWDProjectCredentialsDao,
        INWDDao
    {
        public NWDProjectCredentialsDaoMySqlByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDProjectCredentials>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDProjectCredentials>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDProjectCredentials>(sEnvironment);
        }
        public NWDProjectCredentials Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectCredentials sModel)
        {
            return Create<NWDProjectCredentials>(sEnvironment,sProjectId, sModel, true, false);
        }

        public NWDProjectCredentials Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectCredentials sModel)
        {
            return Update<NWDProjectCredentials>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDProjectCredentials>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDProjectCredentials> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDProjectCredentials>(sEnvironment,sProjectId);
        }

        public List<NWDProjectCredentials> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDProjectCredentials>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDProjectCredentials InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectCredentials sModel)
        {
            return InsertOrUpdate<NWDProjectCredentials>(sEnvironment,sProjectId, sModel, true, false);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDProjectCredentials>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDProjectCredentials>(sEnvironment);
        }

        public List<NWDProjectCredentials> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDProjectCredentials>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}