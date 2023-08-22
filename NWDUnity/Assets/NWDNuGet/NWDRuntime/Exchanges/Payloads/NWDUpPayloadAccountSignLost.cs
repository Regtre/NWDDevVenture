using System;
using NWDFoundation.Exchanges.Payloads;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadAccountSignLost: NWDUpPayload
    {
        #region properties
        public string RescueEmail { set; get; }

        #endregion
    }
}