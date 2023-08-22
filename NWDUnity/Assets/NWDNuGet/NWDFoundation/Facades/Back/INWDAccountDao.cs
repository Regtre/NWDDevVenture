using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDAccountDao : INWDDao
    {
        #region interfaces

        public NWDAccount Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccount sModel);
        public NWDAccount Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccount sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDAccount> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);
        public List<NWDAccount> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate);
        public NWDAccount InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccount sModel);

        public List<NWDAccount> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary,
            string sAndWhere = "");

        #endregion
    }
}