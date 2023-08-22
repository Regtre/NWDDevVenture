using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Configuration;
using NWDWebGitLabReport.Managers;
using NWDWebGitLabReport.Models;
using NWDWebRuntime.Controllers;

namespace NWDWebGitLabReport.Controllers
{
    public class NWDWebGitLabReportController : NWDLibraryInformationController
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

        [HttpPost]
        public ActionResult SprintInfo(NWDSprintInfo sSprintInfo)
        {
            NWDSprintInfoManager.Save(HttpContext, sSprintInfo);
            return PartialView("_SprintInfo",sSprintInfo);
        }
    }
}