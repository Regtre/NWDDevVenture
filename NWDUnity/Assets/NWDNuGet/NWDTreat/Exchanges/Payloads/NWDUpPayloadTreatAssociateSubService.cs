using NWDFoundation.Models;

namespace NWDTreat.Exchanges.Payloads
{
    public class NWDUpPayloadTreatAssociateSubService : NWDUpPayloadTreat
    {
        public NWDAccount OfferByAccount { get; set; } = new NWDAccount();
        public NWDAccount OfferToAccount { get; set; } = new NWDAccount();

        public NWDAccountService OfferByService { get; set; } = new NWDAccountService();
        public NWDAccountService OfferToService { get; set; } = new NWDAccountService();

        public NWDUpPayloadTreatAssociateSubService(NWDAccountService sOfferByService, NWDAccountService sOfferToService, NWDAccount sOfferByAccount, NWDAccount sOfferToAccount)
        {
            OfferByService = sOfferByService;
            OfferToService = sOfferToService;
            OfferByAccount = sOfferByAccount;
            OfferToAccount = sOfferToAccount;
        }
    }
}
