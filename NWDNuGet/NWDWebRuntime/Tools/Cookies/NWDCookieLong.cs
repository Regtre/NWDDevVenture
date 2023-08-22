using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Cookies
{
    public class NWDCookieLong : NWDCookieDefinition
    {
        public NWDCookieLong(string sName, string sTitle, string sDescription, NWDCookieDefinitionGroup sGroup, long sDefaultValue, bool sDeletable = true, bool sManualEditable = true, int sDuration = 365, bool sAutoRenew = false, bool sSecure = true, SameSiteMode sLimitSite = SameSiteMode.Strict)
        {
            Kind = NWDCookieDefinitionKind.LongKind;
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
        public static NWDCookieLong? GetCookieDefinition(string sName)
        {
            NWDCookieLong? rReturn = null;
            if (NWDCookieGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDCookieLong)NWDCookieGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public long GetValue(HttpContext? sHttpContext)
        {
            long rReturn = long.Parse(DefaultValue);
            string? tValue = _GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                long.TryParse(tValue, out rReturn);
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, long sValue)
        {
            _SetValue(sHttpContext, sValue.ToString(CultureInfo.InvariantCulture));
        }

        public string GenerateCookieDataString(long sValue)
        {
            return _GenerateCookieDataString(sValue.ToString(CultureInfo.InvariantCulture));
        }
        public string GenerateOnClick(long sValue)
        {
            return _GenerateOnClick(sValue.ToString());
        }

        public override string RawForm(HttpContext? sHttpContext)
        {
            string rReturn = "<!-- " + nameof(NWDCookieLong) + " RawForm-->";
            if (ManualEditable)
            {
            }
            return rReturn;
        }
    }
}
