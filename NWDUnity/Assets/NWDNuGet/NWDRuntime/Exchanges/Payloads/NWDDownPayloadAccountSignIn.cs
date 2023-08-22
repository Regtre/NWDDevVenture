using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadAccountSignIn: NWDDownPayload
    {
        #region properties

        public bool Success { set; get; }
        public long PlayerLastSync { set; get; }
        public List<NWDPlayerDataStorage> PlayerDataList { set; get; }
        public long StudioLastSync { set; get; }
        public List<NWDStudioDataStorage> StudioDataList { set; get; }

        #endregion
    }
}