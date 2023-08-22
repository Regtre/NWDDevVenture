using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Models;
using NWDWebStandard.Models.Enums;
using UAParser;

namespace NWDWebStandard.Controllers
{
    /*
     https://bitscry.com/Projects/Chart
     */

    public class NWDStatisticsConsolidatedController : NWDBasicController<NWDStatisticsConsolidatedController>
    {
        private readonly ILogger<NWDStatisticsConsolidatedController> _logger;

        public NWDStatisticsConsolidatedController(ILogger<NWDStatisticsConsolidatedController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            PageInformation.JavascriptPathAddonList.Add("/vendors/chart/chart.min.js");
            PageInformation.Title = "Statistiques du site web";
            PageInformation.Keywords.AddRange(new List<string>() {
                "Statistiques",
            });
            string? tUserAgent = HttpContext.Request.Headers["User-Agent"];
            if (string.IsNullOrEmpty(tUserAgent) == false)
            {
                Parser tParser = Parser.GetDefault();
                ClientInfo tClientInfo = tParser.Parse(tUserAgent);
                ViewData["ClientInfo"] = tClientInfo;
            }
            ViewData["Home"] = NWDStatisticsConsolidated.GenerateBarChartWithGroup("Home", "HomePage", DateTime.Now, NWDStatisticsConsolidateRange.ThisDate, -7);
            return View();
        }

        public IActionResult Statistics()
        {
            PageInformation.JavascriptPathAddonList.Add("/vendors/chart/chart.min.js");
            NWDStatisticsGrid tGrid = new NWDStatisticsGrid();
            AddViewDataObject(tGrid);
            PageInformation.Title = "Statistiques du site web";
            PageInformation.Keywords.AddRange(new List<string>() {
                "Statistics",
            });
            // string tUserAgent = HttpContext.Request.Headers["User-Agent"];
            // Parser tParser = Parser.GetDefault();
            // ClientInfo tClientInfo = tParser.Parse(tUserAgent);
            // ViewData["ClientInfo"] = tClientInfo;
            tGrid.Add("Operating system", "", NWDStatisticsConsolidated.GenerateBarChartWithGroup("Operating system", NWDStatisticsConsolidated.K_OS, DateTime.Now, NWDStatisticsConsolidateRange.ThisDate, -7));
            tGrid.Add("Device", "", NWDStatisticsConsolidated.GenerateBarChartWithGroup("Devices", NWDStatisticsConsolidated.K_DEVICE, DateTime.Now, NWDStatisticsConsolidateRange.Month, -3));
            tGrid.Add("Browser", "",NWDStatisticsConsolidated.GenerateBarChartWithGroup("Browser", NWDStatisticsConsolidated.K_BROWSER, DateTime.Now, NWDStatisticsConsolidateRange.Month, -3));
            tGrid.Add("Sessions this date","",NWDStatisticsConsolidated.GenerateBarChartWithGroup("Distinct users", NWDStatisticsConsolidated.K_SESSION, DateTime.Now, NWDStatisticsConsolidateRange.ThisDate, -7));
            tGrid.Add("Sessions this hour", "", NWDStatisticsConsolidated.GenerateBarChartWithGroup("Distinct users", NWDStatisticsConsolidated.K_SESSION, DateTime.Now, NWDStatisticsConsolidateRange.ThisHour, -24));

            return View();
        }
    }
}
