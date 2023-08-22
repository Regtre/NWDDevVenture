using NWDEditor.Exchanges;
using NWDFoundation.Exchanges;
using NWDHub.Configuration;
using NWDTreat.Exchanges;
using NWDTreat.Exchanges.Payloads;
using NWDTreat.Facades;

namespace NWDHub.Managers;

public class NWDTreatManager : INWDTreatManager
{
    public NWDResponseTreat Process(NWDRequestTreat sRequest)
    {
        NWDResponseTreat rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.Unknown, null, NWDRequestStatus.Error);
        switch (sRequest.Kind)
        {
            case NWDExchangeTreatKind.ServiceCreate:
            {
                 rReturn = ServiceCreate(sRequest);
                 break;
            }

            case NWDExchangeTreatKind.ServiceUpdate:
            {
                rReturn = ServiceUpdate(sRequest);
                break;
            }
            case NWDExchangeTreatKind.ServiceDelete:
            {
                rReturn = ServiceDelete(sRequest);
                break;
            }
            case NWDExchangeTreatKind.AssociateService:
            {
                rReturn = AssociateService(sRequest);
                break;
            }
            case NWDExchangeTreatKind.AssociateSubService:
            {
                rReturn = AssociateSubService(sRequest);
                break;
            }
            case NWDExchangeTreatKind.DissociateService:
            {
                rReturn = DissociateService(sRequest);
                break;
            }
            case NWDExchangeTreatKind.GetSubServicesFromAccount:
            {
                rReturn = GetSubServicesFromAccount(sRequest);
                break;
            }
            default:
                rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.Unknown, null, NWDRequestStatus.Error);
                break;
        }
        return rReturn;
    }

 


    private NWDResponseTreat AssociateService(NWDRequestTreat sRequest)
    {
        NWDResponseTreat rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.Unknown, null, NWDRequestStatus.Error);
        if (sRequest.IsValid(NWDHubConfiguration.KConfig))
        {
            rReturn = NWDServiceManager.AssociateService(sRequest);
        }
        return rReturn; 
    }

    private NWDResponseTreat AssociateSubService(NWDRequestTreat sRequest)
    {
        NWDResponseTreat rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.Unknown, null, NWDRequestStatus.Error);
        if (sRequest.IsValid(NWDHubConfiguration.KConfig))
        {
            rReturn = NWDServiceManager.AssociateSubService(sRequest);
        }
        return rReturn; 
    }
    private NWDResponseTreat DissociateService(NWDRequestTreat sRequest)
    {
        NWDResponseTreat rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.Unknown, null, NWDRequestStatus.Error);
        if (sRequest.IsValid(NWDHubConfiguration.KConfig))
        {
            rReturn = NWDServiceManager.DissociateServiceAndSubServices(sRequest);
        }
        return rReturn;
    }
    
    private NWDResponseTreat GetSubServicesFromAccount(NWDRequestTreat sRequest)
    {
        NWDResponseTreat rReturn = new NWDResponseTreat(NWDHubConfiguration.KConfig, sRequest.ProjectId, sRequest.Environment, NWDExchangeTreatKind.Unknown, null, NWDRequestStatus.Error);
        if (sRequest.IsValid(NWDHubConfiguration.KConfig))
        {
            rReturn = NWDServiceManager.GetSubServicesFromAccount(sRequest);
        }
        return rReturn;
    }
    private NWDResponseTreat ServiceDelete(NWDRequestTreat sRequest)
    {
        throw new NotImplementedException();
    }

    private NWDResponseTreat ServiceUpdate(NWDRequestTreat sRequest)
    {
        throw new NotImplementedException();
    }

    private NWDResponseTreat ServiceCreate(NWDRequestTreat sRequest)
    {
        throw new NotImplementedException();
    }
}