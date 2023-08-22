using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions
{
    public class NWDSessionString : NWDSessionDefinition
    {
        public NWDSessionString(string sName, string sTitle, string sDescription, NWDSessionDefinitionGroup sGroup, string sDefaultValue, bool sDeletable = false, bool sManualEditable = false)
        {
            Kind = NWDSessionDefinitionKind.StringKind;
            Name = sName;
            Title = sTitle;
            Explication = sDescription;
            Group = sGroup;
            DefaultValue = sDefaultValue.Replace("'", "\'");
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

        public static NWDSessionString? GetSessionDefinition(string sName)
        {
            NWDSessionString? rReturn = null;
            if (NWDSessionGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDSessionString)NWDSessionGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public string GetValue(HttpContext? sHttpContext)
        {
            string rReturn = DefaultValue;
            string? tValue = _GetValue(sHttpContext);
            if (tValue != null)
            {
                rReturn = tValue;
            }
            return rReturn.Replace("\'", "'");
        }

        public void SetValue(HttpContext? sHttpContext, string? sValue)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = string.Empty;
            }
            _SetValue(sHttpContext, sValue.Replace("'", "\'"));
        }

    }
}
