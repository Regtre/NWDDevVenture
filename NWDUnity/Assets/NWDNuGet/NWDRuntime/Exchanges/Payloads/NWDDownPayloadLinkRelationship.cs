using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDRuntime.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadLinkRelationship : NWDDownPayload
    {
        #region properties

        public NWDRelationshipLinkStatus LinkStatus { set; get; }

        #endregion
    }
}