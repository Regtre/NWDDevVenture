using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDWebHttpErrorSimulator.Controllers;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebHttpErrorSimulator.Services;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Configuration;
using NWDWebStandard.Controllers;

namespace NWDWebHttpErrorSimulator.Managers
{
    [Serializable]
    public class NWDWebHttpErrorSimulatorSideBar :  INWDSideBar
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