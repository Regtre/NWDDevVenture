using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadDataSyncByIncrement: NWDDownPayload
    {
        #region properties

        public ulong PlayerLastSync { set; get; }
        public List<NWDPlayerDataStorage> PlayerDataList { set; get; }
        public ulong StudioLastSync { set; get; }
        public List<NWDStudioDataStorage> StudioDataList { set; get; }

        #endregion
    }
}