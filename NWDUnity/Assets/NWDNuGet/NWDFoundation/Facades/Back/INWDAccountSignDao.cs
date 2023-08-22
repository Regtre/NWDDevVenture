using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDAccountSignDao : INWDDao
    {
        #region interfaces

        public NWDAccountSign Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountSign sModel);
        public NWDAccountSign Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountSign sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDAccountSign> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);
        public List<NWDAccountSign> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate);
        public NWDAccountSign InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountSign sModel);

        public List<NWDAccountSign> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary, string sAndWhere = "");

        #endregion
    }
}