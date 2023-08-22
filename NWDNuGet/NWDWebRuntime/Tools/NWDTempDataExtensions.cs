#define JSON_SYSTEM
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#if JSON_SYSTEM
#else
using Newtonsoft.Json;
#endif

namespace NWDWebRuntime.Tools
{
    public static class NWDTempDataExtensions
    {
        public const string K_KeyObjects = "****";

        public static bool HasObject(this ITempDataDictionary sTempData, Type sType)
        {
            string tKey = K_KeyObjects + sType.Name;
            return sTempData.ContainsKey(tKey);
        }

        public static bool HasObject<T>(this ITempDataDictionary sTempData)
        {
            string tKey = K_KeyObjects + typeof(T).Name;
            return sTempData.ContainsKey(tKey);
        }

        public static void PutObject<T>(this ITempDataDictionary sTempData, T sValue) where T : class
        {
            string tKey = K_KeyObjects + typeof(T).Name;
#if JSON_SYSTEM
            string tValue = JsonSerializer.Serialize(sValue);
#else
            string tValue = JsonConvert.SerializeObject(sValue);
#endif
            sTempData[tKey] = tValue;
        }

        public static void InsertObject(this ITempDataDictionary sTempData, object sValue)
        {
            string tKey = K_KeyObjects + sValue.GetType().Name;
#if JSON_SYSTEM
            string tValue = JsonSerializer.Serialize(sValue);
#else
            string tValue = JsonConvert.SerializeObject(sValue);
#endif
            sTempData[tKey] = tValue;
        }

        public static T? GetObject<T>(this ITempDataDictionary sTempData, bool sDispose = true) where T : class
        {
            string tKey = K_KeyObjects + typeof(T).Name;
            sTempData.TryGetValue(tKey, out object? tValue);
            if (sDispose)
            {
                sTempData.Remove(tKey);
            }
#if JSON_SYSTEM
            return tValue == null ? null : JsonSerializer.Deserialize<T>((string)tValue);
#else
            return tValue == null ? null : JsonConvert.DeserializeObject<T>((string)tValue);
#endif
        }

        public static T? PeekObject<T>(this ITempDataDictionary sTempData, bool sDispose = true) where T : class
        {
            string tKey = K_KeyObjects + typeof(T).Name;
            object? tValue = sTempData.Peek(tKey);
            if (sDispose)
            {
                sTempData.Remove(tKey);
            }
#if JSON_SYSTEM
           return tValue == null ? null : JsonSerializer.Deserialize<T>((string)tValue);
#else
            return tValue == null ? null : JsonConvert.DeserializeObject<T>((string)tValue);
#endif
        }

        public static void Put<T>(this ITempDataDictionary sTempData, string sKey, T sValue) where T : class
        {
            string tKey = K_KeyObjects + sKey;
#if JSON_SYSTEM
            sTempData[tKey] = JsonSerializer.Serialize(sValue);
#else
            sTempData[tKey] = JsonConvert.SerializeObject(sValue);
#endif
        }

        public static T? Get<T>(this ITempDataDictionary sTempData, string sKey) where T : class
        {
            string tKey = K_KeyObjects + sKey;
            if (sTempData.TryGetValue(tKey, out object? tValue))
            {
#if JSON_SYSTEM
                return tValue == null ? null : JsonSerializer.Deserialize<T>((string)tValue);
#else
                return tValue == null ? null : JsonConvert.DeserializeObject<T>((string)tValue);
#endif
            }
            else
            {
                return null;
            }
        }

        public static T? Peek<T>(this ITempDataDictionary sTempData, string sKey) where T : class
        {
            string tKey = K_KeyObjects + sKey;
            object? tValue = sTempData.Peek(tKey);
#if JSON_SYSTEM
            return tValue == null ? null : JsonSerializer.Deserialize<T>((string)tValue);
#else
            return tValue == null ? null : JsonConvert.DeserializeObject<T>((string)tValue);
#endif
        }
    }
}
