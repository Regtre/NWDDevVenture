using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadPlayerDataByReferences: NWDUpPayload
    {
        #region properties

        public List<ulong> PlayerDataReferencesList { set; get; }
        public long StudioLastSync { set; get; }
        public List<NWDVolatileData> PlayerDataLogList { set; get; }

        #endregion
    }
}