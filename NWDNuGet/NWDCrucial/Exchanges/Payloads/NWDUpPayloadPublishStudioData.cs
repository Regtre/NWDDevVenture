using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDCrucial.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadPublishStudioData : NWDUpPayloadCrucial
    {
        #region properties

        public NWDEnvironmentKind Kind { set; get; } = NWDEnvironmentKind.Dev;
        public ulong ProjectId { set; get; } = 0;
        public List<NWDStudioDataStorage>? StudioDataStorageList { set; get; } = null;

        #endregion
    }
}