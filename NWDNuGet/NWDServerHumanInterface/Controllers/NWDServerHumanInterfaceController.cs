using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Configuration;

namespace NWDServerHumanInterface.Controllers
{
    public class NWDServerHumanInterfaceController : Controller
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