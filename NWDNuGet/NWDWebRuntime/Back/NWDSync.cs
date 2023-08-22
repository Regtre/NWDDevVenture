using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDRuntime.Factories;
using NWDRuntime.Models;
using NWDWebRuntime.CallBacks;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;

namespace NWDWebRuntime.Back; 

public class NWDSync
{
    
    #region Tools
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
    #endregion
    
    public static NWDDataInMemoryPlayerAndStudio SyncByIncrement(List<NWDPlayerData> sPlayerData, List<NWDStudioData> sStudioData,NWDAccount sAccount,ushort sDataTrack,HttpContext sHttpContext)
    {
        List<NWDPlayerDataStorage> tPlayerData = NWDPlayerDataFactory.ToPlayerDatasStorage(sPlayerData,sAccount.Reference);
        List<NWDStudioDataStorage> tStudioData = NWDStudioDataFactory.ToStudioDatasStorage(sStudioData);
        NWDUpPayloadDataSyncByIncrement sUpPayload;
            sUpPayload = new NWDUpPayloadDataSyncByIncrement
            {
                PlayerDataList = tPlayerData,
                StudioDataList = tStudioData,
                PlayerDataSyncInformation = GetPlayerLastSyncInformation(sHttpContext),
                StudioDataSyncInformation = GetStudioLastSyncInformation(sHttpContext)
            };
        
        
        NWDDataInMemoryPlayerAndStudio tData = new NWDDataInMemoryPlayerAndStudio();
        
        NWDRequestRuntime tRequest =
            NWDRequestRuntime.CreateRequestSyncDataByIncrement(NWDWebRuntimeConfiguration.KConfig, 
                NWDWebRuntimeCallbackServers.GetRequestPlayerToken(sHttpContext),sUpPayload, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
        
        NWDResponseRuntime? tResponseRuntime = NWDWebRuntimeCallbackServers.PostRequest(tRequest, sHttpContext).Result;
        
        if (tResponseRuntime.Status != NWDRequestStatus.Error)
        {
            NWDDownPayload tDownPayload = tResponseRuntime.GetPayload<NWDDownPayloadDataSyncByIncrement>(NWDWebRuntimeConfiguration.KConfig);
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
    
    public static NWDDataInMemoryPlayerAndStudio GetAllPlayerData(HttpContext sHttpContext)
    {
        NWDDataInMemoryPlayerAndStudio rData = new NWDDataInMemoryPlayerAndStudio();

        NWDRequestRuntime tRequest =
            NWDRequestRuntime.CreateGetAllPlayerData(NWDWebRuntimeConfiguration.KConfig,
                NWDWebRuntimeCallbackServers.GetRequestPlayerToken(sHttpContext), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
        NWDResponseRuntime? tResponseRuntime = NWDWebRuntimeCallbackServers.PostRequest(tRequest, sHttpContext).Result;

        return rData; 
    }

    
    public static void SetPlayerLastSyncInformation(HttpContext? sHttpContext, NWDSyncInformation sSync)
    {
        if (sSync.UseMe == true) // can be false when request is not sync but a "get by reference(s)"
        {
            NWDWebRuntimeStartupService.SessionPlayerSync.SetValue(sHttpContext, sSync);
        }
    }
    public static void SetStudioLastSyncInformation(HttpContext? sHttpContext, NWDSyncInformation sSync)
    {
        if (sSync.UseMe == true) // can be false when request is not sync but a "get by reference(s)"
        {
            NWDWebRuntimeStartupService.SessionStudioSync.SetValue(sHttpContext, sSync);
        }
    }
    

    public static NWDSyncInformation GetPlayerLastSyncInformation(HttpContext? sHttpContext)
    {
        return NWDWebRuntimeStartupService.SessionPlayerSync.GetValue(sHttpContext);
    }
    public static NWDSyncInformation GetStudioLastSyncInformation(HttpContext? sHttpContext)
    {
        return NWDWebRuntimeStartupService.SessionStudioSync.GetValue(sHttpContext);
    }
}
