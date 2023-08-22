using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadDataSyncByIncrement : NWDUpPayload
    {
        #region properties

        public NWDSyncInformation PlayerDataSyncInformation { set; get; }
        public List<NWDPlayerDataStorage> PlayerDataList { set; get; }
        public NWDSyncInformation StudioDataSyncInformation { set; get; }
        public List<NWDStudioDataStorage> StudioDataList { set; get; }
        
        public List<NWDVolatileData> VolatileDataList { set; get; }
        
        #endregion
    }
}