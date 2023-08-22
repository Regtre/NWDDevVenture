using System.Collections.Generic;
using NWDCrucial.Models;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDCrucial.Facades
{
    public interface INWDProjectServiceKeyDao : INWDDao
    {
        #region interfaces

        public NWDProjectServiceKey Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectServiceKey sModel);
        public NWDProjectServiceKey Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectServiceKey sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDProjectServiceKey> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);
        public List<NWDProjectServiceKey> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate);
        public NWDProjectServiceKey InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDProjectServiceKey sModel);

        public List<NWDProjectServiceKey> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary, string sAndWhere = "");

        #endregion
    }
}