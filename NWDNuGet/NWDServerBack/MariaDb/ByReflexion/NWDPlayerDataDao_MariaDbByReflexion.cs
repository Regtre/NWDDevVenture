using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDServerBack.MariaDb.ByReflexion
{
    public class NWDPlayerDataDao_MariaDbByReflexion : NWDBackMariaDbByReflexion, INWDPlayerDataDao, INWDDao
    {
        public NWDPlayerDataDao_MariaDbByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDPlayerDataStorage>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDPlayerDataStorage>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDPlayerDataStorage>(sEnvironment);
        }
        public NWDPlayerDataStorage Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDPlayerDataStorage sModel)
        {
            return Create<NWDPlayerDataStorage>(sEnvironment,sProjectId, sModel, true, false);
        }

        public NWDPlayerDataStorage Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDPlayerDataStorage sModel)
        {
            return Update<NWDPlayerDataStorage>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDPlayerDataStorage>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDPlayerDataStorage> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDPlayerDataStorage>(sEnvironment,sProjectId);
        }

        public List<NWDPlayerDataStorage> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDPlayerDataStorage>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDPlayerDataStorage InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDPlayerDataStorage sModel)
        {
            return InsertOrUpdate(sEnvironment,sProjectId, sModel, true, false);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDPlayerDataStorage>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDPlayerDataStorage>(sEnvironment);
        }

        public List<NWDPlayerDataStorage> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDPlayerDataStorage>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}