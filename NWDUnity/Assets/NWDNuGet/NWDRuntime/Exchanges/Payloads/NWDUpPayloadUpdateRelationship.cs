using System;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadUpdateRelationship: NWDUpPayload
    {
        #region properties

        private NWDRelationship Relationship;

        #endregion

        public NWDUpPayloadUpdateRelationship()
        {
        }

        public NWDUpPayloadUpdateRelationship(NWDRelationship sRelationship)
        {
            Relationship = sRelationship;
        }


    }
}