using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Cookies
{
    public class NWDCookieInt : NWDCookieDefinition
    {
        public NWDCookieInt(string sName, string sTitle, string sDescription, NWDCookieDefinitionGroup sGroup, int sDefaultValue, bool sDeletable = true, bool sManualEditable = true, int sDuration = 365, bool sAutoRenew = false, bool sSecure = true, SameSiteMode sLimitSite = SameSiteMode.Strict)
        {
            Kind = NWDCookieDefinitionKind.IntKind;
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
        public static NWDCookieInt? GetCookieDefinition(string sName)
        {
            NWDCookieInt? rReturn = null;
            if (NWDCookieGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDCookieInt)NWDCookieGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public int GetValue(HttpContext? sHttpContext)
        {
            int rReturn = int.Parse(DefaultValue);
            string? tValue = _GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                int.TryParse(tValue, out rReturn);
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, int sValue)
        {
            _SetValue(sHttpContext, sValue.ToString());
        }

        public string GenerateCookieDataString(int sValue)
        {
            return _GenerateCookieDataString(sValue.ToString(CultureInfo.InvariantCulture));
        }
        public string GenerateOnClick(int sValue)
        {
            return _GenerateOnClick(sValue.ToString(CultureInfo.InvariantCulture));
        }

        public override string RawForm(HttpContext? sHttpContext)
        {
            string rReturn = "<!-- " + nameof(NWDCookieInt) + " RawForm-->";
            if (ManualEditable)
            {
            }
            return rReturn;
        }
    }
}
