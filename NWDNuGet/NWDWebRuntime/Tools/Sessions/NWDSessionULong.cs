using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions
{
    public class NWDSessionULong : NWDSessionDefinition
    {
        public NWDSessionULong(string sName, string sTitle, string sDescription, NWDSessionDefinitionGroup sGroup, long sDefaultValue, bool sDeletable = false, bool sManualEditable = false)
        {
            Kind = NWDSessionDefinitionKind.ULongKind;
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

        public static NWDSessionULong? GetSessionDefinition(string sName)
        {
            NWDSessionULong? rReturn = null;
            if (NWDSessionGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDSessionULong)NWDSessionGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public ulong GetValue(HttpContext? sHttpContext)
        {
            ulong rReturn = ulong.Parse(DefaultValue);
            string? tValue = _GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                ulong.TryParse(tValue, out rReturn);
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, ulong sValue)
        {
            _SetValue(sHttpContext, sValue.ToString(CultureInfo.InvariantCulture));
        }

        public void IncrementValue(HttpContext? sHttpContext, ulong sIncrement = 1)
        {
            _SetValue(sHttpContext, (GetValue(sHttpContext) + sIncrement).ToString(CultureInfo.InvariantCulture));
        }
    }
}