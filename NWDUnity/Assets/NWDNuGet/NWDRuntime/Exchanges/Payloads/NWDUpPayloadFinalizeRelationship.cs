using System;
using NWDFoundation.Exchanges.Payloads;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadFinalizeRelationship: NWDUpPayload
    {
        #region properties

        public ulong RelationshipReference { get; set; }
        
        public bool IsAccepted { get; set; }
        
        #endregion
        
        public NWDUpPayloadFinalizeRelationship()
        {
            
        }
        
        public NWDUpPayloadFinalizeRelationship(ulong sRelationshipReference, bool sIsAccepted)
        {
            RelationshipReference = sRelationshipReference;
            IsAccepted = sIsAccepted;
        }
    }
}