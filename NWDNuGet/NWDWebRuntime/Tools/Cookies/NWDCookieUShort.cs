using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Cookies
{
    public class NWDCookieUShort : NWDCookieDefinition
    {
        public NWDCookieUShort(string sName, string sTitle, string sDescription, NWDCookieDefinitionGroup sGroup, ushort sDefaultValue, bool sDeletable = true, bool sManualEditable = true, int sDuration = 365, bool sAutoRenew = false, bool sSecure = true, SameSiteMode sLimitSite = SameSiteMode.Strict)
        {
            Kind = NWDCookieDefinitionKind.UIntKind;
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
        public static NWDCookieUShort? GetCookieDefinition(string sName)
        {
            NWDCookieUShort? rReturn = null;
            if (NWDCookieGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDCookieUShort)NWDCookieGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public ushort GetValue(HttpContext? sHttpContext)
        {
            ushort rReturn = ushort.Parse(DefaultValue);
            string? tValue = _GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                ushort.TryParse(tValue, out rReturn);
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, uint sValue)
        {
            _SetValue(sHttpContext, sValue.ToString());
        }

        public string GenerateCookieDataString(uint sValue)
        {
            return _GenerateCookieDataString(sValue.ToString(CultureInfo.InvariantCulture));
        }
        public string GenerateOnClick(uint sValue)
        {
            return _GenerateOnClick(sValue.ToString(CultureInfo.InvariantCulture));
        }

        public override string RawForm(HttpContext? sHttpContext)
        {
            string rReturn = "<!-- " + nameof(NWDCookieUShort) + " RawForm-->";
            if (ManualEditable)
            {
            }
            return rReturn;
        }
    }
}
