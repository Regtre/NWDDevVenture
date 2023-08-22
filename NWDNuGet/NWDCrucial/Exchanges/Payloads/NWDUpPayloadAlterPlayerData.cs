using System;
using System.Collections.Generic;
using NWDFoundation.Models;

namespace NWDCrucial.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadAlterPlayerData : NWDUpPayloadCrucial
    {
        #region properties

        public List<NWDPlayerDataStorage>? PlayerDataStorageList { set; get; } = null;

        #endregion
    }
}