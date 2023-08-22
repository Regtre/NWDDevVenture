using NWDFoundation.Models;

namespace NWDCrucial.Exchanges.Payloads;

public class NWDUpPayloadAssociateSubService : NWDUpPayloadCrucial
{
    public NWDAccount OfferByAccount { get; set; } = new NWDAccount();
    public NWDAccount OfferToAccount { get; set; } = new NWDAccount();
    
    public NWDAccountService OfferByService { get; set; } = new NWDAccountService();
    public NWDAccountService OfferToService { get; set; } = new NWDAccountService();
}