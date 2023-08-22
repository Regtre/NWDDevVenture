using System;

namespace NWDFoundation.Exchanges
{
    [Serializable]
    public enum NWDExchangeDevice
    {
        Unknown = 0,
        
        Ios = 11,
        Android = 12,
        
        Macos = 21,
        Windows = 22,
        Linux = 23,
        
        Web = 41,

        Error = 128,
    }
}