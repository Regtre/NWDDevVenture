using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDCrucial.Exchanges.Payloads;

public  class NWDUpPayloadDissociateServiceAndSubService : NWDUpPayloadCrucial
{
    public NWDReference<NWDAccount> AccountReference { get; set; }
    public NWDReference<NWDAccountService> ServiceReference { get; set; } = new NWDReference<NWDAccountService>();
    
}