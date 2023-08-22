using System;
using System.Collections.Generic;
using NWDCrucial.Models;
using NWDFoundation.Models;

namespace NWDCrucial.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadProjectPublish : NWDUpPayloadCrucial
    {
        #region properties

        public List<NWDProjectCredentials> ProjectCredentialsList { set; get; } = new List<NWDProjectCredentials>();
        public List<NWDProjectServiceKey> ProjectServiceKeyList { set; get; } = new List<NWDProjectServiceKey>();
        // public List<NWDDefaultCrucialAccount> AccountList { set; get; } = new List<NWDDefaultCrucialAccount>();
        #endregion
    }
}