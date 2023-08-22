using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NWDFoundation.Configuration;
using NWDFoundation.Models;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Configuration;

namespace NWDWebStandard.Controllers
{
    public partial class NWDAccountController
    {
        //https://docs.microsoft.com/fr-fr/azure/active-directory/develop/web-app-quickstart?pivots=devlang-aspnet-core
        public static string MicrosoftOAuth_Redirection()
        {
            if (string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.MicrosoftClientId) == false &&
                string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.MicrosoftClientSecret) == false)
            {
                return NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/" + ASP_Controller() + "/" +
                       nameof(MicrosoftRedirect);
            }
            else
            {
                return "#";
            }
        }

        public static string MicrosoftOAuth_URL()
        {
            return "https://login.microsoftonline.com/common/oauth2/v2.0/authorize?" +
                   "client_id=" + NWDWebStandardConfiguration.KConfig.MicrosoftClientId +
                   "&client_secret=" + NWDWebStandardConfiguration.KConfig.MicrosoftClientSecret +
                   "&response_type=code" +
                   "&scope=openid" +
                   "&response_mode=query";
        }

        public async Task<IActionResult> MicrosoftRedirect(string code) // don't change name!
        {
            try
            {
                using HttpRequestMessage tRequestMessage =
                    new HttpRequestMessage(HttpMethod.Post,
                        "https://login.microsoftonline.com/common/oauth2/v2.0/token");
                Dictionary<string, string> tRequestValues = new Dictionary<string, string>()
                {
                    { "code", code },
                    { "client_id", NWDWebStandardConfiguration.KConfig.MicrosoftClientId },
                    { "client_secret", NWDWebStandardConfiguration.KConfig.MicrosoftClientSecret },
                    { "grant_type", "authorization_code" },
                    { "scope", "https://graph.microsoft.com/.default" },
                    { "redirect_uri", MicrosoftOAuth_Redirection() },
                };
                
                tRequestMessage.Content = new FormUrlEncodedContent(tRequestValues);
                
                HttpResponseMessage tResponse =
                    await NWDWebRuntimeConfiguration.HttpClientShared.SendAsync(tRequestMessage);
                string tResponseString = await tResponse.Content.ReadAsStringAsync();

                try
                {
                    string? tAccessToken = JObject.Parse(tResponseString)["access_token"]?.ToString();
                    if (!string.IsNullOrEmpty(tAccessToken))
                    {
                        HttpClient tHttpClient = new HttpClient();
                        using HttpRequestMessage tRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
                        
                        tRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tAccessToken);
                        
                        HttpResponseMessage tHttpResponseMessage = await tHttpClient.SendAsync(tRequest);

                        string tResponseStringUser = await tHttpResponseMessage.Content.ReadAsStringAsync();
                        string? tTrustedId = JObject.Parse(tResponseStringUser)["id"]?.ToString();

                        if (string.IsNullOrEmpty(tTrustedId) == false)
                        {
                                NWDAccountSign tSign = NWDAccountSign.CreateMicrosoft(tTrustedId, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                                UseSocialSign(tSign);
                            return View(nameof(System.Index));
                        }
                        else
                        {
                            AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger,
                                "Token error", "Token is null."));
                        }
                    }
                }
                catch
                {
                    AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger,
                        "Token error", "Token is false."));
                }
            }
            catch
            {
                AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger,
                    "Code error", "Code is false."));
            }
            //AddTempDataObject(PageInformation);
            return RedirectToAction(nameof(Error));
        }
    }
}