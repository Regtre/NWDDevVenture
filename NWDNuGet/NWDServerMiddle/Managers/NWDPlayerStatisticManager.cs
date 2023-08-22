using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDPlayerStatistic.Exchanges;
using NWDPlayerStatistic.Exchanges.Payloads;
using NWDPlayerStatistic.Facades;
using NWDRuntime.Factories;
using NWDServerMiddle.Configuration;
using NWDServerMiddle.Managers.ModelManagers;
using NWDServerShared.Configuration;

namespace NWDServerMiddle.Managers;

public class NWDPlayerStatisticManager : INWDPlayerStatisticManager
{
    public NWDPlayerStatisticResponse Process(NWDPlayerStatisticRequest sRequest)
    {
        NWDPlayerStatisticResponse rReturn = new NWDPlayerStatisticResponse(NWDServerMiddleConfiguration.KConfig,
            NWDExchangePlayerStatisticKind.Unknown, null, NWDRequestStatus.Error);
        if (NWDServerConfiguration.KConfig.IsOverFlow() == false)
        {
            if (NWDServerConfiguration.KConfig.Status == NWDServerStatus.Active)
            {
                switch (sRequest.Kind)
                {
                    case NWDExchangePlayerStatisticKind.GetPlayerData:
                        rReturn = GetPlayerData(sRequest);
                        break;
                    default:
                        rReturn = new NWDPlayerStatisticResponse(NWDServerMiddleConfiguration.KConfig,
                            NWDExchangePlayerStatisticKind.Unknown, null, NWDRequestStatus.Unknown);
                        break;
                }
            }
            else
            {
                rReturn = new NWDPlayerStatisticResponse(NWDServerMiddleConfiguration.KConfig,
                    NWDExchangePlayerStatisticKind.None,
                    null, NWDRequestStatus.ServerIsDisabled);
            }
        }
        else
        {
            rReturn = new NWDPlayerStatisticResponse(NWDServerMiddleConfiguration.KConfig,
                NWDExchangePlayerStatisticKind.None,
                null, NWDRequestStatus.PleaseChangeServer);
        }

        return rReturn;
    }

    private NWDPlayerStatisticResponse GetPlayerData(NWDPlayerStatisticRequest sRequest)
    {
        NWDPlayerStatisticResponse rReturn = new NWDPlayerStatisticResponse(NWDServerMiddleConfiguration.KConfig,
            NWDExchangePlayerStatisticKind.Unknown,
            null, NWDRequestStatus.Error);;

        if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
        {
            NWDPlayerStatisticGetDataForPlayerUpPayload? tPayload =
                sRequest.GetPayload<NWDPlayerStatisticGetDataForPlayerUpPayload>(NWDServerMiddleConfiguration.KConfig);
            if (tPayload == null || tPayload.PlayerReference.Reference == 0) return rReturn;
            List<NWDPlayerDataStorage> tPlayerDataStorages =
                NWDPlayerDataManager.GetDataByAccountReferenceAndProjectIdAndType(
                    tPayload.Environment, tPayload.ProjectId, tPayload.PlayerReference.Reference,
                    NWDAccount.ExtractRange(tPayload.PlayerReference.Reference), tPayload.AssemblyQualifiedNameType);

            NWDPlayerStatisticGetDataForPlayerDownPayload tDownPayload =
                new NWDPlayerStatisticGetDataForPlayerDownPayload();
            tDownPayload.PlayerData = tPlayerDataStorages;
            
            rReturn = new NWDPlayerStatisticResponse(NWDServerMiddleConfiguration.KConfig,
                NWDExchangePlayerStatisticKind.GetPlayerData, tDownPayload, NWDRequestStatus.Ok);
        }       

        return rReturn;
    }
}