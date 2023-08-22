using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDTreat.Exchanges.Payloads
{
    public class NWDUpPayloadTreatDissociateServiceAndSubServices : NWDUpPayloadTreat
    {
        public NWDReference<NWDAccountService> ServiceReference { get; set; }

        public NWDReference<NWDAccount> AccountReference { get; set; }


        public NWDUpPayloadTreatDissociateServiceAndSubServices() { }

        public NWDUpPayloadTreatDissociateServiceAndSubServices(NWDReference<NWDAccountService> sService, NWDReference<NWDAccount> sAccountReference)
        {
            ServiceReference = sService;
            AccountReference = sAccountReference;
        }

    }
}
