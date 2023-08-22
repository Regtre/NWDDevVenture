using System;
using System.Collections.Generic;
using NWDFoundation.Models;

namespace NWDFoundation.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayload
    {
        #region properties

        public List<NWDAccountService> AccountServiceList { set; get; } = new List<NWDAccountService>();

        #endregion
    }
}