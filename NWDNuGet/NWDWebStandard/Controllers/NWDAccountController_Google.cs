using System.Web;
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
        // https://www.oauth.com/oauth2-servers/signing-in-with-google/authorization-request/
        // https://www.googleapis.com/oauth2/v4/token
        // https://www.googleapis.com/oauth2/v3/userinfo

        // https://www.oauth.com/oauth2-servers/signing-in-with-google/authorization-request/

        public static string GoogleOAuth_Redirection()
        {
            if (string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.GoogleClientId) == false && string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.GoogleClientSecret) == false)
            {
                return NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/" + ASP_Controller() + "/" + nameof(GoogleRedirect) + "/";
            }
            else
            {
                return "#";
            }
        }

        public static string GoogleOAuth_URL()
        {
            return "https://accounts.google.com/o/oauth2/v2/auth?" +
                   "response_type=code" +
                   "&client_id=" + NWDWebStandardConfiguration.KConfig.GoogleClientId +
                   "&scope=openid" +
                   //"&state=134589779878" +
                   "&redirect_uri=" + HttpUtility.UrlEncode(GoogleOAuth_Redirection()) +
                   "";
        }

        public async Task<IActionResult> GoogleRedirect(string state, string code) // don't change name!
        {
            try
            {
                UriBuilder tUri = new UriBuilder("https://www.googleapis.com/oauth2/v4/token");
                Dictionary<string, string> tRequestValues = new Dictionary<string, string>()
                {
                    { "code", code },
                    { "client_id", NWDWebStandardConfiguration.KConfig.GoogleClientId },
                    { "client_secret", NWDWebStandardConfiguration.KConfig.GoogleClientSecret },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", GoogleOAuth_Redirection() },
                };
                FormUrlEncodedContent tFormContent = new FormUrlEncodedContent(tRequestValues);
                HttpResponseMessage tResponse = await NWDWebRuntimeConfiguration.HttpClientShared.PostAsync(tUri.ToString(), tFormContent);
                string tResponseString = await tResponse.Content.ReadAsStringAsync();

                try
                {
                    string? tAccessToken = JObject.Parse(tResponseString)["id_token"]?.ToString();
                    UriBuilder tUriUser = new UriBuilder("https://www.googleapis.com/oauth2/v2/tokeninfo");
                    if (tAccessToken != null)
                    {
                        Dictionary<string, string> tRequestValuesUser = new Dictionary<string, string>()
                        {
                            { "id_token", tAccessToken },
                        };
                        FormUrlEncodedContent tFormContentUser = new FormUrlEncodedContent(tRequestValuesUser);
                        HttpResponseMessage tResponseUser = await NWDWebRuntimeConfiguration.HttpClientShared.PostAsync(tUriUser.ToString(), tFormContentUser);
                        string tResponseStringUser = await tResponseUser.Content.ReadAsStringAsync();

                        string? tTrustedId = JObject.Parse(tResponseStringUser)["user_id"]?.ToString();

                        if (string.IsNullOrEmpty(tTrustedId) == false)
                        {
                                NWDAccountSign tSign = NWDAccountSign.CreateGoogle(tTrustedId, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
                                UseSocialSign(tSign);
                            return View(nameof(System.Index));
                        }
                        else
                        {
                            AddActualToast(NWDToastStandard.CreateToast(NWDWebNotificationLayoutStyle.PopUp, NWDBootstrapKindOfStyle.Danger, "Token error", "Token is null."));
                        }
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