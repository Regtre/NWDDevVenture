using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions
{
    public class NWDSessionFloat : NWDSessionDefinition
    {
        private const string K_RecordFormat = "0.00000";
        public NWDSessionFloat(string sName, string sTitle, string sDescription, NWDSessionDefinitionGroup sGroup, float sDefaultValue, bool sDeletable = false, bool sManualEditable = false)
        {
            Kind = NWDSessionDefinitionKind.FloatKind;
            Name = sName;
            Title = sTitle;
            Explication = sDescription;
            Group = sGroup;
            DefaultValue = sDefaultValue.ToString(K_RecordFormat,CultureInfo.InvariantCulture);
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

        public static NWDSessionFloat? GetSessionDefinition(string sName)
        {
            NWDSessionFloat? rReturn = null;
            if (NWDSessionGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDSessionFloat)NWDSessionGlobal.KDictionary[sName];
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
            _SetValue(sHttpContext, sValue.ToString(K_RecordFormat,CultureInfo.InvariantCulture));
        }

        public void IncrementValue(HttpContext? sHttpContext, float sIncrement = 1F)
        {
            _SetValue(sHttpContext, (GetValue(sHttpContext) + sIncrement).ToString(K_RecordFormat,CultureInfo.InvariantCulture));
        }
    }
}
