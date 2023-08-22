using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MySql.ByReflexion;

namespace NWDServerBack.MySql.Specific
{
    public class NWDAccountSignDao_MySqlSpecific : NWDBackMySqlSpecific, INWDAccountSignDao
    {
        private NWDBackMySqlByReflexion _ToRemoveDao;

        public NWDAccountSignDao_MySqlSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(sDatabaseCredentials)
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
            return _ToRemoveDao.TableExists<NWDAccountSign>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDAccountSign>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDAccountSign>(sEnvironment);
        }
        public NWDAccountSign Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountSign sModel)
        {
            return _ToRemoveDao.Create<NWDAccountSign>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccountSign Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountSign sModel)
        {
            return _ToRemoveDao.Update<NWDAccountSign>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDAccountSign>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccountSign> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDAccountSign>(sEnvironment,sProjectId);
        }

        public List<NWDAccountSign> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDAccountSign>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccountSign InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountSign sModel)
        {
            return _ToRemoveDao.InsertOrUpdate<NWDAccountSign>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDAccountSign>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDAccountSign>(sEnvironment);
        }

        public List<NWDAccountSign> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDAccountSign>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}