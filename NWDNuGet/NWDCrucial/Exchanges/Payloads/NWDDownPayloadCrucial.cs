using System;

namespace NWDCrucial.Exchanges.Payloads
{
    [Serializable]
    public class NWDDownPayloadCrucial
    {
        #region properties

        public bool Success { set; get; } = false;

        #endregion
    }
}