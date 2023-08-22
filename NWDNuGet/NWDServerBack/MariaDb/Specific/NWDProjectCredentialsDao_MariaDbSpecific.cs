using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MariaDb.ByReflexion;
using NWDCrucial.Facades;
using NWDCrucial.Models;

namespace NWDServerBack.MariaDb.Specific
{
    public class NWDProjectCredentialsDaoMariaDbSpecific : NWDBackMariaDbSpecific, INWDProjectCredentialsDao
    {
        private NWDBackMariaDbByReflexion _ToRemoveDao;

        public NWDProjectCredentialsDaoMariaDbSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
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
            return _ToRemoveDao.TableExists<NWDProjectCredentials>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDProjectCredentials>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDProjectCredentials>(sEnvironment);
        }
        public NWDProjectCredentials Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectCredentials sModel)
        {
            return _ToRemoveDao.Create<NWDProjectCredentials>(sEnvironment,sProjectId, sModel, true, false);
        }

        public NWDProjectCredentials Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectCredentials sModel)
        {
            return _ToRemoveDao.Update<NWDProjectCredentials>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDProjectCredentials>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDProjectCredentials> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDProjectCredentials>(sEnvironment,sProjectId);
        }

        public List<NWDProjectCredentials> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDProjectCredentials>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDProjectCredentials InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectCredentials sModel)
        {
            return _ToRemoveDao.InsertOrUpdate<NWDProjectCredentials>(sEnvironment,sProjectId, sModel, true, false);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDProjectCredentials>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDProjectCredentials>(sEnvironment);
        }

        public List<NWDProjectCredentials> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDProjectCredentials>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}