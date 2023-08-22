using System.Web;
using Microsoft.AspNetCore.Http;
using NWDFoundation.Configuration;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDWebRuntime.CallBacks;
using NWDWebRuntime.Configuration;

namespace NWDWebRuntime.Managers
{
    public static class NWDAccountSignWebManager
    {
        public static NWDResponseRuntime TrySignUp(NWDAccountSign sAccountSign, HttpContext? sHttpContext, bool sRememberInCookie)
        {
            NWDResponseRuntime? rReturn =
                NWDWebRuntimeCallbackServers.PostRequest(NWDRequestRuntime.CreateRequestSignUp(NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeCallbackServers.GetRequestPlayerToken(sHttpContext), sAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web), sHttpContext,
                    sRememberInCookie).Result;
            if (rReturn != null)
            {
                // NWDDownPayloadAccountSignUp tDownPayload = rReturn.GetPayload<NWDDownPayloadAccountSignUp>();
            }
            else
            {
                rReturn = new NWDResponseRuntime() { Status = NWDRequestStatus.Error };
            }

            return rReturn;
        }

        public static NWDResponseRuntime TrySignIn(NWDAccountSign sAccountSign, HttpContext? sHttpContext, bool sRememberInCookie)
        {
            NWDResponseRuntime? rReturn =
                NWDWebRuntimeCallbackServers.PostRequest(NWDRequestRuntime.CreateRequestSignIn(NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeCallbackServers.GetRequestPlayerToken(sHttpContext), sAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web), sHttpContext,
                    sRememberInCookie).Result;
            if (rReturn != null)
            {
                // NWDDownPayloadAccountSignUp tDownPayload = rReturn.GetPayload<NWDDownPayloadAccountSignUp>();
                //NWDWebDataManager.SyncPlayerData(sHttpContext);
                NWDWebDataManager.FastSync(sHttpContext);
            }
            else
            {
                rReturn = new NWDResponseRuntime() { Status = NWDRequestStatus.Error };
            }

            return rReturn;
        }

        public static List<NWDAccountSign> GetAccountSigns(HttpContext sHttpContext)
        {
            List<NWDAccountSign> rReturn = new List<NWDAccountSign>();
            NWDResponseRuntime? tResponse =
                NWDWebRuntimeCallbackServers.PostRequest(NWDRequestRuntime.
                    CreateRequestGetAllSign(NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeCallbackServers.
                        GetRequestPlayerToken(sHttpContext), NWDExchangeOrigin.Web, NWDExchangeDevice.Web), sHttpContext).Result;
            if (tResponse != null)
            {
                if (tResponse.Status != NWDRequestStatus.Error)
                {
                    NWDDownPayloadAccountSignAll tDownPayload = tResponse.GetPayload<NWDDownPayloadAccountSignAll>(NWDWebRuntimeConfiguration.KConfig);
                    if (tDownPayload != null)
                    {
                        if (tDownPayload.AccountSignList != null)
                        {
                            rReturn = tDownPayload.AccountSignList;
                        }
                    }
                }
            }
            return rReturn;
        }

        public static string GetAccountByEmail(string sEmail)
        {
            string rReturn = "0";
            // TODO
            return rReturn;
        }

        public static NWDAccountSign GetAccountSignByReference(ulong sReference)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            // TODO
            return rReturn;
        }

        public static List<NWDAccountSign> GetAccountSignHash(string sSignHas)
        {
            List<NWDAccountSign> rReturn = new List<NWDAccountSign>();

            // TODO
            return rReturn;
        }

        public static NWDResponseRuntime Delete(NWDAccountSign sSign, HttpContext? sHttpContext)
        {
            NWDResponseRuntime? rResponse = NWDWebRuntimeCallbackServers.
                PostRequest(NWDRequestRuntime.
                    CreateRequestSignDelete(NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeCallbackServers.
                        GetRequestPlayerToken(sHttpContext), sSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web), sHttpContext).Result;
            if (rResponse != null)
            {
                // NWDDownPayloadAccountSignDelete tDownPayload = rResponse.GetPayload<NWDDownPayloadAccountSignDelete>();
            }
            else
            {
                rResponse = new NWDResponseRuntime() { Status = NWDRequestStatus.Error };
            }

            return rResponse;
        }

        public static NWDResponseRuntime TryToAdd(NWDAccountSign sSign, HttpContext? sHttpContext)
        {
            NWDResponseRuntime? rResponse = NWDWebRuntimeCallbackServers.PostRequest(NWDRequestRuntime.
                CreateRequestSignAdd(NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeCallbackServers.
                    GetRequestPlayerToken(sHttpContext), sSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web), sHttpContext).Result;
            if (rResponse != null)
            {
                // NWDDownPayloadAccountSignAdd tDownPayload = rResponse.GetPayload<NWDDownPayloadAccountSignAdd>();
            }
            else
            {
                rResponse = new NWDResponseRuntime() { Status = NWDRequestStatus.Error };
            }

            return rResponse;
        }

        public static NWDResponseRuntime TryToModify(HttpContext sHttpContext, NWDAccountSign sOldSign, NWDAccountSign sNewSign)
        {
            NWDResponseRuntime? rResponse = 
                NWDWebRuntimeCallbackServers.PostRequest(NWDRequestRuntime.
                    CreateRequestSignModify(NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeCallbackServers.
                        GetRequestPlayerToken(sHttpContext), sOldSign, sNewSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web), sHttpContext).Result;
            if (rResponse != null)
            {
                // NWDDownPayloadAccountSignModify tDownPayload = rResponse.GetPayload<NWDDownPayloadAccountSignModify>();
            }
            else
            {
                rResponse = new NWDResponseRuntime() { Status = NWDRequestStatus.Error };
            }
            return rResponse;
        }

        public static bool TestIfSignHashExists(NWDAccountSign sSign, HttpContext sHttpContext, bool sBool = true)
        {
            bool rReturn = true;
            // TODO
            return rReturn;
        }
        public static string LostAccountSign(string sEmail, HttpContext sHttpContext, string sEmailPasswordUrl, string sLoginEmailPasswordUrl)
        {
            string rReturn = string.Empty;
            NWDResponseRuntime? rResponse = NWDWebRuntimeCallbackServers.PostRequest(NWDRequestRuntime.CreateRequestSignLost(NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeCallbackServers.GetRequestPlayerToken(sHttpContext),sEmail,NWDExchangeOrigin.Web, NWDExchangeDevice.Web), sHttpContext).Result;
            if (rResponse.Status == NWDRequestStatus.Ok)
            {
                NWDDownPayloadAccountSignLost tInfos = rResponse.GetPayload<NWDDownPayloadAccountSignLost>(NWDWebRuntimeConfiguration.KConfig);
                string tTokenDecrypt = NWDSecurityTools.DecryptSomething(tInfos.RescueTokenSecured, NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment() ,NWDWebRuntimeConfiguration.KConfig,  NWDWebRuntimeConfiguration.KConfig);
                string tEmailSecure = NWDSecurityTools.CryptSomething(sEmail, NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment() ,NWDWebRuntimeConfiguration.KConfig,  NWDWebRuntimeConfiguration.KConfig);
                string tTokenSecure = NWDSecurityTools.CryptAes(tTokenDecrypt, sEmail, NWDWebRuntimeConfiguration.KConfig.GetProjectId().ToString());
                if (tInfos.SignType == NWDAccountSignType.EmailPassword)
                {
                    rReturn = sEmailPasswordUrl;
                }
                else if (tInfos.SignType == NWDAccountSignType.LoginEmailPassword)
                {
                    rReturn = sLoginEmailPasswordUrl;
                }
                rReturn+= "?email="+HttpUtility.UrlEncode(tEmailSecure)+"&token="+HttpUtility.UrlEncode(tTokenSecure)+"&limit="+HttpUtility.UrlEncode(tInfos.Limit.ToString())+"&sign="+HttpUtility.UrlEncode(((int)tInfos.SignType).ToString());
            }
            return rReturn;
        }

        public static bool RescueAccountSignTestToken(NWDAccountSign sSign, HttpContext sHttpContext)
        {
            bool rReturn = false;
            NWDResponseRuntime? rResponse = NWDWebRuntimeCallbackServers.PostRequest(NWDRequestRuntime.CreateRequestSignRescue(NWDWebRuntimeConfiguration.KConfig,NWDWebRuntimeCallbackServers.GetRequestPlayerToken(sHttpContext),sSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web), sHttpContext).Result;
            if (rResponse.Status == NWDRequestStatus.Ok)
            {
                rReturn = true;
            }
            return rReturn;
        }

        public static bool RescueAccountSign(NWDAccountSign sSign, HttpContext sHttpContext)
        {
            bool rReturn = true;
            // TODO
            return rReturn;
        }
    }
}