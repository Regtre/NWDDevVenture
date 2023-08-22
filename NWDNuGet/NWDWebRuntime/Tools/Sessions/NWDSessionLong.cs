using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions
{
    public class NWDSessionLong : NWDSessionDefinition
    {
        public NWDSessionLong(string sName, string sTitle, string sDescription, NWDSessionDefinitionGroup sGroup, long sDefaultValue, bool sDeletable = false, bool sManualEditable = false)
        {
            Kind = NWDSessionDefinitionKind.LongKind;
            Name = sName;
            Title = sTitle;
            Explication = sDescription;
            Group = sGroup;
            DefaultValue = sDefaultValue.ToString();
            Deletable = sDeletable;
            ManualEditable = sManualEditable;
            if (NWDSessionGlobal.KDictionary.ContainsKey(Name))
            {
                NWDSessionGlobal.KDictionary[Name] = this;
            }
            else
            {
                NWDSessionGlobal.KDictionary.Add(Name, this);
            }
        }

        public static NWDSessionLong? GetSessionDefinition(string sName)
        {
            NWDSessionLong? rReturn = null;
            if (NWDSessionGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDSessionLong)NWDSessionGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        private long GetValue(HttpContext? sHttpContext)
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

        public void IncrementValue(HttpContext? sHttpContext, long sIncrement = 1)
        {
            _SetValue(sHttpContext, (GetValue(sHttpContext) + sIncrement).ToString(CultureInfo.InvariantCulture));
        }
    }
}
