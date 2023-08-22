using NWDTreat.Exchanges;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDTreat.Exchanges.Payloads;
using NWDWebRuntime.CallBacks;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebTreat.CallBacks;
using NWDWebTreat.Configuration;

namespace NWDWebTreat.Managers;

public class NWDWebDataTreatManger
{
    public static async void CreateService(NWDProjectServiceKey sService)
    {
        NWDRequestTreat tRequestTreat = NWDRequestTreat.CreateRequestServiceCreate(NWDWebTreatConfiguration.KConfig,
            NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
            NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web,
            new List<NWDProjectServiceKey>() { sService });

        NWDResponseTreat tResponseTreat = await NWDWebTreatCallbackServers.PostRequest(tRequestTreat);

        if (tResponseTreat != null && tResponseTreat.IsValid(NWDWebTreatConfiguration.KConfig))
        {
            NWDLogger.Trace("Service created");
        }
    }

    public static async void AssociateService(NWDAccountService sService /*, NWDRequestPlayerToken sPlayerToken*/)
    {
        NWDRequestTreat tRequestTreat = NWDRequestTreat.CreateRequestAssociateService(NWDWebTreatConfiguration.KConfig,
            NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
            NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web,
            sService /*,sPlayerToken*/);

        NWDResponseTreat tResponseTreat = await NWDWebTreatCallbackServers.PostRequest(tRequestTreat);

        if (tResponseTreat != null && tResponseTreat.IsValid(NWDWebTreatConfiguration.KConfig))
        {
            NWDLogger.Trace("Service associated");
        }
    }

    public static async void AssociateSubService(NWDAccountService sOfferByService, NWDAccountService sOfferToService,
        NWDAccount sOfferByAccount, NWDAccount sOfferToAccount)
    {
        NWDRequestTreat tRequestTreat = NWDRequestTreat.CreateRequestAssociateSubService(
            NWDWebTreatConfiguration.KConfig, NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
            NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web,
            sOfferByService, sOfferToService, sOfferByAccount, sOfferToAccount);

        NWDResponseTreat tResponseTreat = await NWDWebTreatCallbackServers.PostRequest(tRequestTreat);

        if (tResponseTreat != null && tResponseTreat.IsValid(NWDWebTreatConfiguration.KConfig))
        {
            NWDLogger.Trace("Service associated");
        }
    }

    public static async void DissociateServiceAndSubService(NWDReference<NWDAccountService> sService,NWDReference<NWDAccount> sAccount)
    {
        NWDRequestTreat tRequestTreat = NWDRequestTreat.CreateRequestDissociateServiceAndSubServices(
            NWDWebTreatConfiguration.KConfig, NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
            NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web,
            sService,sAccount);

        NWDResponseTreat tResponseTreat = await NWDWebTreatCallbackServers.PostRequest(tRequestTreat);

        if (tResponseTreat != null && tResponseTreat.IsValid(NWDWebTreatConfiguration.KConfig))
        {
            NWDLogger.Trace("Service dissociated");
        }
    }
    public static async Task<List<NWDAccountService>> GetSubServicesFromAccount(NWDAccount sAccount)
    {
        NWDRequestTreat tRequestTreat = NWDRequestTreat.CreateRequestGetSubServicesFromAccount(
            NWDWebTreatConfiguration.KConfig, NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
            NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web,
            sAccount);

        NWDResponseTreat tResponseTreat = await NWDWebTreatCallbackServers.PostRequest(tRequestTreat);

        
        if (tResponseTreat != null && tResponseTreat.IsValid(NWDWebTreatConfiguration.KConfig))
        {
            NWDLogger.Trace("GetSubServicesFromAccount");
        }

        return tResponseTreat.GetPayload<NWDDownPayloadTreatGetSubServicesFromAccount>(NWDWebTreatConfiguration.KConfig).AccountServiceList;
    }
}