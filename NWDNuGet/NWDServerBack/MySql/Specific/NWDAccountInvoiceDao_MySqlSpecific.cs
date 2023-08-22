using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDServerBack.MySql.ByReflexion;

namespace NWDServerBack.MySql.Specific
{
    public class NWDAccountInvoiceDao_MySqlSpecific : NWDBackMySqlSpecific, INWDAccountInvoiceDao
    {
        private NWDBackMySqlByReflexion _ToRemoveDao;

        public NWDAccountInvoiceDao_MySqlSpecific(NWDDatabaseCredentials sDatabaseCredentials) : base(sDatabaseCredentials)
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
            return _ToRemoveDao.TableExists<NWDAccountInvoice>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrintName<NWDAccountInvoice>(sEnvironment);
        }
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return _ToRemoveDao.FingerPrint<NWDAccountInvoice>(sEnvironment);
        }
        public NWDAccountInvoice Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountInvoice sModel)
        {
            return _ToRemoveDao.Create<NWDAccountInvoice>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccountInvoice Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountInvoice sModel)
        {
            return _ToRemoveDao.Update<NWDAccountInvoice>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            _ToRemoveDao.Delete<NWDAccountInvoice>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccountInvoice> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return _ToRemoveDao.FindAll<NWDAccountInvoice>(sEnvironment,sProjectId);
        }

        public List<NWDAccountInvoice> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return _ToRemoveDao.FindAllModified<NWDAccountInvoice>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccountInvoice InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountInvoice sModel)
        {
            return _ToRemoveDao.InsertOrUpdate<NWDAccountInvoice>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.CreateTable<NWDAccountInvoice>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            _ToRemoveDao.DeleteTable<NWDAccountInvoice>(sEnvironment);
        }

        public List<NWDAccountInvoice> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return _ToRemoveDao.GetBy<NWDAccountInvoice>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}