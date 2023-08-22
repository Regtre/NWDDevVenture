using Microsoft.AspNetCore.Mvc;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NWDFoundation.Configuration;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;
using NWDWebStandard.Configuration;
using NWDWebStandard.Extensions;
using NWDWebStandard.Models;

namespace NWDWebStandard.Controllers
{

    // [Microsoft.AspNetCore.Components.Route("Page")]
    public class NWDPageController : NWDBasicController<NWDPageController>
    {
        public ActionResult Index()
        {
            PageInformation.Title = "Page";
            PageInformation.Keywords.AddRange(new List<string>()
            {
                "page",
                "list",
            });
            PageInformation.ShowAuthentication = false;
            return View();
        }

        [Route(nameof(NWDPageController.Show)+"/{sReference}/{sPageName}")]
        public ActionResult Show(ulong sReference, string sPageName)
        {
            // Console.WriteLine("ok je suis là");
            PageInformation.PageStyle = NWDPageContainer.ContainerPage;
            NWDPage tPage = NWDWebDBDataManager.GetDataByReference<NWDPage>(sReference);
            if (tPage != null)
            {
                PageInformation.Title = tPage.Title;
                PageInformation.Description = tPage.MetaDescription;
                PageInformation.Keywords = tPage.MetaKeywords.Split(',', StringSplitOptions.TrimEntries).ToList();
                
                return View(tPage);
            }
            return RedirectToAction(nameof(Error));
        }
    }
}