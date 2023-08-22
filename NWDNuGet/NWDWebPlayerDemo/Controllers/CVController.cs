using Microsoft.AspNetCore.Mvc;
using NWDFoundation.WebEdition.Attributes;
using NWDWebPlayerDemo.Managers;
using NWDWebRuntime.Managers;
using NWDWebPlayerDemo.Models.CV;
using NWDWebRuntime.Controllers;
using NWDWebStandard.Controllers;

namespace NWDWebPlayerDemo.Controllers;

public class CVController : NWDBasicController<CVController>
{
    // GET
    [NWDWebMethodTest()]
    [NWDWebMethodTestGet("?ide=lll")]
    [NWDWebMethodTestPost("{\"ide\":\"lll\"}")]
    public IActionResult Index()
    {
        return View( NWDWebDataManager.GetDataForPlayerByClass<CV>(HttpContext));
    }

    public IActionResult Create()
    {
        ViewData["Action"] = "Create";
        return View("FormCV", new CV());
    }
    public IActionResult Edit(ulong Reference)
    {
        ViewData["Action"] = "Edit";
        return View("FormCV", NWDWebDataManager.GetDataByReference<CV>(HttpContext, Reference));
    }

    [HttpPost]
    public IActionResult Save(CV sCV)
    {
        NWDCVManager.Save(HttpContext,sCV);
        return RedirectToAction("Index"); 
    }
}