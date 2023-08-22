using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDMauiRuntime.Configurations;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDRuntime.Factories;
using NWDRuntime.Models;

namespace NWDMauiRuntime.Back;

public class NWDSync
{
      private static NWDDataInMemory PlayerDataToDataInMemory(List<NWDPlayerDataStorage> sPlayerDatas)
    {
        NWDDataInMemory tDataInMemory  = new NWDDataInMemory();
        foreach (NWDPlayerDataStorage tPlayerData in sPlayerDatas)
        {
            Type? tDataType = Type.GetType(tPlayerData.ClassName);
            if (tDataType == null)
            {
                NWDLogger.Warning("ERROR la class n'existe pas ");
            }
            else
            {
                NWDBasicModel? tObject = NWDPlayerDataFactory.FromPlayerDataStorage(tPlayerData) as NWDPlayerData;
                if (tObject != null)
                {
                    tDataInMemory.AddDataInMemory(tObject, tDataType);
                }
            }
        }

        return tDataInMemory;
    }
    private static NWDDataInMemory StudioDataToDataInMemory(List<NWDStudioDataStorage> sStudioDatas)
    {
        NWDDataInMemory tDataInMemory  = new NWDDataInMemory();
        foreach (NWDStudioDataStorage tStudioData in sStudioDatas)
        {
            Type? tDataType = Type.GetType(tStudioData.ClassName);
            if (tDataType == null)
            {
                NWDLogger.Warning("ERROR la class n'existe pas ");
            }
            else
            {
                NWDBasicModel? tObject =
                    JsonConvert.DeserializeObject(tStudioData.Json, tDataType) as NWDStudioData;
                if (tObject != null)
                {
                    tDataInMemory.AddDataInMemory(tObject, tDataType);
                }
            }
        }
        return tDataInMemory;
    }
    
    public static NWDDataInMemoryPlayerAndStudio SyncByIncrement(List<NWDPlayerData> sPlayerData, List<NWDStudioData> sStudioData,NWDAccount sAccount,ushort sDataTrack)
    {
        List<NWDPlayerDataStorage> tPlayerData = NWDPlayerDataFactory.ToPlayerDatasStorage(sPlayerData,sAccount.Reference);
        List<NWDStudioDataStorage> tStudioData = NWDStudioDataFactory.ToStudioDatasStorage(sStudioData);
        NWDUpPayloadDataSyncByIncrement sUpPayload;
            sUpPayload = new NWDUpPayloadDataSyncByIncrement
            {
                PlayerDataList = tPlayerData,
                StudioDataList = tStudioData,
                PlayerDataSyncInformation = GetPlayerLastSyncInformation(),
                StudioDataSyncInformation = GetStudioLastSyncInformation()
            };
        
        
        NWDDataInMemoryPlayerAndStudio tData = new NWDDataInMemoryPlayerAndStudio();
        
        NWDRequestRuntime tRequest =
            NWDRequestRuntime.CreateRequestSyncDataByIncrement(NWDMauiRuntimeConfiguration.KConfig, 
                NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),sUpPayload, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
        
        NWDResponseRuntime? tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(tRequest).Result;
        
        if (tResponseRuntime.Status != NWDRequestStatus.Error)
        {
            NWDDownPayload tDownPayload = tResponseRuntime.GetPayload<NWDDownPayloadDataSyncByIncrement>(NWDMauiRuntimeConfiguration.KConfig);
            NWDDownPayloadDataSyncByIncrement tPayloadDataSyncByIncrement =
                (NWDDownPayloadDataSyncByIncrement)tDownPayload;
            if (tPayloadDataSyncByIncrement != null)
            {
                if (tPayloadDataSyncByIncrement.PlayerDataList != null)
                {
                    tData.PlayerData = PlayerDataToDataInMemory(tPayloadDataSyncByIncrement.PlayerDataList);
                }

                if (tPayloadDataSyncByIncrement.StudioDataList != null)
                {
                    tData.StudioData = StudioDataToDataInMemory(tPayloadDataSyncByIncrement.StudioDataList);

                }
            }
        }
        return tData;
    }
    
        
    public static void SetPlayerLastSyncInformation(NWDSyncInformation sSync)
    {
        if (sSync.UseMe == true) // can be false when request is not sync but a "get by reference(s)"
        {
            NWDRuntimeCallback.SharedInstance().SetSessionPlayerSync(sSync);
        }
    }
    public static void SetStudioLastSyncInformation( NWDSyncInformation sSync)
    {
        if (sSync.UseMe == true) // can be false when request is not sync but a "get by reference(s)"
        {
            NWDRuntimeCallback.SharedInstance().SetSessionStudioSync(sSync);
        }
    }

    public static NWDSyncInformation? GetPlayerLastSyncInformation()
    {
        return NWDRuntimeCallback.SharedInstance().GetSessionPlayerSync();
    }
    public static NWDSyncInformation GetStudioLastSyncInformation()
    {
        return NWDRuntimeCallback.SharedInstance().GetSessionStudioSync();
    }
}