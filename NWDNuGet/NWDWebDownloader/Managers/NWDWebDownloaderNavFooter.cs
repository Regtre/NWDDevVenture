using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Models;
using NWDWebRuntime.Facades;
using NWDWebStandard.Configuration;
using NWDWebStandard.Controllers;

namespace NWDWebDownloader.Managers
{
    [Serializable]
    public class NWDWebDownloaderNavFooter :  INWDNavFooter
    {
        public NWDNavBarCategory[]? AddNavFooterMenu(HttpContext sHttpContext)
        {
            return null;
        }
    }
}