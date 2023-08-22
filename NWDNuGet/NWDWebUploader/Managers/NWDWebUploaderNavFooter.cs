using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Models;
using NWDWebRuntime.Facades;
using NWDWebStandard.Configuration;
using NWDWebStandard.Controllers;

namespace NWDWebUploader.Managers
{
    [Serializable]
    public class NWDWebUploaderNavFooter :  INWDNavFooter
    {
        public NWDNavBarCategory[]? AddNavFooterMenu(HttpContext sHttpContext)
        {
            return null;
        }
    }
}