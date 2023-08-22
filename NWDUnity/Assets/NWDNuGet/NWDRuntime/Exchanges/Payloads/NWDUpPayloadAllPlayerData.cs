using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadAllPlayerData : NWDUpPayload
    {
        #region properties

        public List<NWDVolatileData> PlayerDataLogList { set; get; }

        #endregion
    }
}