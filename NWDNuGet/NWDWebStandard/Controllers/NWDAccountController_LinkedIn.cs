using System.Collections.Specialized;
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
        public static string LinkedInOAuth_Redirection()
        {
            if (string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.LinkedInClientId) == false && string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.LinkedInClientSecret) == false)
            {
                return NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/" + ASP_Controller() + "/" + nameof(LinkedInRedirect) + "/";
            }
            else
            {
                return "#";
            }
        }

        public static string LinkedInOAuth_URL()
        {
            //< a  href = "https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=78v7uh5jskk04m&redirect_uri=https://127.0.0.1:5001/Home/LinkedInRedirect&scope=r_liteprofile%20r_emailaddress" >
            return "https://www.linkedin.com/oauth/v2/authorization?" +
                "response_type=code" +
                "&client_id=" + NWDWebStandardConfiguration.KConfig.LinkedInClientId +
                "&scope=r_liteprofile" +
                "&redirect_uri=" + LinkedInOAuth_Redirection() +
                "&grant_type=authorization_code" +
                "";
        }

        public async Task<IActionResult> LinkedInRedirect(string state, string code) // don't change name!
        {
            try
            {
                UriBuilder tUri = new UriBuilder("https://www.linkedin.com/oauth/v2/accessToken");
                Dictionary<string, string> tRequestValues = new Dictionary<string, string>()
                    {
                        { "code", code},
                        { "client_id", NWDWebStandardConfiguration.KConfig.LinkedInClientId},
                        { "client_secret", NWDWebStandardConfiguration.KConfig.LinkedInClientSecret},
                        { "grant_type", "authorization_code" },
                        { "redirect_uri", LinkedInOAuth_Redirection() },
                    };
                NameValueCollection tQuery = HttpUtility.ParseQueryString(string.Empty);
                foreach (KeyValuePair<string, string> tKeyValue in tRequestValues)
                {
                    tQuery.Set(tKeyValue.Key, tKeyValue.Value);
                }
                tUri.Query = tQuery.ToString();
                HttpResponseMessage tResponse = await NWDWebRuntimeConfiguration.HttpClientShared.GetAsync(tUri.ToString());
                string tResponseString = await tResponse.Content.ReadAsStringAsync();

                try
                {
                    string? tAccessToken = JObject.Parse(tResponseString)["access_token"]?.ToString();
                    UriBuilder tUriUser = new UriBuilder("https://api.linkedin.com/v2/me");
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
                            NWDAccountSign tSign = NWDAccountSign.CreateLinkedIn(tTrustedId, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
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