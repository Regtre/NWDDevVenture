using System.Text;
using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDRuntime.Exchanges;
using NWDWebRuntime.Configuration;

namespace NWDWebRuntime.CallBacks
{
    public static class NWDProjectLicenseCallbackServers
    {
        public static async Task<NWDResponseLicense?> PostRequest(NWDRequestLicense sRequest)
        {
            NWDResponseLicense? rReturn;
            try
            {
                string tUrl = NWDWebRuntimeConfiguration.KConfig.GetHubDnsHttps()+"/NWDHubLicense/";
                string tResquestJson = JsonConvert.SerializeObject(sRequest);
                // TODO : use servers definition to select one server!
                // use IP to be nearest the user
                HttpClient tHttpClient = new HttpClient();
                HttpRequestMessage tHttpRequest = new HttpRequestMessage(HttpMethod.Post, tUrl);
                tHttpRequest.Content = new StringContent(tResquestJson, Encoding.UTF8, "application/json");
                HttpResponseMessage tResponse = await tHttpClient.SendAsync(tHttpRequest);
                using (StreamReader tStreamReader = new StreamReader(await tResponse.Content.ReadAsStreamAsync()))
                {
                    string tResponseJson = await tStreamReader.ReadToEndAsync();
                    rReturn = JsonConvert.DeserializeObject<NWDResponseLicense>(tResponseJson)!;
                }
            }
            catch (Exception)
            {
                rReturn = new NWDResponseLicense
                {
                    Status = NWDRequestStatus.NoNetwork
                };
            }

            if (rReturn == null)
            {
                rReturn = new NWDResponseLicense()
                {
                    Status = NWDRequestStatus.Error,
                };
            }
            return rReturn;
        }
    }
}