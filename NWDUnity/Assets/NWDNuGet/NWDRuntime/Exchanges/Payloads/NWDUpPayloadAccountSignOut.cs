using System;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadAccountSignOut: NWDUpPayload
    {
        #region properties

        public NWDAccountSign DeviceSign { set; get; }

        #endregion
    }
}