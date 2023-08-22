using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Cookies
{
    public class NWDCookieBool : NWDCookieDefinition
    {
        public NWDCookieBool(string sName, string sTitle, string sDescription, NWDCookieDefinitionGroup sGroup, bool sDefaultValue, bool sDeletable = true, bool sManualEditable = true, int sDuration = 365, bool sAutoRenew = false, bool sSecure = true, SameSiteMode sLimitSite = SameSiteMode.Strict)
        {
            Kind = NWDCookieDefinitionKind.BoolKind;
            Name = sName;
            Title = sTitle;
            Explication = sDescription;
            Group = sGroup;
            Duration = sDuration;
            DefaultValue = sDefaultValue.ToString();
            LimitSite = sLimitSite;
            Secure = sSecure;
            AutoRenew = sAutoRenew;
            Deletable = sDeletable;
            ManualEditable = sManualEditable;
            if (sGroup == NWDCookieDefinitionGroup.Functional || sGroup == NWDCookieDefinitionGroup.Consent)
            {
                Deletable = false;
                ManualEditable = false;
            }
            if (NWDCookieGlobal.KDictionary.ContainsKey(Name))
            {
                NWDCookieGlobal.KDictionary[Name] = this;
            }
            else
            {
                NWDCookieGlobal.KDictionary.Add(Name, this);
            }
        }
        public static NWDCookieBool? GetCookieDefinition(string sName)
        {
            NWDCookieBool? rReturn = null;
            if (NWDCookieGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDCookieBool)NWDCookieGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public bool GetValue(HttpContext? sHttpContext)
        {
            bool rReturn = bool.Parse(DefaultValue);
            string? tValue = _GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                bool.TryParse(tValue, out rReturn);
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, bool sValue)
        {
            _SetValue(sHttpContext, sValue.ToString());
        }

        public string GenerateCookieDataString(bool sValue)
        {
            return _GenerateCookieDataString(sValue.ToString(CultureInfo.InvariantCulture));
        }
        public string GenerateOnClick(bool sValue)
        {
            return _GenerateOnClick(sValue.ToString(CultureInfo.InvariantCulture));
        }
        public override string RawForm(HttpContext? sHttpContext)
        {
            string rReturn = "<!-- " + nameof(NWDCookieBool) + " RawForm-->";
            if (ManualEditable)
            {
                rReturn = rReturn + " <div class=\"form-check form-switch\">";
                if (GetValue(sHttpContext))
                {
                    rReturn = rReturn + " <input class=\"form-check-input\" type=\"checkbox\" onclick=\"" + InstallOnClick("false") + ";window.location.reload();\" checked />";
                }
                else
                {
                    rReturn = rReturn + " <input class=\"form-check-input\" type=\"checkbox\" onclick=\"" + InstallOnClick("true") + ";window.location.reload();\" />";
                }
                rReturn = rReturn + "</div>";
            }
            return rReturn;
        }
    }
}
