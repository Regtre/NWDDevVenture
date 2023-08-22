using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Managers
{
    public class NWDAuthorizeAdminOnly : Attribute, IAuthorizationFilter
    {
        private string _RedirectOnFail;
        private string _RedirectOnFailWithoutLayout;
        private bool _WithLayout;

        public NWDAuthorizeAdminOnly(bool sWithLayout = true, string sRedirectOnFail = "/Home/AdminOnly/", string sRedirectOnFailWithoutLayout = "/Home/AdminOnlyWithoutLayout/")
        {
            this._RedirectOnFail = sRedirectOnFail;
            this._WithLayout = sWithLayout;
            this._RedirectOnFailWithoutLayout = sRedirectOnFailWithoutLayout;
        }

        public static bool ValidFor(HttpContext? sContext)
        {
            bool rReturn = false;
            if (sContext != null)
            {
                rReturn = NWDAuthorizeByAllOfServices.ValidFor(sContext, new  NWDGenericServiceEnum[] { /*NWDWebRuntimeConfiguration.KConfig.AdminService,*/ NWDGenericServiceEnum.Admin });
            }

            return rReturn;
        }

        public void OnAuthorization(AuthorizationFilterContext sContext)
        {
            HttpContext tContext = sContext.HttpContext;
            bool tReturn = ValidFor(tContext);
            if (tReturn == false)
            {
                if (_WithLayout)
                {
                    sContext.Result = new RedirectResult(_RedirectOnFail);
                }
                else
                {
                    sContext.Result = new RedirectResult(_RedirectOnFailWithoutLayout);
                }
            }
        }
    }
}