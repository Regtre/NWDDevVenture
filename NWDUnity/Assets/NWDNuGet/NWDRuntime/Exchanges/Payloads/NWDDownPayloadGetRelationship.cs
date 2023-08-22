using System;
using System.Collections.Generic;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;
using NWDRuntime.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadGetRelationship : NWDDownPayload
    {
        #region properties

        public List<NWDRelationship> RelationshipList { set; get; }

        #endregion
    }
}