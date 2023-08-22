using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NWDFoundation.Configuration;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Configuration;

// https://discord.com/developers/docs/topics/oauth2
namespace NWDWebStandard.Controllers
{
    public partial class NWDAccountController : NWDBasicController<NWDWebStandard.Controllers.NWDAccountController>
    {
        public static string DiscordOAuth_Redirection()
        {
            if (string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.DiscordClientId) == false && string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.DiscordClientSecret) == false)
            {
                return NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/" + ASP_Controller() + "/" + nameof(DiscordRedirect) + "/";
            }
            else
            {
                return "#";
            }
        }

        public static string DiscordOAuth_URL()
        {
            //  https://discord.com/oauth2/authorize?response_type=code&client_id=157730590492196864&scope=identify%20guilds.join&state=15773059ghq9183habn&redirect_uri=https%3A%2F%2Fnicememe.website&prompt=consent

            https: //discord.com/api/oauth2/authorize?client_id=1110163489428877355&redirect_uri=https%3A%2F%2Flocalhost%3A2051%2FNWDAccount%2FDiscordRedirect%2F&response_type=code&scope=identify

            string rReturn = "https://discord.com/oauth2/authorize?" +
                             "response_type=code" +
                             "&client_id=" + NWDWebStandardConfiguration.KConfig.DiscordClientId +
                             "&scope=identify" +
                             "&redirect_uri=" + HttpUtility.UrlEncode(DiscordOAuth_Redirection()) +
                             "";
            //NWDLogger.Critical(nameof(DiscordOAuth_URL), rReturn);
            return rReturn;
        }

        public async Task<IActionResult> DiscordRedirect(string state, string code) // don't change name!
        {
            //NWDLogger.Information(nameof(DiscordRedirect), "state = " + state + " / code = " + code);
            try
            {
                Dictionary<string, string> tRequestValues = new Dictionary<string, string>()
                {
                    { "code", code },
                    { "client_id", NWDWebStandardConfiguration.KConfig.DiscordClientId },
                    { "client_secret", NWDWebStandardConfiguration.KConfig.DiscordClientSecret },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", DiscordOAuth_Redirection() },
                };
                FormUrlEncodedContent tFormContent = new FormUrlEncodedContent(tRequestValues);
                HttpResponseMessage tResponse = await NWDWebRuntimeConfiguration.HttpClientShared.PostAsync("https://discord.com/api/oauth2/token", tFormContent);
                string tResponseString = await tResponse.Content.ReadAsStringAsync();
                //NWDLogger.Critical(nameof(DiscordRedirect) + " Basic Auth", tResponseString);
                try
                {
                    string? tAccessToken = JObject.Parse(tResponseString)["access_token"]?.ToString();
                    string? tRefresh_token = JObject.Parse(tResponseString)["refresh_token"]?.ToString();

                    if (tAccessToken != null && tRefresh_token != null)
                    {
                        NWDLogger.Critical(nameof(DiscordRedirect), "tAccessToken = " + tAccessToken);

                        HttpRequestMessage tRequest = new HttpRequestMessage()
                        {
                            RequestUri = new Uri("https://discordapp.com/api/users/@me"),
                            Method = HttpMethod.Get,
                        };
                        tRequest.Headers.Add("Authorization", "Bearer " + tAccessToken);


                        HttpResponseMessage tResponseUser = await NWDWebRuntimeConfiguration.HttpClientShared.SendAsync(tRequest);

                        string tResponseStringUser = await tResponseUser.Content.ReadAsStringAsync();

                        //NWDLogger.Critical(nameof(DiscordRedirect) + " Final Auth", tResponseStringUser);

                        string? tTrustedId = JObject.Parse(tResponseStringUser)["id"]?.ToString();

                        if (string.IsNullOrEmpty(tTrustedId) == false)
                        {
                            //NWDLogger.Critical(nameof(DiscordRedirect), "client_id = " + tTrustedId);
                                NWDAccountSign tSign = NWDAccountSign.CreateDiscord(tTrustedId, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                                UseSocialSign(tSign);
                            return View(nameof(System.Index));
                        }
                        else
                        {
                            AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Token error", "Final Token is null."));
                        }
                    }
                    else
                    {
                        AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Token error", "Initial Token is null."));
                    }
                }
                catch
                {
                    AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Token error", "Token is false."));
                }
            }
            catch
            {
                AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Code error", "Code is false."));
            }

            //AddTempDataObject(PageInformation);
            return RedirectToAction(nameof(Error));
        }
    }
}