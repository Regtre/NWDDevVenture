using System;
using NWDFoundation.Exchanges.Payloads;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadAccountChangeRange: NWDUpPayload
    {
        #region properties

        public ushort NewRange { set; get; }

        #endregion
    }
}