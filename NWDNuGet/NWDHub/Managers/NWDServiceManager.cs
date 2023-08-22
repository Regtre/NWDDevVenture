using NWDCrucial.Exchanges;
using NWDCrucial.Exchanges.Payloads;
using NWDFoundation.Exchanges;
using NWDFoundation.Exchanges.Payloads;
using NWDHub.Configuration;
using NWDTreat.Exchanges;
using NWDTreat.Exchanges.Payloads;

namespace NWDHub.Managers;

public class NWDServiceManager
{
    public static NWDResponseTreat AssociateService(NWDRequestTreat sRequest)
    {
        NWDResponseTreat rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.Unknown, null, NWDRequestStatus.Error);
        NWDUpPayloadTreatService? sPayload = sRequest.GetPayload<NWDUpPayloadTreatService>(NWDHubConfiguration.KConfig);
        if (sPayload != null)
        {
            NWDUpPayloadAssociateService tPayload = new NWDUpPayloadAssociateService
                {
                    // PlayerToken = sPayload.PlayerToken,
                    AccountService = sPayload.AccountService,
                };
            NWDRequestCrucial tRequest = new NWDRequestCrucial(NWDHubConfiguration.KConfig,NWDExchangeCrucialKind.AssociateService,tPayload,NWDExchangeOrigin.Web,NWDExchangeDevice.Unknown );
            NWDResponseCrucial? tResponseCrucial = NWDWebCrucialCallbackServers.PostRequest(tRequest).Result; 
            if(tResponseCrucial!=null && tResponseCrucial.Status == NWDRequestStatus.Ok)
            {
                rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.AssociateService, null, NWDRequestStatus.Ok);
            }
        }
        return rReturn;
    }
    public static NWDResponseTreat AssociateSubService(NWDRequestTreat sRequest)
    {
        NWDResponseTreat rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.Unknown, null, NWDRequestStatus.Error);
        NWDUpPayloadTreatAssociateSubService? sPayload = sRequest.GetPayload<NWDUpPayloadTreatAssociateSubService>(NWDHubConfiguration.KConfig);
        if (sPayload != null)
        {
            NWDUpPayloadAssociateSubService tPayload = new NWDUpPayloadAssociateSubService
            {
             
                OfferByAccount = sPayload.OfferByAccount,
                OfferByService = sPayload.OfferByService,
                OfferToService = sPayload.OfferToService,
                OfferToAccount = sPayload.OfferToAccount,
            };
            NWDRequestCrucial tRequest = new NWDRequestCrucial(NWDHubConfiguration.KConfig,NWDExchangeCrucialKind.AssociateSubService,tPayload,NWDExchangeOrigin.Web,NWDExchangeDevice.Unknown );
            NWDResponseCrucial? tResponseCrucial = NWDWebCrucialCallbackServers.PostRequest(tRequest).Result; 
            if(tResponseCrucial is { Status: NWDRequestStatus.Ok })
            {
                rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.AssociateSubService, null, NWDRequestStatus.Ok);
            }
        }
        return rReturn;
    }


    public static NWDResponseTreat DissociateServiceAndSubServices(NWDRequestTreat sRequest)
    {
        NWDResponseTreat rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.Unknown, null, NWDRequestStatus.Error);
        NWDUpPayloadTreatDissociateServiceAndSubServices? sPayload = sRequest.GetPayload<NWDUpPayloadTreatDissociateServiceAndSubServices>(NWDHubConfiguration.KConfig);
        if (sPayload != null)
        {
            NWDUpPayloadDissociateServiceAndSubService tPayload = new NWDUpPayloadDissociateServiceAndSubService
            {
             
                ServiceReference = sPayload.ServiceReference,
                AccountReference = sPayload.AccountReference,
            };
            NWDRequestCrucial tRequest = new NWDRequestCrucial(NWDHubConfiguration.KConfig,NWDExchangeCrucialKind.DissociateServiceAndSubServices,tPayload,NWDExchangeOrigin.Web,NWDExchangeDevice.Unknown );
            NWDResponseCrucial? tResponseCrucial = NWDWebCrucialCallbackServers.PostRequest(tRequest).Result; 
            if(tResponseCrucial is { Status: NWDRequestStatus.Ok })
            {
                rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.DissociateService, null, NWDRequestStatus.Ok);
            }
        }
        return rReturn;
    }

    public static NWDResponseTreat GetSubServicesFromAccount(NWDRequestTreat sRequest)
    {
        NWDResponseTreat rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.Unknown, null, NWDRequestStatus.Error);
        NWDUpPayloadTreatGetSubServicesFromAccount? sPayload = sRequest.GetPayload<NWDUpPayloadTreatGetSubServicesFromAccount>(NWDHubConfiguration.KConfig);
        if (sPayload != null)
        {
            NWDUpPayloadGetSubServicesFromAccount tPayload = new NWDUpPayloadGetSubServicesFromAccount
            {
             
                Account = sPayload.Account,
                Environment = sPayload.Environment,
            };
            
            NWDRequestCrucial tRequest = new NWDRequestCrucial(NWDHubConfiguration.KConfig,NWDExchangeCrucialKind.GetSubServicesFromAccount,tPayload,NWDExchangeOrigin.Web,NWDExchangeDevice.Unknown );
            NWDResponseCrucial? tResponseCrucial = NWDWebCrucialCallbackServers.PostRequest(tRequest).Result; 
            if(tResponseCrucial is { Status: NWDRequestStatus.Ok })
            {
                NWDDownPayloadGetSubServicesFromAccount? tDownPayloadGetSubServicesFromAccount =
                    tResponseCrucial.GetPayload<NWDDownPayloadGetSubServicesFromAccount>(NWDHubConfiguration.KConfig);
                NWDDownPayloadTreatGetSubServicesFromAccount tDownPayloadTreatGetSubServicesFromAccount = null;
                if (tDownPayloadGetSubServicesFromAccount?.SubServices != null)
                {
                    tDownPayloadTreatGetSubServicesFromAccount = new NWDDownPayloadTreatGetSubServicesFromAccount
                    {
                        AccountServiceList = tDownPayloadGetSubServicesFromAccount?.SubServices,
                    };
                   
                }
                rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.GetSubServicesFromAccount, 
                    tDownPayloadTreatGetSubServicesFromAccount, 
                    NWDRequestStatus.Ok);
            }
        }
        return rReturn;
    }
}