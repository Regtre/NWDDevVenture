using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Models;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Models.Enums;

namespace NWDWebDevelopment.Managers
{
    [Serializable]
    public class NWDWebDevelopmentSideBar :  INWDSideBar
    {
       public NWDSideBarBlock[]? AddSideBarBlock(NWDSideBarKind sSideBarKind, HttpContext sHttpContext)
        {
            return null;
        }
        public NWDSideBarAnnexe[]? AddSideBarAnnexe(NWDSideBarKind sSideBarKind, HttpContext sHttpContext)
        {
            return null;
        }
    }
}