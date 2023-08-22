using System;
using NWDFoundation.Exchanges.Payloads;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadGiveMeAccount: NWDUpPayload
    {
        #region properties

        public string DeviceFingerPrint { set; get; }
        public long StudioLastSync { set; get; }

        #endregion
    }
}