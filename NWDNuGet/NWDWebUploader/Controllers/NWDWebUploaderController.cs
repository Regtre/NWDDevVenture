using NWDWebDownloader.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWDFoundation.Configuration;
using NWDFoundation.WebEdition.Enums;
using NWDWebDownloader.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Controllers;
using NWDWebUploader.Configuration;
using NWDWebUploader.Managers;
using NWDWebUploader.Models;

namespace NWDWebUploader.Controllers
{
    public class NWDWebUploaderController : NWDBasicController<NWDWebUploaderController>
    {
        public static NWDToastStandard UploadSucessFull =
            NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.Alert, NWDBootstrapKindOfStyle.Success,
                "Upload successfull", "Your file has been uploaded successfully");
        

        public static NWDToastStandard UploadFailed = NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp,
            NWDBootstrapKindOfStyle.Success, "Something went wrong", "Upload failed");


        public override void OnActionExecuting(ActionExecutingContext sContext)
        {
            base.OnActionExecuting(sContext);
            PageInformation.Title = "Upload";
            //PageInformation.SideBarStyle = NWDSideBarKind.None;
            
        }

        [NWDAuthorizeAdminOnly()]
        public ActionResult Index()
        {
            Directory.GetCurrentDirectory();
            List<NWDLibraryInfos> tFileVersionInfoList = NWDLibrariesInstalled.GetFileVersionInfoList();
            ViewData.Add(nameof(NWDLibraryInfos), tFileVersionInfoList.ToArray());
            return View();
        }

        [NWDAuthorizeAdminOnly()]
        public ActionResult Upload(string sDownloadPageNameFor)
        {
            NWDDownloadConfig? tConfig =
                NWDWebDownloaderConfiguration.KConfig.Downloads.Find(sX => sX.PageName == sDownloadPageNameFor);
            return View(tConfig);
        }

        [NWDAuthorizeAdminOnly()]
        [HttpPost]
        [RequestSizeLimit(NWDWebUploaderConfiguration.MaxUploadSizeInBytes)]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public ActionResult Upload(NWDUploadFile sUploadFile, string sDownloadPageNameFor)
        {
            NWDToastStandard tStandard;
            if (sUploadFile is { File: { } } && NWDUploadFileManager.UploadFile(sUploadFile))
            {
                tStandard = UploadSucessFull;
            }
            else
            {
                tStandard = UploadFailed;
            }

            return PartialView("_ToastStandard", tStandard);
        }

        [NWDAuthorizeAdminOnly()]
        [HttpPost]
        public ActionResult CreateNewDirectory(string sNewDirectory, string sRootPath, string sDownloadPageName)
        {
            if (string.IsNullOrEmpty(sNewDirectory) == false && string.IsNullOrEmpty(sRootPath) == false)
            {
                string tPath = Path.Combine(sRootPath, sNewDirectory);
                if (!Directory.Exists(tPath))
                {
                    Directory.CreateDirectory(tPath);
                }
            }

            return PartialView("_Category", NWDDownloadManager.GetDirectories(NWDWebDownloaderConfiguration.KConfig.Downloads.Find(sX => sX.PageName == sDownloadPageName)));
        }

        public ActionResult Delete(string sPath, string sDownloadPageName)
        {
            NWDUploadFileManager.DeleteFile(sPath);
            return RedirectToAction("DownloadList", "NWDWebUploader", new { sName = sDownloadPageName });
        }


        [NWDAuthorizeAdminOnly()]
        public ActionResult DownloadList(string sName)
        {
            NWDDownloadConfig? tConfig =
                NWDWebDownloaderConfiguration.KConfig.Downloads.Find(sX => sX.PageName == sName);
            if (tConfig != null)
            {
                NWDDownloadManager.PrepareBuilds(tConfig);
            }

            return PartialView("_BuildByCategories", tConfig);
        }
    }
}