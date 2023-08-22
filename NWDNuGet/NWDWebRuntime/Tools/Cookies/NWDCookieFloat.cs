using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Cookies
{
    public class NWDCookieFloat : NWDCookieDefinition
    {
        private const string K_RecordFormat = "0.00000";
        
        public NWDCookieFloat(string sName, string sTitle, string sDescription, NWDCookieDefinitionGroup sGroup, float sDefaultValue, bool sDeletable = true, bool sManualEditable = true, int sDuration = 365, bool sAutoRenew = false, bool sSecure = true, SameSiteMode sLimitSite = SameSiteMode.Strict)
        {
            Kind = NWDCookieDefinitionKind.FloatKind;
            Name = sName;
            Title = sTitle;
            Explication = sDescription;
            Group = sGroup;
            Duration = sDuration;
            DefaultValue = sDefaultValue.ToString(K_RecordFormat, CultureInfo.InvariantCulture);
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
        public static NWDCookieFloat? GetCookieDefinition(string sName)
        {
            NWDCookieFloat? rReturn = null;
            if (NWDCookieGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDCookieFloat)NWDCookieGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public float GetValue(HttpContext? sHttpContext)
        {
            float rReturn = float.Parse(DefaultValue);
            string? tValue = _GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                float.TryParse(tValue, out rReturn);
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, float sValue)
        {
            _SetValue(sHttpContext, sValue.ToString(K_RecordFormat, CultureInfo.InvariantCulture));
        }

        public string GenerateCookieDataString(float sValue)
        {
            return _GenerateCookieDataString(sValue.ToString(K_RecordFormat, CultureInfo.InvariantCulture));
        }
        public string GenerateOnClick(float sValue)
        {
            return _GenerateOnClick(sValue.ToString(K_RecordFormat, CultureInfo.InvariantCulture));
        }

        public override string RawForm(HttpContext? sHttpContext)
        {
            string rReturn = "<!-- " + nameof(NWDCookieFloat) + " RawForm-->";
            if (ManualEditable)
            {
            }
            return rReturn;
        }
    }
}
