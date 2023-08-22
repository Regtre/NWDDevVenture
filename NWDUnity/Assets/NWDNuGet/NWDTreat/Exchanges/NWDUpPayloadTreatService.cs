using NWDFoundation.Exchanges;
using NWDFoundation.Models;

namespace NWDTreat.Exchanges.Payloads
{
    public class NWDUpPayloadTreatService : NWDUpPayloadTreat

    {
        #region properties

        // public NWDRequestPlayerToken? PlayerToken { set; get; } = null;
        public NWDAccountService? AccountService { set; get; } = null;

        #endregion

        public NWDUpPayloadTreatService()
        {
        }

        public NWDUpPayloadTreatService(/*NWDRequestPlayerToken sPlayerToken,*/ NWDAccountService sAccountService)
        {
            //PlayerToken = sPlayerToken;
            AccountService = sAccountService;
        }
    }
}