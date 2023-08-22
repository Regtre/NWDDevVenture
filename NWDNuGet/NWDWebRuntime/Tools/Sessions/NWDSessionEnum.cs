using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions
{
    public class NWDSessionEnum<T> : NWDSessionDefinition where T : Enum
    {
        public Type EnumType;

        public NWDSessionEnum(string sName, string sTitle, string sDescription, NWDSessionDefinitionGroup sGroup, T sDefaultValue, bool sDeletable = false, bool sManualEditable = false)
        {
            EnumType = typeof(T);
            Kind = NWDSessionDefinitionKind.EnumKind;
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

        public static NWDSessionEnum<T>? GetSessionDefinition(string sName)
        {
            NWDSessionEnum<T>? rReturn = null;
            if (NWDSessionGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDSessionEnum<T>)NWDSessionGlobal.KDictionary[sName];
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
    }
}
