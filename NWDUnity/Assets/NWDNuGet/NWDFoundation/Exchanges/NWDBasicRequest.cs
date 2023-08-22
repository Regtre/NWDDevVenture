using System;
using NWDFoundation.Configuration.Environments;

namespace NWDFoundation.Exchanges
{
    [Serializable]
    public abstract class NWDBasicRequest : NWDBasicExchange
    {
        public NWDExchangeOrigin Origin { set; get; } = NWDExchangeOrigin.Unknown;
        public NWDExchangeDevice Device { set; get; } = NWDExchangeDevice.Unknown;

    }
}

