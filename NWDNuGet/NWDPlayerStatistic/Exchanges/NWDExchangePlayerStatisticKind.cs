using NWDFoundation.Exchanges;

namespace NWDPlayerStatistic.Exchanges;

public enum NWDExchangePlayerStatisticKind
{
    /// <summary>
    /// No exchange kind (!)
    /// </summary>
    None = NWDExchangeKind.None,
    /// <summary>
    /// Use to test connection : ok or ko
    /// </summary>
    Test = NWDExchangeKind.Test,
    /// <summary>
    /// Not yet specified
    /// </summary>
    Unknown = NWDExchangeKind.Unknown,
    
    GetPlayerData = 100,
}