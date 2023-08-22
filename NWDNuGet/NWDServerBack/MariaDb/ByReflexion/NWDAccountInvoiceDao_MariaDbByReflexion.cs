using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDServerBack.MariaDb.ByReflexion
{
    public class NWDAccountInvoiceDao_MariaDbByReflexion : NWDBackMariaDbByReflexion, INWDAccountInvoiceDao, INWDDao
    {
        public NWDAccountInvoiceDao_MariaDbByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
            sDatabaseCredentials)
        {
        }
        public bool TableExists(NWDEnvironmentKind sEnvironment)
        {
            return TableExists<NWDAccountInvoice>(sEnvironment);
        }
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrintName<NWDAccountInvoice>(sEnvironment);
        }

        public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
        {
            return FingerPrint<NWDAccountInvoice>(sEnvironment);
        }
        public NWDAccountInvoice Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountInvoice sModel)
        {
            return Create<NWDAccountInvoice>(sEnvironment,sProjectId, sModel, true, true);
        }

        public NWDAccountInvoice Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountInvoice sModel)
        {
            return Update<NWDAccountInvoice>(sEnvironment,sProjectId, sModel);
        }

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
        {
            Delete<NWDAccountInvoice>(sEnvironment,sProjectId, sReference);
        }

        public List<NWDAccountInvoice> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
        {
            return FindAll<NWDAccountInvoice>(sEnvironment,sProjectId);
        }

        public List<NWDAccountInvoice> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
        {
            return FindAllModified<NWDAccountInvoice>(sEnvironment,sProjectId, sSyncDate);
        }

        public NWDAccountInvoice InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountInvoice sModel)
        {
            return InsertOrUpdate<NWDAccountInvoice>(sEnvironment,sProjectId, sModel, true, true);
        }

        public void CreateTable(NWDEnvironmentKind sEnvironment)
        {
            CreateTable<NWDAccountInvoice>(sEnvironment);
        }
        public void DeleteTable(NWDEnvironmentKind sEnvironment)
        {
            DeleteTable<NWDAccountInvoice>(sEnvironment);
        }

        public List<NWDAccountInvoice> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "")
        {
            return GetBy<NWDAccountInvoice>(sEnvironment,sProjectId, sDictionary, sAndWhere);
        }
    }
}