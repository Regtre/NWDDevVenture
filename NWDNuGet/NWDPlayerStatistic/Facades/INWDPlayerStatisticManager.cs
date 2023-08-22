using NWDPlayerStatistic.Exchanges;

namespace NWDPlayerStatistic.Facades;

public interface INWDPlayerStatisticManager
{
    #region interfaces

    public NWDPlayerStatisticResponse Process (NWDPlayerStatisticRequest sRequestRuntime);

    #endregion
}