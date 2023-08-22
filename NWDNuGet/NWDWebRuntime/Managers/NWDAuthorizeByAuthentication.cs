using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Managers
{
    public class NWDAuthorizeByAuthentication : Attribute, IAuthorizationFilter
    {
        private bool _Authenticated;
        private string _RedirectOnFail;
        private string _RedirectOnFailWithoutLayout;
        private bool _WithLayout;
        
        public const string K_ACCOUNT_ONLY = "/Home/AccountOnly/";
        public  const string K_ACCOUNT_ONLY_WITHOUT_LAYOUT = "/Home/AccountOnlyWithoutLayout/";
        public const string K_SERVICE_ONLY = "/Home/ServiceOnly/";
        public const string K_SERVICE_ONLY_WITHOUT_LAYOUT = "/Home/ServiceOnlyWithoutLayout/";
        
        public NWDAuthorizeByAuthentication(bool sAuthenticated, bool sWithLayout = true, string sRedirectOnFail = K_ACCOUNT_ONLY, string sRedirectOnFailWithoutLayout = K_ACCOUNT_ONLY_WITHOUT_LAYOUT)
        {
            this._Authenticated = sAuthenticated;
            this._RedirectOnFail = sRedirectOnFail;
            this._RedirectOnFailWithoutLayout = sRedirectOnFailWithoutLayout;
            this._WithLayout = sWithLayout;
        }
        public static bool ValidFor(HttpContext sContext, bool sAuthenticated)
        {
            return NWDAccountWebManager.AccountIsConnected(sContext) == sAuthenticated;
        }

        public void OnAuthorization(AuthorizationFilterContext sContext)
        {
            HttpContext tContext = sContext.HttpContext;
            bool tReturn = ValidFor(tContext, _Authenticated);
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