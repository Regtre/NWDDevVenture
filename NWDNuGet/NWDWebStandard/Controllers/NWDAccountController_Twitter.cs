using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NWDFoundation.Configuration;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;
using NWDWebStandard.Configuration;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace NWDWebStandard.Controllers 
{
    public partial class NWDAccountController
    {
        // https://github.com/kg0r0/twitter-oauth2
        
        // https://developer.twitter.com/en/support/twitter-api/error-troubleshooting

        public static string TwitterOAuth_Redirection()
        {
            if (string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.TwitterClientId) == false && string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.TwitterClientSecret) == false)
            {
                return NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/" + ASP_Controller() + "/" + nameof(TwitterRedirect) + "/";
            }
            else
            {
                return "#";
            }
        }

        public static string TwitterOAuth_URL()
        {
            // https://twitter.com/i/oauth2/authorize?response_type=code&client_id=SXYyc0xpRnNrd09Jc2RUVFhvLVE6MTpjaQ&redirect_uri=https://127.0.0.1:5001/Home/TwitterRedirect&scope=tweet.read%20users.read%20follows.read%20follows.write&state=state&code_challenge=challengegzqerggqgrGRzZE&code_challenge_method=plain
            return "https://twitter.com/i/oauth2/authorize?" +
                "response_type=code" +
                "&client_id=" + NWDWebStandardConfiguration.KConfig.TwitterClientId +
                //"&scope=tweet.read%20users.read%20follows.read%20follows.write" +
                "&scope=offline.access" +
                "&redirect_uri=" + TwitterOAuth_Redirection() +
                //"&grant_type=authorization_code" +
                "&state=state" +
                "&code_challenge=challenge" +
                "&code_challenge_method=plain";
        }
        
        [Serializable]
        private class TwitterReq
        {
            public string code { set; get; } = string.Empty;
           public string client_type { set; get; } = "service_client";
           public string scope { set; get; } = "offline.access";
           //public string client_id { set; get; } = NWDWebRuntimeConfiguration.KConfig.TwitterClientId;
           public string client_secret { set; get; } = NWDWebStandardConfiguration.KConfig.TwitterClientSecret;
           public string grant_type { set; get; } = "client_credentials";
           public string redirect_uri { set; get; } = TwitterOAuth_Redirection();
        }
        public async Task<IActionResult> TwitterRedirect(string state, string code) // don't change name!
        {
           
            try
            {
                 TwitterReq tTwitterReq = new TwitterReq() { code = code};
                HttpRequestMessage tHttpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/2/oauth2/token");
                string tResquestJson = JsonConvert.SerializeObject(tTwitterReq);
                tHttpRequest.Content = new StringContent(tResquestJson, Encoding.UTF8,"application/json");
                tHttpRequest.Headers.Add("Authorization", "Basic "+NWDSecurityTools.Base64Encode(NWDWebStandardConfiguration.KConfig.TwitterClientId+":"+NWDWebStandardConfiguration.KConfig.TwitterClientSecret));
                
                HttpResponseMessage tResponse = await NWDWebRuntimeConfiguration.HttpClientShared.SendAsync(tHttpRequest);
                string tResponseString = await tResponse.Content.ReadAsStringAsync();
                
                // UriBuilder tUri = new UriBuilder("https://api.twitter.com/2/oauth2/token");
                // TwitterReq tTwitterReq = new TwitterReq() { code = code};
                // string tResquestJson = JsonConvert.SerializeObject(tTwitterReq);
                // HttpClient tHttpClient = new HttpClient();
                // HttpRequestMessage tHttpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/2/oauth2/token");
                //
                // //tHttpRequest.Content = new StringContent(tResquestJson, Encoding.UTF8, "application/x-www-form-urlencoded");
                // Dictionary<string, string> tRequestValues = new Dictionary<string, string>()
                // {
                //     { "code", code},
                //     { "client_type", "public"},
                //     { "scope","offline.access" },
                //     { "client_id", NWDWebRuntimeConfiguration.KConfig.TwitterClientId},
                //     { "client_secret", NWDWebRuntimeConfiguration.KConfig.TwitterClientSecret},
                //     { "grant_type", "client_credentials" },
                //     { "redirect_uri", TwitterOAuth_Redirection() },
                // };
                // FormUrlEncodedContent tForm = new FormUrlEncodedContent(tRequestValues);
                // string authHeaderFormat = "Basic {0}";
                // string authHeader = string.Format(authHeaderFormat,
                //     Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(NWDWebRuntimeConfiguration.KConfig.TwitterClientId) + ":" +
                //                                                   Uri.EscapeDataString((NWDWebRuntimeConfiguration.KConfig.TwitterClientSecret)))
                //     ));
                // tHttpRequest.Headers.Add("Authorization", authHeader);
                //
                // tHttpClient.DefaultRequestHeaders.Authorization = authHeader;
                // HttpResponseMessage tResponse = await tHttpClient.SendAsync(tHttpRequest);
                //
                // string tResponseString = await tResponse.Content.ReadAsStringAsync();
                //
                //
                //
                // foreach (KeyValuePair<string, string> tKeyValue in tRequestValues)
                // {
                //     tQuery.Set(tKeyValue.Key, tKeyValue.Value);
                // }
                // tUri.Query = tQuery.ToString();
                // HttpResponseMessage tResponse = await NWDWebRuntimeConfiguration.HttpClientShared.GetAsync(tUri.ToString());
                // string tResponseString = await tResponse.Content.ReadAsStringAsync();
                //
                //
                // var authHeaderFormat = "Basic {0}";
                // var authHeader = string.Format(authHeaderFormat,
                //     Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(NWDWebRuntimeConfiguration.KConfig.TwitterClientId) + ":" +
                //                                                   Uri.EscapeDataString((NWDWebRuntimeConfiguration.KConfig.TwitterClientSecret)))
                //     ));
                //
                // var postBody = "grant_type=client_credentials";
                //
                // HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create("https://api.twitter.com/2/oauth2/token");
                // authRequest.Headers.Add("Authorization", authHeader);
                // authRequest.Method = "POST";
                // authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                // authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                //
                // using (Stream stream = authRequest.GetRequestStream())
                // {
                //     byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                //     stream.Write(content, 0, content.Length);
                // }
                //
                // authRequest.Headers.Add("Accept-Encoding", "gzip");
                // WebResponse authResponse = authRequest.GetResponse();
                //
                //
                // FormUrlEncodedContent tFormContent = new FormUrlEncodedContent(tRequestValues);
                // //tFormContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8" );
                // // tFormContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded;charset=UTF-8");
                // //tFormContent.Headers.ContentType = "application/x-www-form-urlencoded;charset=UTF-8"
                //     
                // string authHeaderFormat = "Basic {0}";
                // string authHeader = string.Format(authHeaderFormat,
                //     Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(NWDWebRuntimeConfiguration.KConfig.TwitterClientId) + ":" +
                //                                                   Uri.EscapeDataString((NWDWebRuntimeConfiguration.KConfig.TwitterClientSecret)))
                //     ));
                //
                // tFormContent.Headers.Add("Authorization", authHeader);
                // HttpResponseMessage tResponse = await NWDWebRuntimeConfiguration.HttpClientShared.PostAsync(tUri.ToString(), tFormContent);
                // string tResponseString = await tResponse.Content.ReadAsStringAsync();

                try
                {
                    string? tAccessToken = JObject.Parse(tResponseString)["access_token"]?.ToString();
                    UriBuilder tUriUser = new UriBuilder("https://api.Twitter.com/v2/me");
                    if (tAccessToken != null)
                    {
                        Dictionary<string, string> tRequestValuesUser = new Dictionary<string, string>()
                        {
                            { "fields", "id"},
                            { "oauth2_access_token", tAccessToken},
                        };
                        NameValueCollection tQueryUser = HttpUtility.ParseQueryString(string.Empty);
                        foreach (KeyValuePair<string, string> tKeyValue in tRequestValuesUser)
                        {
                            tQueryUser.Set(tKeyValue.Key, tKeyValue.Value);
                        }
                        tUriUser.Query = tQueryUser.ToString();
                    }

                    HttpResponseMessage tResponseUser = await NWDWebRuntimeConfiguration.HttpClientShared.GetAsync(tUriUser.ToString());
                    string tResponseStringUser = await tResponseUser.Content.ReadAsStringAsync();

                    string? tTrustedId = JObject.Parse(tResponseStringUser)["id"]?.ToString();

                    if (string.IsNullOrEmpty(tTrustedId) == false)
                    {
                            NWDAccountSign tSign = NWDAccountSign.CreateTwitter(tTrustedId, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                            UseSocialSign(tSign);

                        return View(nameof(System.Index));
                    }
                    else
                    {
                        AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Token error", "Token is null."));
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