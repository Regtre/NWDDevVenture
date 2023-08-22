using System;
using System.Collections.Generic;
using NWDCrucial.Facades;
using NWDCrucial.Models;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MySql.ByReflexion;

namespace NWDServerBack.MySql.Specific
{
    public class NWDProjectServiceKeyDao_MySqlSpecific : NWDBackMySqlSpecific, INWDProjectServiceKeyDao
    {
        private NWDBackMySqlByReflexion _ToRemoveDao;

        public NWDProjectServiceKeyDao_MySqlSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(sDatabaseCredentials)
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
            return _ToRemoveDao.TableExists<NWDProjectServiceKey>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDProjectServiceKey>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDProjectServiceKey>(sEnvironment);
        }
        public NWDProjectServiceKey Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectServiceKey sModel)
        {
            return _ToRemoveDao.Create<NWDProjectServiceKey>(sEnvironment,sProjectId, sModel, true, false);
        }

        public NWDProjectServiceKey Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectServiceKey sModel)
        {
            return _ToRemoveDao.Update<NWDProjectServiceKey>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDProjectServiceKey>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDProjectServiceKey> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDProjectServiceKey>(sEnvironment,sProjectId);
        }

        public List<NWDProjectServiceKey> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDProjectServiceKey>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDProjectServiceKey InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectServiceKey sModel)
        {
            return _ToRemoveDao.InsertOrUpdate<NWDProjectServiceKey>(sEnvironment,sProjectId, sModel, true, false);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDProjectServiceKey>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDProjectServiceKey>(sEnvironment);
        }

        public List<NWDProjectServiceKey> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDProjectServiceKey>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}