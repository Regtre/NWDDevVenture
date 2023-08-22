using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Configuration;
using NWDWebRuntime.Managers;

namespace NWDWebRuntime.Controllers
{
    public class NWDWebRuntimeController : NWDLibraryInformationController
    {
        public  ActionResult SetUp()
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