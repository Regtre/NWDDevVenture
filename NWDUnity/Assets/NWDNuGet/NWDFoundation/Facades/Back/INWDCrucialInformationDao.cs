using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDCrucialInformationDao : INWDDao
    {
        #region interfaces

        public NWDCrucialInformation Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDCrucialInformation sModel);
        public NWDCrucialInformation Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDCrucialInformation sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDCrucialInformation> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);
        public List<NWDCrucialInformation> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate);
        public NWDCrucialInformation InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDCrucialInformation sModel);

        public List<NWDCrucialInformation> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary, string sAndWhere = "");

        public ulong NewValidReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId);

        #endregion
    }
}