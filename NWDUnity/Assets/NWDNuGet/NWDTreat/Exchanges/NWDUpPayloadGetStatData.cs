using System;
using NWDFoundation.Models;

namespace NWDTreat.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadGetStatData : NWDUpPayloadTreat
    {
        #region properties

        public NWDReference<NWDAccount> FriendReference { get; set; }
        public NWDReference<NWDAccount> AccountReference { get; set; }
        public Type TypeObject { get; set; }
        #endregion

        public NWDUpPayloadGetStatData(NWDReference<NWDAccount> sFriendReference, NWDReference<NWDAccount> sAccountReference, Type sTypeObject)
        {
            FriendReference = sFriendReference;
            AccountReference = sAccountReference;
            TypeObject = sTypeObject;
        }
    }
}