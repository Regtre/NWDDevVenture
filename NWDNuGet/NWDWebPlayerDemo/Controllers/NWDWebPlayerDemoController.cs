using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Configuration;
using NWDWebStandard.Controllers;

namespace NWDWebPlayerDemo.Controllers
{
    public class NWDWebPlayerDemoController : NWDBasicController<NWDAccountController>
    {
        public ActionResult SetUp()
        {
            NWDLibraryInfos? tFileVersionInfo = NWDLibrariesInstalled.GetFileVersionInfo(GetType());
            if (tFileVersionInfo != null)
            {
                ViewData.Add(nameof(NWDLibraryInfos), tFileVersionInfo);
            }
            return View();
        }
    }
}