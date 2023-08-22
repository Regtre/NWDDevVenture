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
using NWDWebDevelopment.Controllers;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebDevelopment.Services;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Configuration;
using NWDWebStandard.Controllers;

namespace NWDWebDevelopment.Managers
{
    [Serializable]
    public class NWDWebDevelopmentNavFooter :  INWDNavFooter
    {
        public NWDNavBarCategory[]? AddNavFooterMenu(HttpContext sHttpContext)
        {
            return null;
        }
    }
}