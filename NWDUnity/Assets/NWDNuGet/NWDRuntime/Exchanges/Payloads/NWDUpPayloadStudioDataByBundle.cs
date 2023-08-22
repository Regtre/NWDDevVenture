using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadStudioDataByBundle: NWDUpPayload
    {
        #region properties

        public int BundleId { set; get; }
        public List<NWDVolatileData> PlayerDataLogList { set; get; }

        #endregion
    }
}