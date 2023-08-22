using Microsoft.AspNetCore.Mvc;

namespace NWDServer.Controllers
{
    public class NWDDefaultController : Controller
    {
        public IActionResult Index()
        {
            return View("/Views/NWDDefault/Index.cshtml");
        }
    }
}