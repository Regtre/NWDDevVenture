using System.Text;
using NWDWebDownloader.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWDFoundation.Configuration;
using NWDWebDownloader.Configuration;
using NWDWebDownloader.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Controllers;
using NWDWebStandard.Models;
using NWDWebStandard.Models.Enums;
using NWDWebRuntime.Managers;

namespace NWDWebDownloader.Controllers
{
    public class NWDWebDownloaderController : NWDBasicController<NWDWebDownloaderController>
    {
        public override void OnActionExecuting(ActionExecutingContext sContext)
        {
            base.OnActionExecuting(sContext);
            PageInformation.Title = "Upload";
            //PageInformation.SideBarStyle = NWDSideBarKind.None;
        }

        public ActionResult SetUp()
        {
            NWDLibraryInfos? tFileVersionInfo = NWDLibrariesInstalled.GetFileVersionInfo(GetType());
            if (tFileVersionInfo != null)
            {
                ViewData.Add(nameof(NWDLibraryInfos), tFileVersionInfo);
            }

            return View();
        }

        public IActionResult Download(string sName)
        {
            NWDDownloadConfig? tConfig = NWDWebDownloaderConfiguration.KConfig.Downloads.Find(sX => sX.PageName == sName);
            if (tConfig != null)
            {
                if (tConfig.AllowAnonymous || (tConfig.AllowConnected && NWDAuthorizeByAuthentication.ValidFor(HttpContext, true) || (NWDAuthorizeByAllOfServices.ValidFor(HttpContext, tConfig.Service ))))
                {
                    NWDStatisticsConsolidated.IncrementForValue(sName, "DownloadView", sName, "", HttpContext);
                    NWDDownloadManager.PrepareBuilds(tConfig);
                    return View(tConfig);
                }
                else
                {
                    return RedirectToAction(nameof(ServiceOnly));
                }
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }

        public IActionResult DownloadList(string sName)
        {
            NWDStatisticsConsolidated.IncrementForValue(sName, "DownloadList", sName, "", HttpContext);
            NWDDownloadConfig? tConfig = NWDWebDownloaderConfiguration.KConfig.Downloads.Find(sX => sX.PageName == sName);
            if (tConfig != null)
            {
                if (tConfig.AllowAnonymous || (tConfig.AllowConnected && NWDAuthorizeByAuthentication.ValidFor(HttpContext, true) || (NWDAuthorizeByAllOfServices.ValidFor(HttpContext, tConfig.Service ))))
                {
                    NWDDownloadManager.PrepareBuilds(tConfig);
                    return PartialView("_DownloadByCategory", tConfig);
                }
                else
                {
                    return RedirectToAction(nameof(ServiceOnly));
                }
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpPost]
        public FileResult Download(NWDBuild sBuild)
        {
            // TODO secure download to service, folder and file in folder
            NWDDownloadConfig? tConfig = NWDWebDownloaderConfiguration.KConfig.Downloads.Find(sX => sX.PageName == sBuild.FolderName);
            if (tConfig != null)
            {
                Console.WriteLine("Config exist for " + sBuild.FolderName);
                if (tConfig.AllowAnonymous || (tConfig.AllowConnected && NWDAuthorizeByAuthentication.ValidFor(HttpContext, true) || (NWDAuthorizeByAllOfServices.ValidFor(HttpContext, tConfig.Service ))))
                {
                    // TODO Verif file path exist in this folder !
                    Console.WriteLine("Ok you can download this file");
                    
                    NWDStatisticsConsolidated.IncrementForValue(sBuild.Name, "DownloadBuild", sBuild.Name, "", HttpContext);
                    string tMime = GetMime(sBuild.Path);
                    return PhysicalFile(sBuild.Path, tMime, sBuild.Name);
                }
                else
                {
                    Console.WriteLine("NO! you cannot download this file");
                }
            }
            else
            {
                
                Console.WriteLine("Config unknown for " + sBuild.FolderName);
            }
            // return error
            var tContentType = "text/xml";
            var tContent = "error";
            var tBytes = Encoding.UTF8.GetBytes(tContent);
            var tResult = new FileContentResult(tBytes, tContentType);
            tResult.FileDownloadName = "error.txt";
            return tResult;
        }

        private string GetMime(string sFilename)
        {
            string tMime = "application/octet-stream";
            switch (Path.GetExtension(sFilename))
            {
                case ".plist":
                case ".xml":
                    tMime = "text/xml";
                    break;
                case ".ipa":
                    tMime = "application/octet-stream";
                    break;
                case ".apk":
                    tMime = "application/vnd.android.package-archive";
                    break;
                case ".app":
                    tMime = "application/octet-stream";
                    break;
                case ".exe":
                    tMime = "application/octet-stream";
                    break;
                case ".png":
                    tMime = "image/png";
                    break;
                case ".jpg":
                case ".jpeg":
                    tMime = "image/jpg";
                    break;
                case ".zip":
                    tMime = "application/zip";
                    break;
            }

            return tMime;
        }

        public IActionResult Statistics()
        {
            NWDStatisticsGrid tGrid = new NWDStatisticsGrid();
            AddViewDataObject(tGrid);
            PageInformation.Title = "Statistiques of download";
            PageInformation.Keywords.AddRange(new List<string>()
            {
                "Statistics",
            });
            //PageInformation.SideBarStyle = NWDSideBarKind.None;
            tGrid.Add("", "", NWDStatisticsConsolidated.GenerateBarChartWithGroup("Build", "DownloadBuild", DateTime.Now, NWDStatisticsConsolidateRange.ThisDate, -7));
            tGrid.Add("", "", NWDStatisticsConsolidated.GenerateBarChartWithGroup("List", "DownloadList", DateTime.Now, NWDStatisticsConsolidateRange.Month, -3));
            tGrid.Add("", "", NWDStatisticsConsolidated.GenerateBarChartWithGroup("DownloadView", "DownloadView", DateTime.Now, NWDStatisticsConsolidateRange.Month, -3));
            return View("/Views/NWDStatisticsConsolidated/Statistics.cshtml", tGrid);
        }
    }
}