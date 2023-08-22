using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MySql.ByReflexion;

namespace NWDServerBack.MySql.Specific
{
    public class NWDAccountOrderDao_MySqlSpecific : NWDBackMySqlSpecific, INWDAccountOrderDao
    {
        private NWDBackMySqlByReflexion _ToRemoveDao;

        public NWDAccountOrderDao_MySqlSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(sDatabaseCredentials)
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
            return _ToRemoveDao.TableExists<NWDAccountOrder>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDAccountOrder>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDAccountOrder>(sEnvironment);
        }
        public NWDAccountOrder Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountOrder sModel)
        {
            return _ToRemoveDao.Create<NWDAccountOrder>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccountOrder Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountOrder sModel)
        {
            return _ToRemoveDao.Update<NWDAccountOrder>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDAccountOrder>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccountOrder> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDAccountOrder>(sEnvironment,sProjectId);
        }

        public List<NWDAccountOrder> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDAccountOrder>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccountOrder InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountOrder sModel)
        {
            return _ToRemoveDao.InsertOrUpdate<NWDAccountOrder>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDAccountOrder>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDAccountOrder>(sEnvironment);
        }

        public List<NWDAccountOrder> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDAccountOrder>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}