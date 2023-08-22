using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Configuration;
using NWDWebRuntime.Controllers;

namespace NWDIdemobi.Controllers
{
    public class NWDIdemobiController : NWDLibraryInformationController
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