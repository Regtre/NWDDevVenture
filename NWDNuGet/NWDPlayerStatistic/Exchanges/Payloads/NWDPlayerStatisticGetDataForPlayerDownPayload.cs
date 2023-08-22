using System.Collections.Generic;
using NWDFoundation.Models;

namespace NWDPlayerStatistic.Exchanges.Payloads;

public class NWDPlayerStatisticGetDataForPlayerDownPayload : NWDPlayerStatisticDownPayload
{
    public List<NWDPlayerDataStorage> PlayerData { get; set; } = new List<NWDPlayerDataStorage>();

    public NWDPlayerStatisticGetDataForPlayerDownPayload(List<NWDPlayerDataStorage> sPlayerData)
    {
        PlayerData = sPlayerData;
    }

    public NWDPlayerStatisticGetDataForPlayerDownPayload() { }
}