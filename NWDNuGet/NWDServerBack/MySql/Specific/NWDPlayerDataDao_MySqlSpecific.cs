using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MySql.ByReflexion;

namespace NWDServerBack.MySql.Specific
{
    public class NWDPlayerDataDao_MySqlSpecific : NWDBackMySqlSpecific, INWDPlayerDataDao
    {
        private NWDBackMySqlByReflexion _ToRemoveDao;

        public NWDPlayerDataDao_MySqlSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(sDatabaseCredentials)
        {
            _ToRemoveDao = new NWDBackMySqlByReflexion(sDatabaseCredentials);
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
            return _ToRemoveDao.TableExists<NWDPlayerDataStorage>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDPlayerDataStorage>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDPlayerDataStorage>(sEnvironment);
        }
        public NWDPlayerDataStorage Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDPlayerDataStorage sModel)
        {
            return _ToRemoveDao.Create<NWDPlayerDataStorage>(sEnvironment,sProjectId, sModel, true, false);
        }

        public NWDPlayerDataStorage Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDPlayerDataStorage sModel)
        {
            return _ToRemoveDao.Update<NWDPlayerDataStorage>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDPlayerDataStorage>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDPlayerDataStorage> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDPlayerDataStorage>(sEnvironment,sProjectId);
        }

        public List<NWDPlayerDataStorage> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDPlayerDataStorage>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDPlayerDataStorage InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDPlayerDataStorage sModel)
        {
            return _ToRemoveDao.InsertOrUpdate<NWDPlayerDataStorage>(sEnvironment,sProjectId, sModel, true, false);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDPlayerDataStorage>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDPlayerDataStorage>(sEnvironment);
        }

        public List<NWDPlayerDataStorage> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDPlayerDataStorage>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}