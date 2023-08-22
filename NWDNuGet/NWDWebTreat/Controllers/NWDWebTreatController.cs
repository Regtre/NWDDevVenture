using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Configuration;
using NWDWebRuntime.Controllers;

namespace NWDWebTreat.Controllers
{
    public class NWDWebTreatController : NWDLibraryInformationController
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