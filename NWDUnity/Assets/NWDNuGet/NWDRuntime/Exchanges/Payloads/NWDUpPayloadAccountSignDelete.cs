using System;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadAccountSignDelete: NWDUpPayload
    {
        #region properties

        public NWDAccountSign AccountSign { set; get; }

        #endregion
    }
}