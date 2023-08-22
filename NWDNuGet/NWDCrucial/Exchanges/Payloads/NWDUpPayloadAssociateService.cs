using NWDFoundation.Exchanges;
using NWDFoundation.Models;

namespace NWDCrucial.Exchanges.Payloads;

public class NWDUpPayloadAssociateService : NWDUpPayloadCrucial
{
    #region properties

    // public NWDRequestPlayerToken? PlayerToken { set; get; } = null;
    public NWDAccountService? AccountService { set; get; } = null;

    #endregion

    public NWDUpPayloadAssociateService()
    {
    }

    public NWDUpPayloadAssociateService(/*NWDRequestPlayerToken sPlayerToken,*/ NWDAccountService sAccountService)
    {
        // PlayerToken = sPlayerToken;
        AccountService = sAccountService;
    }

    
}