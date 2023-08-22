using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions
{
    public class NWDSessionBool : NWDSessionDefinition
    {
        public NWDSessionBool(string sName, string sTitle, string sDescription, NWDSessionDefinitionGroup sGroup, bool sDefaultValue, bool sDeletable = false, bool sManualEditable = false)
        {
            Kind = NWDSessionDefinitionKind.BoolKind;
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

        public static NWDSessionBool? GetSessionDefinition(string sName)
        {
            NWDSessionBool? rReturn = null;
            if (NWDSessionGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDSessionBool)NWDSessionGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public bool GetValue(HttpContext? sHttpContext)
        {
            bool rReturn = bool.Parse(DefaultValue);
            string? tValue = _GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                bool.TryParse(tValue, out rReturn);
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, bool sValue)
        {
            _SetValue(sHttpContext, sValue.ToString());
        }
    }
}
