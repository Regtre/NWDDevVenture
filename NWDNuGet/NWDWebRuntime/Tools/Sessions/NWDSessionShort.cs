using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions
{
    public class NWDSessionShort : NWDSessionDefinition
    {
        public NWDSessionShort(string sName, string sTitle, string sDescription, NWDSessionDefinitionGroup sGroup, short sDefaultValue, bool sDeletable = false, bool sManualEditable = false)
        {
            Kind = NWDSessionDefinitionKind.UIntKind;
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

        public static NWDSessionShort? GetSessionDefinition(string sName)
        {
            NWDSessionShort? rReturn = null;
            if (NWDSessionGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDSessionShort)NWDSessionGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        private short GetValue(HttpContext? sHttpContext)
        {
            short rReturn = short.Parse(DefaultValue);
            string? tValue = _GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                short.TryParse(tValue, out rReturn);
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, short sValue)
        {
            _SetValue(sHttpContext, sValue.ToString(CultureInfo.InvariantCulture));
        }

        public void IncrementValue(HttpContext? sHttpContext, short sIncrement = 1)
        {
            _SetValue(sHttpContext, (GetValue(sHttpContext) + sIncrement).ToString(CultureInfo.InvariantCulture));
        }
    }
}
