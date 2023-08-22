using System;

namespace NWDFoundation.Exchanges
{
    [Serializable]
    public enum NWDExchangeOrigin
    {
        Unknown = 0,
        
        Game = 1,
        
        App = 2,
        
        Web = 4,

        UnityEditor = 8,
    }
}