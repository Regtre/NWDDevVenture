using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadStudioDataByReferences: NWDDownPayload
    {
        #region properties

        public List<NWDStudioDataStorage> StudioDataList { set; get; }

        #endregion
    }
}