using System;
using System.Collections.Generic;
using NWDCrucial.Models;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDCrucial.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadProjectDelete : NWDUpPayloadCrucial
    {
        #region properties

        public List<NWDProjectCredentials> ProjectCredentialsList { set; get; } = new List<NWDProjectCredentials>();
        public List<NWDProjectAbstractDataTrack> ProjectDataTrackList { set; get; } = new List<NWDProjectAbstractDataTrack>();

        #endregion
    }
}