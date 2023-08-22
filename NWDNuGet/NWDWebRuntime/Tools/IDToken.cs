// using System.Text;
// using Microsoft.AspNetCore.Http;
//
// namespace NWDWebRuntime.Tools
// {
//     public static class IDToken
//     {
//         static string kIdTokenKey = "lknShdegEzvjVUGrkIOeyfOgzBa";
//         static int kUniqueCounter = 0;
//         static int kLength = 24;
//
//         public static string GetLast(HttpContext sHttpContext)
//         {
//             return sHttpContext.Session.GetString(kIdTokenKey);
//         }
//
//         public static string NewOne(HttpContext sHttpContext)
//         {
//             kUniqueCounter++;
//             if (kUniqueCounter < 1)
//             {
//                 kUniqueCounter = 1;
//             }
//             StringBuilder rReturn = new StringBuilder();
//             const string tChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
//             int tCharLenght = tChars.Length;
//             Random tRand = new Random();
//             rReturn.Append(tChars[kUniqueCounter % tCharLenght]);
//             while (rReturn.Length < kLength)
//             {
//                 int tR = tRand.Next(0, tCharLenght);
//                 rReturn.Append(tChars[tR]);
//             }
//             string rReturnString = rReturn.ToString();
//             sHttpContext.Session.SetString(kIdTokenKey, rReturnString);
//             return rReturnString;
//         }
//     }
// }