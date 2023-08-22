using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDRuntime.Exchanges;
using NWDWebRuntime.Back;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;

namespace NWDWebRuntime.CallBacks;

public static class NWDWebRuntimeCallbackServers
{
    // public static async Task<bool> AccountIsConnected(HttpContext? sHttpContext)        
    public static async Task<bool> AccountIsConnected(HttpContext? sHttpContext)
    {
        bool rReturn = false;
        if (sHttpContext != null)
        {
            if (NWDWebRuntimeStartupService.SessionAccount.GetValue(sHttpContext) != 0)
            {
                //  NWDLogger.WriteLine(nameof(AccountIsConnected) + " by Session");
                rReturn = true;
            }
            else
            {
                if (NWDWebRuntimeStartupService.SessionFromCookie.GetValue(sHttpContext) == false)
                {
                    NWDWebRuntimeStartupService.SessionFromCookie.SetValue(sHttpContext, true);
                    // test if cookies can restore session 
                    if (NWDWebRuntimeStartupService.CookieAccountRemember.GetValue(sHttpContext))
                    {
                        NWDRequestPlayerToken tRequestPlayerToken =
                            new NWDRequestPlayerToken(NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment());
                        tRequestPlayerToken.PlayerReference = NWDWebRuntimeStartupService.CookieAccount.GetValue(sHttpContext);
                        tRequestPlayerToken.AccountRange = NWDWebRuntimeStartupService.CookieAccountRange.GetValue(sHttpContext);
                        tRequestPlayerToken.Token = NWDWebRuntimeStartupService.CookieAccountToken.GetValue(sHttpContext);
                        tRequestPlayerToken.EnvironmentKind = NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment();
                        tRequestPlayerToken.ExchangeOrigin = NWDExchangeOrigin.Web;
                        NWDResponseRuntime tResponseRuntime = await PostRequest(NWDRequestRuntime.CreateRequestTest(NWDWebRuntimeConfiguration.KConfig, tRequestPlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web), sHttpContext, true);

                        if (tResponseRuntime.PlayerToken?.PlayerReference == NWDWebRuntimeStartupService.CookieAccount.GetValue(sHttpContext)
                            && tResponseRuntime.PlayerToken.AccountRange == NWDWebRuntimeStartupService.CookieAccountRange.GetValue(sHttpContext))
                        {
                            rReturn = true;
                        }
                    }
                }
            }
        }

        return rReturn;
    }

    public static void SetRequestPlayerToken(NWDRequestPlayerToken? sRequestPlayerToken, HttpContext? sHttpContext, bool sCookieRemember)
    {
        if (sHttpContext != null)
        {
            if (sRequestPlayerToken != null)
            {
                NWDWebRuntimeStartupService.SessionAccount.SetValue(sHttpContext, sRequestPlayerToken.PlayerReference);
                NWDWebRuntimeStartupService.SessionAccountRange.SetValue(sHttpContext, sRequestPlayerToken.AccountRange);
                NWDWebRuntimeStartupService.SessionAccountToken.SetValue(sHttpContext, sRequestPlayerToken.Token);
                if (sCookieRemember)
                {
                    NWDWebRuntimeStartupService.CookieAccount.SetValue(sHttpContext, sRequestPlayerToken.PlayerReference);
                    NWDWebRuntimeStartupService.CookieAccountRange.SetValue(sHttpContext, sRequestPlayerToken.AccountRange);
                    NWDWebRuntimeStartupService.CookieAccountToken.SetValue(sHttpContext, sRequestPlayerToken.Token);
                    NWDWebRuntimeStartupService.CookieAccountRemember.SetValue(sHttpContext, true);
                }
                else
                {
                    if (NWDWebRuntimeStartupService.CookieAccountRemember.GetValue(sHttpContext))
                    {
                        NWDWebRuntimeStartupService.CookieAccount.SetValue(sHttpContext, sRequestPlayerToken.PlayerReference);
                        NWDWebRuntimeStartupService.CookieAccountRange.SetValue(sHttpContext, sRequestPlayerToken.AccountRange);
                        NWDWebRuntimeStartupService.CookieAccountToken.SetValue(sHttpContext, sRequestPlayerToken.Token);
                    }
                }
            }
            else
            {
                //NWDLogger.Critical("Player Token is null");
                if (string.IsNullOrEmpty(NWDWebRuntimeStartupService.SessionAccountToken.GetValue(sHttpContext)) == false)
                {
                    NWDLogger.Critical("Player Token is invalid");
                    NWDLogger.Critical("I NEED TO LOGOUT THIS GUY!");
                    DeleteRequestPlayerToken(sHttpContext);
                    NWDLogger.Critical("YOU'RE LOGOUT?");
                    NWDLogger.Critical("I NEED TO REDIRECT TO ERROR PAGE!");
                }
            }
        }
    }

    public static NWDRequestPlayerToken GetRequestPlayerToken(HttpContext? sHttpContext)
    {
        NWDRequestPlayerToken tRequestPlayerToken = new NWDRequestPlayerToken(NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment());
        if (sHttpContext != null)
        {
            tRequestPlayerToken.PlayerReference = NWDWebRuntimeStartupService.SessionAccount.GetValue(sHttpContext);
            tRequestPlayerToken.AccountRange = NWDWebRuntimeStartupService.SessionAccountRange.GetValue(sHttpContext);
            tRequestPlayerToken.Token = NWDWebRuntimeStartupService.SessionAccountToken.GetValue(sHttpContext);
            tRequestPlayerToken.ExchangeOrigin = NWDExchangeOrigin.Web;
            // if (NWDWebRuntimeStartupService.CookieAccountRemember.GetValue(sHttpContext))
            // {
            //     tRequestPlayerToken.PlayerReference = NWDWebRuntimeStartupService.CookieAccount.GetValue(sHttpContext);
            //     tRequestPlayerToken.AccountRange = NWDWebRuntimeStartupService.CookieAccountRange.GetValue(sHttpContext);
            //     tRequestPlayerToken.Token = NWDWebRuntimeStartupService.CookieAccountToken.GetValue(sHttpContext);
            // }
        }

        return tRequestPlayerToken;
    }

    public static void DeleteRequestPlayerToken(HttpContext? sHttpContext)
    {
        //NWDLogger.Critical(nameof(DeleteRequestPlayerToken) + "(); NOW!");
        // flush my data in memory ... 
        NWDWebDataManager.FlushAllForAccount(sHttpContext);
        // stop to remember me
        NWDWebRuntimeStartupService.CookieAccountRemember.SetValue(sHttpContext, false);
        //reset to empty value
        NWDWebRuntimeStartupService.SessionAccount.SetValue(sHttpContext, 0);
        NWDWebRuntimeStartupService.SessionAccountRange.SetValue(sHttpContext, 0);
        NWDWebRuntimeStartupService.SessionAccountToken.SetValue(sHttpContext, string.Empty);
        NWDWebRuntimeStartupService.CookieAccount.SetValue(sHttpContext, 0);
        NWDWebRuntimeStartupService.CookieAccountToken.SetValue(sHttpContext, string.Empty);
        // try to delete 
        NWDWebRuntimeStartupService.SessionAccount.DeleteFrom(sHttpContext);
        NWDWebRuntimeStartupService.SessionAccountRange.DeleteFrom(sHttpContext);
        NWDWebRuntimeStartupService.SessionAccountToken.DeleteFrom(sHttpContext);
        NWDWebRuntimeStartupService.CookieAccount.DeleteCookie(sHttpContext);
        NWDWebRuntimeStartupService.CookieAccountToken.DeleteCookie(sHttpContext);
        // destroy all service
        NWDAccountServiceWebManager.FlushAllService(sHttpContext);
    }

    public static async Task<NWDResponseRuntime> PostRequest(NWDRequestRuntime sRequest, HttpContext? sHttpContext, bool sCookieRemember = false)
    {
        NWDResponseRuntime rReturn;
        string tResquestJson = JsonConvert.SerializeObject(sRequest);
        string tUrl = NWDWebRuntimeConfiguration.KConfig.GetBestUrlForServer() + "/NWDRuntime/";
        try
        {
            HttpClient tHttpClient = new HttpClient();
            HttpRequestMessage tHttpRequest = new HttpRequestMessage(HttpMethod.Post, tUrl);
            tHttpRequest.Content = new StringContent(tResquestJson, Encoding.UTF8, "application/json");
            HttpResponseMessage tResponse = await tHttpClient.SendAsync(tHttpRequest);
            using (StreamReader tStreamReader = new StreamReader(await tResponse.Content.ReadAsStreamAsync()))
            {
                string tResponseJson = await tStreamReader.ReadToEndAsync();
                rReturn = JsonConvert.DeserializeObject<NWDResponseRuntime>(tResponseJson)!;
            }

            if (rReturn != null && rReturn.IsValid(NWDWebRuntimeConfiguration.KConfig))
            {
                NWDSync.SetPlayerLastSyncInformation(sHttpContext, rReturn.PlayerToken.PlayerSyncInformation);
                NWDSync.SetStudioLastSyncInformation(sHttpContext, rReturn.PlayerToken.StudioSyncInformation);
                NWDAccountServiceWebManager.RegisterService(rReturn, sHttpContext);
            }
            else
            {
                rReturn = new NWDResponseRuntime() { Status = NWDRequestStatus.HashInvalid };
            }
        }
        catch (Exception)
        {
            rReturn = new NWDResponseRuntime
            {
                Status = NWDRequestStatus.NoNetwork
            };
        }
        if (rReturn.Status != NWDRequestStatus.Ok)
        {
            NWDLogger.TraceError(rReturn.Status.ToString());
            DeleteRequestPlayerToken(sHttpContext);
        }
        else
        {
            SetRequestPlayerToken(rReturn.PlayerToken, sHttpContext, sCookieRemember);
        }

        rReturn.Logger(sRequest.Logger(tUrl));
        return rReturn;
    }
}