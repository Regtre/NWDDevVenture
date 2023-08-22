using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDTreat.Exchanges;
using NWDWebRuntime.CallBacks;
using NWDWebRuntime.Configuration;
using NWDWebTreat.CallBacks;
using NWDWebTreat.Configuration;

namespace NWDWebTreat.Managers;

public class NWDTreatRequestManager
{
    public static async void CreateService(NWDProjectServiceKey sService)
    {
        NWDRequestTreat tRequestTreat = NWDRequestTreat.CreateRequestServiceCreate(NWDWebTreatConfiguration.KConfig, NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDExchangeOrigin.Web,NWDExchangeDevice.Web,new List<NWDProjectServiceKey>(){ sService});

        NWDResponseTreat tResponseTreat = await NWDWebTreatCallbackServers.PostRequest(tRequestTreat); 
        
        if (tResponseTreat != null && tResponseTreat.IsValid(NWDWebTreatConfiguration.KConfig))
        {
        }
    }

    public static async Task<bool> AssociateService(NWDAccountService sService/*, NWDRequestPlayerToken sPlayerToken*/)
    {
        bool rReturn = false;
        NWDRequestTreat tRequestTreat = NWDRequestTreat.CreateRequestAssociateService(NWDWebTreatConfiguration.KConfig, NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDExchangeOrigin.Web,NWDExchangeDevice.Web, sService/*,sPlayerToken*/);

        NWDResponseTreat tResponseTreat = await NWDWebTreatCallbackServers.PostRequest(tRequestTreat); 
        
        if (tResponseTreat != null && tResponseTreat.IsValid(NWDWebTreatConfiguration.KConfig))
        {
            rReturn = true; 
        }
        return rReturn;
    }
}