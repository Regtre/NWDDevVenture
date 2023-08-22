using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadPleaseChangeServer: NWDDownPayload
    {
        #region properties

        private List<string> ServerDomainNameList { set; get; }

        #endregion
    }
}