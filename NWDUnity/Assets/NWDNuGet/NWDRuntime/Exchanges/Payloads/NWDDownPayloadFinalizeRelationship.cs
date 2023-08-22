using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDRuntime.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadFinalizeRelationship : NWDDownPayload
    {
        #region properties

        public NWDRelationshipFinalizeStatus FinalizeStatus { set; get; }

        #endregion
    }
}