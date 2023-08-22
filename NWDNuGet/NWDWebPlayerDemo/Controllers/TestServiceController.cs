using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Models;
using NWDWebRuntime.Managers;
using NWDWebStandard.Controllers;
using NWDWebTreat.Managers;

namespace NWDWebPlayerDemo.Controllers;

public class TestServiceController : NWDBasicController<TestServiceController>
{
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Dissociate(ulong referenceAccount, ulong referenceService)
    {
        NWDWebDataTreatManger.DissociateServiceAndSubService( new NWDReference<NWDAccountService>(referenceService), new NWDReference<NWDAccount>(referenceAccount));
        
        return View("Index");
    }
    public IActionResult Associate(ulong referenceAccount, ulong serviceNumber)
    {
        NWDAccount tAccount = NWDAccountWebManager.GetAccountInContext(HttpContext);
        NWDAccount tAccountOfferTo = new NWDAccount()
        {
            Reference = referenceAccount
        };

        NWDAccountService service = NWDAccountServiceWebManager.GetServices(HttpContext).Where(x => x.Reference != 0)
            .ToList().FirstOrDefault();

        NWDWebDataTreatManger.AssociateSubService(service,new NWDAccountService(),tAccount,tAccountOfferTo);

        return View("Index");
    }
}