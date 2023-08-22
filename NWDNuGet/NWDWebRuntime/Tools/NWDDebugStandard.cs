// using System.Reflection;
//
// namespace NWDWebRuntime.Tools
// {
//     public class NWDDebugStandard
//     {
//         private static readonly Dictionary<string, string> DebugDictionary = new Dictionary<string, string>();
//
//         public static void AddDebugTrace(MethodBase sMethodBase, string sAddOn = "")
//         {
//             if (sMethodBase.ReflectedType != null)
//             {
//                 AddDebug(sMethodBase.ReflectedType.Name, sMethodBase.Name + "() " + sAddOn);
//             }
//         }
//
//         public static void AddDebug(string sKey, string sValue)
//         {
//             string tKey = DateTime.Now.ToShortTimeString() + " : " + sKey;
// #if DEBUG
//             if (DebugDictionary.ContainsKey(tKey) == false)
//             {
//                 DebugDictionary.Add(tKey, sValue);
//             }
//             else
//             {
//                 AddDebug(sKey + " ", sValue);
//             }
// #endif
//         }
//
//         public static void Clear()
//         {
//             NWDLogger.WriteLine(""+nameof(NWDDebugStandard)+" "+nameof(Clear)+"()!");
//             DebugDictionary.Clear();
//         }
//
//         public static Dictionary<string, string> Debug()
//         {
//             return DebugDictionary;
//         }
//
//         public static bool ShowDebug()
//         {
// #if DEBUG
//             return true;
// #else
//             return false;
// #endif
//         }
//     }
// }
