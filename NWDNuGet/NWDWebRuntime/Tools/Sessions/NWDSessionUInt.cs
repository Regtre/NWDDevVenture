using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions
{
    public class NWDSessionUInt : NWDSessionDefinition
    {
        public NWDSessionUInt(string sName, string sTitle, string sDescription, NWDSessionDefinitionGroup sGroup, uint sDefaultValue, bool sDeletable = false, bool sManualEditable = false)
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

        public static NWDSessionUInt? GetSessionDefinition(string sName)
        {
            NWDSessionUInt? rReturn = null;
            if (NWDSessionGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDSessionUInt)NWDSessionGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        private uint GetValue(HttpContext? sHttpContext)
        {
            uint rReturn = uint.Parse(DefaultValue);
            string? tValue = _GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                uint.TryParse(tValue, out rReturn);
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, uint sValue)
        {
            _SetValue(sHttpContext, sValue.ToString(CultureInfo.InvariantCulture));
        }

        public void IncrementValue(HttpContext? sHttpContext, uint sIncrement = 1)
        {
            _SetValue(sHttpContext, (GetValue(sHttpContext) + sIncrement).ToString(CultureInfo.InvariantCulture));
        }
    }
}
