using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDServerBack.MariaDb.ByReflexion
{
    public class NWDStudioDataDao_MariaDbByReflexion : NWDBackMariaDbByReflexion, INWDStudioDataDao, INWDDao
    {
        public NWDStudioDataDao_MariaDbByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDStudioDataStorage>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDStudioDataStorage>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDStudioDataStorage>(sEnvironment);
        }
        public NWDStudioDataStorage Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDStudioDataStorage sModel)
        {
            return Create<NWDStudioDataStorage>(sEnvironment,sProjectId, sModel, true, false);
        }

        public NWDStudioDataStorage Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDStudioDataStorage sModel)
        {
            return Update<NWDStudioDataStorage>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDStudioDataStorage>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDStudioDataStorage> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDStudioDataStorage>(sEnvironment,sProjectId);
        }

        public List<NWDStudioDataStorage> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDStudioDataStorage>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDStudioDataStorage InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDStudioDataStorage sModel)
        {
            return InsertOrUpdate(sEnvironment,sProjectId, sModel, true, false, new Dictionary<string, string>()
            {
                {nameof(NWDStudioDataStorage.DataTrack), sModel.DataTrack.ToString() }
            });
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDStudioDataStorage>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDStudioDataStorage>(sEnvironment);
        }

        public List<NWDStudioDataStorage> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDStudioDataStorage>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
        public ulong NewValidReference(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return NewValidReference(sEnvironment,sProjectId, typeof(NWDStudioDataStorage), true);
        }
    }
}