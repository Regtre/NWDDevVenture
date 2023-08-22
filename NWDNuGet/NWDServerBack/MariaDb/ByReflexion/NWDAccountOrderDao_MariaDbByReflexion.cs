using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDServerBack.MariaDb.ByReflexion
{
    public class NWDAccountOrderDao_MariaDbByReflexion : NWDBackMariaDbByReflexion, INWDAccountOrderDao, INWDDao
    {
        public NWDAccountOrderDao_MariaDbByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDAccountOrder>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDAccountOrder>(sEnvironment);
        }

        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDAccountOrder>(sEnvironment);
        }
        public NWDAccountOrder Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountOrder sModel)
        {
            return Create<NWDAccountOrder>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccountOrder Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountOrder sModel)
        {
            return Update<NWDAccountOrder>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDAccountOrder>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccountOrder> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDAccountOrder>(sEnvironment,sProjectId);
        }

        public List<NWDAccountOrder> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDAccountOrder>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccountOrder InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountOrder sModel)
        {
            return InsertOrUpdate<NWDAccountOrder>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDAccountOrder>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDAccountOrder>(sEnvironment);
        }

        public List<NWDAccountOrder> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDAccountOrder>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}