using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions
{
    public class NWDSessionInt : NWDSessionDefinition
    {
        public NWDSessionInt(string sName, string sTitle, string sDescription, NWDSessionDefinitionGroup sGroup, int sDefaultValue, bool sDeletable = false, bool sManualEditable = false)
        {
            Kind = NWDSessionDefinitionKind.IntKind;
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

        public static NWDSessionInt? GetSessionDefinition(string sName)
        {
            NWDSessionInt? rReturn = null;
            if (NWDSessionGlobal.KDictionary.ContainsKey(sName) == true)
            {
                rReturn = (NWDSessionInt)NWDSessionGlobal.KDictionary[sName];
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
            _SetValue(sHttpContext, sValue.ToString(CultureInfo.InvariantCulture));
        }

        public void IncrementValue(HttpContext? sHttpContext, int sIncrement = 1)
        {
            _SetValue(sHttpContext, (GetValue(sHttpContext) + sIncrement).ToString(CultureInfo.InvariantCulture));
        }
    }
}
