using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadAccountSignOut: NWDDownPayload
    {
        #region properties

        public NWDAccountSign DeviceSign { set; get; }

        #endregion
    }
}