using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDAccountInvoiceDao : INWDDao
    {
        #region interfaces

        public NWDAccountInvoice Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountInvoice sModel);
        public NWDAccountInvoice Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountInvoice sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDAccountInvoice> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);
        public List<NWDAccountInvoice> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate);
        public NWDAccountInvoice InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountInvoice sModel);

        public List<NWDAccountInvoice> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary, string sAndWhere = "");

        #endregion
    }
}