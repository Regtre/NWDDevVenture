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
    public partial class NWDAccountController : NWDBasicController<NWDWebStandard.Controllers.NWDAccountController>
    {
        public static string FacebookOAuth_Redirection()
        {
            if (string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.FacebookClientId) == false && string.IsNullOrEmpty(NWDWebStandardConfiguration.KConfig.FacebookClientSecret) == false)
            {
                return NWDWebRuntimeConfiguration.KConfig.GetDnsHttps() + "/" + ASP_Controller() + "/" + nameof(FacebookRedirect) + "/";
            }
            else
            {
                return "#";
            }
        }

        public static string FacebookOAuth_URL()
        {
            return "https://www.facebook.com/v14.0/dialog/oauth?" +
                "response_type=code" +
                "&client_id=" + NWDWebStandardConfiguration.KConfig.FacebookClientId +
                //"&state={st=state123abc,ds=123456789}" +
                "&redirect_uri=" + HttpUtility.UrlEncode(FacebookOAuth_Redirection()) +
                "";
        }

        public async Task<IActionResult> FacebookRedirect(string state, string code) // don't change name!
        {
            try
            {
                UriBuilder tURI = new UriBuilder("https://graph.facebook.com/oauth/access_token");
                Dictionary<string, string> tRequestValues = new Dictionary<string, string>()
                    {
                        { "code", code},
                        { "client_id", NWDWebStandardConfiguration.KConfig.FacebookClientId},
                        { "client_secret", NWDWebStandardConfiguration.KConfig.FacebookClientSecret},
                        { "fields", "id" },
                        { "redirect_uri", FacebookOAuth_Redirection() },
                    };
                NameValueCollection tQuery = HttpUtility.ParseQueryString(string.Empty);
                foreach (KeyValuePair<string, string> tKeyValue in tRequestValues)
                {
                    tQuery.Set(tKeyValue.Key, tKeyValue.Value);
                }
                tURI.Query = tQuery.ToString();
                HttpResponseMessage tResponse = await NWDWebRuntimeConfiguration.HttpClientShared.GetAsync(tURI.ToString());
                string tResponseString = await tResponse.Content.ReadAsStringAsync();

                try
                {
                    string? tAccessToken = JObject.Parse(tResponseString)["access_token"]?.ToString();
                    UriBuilder tUriUser = new UriBuilder("https://graph.facebook.com/me");
                    if (tAccessToken != null)
                    {
                        Dictionary<string, string> tRequestValuesUser = new Dictionary<string, string>()
                        {
                            { "fields", "id"},
                            { "access_token", tAccessToken},
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
                            NWDAccountSign tSign = NWDAccountSign.CreateFacebook(tTrustedId, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
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