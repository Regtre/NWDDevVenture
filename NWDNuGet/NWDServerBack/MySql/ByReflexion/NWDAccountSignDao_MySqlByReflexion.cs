using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDServerBack.MySql.ByReflexion
{
    public class NWDAccountSignDao_MySqlByReflexion : NWDBackMySqlByReflexion, INWDAccountSignDao, INWDDao
    {
        public NWDAccountSignDao_MySqlByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDAccountSign>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDAccountSign>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDAccountSign>(sEnvironment);
        }
        public NWDAccountSign Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountSign sModel)
        {
            return Create<NWDAccountSign>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccountSign Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountSign sModel)
        {
            return Update<NWDAccountSign>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDAccountSign>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccountSign> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDAccountSign>(sEnvironment,sProjectId);
        }

        public List<NWDAccountSign> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDAccountSign>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccountSign InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountSign sModel)
        {
            return InsertOrUpdate<NWDAccountSign>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDAccountSign>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDAccountSign>(sEnvironment);
        }

        public List<NWDAccountSign> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDAccountSign>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}