using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDStudioDataDao : INWDDao
    {
        #region interfaces

        public NWDStudioDataStorage Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDStudioDataStorage sModel);
        public NWDStudioDataStorage Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDStudioDataStorage sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDStudioDataStorage> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);
        public List<NWDStudioDataStorage> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate);
        public NWDStudioDataStorage InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDStudioDataStorage sModel);

        public List<NWDStudioDataStorage> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary, string sAndWhere = "");

        public ulong NewValidReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId);

        #endregion
    }
}