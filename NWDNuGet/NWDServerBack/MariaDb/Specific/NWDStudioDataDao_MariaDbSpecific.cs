using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MariaDb.ByReflexion;

namespace NWDServerBack.MariaDb.Specific
{
    public class NWDStudioDataDao_MariaDbSpecific : NWDBackMariaDbSpecific, INWDStudioDataDao
    {
        private NWDBackMariaDbByReflexion _ToRemoveDao;

        public NWDStudioDataDao_MariaDbSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(sDatabaseCredentials)
        {
            _ToRemoveDao = new NWDBackMariaDbByReflexion(sDatabaseCredentials);
        }
        public DateTime GetCurrentDatetime()
        {
            return _ToRemoveDao.GetCurrentDatetime();
        }
        public Int64 GenerateNewCommitId()
        {
            return _ToRemoveDao.GenerateNewCommitId();
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.TableExists<NWDStudioDataStorage>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDStudioDataStorage>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDStudioDataStorage>(sEnvironment);
        }
        public NWDStudioDataStorage Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDStudioDataStorage sModel)
        {
            return _ToRemoveDao.Create<NWDStudioDataStorage>(sEnvironment,sProjectId, sModel, true, false);
        }

        public NWDStudioDataStorage Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDStudioDataStorage sModel)
        {
            return _ToRemoveDao.Update<NWDStudioDataStorage>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDStudioDataStorage>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDStudioDataStorage> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDStudioDataStorage>(sEnvironment,sProjectId);
        }

        public List<NWDStudioDataStorage> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDStudioDataStorage>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDStudioDataStorage InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDStudioDataStorage sModel)
        {
            return _ToRemoveDao.InsertOrUpdate(sEnvironment,sProjectId, sModel, true, false);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDStudioDataStorage>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDStudioDataStorage>(sEnvironment);
        }

        public List<NWDStudioDataStorage> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDStudioDataStorage>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
        public ulong NewValidReference(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.NewValidReference(sEnvironment,sProjectId, typeof(NWDStudioDataStorage), true);
        }
    }
}