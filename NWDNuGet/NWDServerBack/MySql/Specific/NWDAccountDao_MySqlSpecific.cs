using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MySql.ByReflexion;

namespace NWDServerBack.MySql.Specific
{
    public class NWDAccountDao_MySqlSpecific : NWDBackMySqlSpecific, INWDAccountDao
    {
        private NWDBackMySqlByReflexion _ToRemoveDao;

        public NWDAccountDao_MySqlSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
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
            return _ToRemoveDao.TableExists<NWDAccount>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDAccount>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDAccount>(sEnvironment);
        }
        public NWDAccount Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccount sModel)
        {
            return _ToRemoveDao.Create<NWDAccount>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccount Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccount sModel)
        {
            return _ToRemoveDao.Update<NWDAccount>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDAccount>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccount> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDAccount>(sEnvironment,sProjectId);
        }

        public List<NWDAccount> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDAccount>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccount InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccount sModel)
        {
            return _ToRemoveDao.InsertOrUpdate<NWDAccount>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDAccount>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDAccount>(sEnvironment);
        }

        public List<NWDAccount> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDAccount>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}