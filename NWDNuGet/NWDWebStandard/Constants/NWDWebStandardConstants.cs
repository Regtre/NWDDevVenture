using Microsoft.AspNetCore.Mvc;

namespace NWDWebStandard.Constants;

public class NWDWebStandardConstants
{
    public const string K_ViewsFolder = "/NWDWebStandard/NWDWebStandard/Views/";
    public const string K_ViewsFolderShared = "/NWDWebStandard/NWDWebStandard/Views/Shared/";
    public const string K_ViewsFolderFormat = K_ViewsFolder + "{1}/{0}.cshtml";
    public const string K_ViewsFolderSharedFormat = K_ViewsFolder + "Shared/{0}.cshtml";
    public const string K_RightLocation = "/Front/Views/{1}/{0}.cshtml"; //RazorViewEngine.ViewExtension;
    
    public static string ViewsPath<T>(string sView) where T : Controller
    {
        return (K_ViewsFolder + typeof(T).Name.Replace("Controller", "") + "/" + sView + ".cshtml").Replace(".cshtml.cshtml", ".cshtml").Replace("//", "/");
    }
}