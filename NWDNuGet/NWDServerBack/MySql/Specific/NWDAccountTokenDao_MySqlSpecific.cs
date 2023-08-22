using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MySql.ByReflexion;

namespace NWDServerBack.MySql.Specific
{
    public class NWDAccountTokenDao_MySqlSpecific : NWDBackMySqlSpecific, INWDAccountTokenDao
    {
        private NWDBackMySqlByReflexion _ToRemoveDao;

        public NWDAccountTokenDao_MySqlSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(
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
            return _ToRemoveDao.TableExists<NWDAccountToken>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDAccountToken>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDAccountToken>(sEnvironment);
        }
        public NWDAccountToken Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountToken sModel)
        {
            return _ToRemoveDao.Create<NWDAccountToken>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccountToken Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountToken sModel)
        {
            return _ToRemoveDao.Update<NWDAccountToken>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDAccountToken>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccountToken> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDAccountToken>(sEnvironment,sProjectId);
        }

        public List<NWDAccountToken> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDAccountToken>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccountToken InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountToken sModel)
        {
            return _ToRemoveDao.InsertOrUpdate<NWDAccountToken>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDAccountToken>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDAccountToken>(sEnvironment);
        }

        public List<NWDAccountToken> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDAccountToken>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}