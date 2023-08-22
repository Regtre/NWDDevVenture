using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDAccountServiceDao : INWDDao
    {
        #region interfaces

        public NWDAccountService Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountService sModel);
        public NWDAccountService Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountService sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDAccountService> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);

        public List<NWDAccountService> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            ulong sSyncDate);

        public NWDAccountService InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            NWDAccountService sModel);

        public List<NWDAccountService> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary,
            string sAndWhere = "");

        #endregion
    }
}