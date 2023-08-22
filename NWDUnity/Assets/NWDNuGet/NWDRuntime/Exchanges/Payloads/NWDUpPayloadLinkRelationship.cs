using System;
using NWDFoundation.Exchanges.Payloads;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadLinkRelationship: NWDUpPayload
    {
        #region properties

        public string code { get; set; }

        #endregion

        public NWDUpPayloadLinkRelationship()
        {
            
        }
        public NWDUpPayloadLinkRelationship(string sCode)
        {
            code = sCode;
        }
    }
}