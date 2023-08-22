using System;
using System.Collections.Generic;
using NWDFoundation.Models;

namespace NWDTreat.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadServiceDelete : NWDUpPayloadTreat
    {
        #region properties
        public List<NWDProjectServiceKey> ServiceList { set; get; } = new List<NWDProjectServiceKey>();
        #endregion
    }
}