using System;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models.Enums;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadAccountSignLost: NWDDownPayload
    {
        #region properties
        public string RescueTokenSecured { set; get; } = string.Empty;
        public NWDAccountSignType SignType { set; get; } = NWDAccountSignType.None;
        public int Limit { set; get; } = 0;

        #endregion
    }
}