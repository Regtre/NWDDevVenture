using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDAccountOrderDao : INWDDao
    {
        #region interfaces

        public NWDAccountOrder Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountOrder sModel);
        public NWDAccountOrder Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountOrder sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDAccountOrder> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);
        public List<NWDAccountOrder> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate);
        public NWDAccountOrder InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDAccountOrder sModel);

        public List<NWDAccountOrder> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary, string sAndWhere = "");

        #endregion
    }
}