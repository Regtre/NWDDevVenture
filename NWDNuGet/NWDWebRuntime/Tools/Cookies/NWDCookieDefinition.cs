using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Configuration;

namespace NWDWebRuntime.Tools.Cookies
{
    public abstract class NWDCookieDefinition
    {
        public NWDCookieDefinitionKind Kind = NWDCookieDefinitionKind.StringKind;
        public string Name = string.Empty;
        public string Title = string.Empty;
        public string Explication = string.Empty;
        public NWDCookieDefinitionGroup Group = NWDCookieDefinitionGroup.Optional;
        public bool AutoRenew = false;
        public int Duration = 3600;
        public string DefaultValue = string.Empty;
        public bool Secure = true;
        public SameSiteMode LimitSite = SameSiteMode.Strict;
        public bool Deletable = true;
        public bool ManualEditable = false;

        protected string _GenerateCookieDataString(string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = string.Empty;
            }

            string rReturn = Name + "=" + sValue + ";" +
                             " expires=" + DateTime.UtcNow.AddDays(Duration).ToString("ddd, dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture) + " GMT;" +
                             " path=/;" +
                             " samesite=" + LimitSite.ToString();
                rReturn = rReturn + "; Secure";
                return rReturn;
        }

        protected string _GenerateOnClick(string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = string.Empty;
            }

            string rReturn = "var now = new Date(); var year = now.getFullYear(); var month = now.getMonth(); var day = now.getDate(); var next = new Date(year, month, day+ " + Duration +
                             "); document.cookie = '" + Name + "=" + sValue + "; SameSite=" + LimitSite.ToString() + "; Path=/; expires=' + next.toUTCString() + '";
            rReturn = rReturn + "; Secure";
            rReturn = rReturn + "';";
            return rReturn;
        }

        protected string? _GetValue(HttpContext? sHttpContext)
        {
            string? rReturn = null;
            if (sHttpContext != null)
            {
                foreach (var tCookie in sHttpContext.Request.Cookies)
                {
                    if (tCookie.Key == Name)
                    {
                        rReturn = tCookie.Value;
                        if (AutoRenew)
                        {
                            _SetValue(sHttpContext, rReturn);
                        }

                        break;
                    }
                }
            }

            return rReturn;
        }
        
        // protected DateTime? _GetExpires(HttpContext? sHttpContext)
        // {
        //     DateTime? rReturn = null;
        //     if (sHttpContext != null)
        //     {
        //         foreach (var tCookie in sHttpContext.Request.Cookies)
        //         {
        //             if (tCookie.Key == Name)
        //             {
        //                 rReturn = tCookie. .. // impossible ... cookie not return with value from navigator?!;
        //             }
        //         }
        //     }
        //     return rReturn;
        // }

        protected void _SetValue(HttpContext? sHttpContext, string sValue)
        {
            CookieOptions tCookieOptions = new CookieOptions();
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = string.Empty;
            }
            tCookieOptions.Expires = DateTimeOffset.Now.AddDays(Duration);
            tCookieOptions.SameSite = LimitSite;
            tCookieOptions.Secure = Secure;
            if (sHttpContext != null)
            {
                sHttpContext.Response.Cookies.Append(Name, sValue, tCookieOptions);
            }
        }

        public bool Exists(HttpContext sHttpContext)
        {
            bool rReturn = false;
            foreach (var tCookie in sHttpContext.Request.Cookies)
            {
                if (tCookie.Key == Name)
                {
                    rReturn = true;
                    break;
                }
            }
            return rReturn;
        }
        // public DateTime GetExpires(HttpContext? sHttpContext)
        // {
        //     return _GetExpires(sHttpContext);
        // }
        public string GetValueAsString(HttpContext? sHttpContext, bool sForHtml = false)
        {
            if (sForHtml)
            {
                string? tR = _GetValue(sHttpContext);
                if (tR != null)
                {
                    return "<span>"+Regex.Replace(tR, ".{12}", "$0</span>&hairsp;<span>") + "</span>";
                    //return Regex.Replace(tR, ".{8}", "$0 ");
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                string? tValue = _GetValue(sHttpContext);
                if (tValue != null)
                {
                    return tValue;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public void DeleteCookie(HttpContext? sHttpContext)
        {
            if (sHttpContext != null)
            {
                sHttpContext.Response.Cookies.Delete(Name);
            }
        }

        public string DeleteOnClick(string sValue = "delete")
        {
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = string.Empty;
            }

            string rReturn = "document.cookie = '" + Name + "=" + sValue + "; SameSite=" + LimitSite.ToString() + "; Path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT";
            rReturn = rReturn + "; Secure";
            rReturn = rReturn + "';";
            return rReturn;
        }

        public string InstallOnClick()
        {
            string rReturn = "document.cookie = '" + Name + "=" + DefaultValue + "; SameSite=" + LimitSite.ToString() + "; Path=/; expires=" +DateTime.UtcNow.AddDays(Duration).ToString("ddd, dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture) + " GMT";
                rReturn = rReturn + "; Secure";
                rReturn = rReturn + "';";
            return rReturn;
        }

        public string InstallOnClick(string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = string.Empty;
            }

            string rReturn = "document.cookie = '" + Name + "=" + sValue + "; SameSite=" + LimitSite.ToString() + "; Path=/; expires=" +DateTime.UtcNow.AddDays(Duration).ToString("ddd, dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture) + " GMT";
            rReturn = rReturn + "; Secure";
            rReturn = rReturn + "';";
            return rReturn;
        }

        public virtual string RawForm(HttpContext? sHttpContext)
        {
            string rReturn = "<!-- " + nameof(NWDCookieDefinition) + " RawForm-->";
            return rReturn;
        }
    }
}