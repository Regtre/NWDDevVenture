using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Cookies
{
    public class NWDCookieEnum<T> : NWDCookieDefinition where T : Enum
    {
        private Type _EnumType;
        public NWDCookieEnum(string sName, string sTitle, string sDescription, NWDCookieDefinitionGroup sGroup, T sDefaultValue, bool sDeletable = true, bool sManualEditable = true, int sDuration = 365, bool sAutoRenew = false, bool sSecure = true, SameSiteMode sLimitSite = SameSiteMode.Strict)
        {
            _EnumType = typeof(T);
            Kind = NWDCookieDefinitionKind.EnumKind;
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
        public static NWDCookieEnum<T>? GetCookieDefinition(string sName)
        {
            NWDCookieEnum<T>? rReturn = null;
            if (NWDCookieGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDCookieEnum<T>)NWDCookieGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public T GetValue(HttpContext? sHttpContext)
        {
            T rReturn = (T)Enum.Parse(typeof(T), DefaultValue);
            string? tValue = _GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                rReturn = (T)Enum.Parse(typeof(T), tValue);
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, T sValue)
        {
            _SetValue(sHttpContext, sValue.ToString());
        }

        public string GenerateCookieDataString(T sValue)
        {
            return _GenerateCookieDataString(sValue.ToString());
        }
        public string GenerateOnClick(T sValue)
        {
            return _GenerateOnClick(sValue.ToString());
        }

        public override string RawForm(HttpContext? sHttpContext)
        {

            string tReturnJs = "document.cookie = '" + Name + "='+this.value+'; SameSite=" + LimitSite.ToString() + "; Path=/; expires=" + DateTime.UtcNow.AddDays(Duration).ToString("ddd, dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture) + " GMT";
#if !DEBUG
            if (Secure == true)
            {
                tReturnJs = tReturnJs + "; Secure";
            }
#endif
            tReturnJs = tReturnJs + "';";


            string rReturn = "<!-- " + nameof(NWDCookieEnum<T>) + " RawForm-->";
            if (ManualEditable)
            {
                T tValue = GetValue(sHttpContext);
                rReturn = rReturn + "<select class=\"btn-primary form-select form-select-lg mb-3\" onchange=\"" + tReturnJs + ";window.location.reload();\"> ";
                foreach (T tEnum in Enum.GetValues(typeof(T)))
                {
                    if (tValue.ToString() == tEnum.ToString())
                    {
                        rReturn = rReturn + "<option value=\"" + tEnum.ToString() + "\" selected>" + tEnum.ToString() + "</option>";
                    }
                    else
                    {
                        rReturn = rReturn + "<option value=\"" + tEnum.ToString() + "\">" + tEnum.ToString() + "</option>";
                    }
                }
                rReturn = rReturn + "</select>";
            }
            return rReturn;
        }
    }
}
