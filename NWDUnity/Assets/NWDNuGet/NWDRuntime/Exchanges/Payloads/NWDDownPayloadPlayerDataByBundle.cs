using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadPlayerDataByBundle: NWDDownPayload
    {
        #region properties

        public List<NWDPlayerDataStorage> PlayerDataList { set; get; }
        public long StudioLastSync { set; get; }
        public List<NWDStudioDataStorage> StudioDataList { set; get; }

        #endregion
    }
}