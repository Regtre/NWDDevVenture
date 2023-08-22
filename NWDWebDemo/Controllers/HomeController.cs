using Microsoft.AspNetCore.Mvc;
using NWDWebStandard.Controllers;
using NWDWebStandard.Models;

namespace NWDWeb.Controllers;
public class HomeController : NWDBasicController<HomeController>
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        NWDStatisticsConsolidated.IncrementForValue("Home", "HomePage", "Home", "", HttpContext);
        return View();
    }
}