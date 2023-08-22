using System;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadLicense
    {
        #region properties
        public string HttpsDns { set; get; } = string.Empty;

        #endregion
    }
}