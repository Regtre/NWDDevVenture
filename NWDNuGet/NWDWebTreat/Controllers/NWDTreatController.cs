using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Extensions;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;

namespace NWDWebTreat.Controllers
{
    public abstract class NWDTreatController<T> : NWDRawController where T : Controller
    {
        #region static for generic

        public static string ASP_Controller()
        {
            return typeof(T).Name.Replace("Controller", "");
        }

        #endregion
    }
}