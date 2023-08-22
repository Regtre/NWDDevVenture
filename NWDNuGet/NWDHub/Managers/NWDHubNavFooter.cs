using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Models;
using NWDWebRuntime.Facades;
using NWDWebStandard.Configuration;
using NWDWebStandard.Controllers;

namespace NWDHub.Managers
{
    [Serializable]
    public class NWDHubNavFooter :  INWDNavFooter
    {
        public NWDNavBarCategory[]? AddNavFooterMenu(HttpContext sHttpContext)
        {
            return null;
        }
    }
}