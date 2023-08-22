using System;
using NWDFoundation.Configuration.Environments;

namespace NWDFoundation.Exchanges
{
    [Serializable]
    public abstract class NWDBasicResponse : NWDBasicExchange
    {
        public NWDRequestStatus Status { set; get; } = NWDRequestStatus.None;
        public string ServerIdentity { set; get; } = string.Empty;
        public string Debug { set; get; } = string.Empty;
    }
}

