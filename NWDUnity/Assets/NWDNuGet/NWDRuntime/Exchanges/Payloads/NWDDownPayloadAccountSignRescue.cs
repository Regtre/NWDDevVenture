using System;
using NWDFoundation.Exchanges.Payloads;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadAccountSignRescue: NWDDownPayload
    {
        #region properties

        public bool Success { set; get; }
        // public string RescueFormUrl { set; get; }
        
        #endregion
    }
}