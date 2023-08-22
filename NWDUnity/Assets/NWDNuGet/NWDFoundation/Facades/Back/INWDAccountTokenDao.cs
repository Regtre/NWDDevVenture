using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDAccountTokenDao : INWDDao
    {
        #region interfaces

        public NWDAccountToken Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountToken sModel);
        public NWDAccountToken Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountToken sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDAccountToken> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);
        public List<NWDAccountToken> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate);
        public NWDAccountToken InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountToken sModel);

        public List<NWDAccountToken> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary, string sAndWhere = "");

        #endregion
    }
}