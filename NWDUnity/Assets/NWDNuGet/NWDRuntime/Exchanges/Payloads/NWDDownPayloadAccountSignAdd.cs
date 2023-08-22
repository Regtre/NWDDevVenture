using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadAccountSignAdd: NWDDownPayload
    {
        #region properties

        public NWDAccountSign AccountSign { set; get; }

        #endregion
    }
}