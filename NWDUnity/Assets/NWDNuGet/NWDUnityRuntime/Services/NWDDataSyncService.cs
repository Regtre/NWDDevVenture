using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDUnityShared.Factories;
using NWDUnityShared.Services;
using NWDUnityShared.Tools;
using System.Collections.Generic;
using NWDAccountService = NWDUnityShared.Services.NWDAccountService;

namespace NWDUnityRuntime.Services
{
    public class NWDDataSyncService : NWDRuntimeService
    {
        static public NWDDownPayloadDataSyncByIncrement Synchronize (NWDRequestPlayerToken sPlayerToken, ref NWDAsyncHandler sAsyncHandler, List<NWDPlayerDataStorage> sPlayerData, List<NWDStudioDataStorage> sStudioData)
        {
            NWDUpPayloadDataSyncByIncrement tUpPayload = new NWDUpPayloadDataSyncByIncrement()
            {
                PlayerDataList = sPlayerData,
                StudioDataList = sStudioData,
                PlayerDataSyncInformation = sPlayerToken.PlayerSyncInformation,
                StudioDataSyncInformation = sPlayerToken.StudioSyncInformation,
            };

            NWDRequestRuntime tRequest = NWDRequestRuntimeFactory.CreateSyncAllRequest(sPlayerToken, tUpPayload);
            NWDResponseRuntime tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            return NWDAccountService.ProcessServerResponse<NWDDownPayloadDataSyncByIncrement>(tResponse, ref sAsyncHandler);
        }
    }
}
