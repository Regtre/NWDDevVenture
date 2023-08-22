using Microsoft.AspNetCore.Http;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDWebRuntime.CallBacks;

namespace NWDWebRuntime.Managers
{
    public static class NWDAccountWebManager
    {
        public static bool DeleteAccount(HttpContext sHttpContext)
        {
            bool rReturn = true;
            // TODO
            return rReturn;
        }
        // public static bool DeleteAccountInContext(HttpContext sHttpContext)
        // {
        //     bool rReturn = false;
        //     DeleteAccountToken(sHttpContext);
        //     NWDWebRuntimeStartupService.CookieAccount.DeleteCookie(sHttpContext);
        //     NWDWebRuntimeStartupService.SessionAccount.DeleteFrom(sHttpContext);
        //     return rReturn;
        // }

        public static bool AccountIsConnected(HttpContext? sHttpContext)
        {
            // TODO : Remove this method to call directly NWDWebRuntimeCallbackServers ?
             return NWDWebRuntimeCallbackServers.AccountIsConnected(sHttpContext).Result;
            //return NWDWebRuntimeCallbackServers.AccountIsConnected(sHttpContext);
        } 
        public static void AccountSignOut(HttpContext? sHttpContext)
        {
            // TODO : Remove this method to call directly NWDWebRuntimeCallbackServers ?
            NWDWebRuntimeCallbackServers.DeleteRequestPlayerToken(sHttpContext);
        }

        // public static void SetAccountSignInContext(NWDAccountSign sAccountSign, HttpContext sHttpContext)
        // {
        //     NWDAccount tAccount = new NWDAccount() { Reference = sAccountSign.Account };
        //     SetAccountInContext(tAccount, sHttpContext);
        // }
        //
        // public static void SetAccountInContext(NWDAccount sAccount, HttpContext sHttpContext)
        // {
        //     NWDWebRuntimeStartupService.SessionAccount.SetValue(sHttpContext, sAccount.Reference);
        // }
        
        // public static void RememberAccountInCookie(NWDAccountSign sAccountSign, HttpContext sHttpContext)
        // {
        //     NWDAccount tAccount = new NWDAccount() { Reference = sAccountSign.Account };
        //     RememberAccountInCookie(tAccount, sHttpContext);
        // }
        //
        // public static void RememberAccountInCookie(NWDAccount sAccount, HttpContext sHttpContext)
        // {
        //     // TODO
        //     if (sAccount.Reference!=0)
        //     {
        //         //NWDLogger.WriteLine(" Token cookie is reccord");
        //         // NWDWebRuntimeStartupService.CookieAccount.SetValue(sHttpContext, tToken.TokenCookie);
        //         //NWDLogger.WriteLine(" Token cookie is reccord in cookie");
        //     }
        // }

        public static ushort GetDataTrack(HttpContext? sHttpContext) // TODO : Where get Data Track ? 
        {
            NWDRequestPlayerToken tPlayerToken = NWDWebRuntimeCallbackServers.GetRequestPlayerToken(sHttpContext);
            return 0     ;
        }
        public static NWDAccount GetAccountInContext(HttpContext? sHttpContext)
        {
            NWDRequestPlayerToken tPlayerToken = NWDWebRuntimeCallbackServers.GetRequestPlayerToken(sHttpContext);
            return new NWDAccount(){Reference = tPlayerToken.PlayerReference};
        }

        // public static NWDAccount RestaureAccountFromCookie(HttpContext sHttpContext)
        // {
        //     NWDAccount sAccount = new NWDAccount();
        //     if (NWDWebRuntimeStartupService.CookieAccount.Exists(sHttpContext))
        //     {
        //         ulong sToken = NWDWebRuntimeStartupService.CookieAccount.GetValue(sHttpContext);
        //         sAccount.Reference = sToken;
        //
        //         // List<AccountTokenCookie> tTokenList = SQLToolbox.GetBy<AccountTokenCookie>(new Dictionary<string, string>() {
        //         //     { nameof(AccountTokenCookie.TokenCookie), sToken },
        //         //     { nameof(AccountTokenCookie.Domain) , ((int) AccountTokenCookieDomain.Web).ToString()}
        //         // });
        //         // if (tTokenList.Count == 1)
        //         // {
        //         //     NWDWebRuntimeStartupService.SessionAccount.SetValue(sHttpContext, tTokenList[0].Account.Reference);
        //         //     RememberAccountInCookie(sHttpContext);
        //         // }
        //         // else
        //         // {
        //         //     NWDWebRuntimeStartupService.CookieAccount.GetValue(sHttpContext);
        //         // }
        //         // foreach (AccountTokenCookie tC in tTokenList)
        //         // {
        //         //     tC.Delete();
        //         // }
        //     }
        //     else
        //     {
        //     }
        //     return sAccount;
        // }
        
        // public static void DeleteAccountToken(HttpContext sHttpContext)
        // {
        //     // TODO 
        //     // string sAccount = GetAccountInContext(sHttpContext);
        //     // List<AccountTokenCookie> tTokenList = SQLToolbox.GetBy<AccountTokenCookie>(new Dictionary<string, string>() {
        //     //     { nameof(AccountTokenCookie.Account), sAccount },
        //     //     { nameof(AccountTokenCookie.Domain) , ((int) AccountTokenCookieDomain.Web).ToString()}
        //     // });
        //     // foreach (AccountTokenCookie tC in tTokenList)
        //     // {
        //     //     tC.Delete();
        //     // }
        // }
        
        // public static void CheckUnicity(HttpContext sHttpContext)
        // {
        //     // TODO
        //     
        //     
        //     // string rReturn = GetAccountInContext(sHttpContext);
        //     // if (string.IsNullOrEmpty(rReturn) == false)
        //     // {
        //     //     List<AccountTokenUnique> tTokenList = SQLToolbox.GetBy<AccountTokenUnique>(new Dictionary<string, string>() { { nameof(AccountTokenUnique.Account), rReturn } });
        //     //     if (tTokenList.Count == 0)
        //     //     {
        //     //         //NWDLogger.WriteLine(" no token for you ... create one");
        //     //         AccountTokenUnique tN = new AccountTokenUnique();
        //     //         tN.Account = new NWDSQLReference<NWDSQLAccount>(rReturn);
        //     //         tN.Token = NWDWebRuntimeStartupService.SessionCookie.GetValue(sHttpContext);
        //     //         tN.Reccord();
        //     //     }
        //     //     else if (tTokenList.Count == 1)
        //     //     {
        //     //         AccountTokenUnique tN = tTokenList[0];
        //     //         if (tN.Token == NWDWebRuntimeStartupService.SessionCookie.GetValue(sHttpContext))
        //     //         {
        //     //             //NWDLogger.WriteLine(" You are alone! good");
        //     //         }
        //     //         else
        //     //         {
        //     //             //NWDLogger.WriteLine(" not your session BAD!");
        //     //             DeleteAccountInContext(sHttpContext);
        //     //             tN.Delete();
        //     //         }
        //     //     }
        //     //     else
        //     //     {
        //     //         //NWDLogger.WriteLine(" too much session");
        //     //         DeleteAccountInContext(sHttpContext);
        //     //         foreach (AccountTokenUnique tC in tTokenList)
        //     //         {
        //     //             tC.Delete();
        //     //         }
        //     //     }
        //     // }
        // }
    }
}