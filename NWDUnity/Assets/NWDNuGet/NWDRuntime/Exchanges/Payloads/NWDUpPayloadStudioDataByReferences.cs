using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadStudioDataByReferences : NWDUpPayload
    {
        #region properties

        public List<ulong> StudioDataReferenceList { set; get; }
        public List<NWDVolatileData> PlayerDataLogList { set; get; }

        #endregion
    }
}