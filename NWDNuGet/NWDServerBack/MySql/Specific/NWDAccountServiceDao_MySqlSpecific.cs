using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MySql.ByReflexion;

namespace NWDServerBack.MySql.Specific
{
    public class NWDAccountServiceDao_MySqlSpecific : NWDBackMySqlSpecific, INWDAccountServiceDao
    {
        private NWDBackMySqlByReflexion _ToRemoveDao;

        public NWDAccountServiceDao_MySqlSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(
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
            return _ToRemoveDao.TableExists<NWDAccountService>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDAccountService>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDAccountService>(sEnvironment);
        }
        public NWDAccountService Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountService sModel)
        {
            return _ToRemoveDao.Create<NWDAccountService>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccountService Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountService sModel)
        {
            return _ToRemoveDao.Update<NWDAccountService>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDAccountService>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccountService> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDAccountService>(sEnvironment,sProjectId);
        }

        public List<NWDAccountService> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDAccountService>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccountService InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountService sModel)
        {
            return _ToRemoveDao.InsertOrUpdate<NWDAccountService>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDAccountService>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDAccountService>(sEnvironment);
        }

        public List<NWDAccountService> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDAccountService>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}