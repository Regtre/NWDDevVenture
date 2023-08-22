using System;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;

namespace NWDRuntime.Exchanges.Payloads
{
    [Serializable]
    public class NWDUpPayloadAccountSignModify: NWDUpPayload
    {
        #region properties

        public NWDAccountSign OldAccountSign { set; get; }
        public NWDAccountSign NewAccountSign { set; get; }

        #endregion
    }
}