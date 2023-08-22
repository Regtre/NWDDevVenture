using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Exchanges;
using NWDRuntime.Exchanges;
using NWDWebRuntime.CallBacks;
using NWDWebRuntime.Configuration;
using NWDWebStandard.Controllers;
using NWDWebStandard.Models;

namespace NWDWeb.Controllers;
public class RelationshipTestController : NWDBasicController<RelationshipTestController>
{
    public async Task<IActionResult> Index()
    {
        NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestCreateRelationship(
            NWDWebRuntimeConfiguration.KConfig, 
            NWDWebRuntimeCallbackServers.GetRequestPlayerToken(HttpContext), 
            NWDExchangeOrigin.Web, 
            NWDExchangeDevice.Web );
        NWDResponseRuntime tRuntimeResponse = await NWDWebRuntimeCallbackServers.PostRequest(tRequestRuntime, HttpContext, true);
        
       
        tRuntimeResponse = await NWDWebRuntimeCallbackServers.PostRequest(tRequestRuntime, HttpContext, true);

        
        Console.WriteLine(tRuntimeResponse.Status);

        return View();
    }

    public async Task<IActionResult> InsertCode(string code)
    {
        NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestLinkRelationship(
            NWDWebRuntimeConfiguration.KConfig, 
            NWDWebRuntimeCallbackServers.GetRequestPlayerToken(HttpContext), 
            NWDExchangeOrigin.Web, 
            NWDExchangeDevice.Web,code);
        
        NWDResponseRuntime tRuntimeResponse = await NWDWebRuntimeCallbackServers.PostRequest(tRequestRuntime, HttpContext, true);
        
        Console.WriteLine(tRuntimeResponse.Status);
        
        return View("Index");
    }
}