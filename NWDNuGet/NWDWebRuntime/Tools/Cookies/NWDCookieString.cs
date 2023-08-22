using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Cookies
{
    public class NWDCookieString : NWDCookieDefinition
    {
        public NWDCookieString(string sName, string sTitle, string sDescription, NWDCookieDefinitionGroup sGroup, string sDefaultValue, bool sDeletable = true, bool sManualEditable = true, int sDuration = 365, bool sAutoRenew = false, bool sSecure = true, SameSiteMode sLimitSite = SameSiteMode.Strict)
        {
            Kind = NWDCookieDefinitionKind.StringKind;
            Name = sName;
            Title = sTitle;
            Explication = sDescription;
            Group = sGroup;
            Duration = sDuration;
            DefaultValue = sDefaultValue.Replace("'", "\'");
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
        public static NWDCookieString? GetCookieDefinition(string sName)
        {
            NWDCookieString? rReturn = null;
            if (NWDCookieGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDCookieString)NWDCookieGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public string GetValue(HttpContext? sHttpContext)
        {
            string rReturn = DefaultValue;
            string? tValue = _GetValue(sHttpContext);
            if (tValue != null)
            {
                rReturn = tValue;
            }
            return rReturn.Replace("\'", "'");
        }

        public void SetValue(HttpContext? sHttpContext, string? sValue)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = string.Empty;
            }
            _SetValue(sHttpContext, sValue.Replace("'", "\'"));
        }

        public string GenerateCookieDataString(string sValue)
        {
            return _GenerateCookieDataString(sValue);
        }
        public string GenerateOnClick(string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = string.Empty;
            }
            return _GenerateOnClick(sValue.Replace("'", "\'"));
        }

        public override string RawForm(HttpContext? sHttpContext)
        {
            string rReturn = "<!-- " + nameof(NWDCookieString) + " RawForm-->";
            if (ManualEditable)
            {
            }
            return rReturn;
        }
    }
}
