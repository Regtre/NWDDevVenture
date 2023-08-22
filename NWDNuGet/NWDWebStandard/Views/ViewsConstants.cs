/*using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;

namespace StandardWebSite
{
    public abstract class ViewsConstants
    {
        /// <summary>
        /// in Startup.css add
        /// services.AddMvc().AddRazorOptions(options =>
        /// options.ViewLocationFormats.Add(StandardWebSite.ViewsConstants.ViewsPathFormat);
        /// options.ViewLocationFormats.Add(StandardWebSite.ViewsConstants.ViewsPathModelFormat);
        /// }
        /// </summary>
        /// 
        // {0} - Action Name
        // {1} - Controller Name
        // {2} - Area Name
        // {3} - Feature Name
        // Replace normal view location entirely
        public const string ViewsFolder = "/Views/";
        public const string ViewsFolderShared = "/Views/Shared/";
        public const string ViewsFolderFormat = ViewsFolder + "{1}/{0}.cshtml";
        public const string ViewsFolderSharedFormat = ViewsFolder + "Shared/{0}.cshtml";
        public const string RightLocation = "/Front/Views/{1}/{0}.cshtml"; //RazorViewEngine.ViewExtension;

        public static string ViewsPath<T>(string sView) where T : Controller
        {
            return (ViewsFolder + typeof(T).Name.Replace("Controller", "") + "/" + sView + ".cshtml").Replace(".cshtml.cshtml", ".cshtml").Replace("//", "/");
        }
    }
}*/