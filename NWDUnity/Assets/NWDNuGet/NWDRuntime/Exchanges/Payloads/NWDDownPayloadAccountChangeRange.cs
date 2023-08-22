using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadAccountChangeRange: NWDDownPayload
    {
        #region properties

        public List<NWDAccountSign> AccountSignList { set; get; }
        public List<NWDPlayerDataStorage> PlayerDataList { set; get; }

        #endregion
    }
}