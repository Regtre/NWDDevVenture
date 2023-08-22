using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Configuration;

namespace NWDWebStandard.Controllers
{
    public class NWDSetUpController : NWDBasicController<NWDSetUpController>
    {
        public IActionResult Index()
        {
            List<NWDLibraryInfos> tFileVersionInfoList = NWDLibrariesInstalled.GetFileVersionInfoList();
            ViewData.Add(nameof(NWDLibraryInfos), tFileVersionInfoList.ToArray());
            return View();
        }
    }
}