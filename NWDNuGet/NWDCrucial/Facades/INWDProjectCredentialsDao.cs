using System.Collections.Generic;
using NWDCrucial.Models;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;

namespace NWDCrucial.Facades
{
    public interface INWDProjectCredentialsDao : INWDDao
    {
        #region interfaces

        public NWDProjectCredentials Create(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            NWDProjectCredentials sModel);

        public NWDProjectCredentials Update(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            NWDProjectCredentials sModel);

        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDProjectCredentials> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);

        public List<NWDProjectCredentials> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            ulong sSyncDate);

        public NWDProjectCredentials InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            NWDProjectCredentials sModel);

        public List<NWDProjectCredentials> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary, string sAndWhere = "");

        #endregion
    }
}