using Microsoft.AspNetCore.Mvc;
using NWDWebRuntime.Models.Enums;

namespace NWDWebStandard.Controllers
{

    [Microsoft.AspNetCore.Components.Route("Blog")]
    public class NWDBlogController : NWDBasicController<NWDAccountController>
    {
        [Route("Blog")]
        public ActionResult Index()
        {
            PageInformation.Title = "Blog";
            PageInformation.Keywords.AddRange(new List<string>()
            {
                "blog",
                "list",
            });
            PageInformation.ShowAuthentication = false;
            return View();
        }
    }
}