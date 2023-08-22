using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDPlayerDataDao : INWDDao
    {
        #region interfaces

        public NWDPlayerDataStorage Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDPlayerDataStorage sModel);
        public NWDPlayerDataStorage Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDPlayerDataStorage sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDPlayerDataStorage> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);
        public List<NWDPlayerDataStorage> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate);
        public NWDPlayerDataStorage InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDPlayerDataStorage sModel);

        public List<NWDPlayerDataStorage> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary, string sAndWhere = "");

        #endregion
    }
}