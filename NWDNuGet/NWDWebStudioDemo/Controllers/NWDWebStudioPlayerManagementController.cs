using Microsoft.AspNetCore.Mvc;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebStandard.Controllers;

namespace NWDWebStudioDemo.Controllers;

public class NWDWebStudioPlayerManagementController : NWDBasicController<NWDWebStudioPlayerManagementController>
{
    [NWDAuthorizeAdminOnly()]
    public IActionResult Index()
    {
        //PageInformation.SideBarPartialList.Add(new NWDPartialView(){PartialPath = "/Views/NWDWebStudioPlayerManagement/_SideBar_Tools.cshtml"});
        return View();
    }
    
    [NWDAuthorizeAdminOnly()]
    public IActionResult OfferServices()
    {
        //PageInformation.SideBarPartialList.Add(new NWDPartialView(){PartialPath = "/Views/NWDWebStudioPlayerManagement/_SideBar_Tools.cshtml"});
        return View();
    }

    [NWDAuthorizeAdminOnly()]
    public IActionResult Statistics()
    {
        //PageInformation.SideBarPartialList.Add(new NWDPartialView() { PartialPath = "/Views/NWDWebStudioPlayerManagement/_SideBar_Tools.cshtml" });
        return View();
    }
}