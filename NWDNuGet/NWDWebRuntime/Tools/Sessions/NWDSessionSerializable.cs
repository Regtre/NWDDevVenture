using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NWDFoundation.Facades;

namespace NWDWebRuntime.Tools.Sessions
{
    public class NWDSessionSerializable<T> : NWDSessionDefinition  where T : INWDSerializable
    {
        public NWDSessionSerializable(string sName, string sTitle, string sDescription, NWDSessionDefinitionGroup sGroup, T sDefaultValue, bool sDeletable = false, bool sManualEditable = false)
        {
            Kind = NWDSessionDefinitionKind.StringKind;
            Name = sName;
            Title = sTitle;
            Explication = sDescription;
            Group = sGroup;
            if (sDefaultValue != null)
            {
                string tValue = JsonConvert.SerializeObject(sDefaultValue);
                DefaultValue = tValue.Replace("'", "\'");
            }
            else
            {
                string tValue = JsonConvert.SerializeObject(default(T));
                DefaultValue = tValue.Replace("'", "\'");
            }
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

        public static NWDSessionSerializable<T>? GetSessionDefinition(string sName)
        {
            NWDSessionSerializable<T>? rReturn = null;
            if (NWDSessionGlobal.KDictionary.ContainsKey(sName))
            {
                rReturn = (NWDSessionSerializable<T>)NWDSessionGlobal.KDictionary[sName];
            }
            return rReturn;
        }

        public T GetValue(HttpContext? sHttpContext)
        {
            T rReturn = default(T);
            string? tValue = _GetValue(sHttpContext);
            if (tValue != null)
            {
                rReturn = JsonConvert.DeserializeObject<T>(tValue.Replace("\'", "'"));
            }
            return rReturn;
        }

        public void SetValue(HttpContext? sHttpContext, T? sValue)
        {
            if (sValue != null)
            {
                string tValue = JsonConvert.SerializeObject(sValue);
                _SetValue(sHttpContext, tValue.Replace("'", "\'"));
            }
        }

    }
}
