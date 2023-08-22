using Microsoft.AspNetCore.Mvc;

namespace NWDWebRuntime.Controllers
{
    public class NWDRedirectionController : NWDRawController
    {
        public static string ASP_Controller()
        {
            return nameof(NWDRedirectionController).Replace("Controller", "");
        }
        
        public IActionResult GoToWebsite(string sUrl)
        {
            Console.WriteLine(nameof(GoToWebsite) +" => " + sUrl);
            return Redirect(sUrl);
        }
    }
}