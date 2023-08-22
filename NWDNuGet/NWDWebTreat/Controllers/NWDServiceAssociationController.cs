using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Models;
using NWDWebRuntime.Managers;
using NWDWebStandard.Controllers;
using NWDWebTreat.Managers;

namespace NWDWebTreat.Controllers;

public class NWDServiceAssociationController : NWDBasicController<NWDServiceAssociationController>
{
    public IActionResult Index()
    {
        
        return View();
    }
    public IActionResult AssociateSubService(string service, string accountReferenceToOffer)
    {
        NWDAccountService? tService = NWDAccountServiceWebManager.GetServices(HttpContext)?.First(sItem => sItem.Service ==  long.Parse(service));
        if (tService != null)
        {
            NWDWebDataTreatManger.AssociateSubService(tService, tService,
                NWDAccountWebManager.GetAccountInContext(HttpContext),
                new NWDAccount()
                {
                    Reference = ulong.Parse(accountReferenceToOffer)
                });
        }
        return RedirectToAction("Index");
    }

    public IActionResult DissociateSubService(string service, string accountReferenceToOffer)
    {
        
        NWDWebDataTreatManger.DissociateServiceAndSubService(new NWDReference<NWDAccountService>()
        {
            Reference = ulong.Parse(service)
        }, new NWDReference<NWDAccount>()
        {
            Reference = ulong.Parse(accountReferenceToOffer)
        });
        
        return RedirectToAction("Index");
    }
    

    
}