using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDServerBack.MariaDb.ByReflexion
{
    public class NWDCrucialInformationDao_MariaDbByReflexion : NWDBackMariaDbByReflexion, INWDCrucialInformationDao, INWDDao
    {
        public NWDCrucialInformationDao_MariaDbByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDCrucialInformation>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDCrucialInformation>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDCrucialInformation>(sEnvironment);
        }
        public NWDCrucialInformation Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDCrucialInformation sModel)
        {
            return Create<NWDCrucialInformation>(sEnvironment,sProjectId, sModel, true, false);
        }

        public NWDCrucialInformation Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDCrucialInformation sModel)
        {
            return Update<NWDCrucialInformation>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDCrucialInformation>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDCrucialInformation> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDCrucialInformation>(sEnvironment,sProjectId);
        }

        public List<NWDCrucialInformation> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDCrucialInformation>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDCrucialInformation InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDCrucialInformation sModel)
        {
            return InsertOrUpdate(sEnvironment,sProjectId, sModel, true, false);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDCrucialInformation>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDCrucialInformation>(sEnvironment);
        }

        public List<NWDCrucialInformation> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDCrucialInformation>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
        public ulong NewValidReference(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return NewValidReference(sEnvironment,sProjectId, typeof(NWDCrucialInformation), true);
        }
    }
}