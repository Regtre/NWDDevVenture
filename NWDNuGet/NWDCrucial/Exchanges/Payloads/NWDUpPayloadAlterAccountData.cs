using System;
using System.Collections.Generic;
using NWDFoundation.Models;

namespace NWDCrucial.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadAlterAccountData : NWDUpPayloadCrucial
    {
        #region properties

        public List<NWDAccountService>? AccountServiceList { set; get; } = null;
        public List<NWDAccountSign>? AccountSignList { set; get; } = null;

        #endregion
    }
}