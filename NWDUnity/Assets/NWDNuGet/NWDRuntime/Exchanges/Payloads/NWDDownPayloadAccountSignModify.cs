using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadAccountSignModify: NWDDownPayload
    {
        #region properties

        public List<NWDAccountSign> AccountSignList { set; get; } = new List<NWDAccountSign>();

        #endregion
    }
}