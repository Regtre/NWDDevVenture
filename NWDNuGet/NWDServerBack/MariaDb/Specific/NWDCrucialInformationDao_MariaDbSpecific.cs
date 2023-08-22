using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MariaDb.ByReflexion;

namespace NWDServerBack.MariaDb.Specific
{
    public class NWDCrucialInformationDao_MariaDbSpecific : NWDBackMariaDbSpecific, INWDCrucialInformationDao
    {
        private NWDBackMariaDbByReflexion _ToRemoveDao;

        public NWDCrucialInformationDao_MariaDbSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(sDatabaseCredentials)
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
            return _ToRemoveDao.TableExists<NWDCrucialInformation>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDCrucialInformation>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDCrucialInformation>(sEnvironment);
        }
        public NWDCrucialInformation Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDCrucialInformation sModel)
        {
            return _ToRemoveDao.Create<NWDCrucialInformation>(sEnvironment,sProjectId, sModel, true, false);
        }

        public NWDCrucialInformation Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDCrucialInformation sModel)
        {
            return _ToRemoveDao.Update<NWDCrucialInformation>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDCrucialInformation>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDCrucialInformation> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDCrucialInformation>(sEnvironment,sProjectId);
        }

        public List<NWDCrucialInformation> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDCrucialInformation>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDCrucialInformation InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDCrucialInformation sModel)
        {
            return _ToRemoveDao.InsertOrUpdate(sEnvironment,sProjectId, sModel, true, false);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDCrucialInformation>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDCrucialInformation>(sEnvironment);
        }

        public List<NWDCrucialInformation> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDCrucialInformation>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
        public ulong NewValidReference(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.NewValidReference(sEnvironment,sProjectId, typeof(NWDCrucialInformation), true);
        }
    }
}