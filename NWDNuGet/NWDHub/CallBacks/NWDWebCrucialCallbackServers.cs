using System.Text;
using Newtonsoft.Json;
using NWDCrucial.Exchanges;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebRuntime.Configuration;

namespace NWDHub.Managers
{
    public static class NWDWebCrucialCallbackServers
    {
        public static async Task<NWDResponseCrucial?> PostRequest(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial? rReturn;
            try
            {
                string tResquestJson = JsonConvert.SerializeObject(sRequest);
                string tUrl = NWDWebRuntimeConfiguration.KConfig.GetBestUrlForServer() + "/NWDCrucial/";
                HttpClient tHttpClient = new HttpClient();
                HttpRequestMessage tHttpRequest = new HttpRequestMessage(HttpMethod.Post, tUrl);
                tHttpRequest.Content = new StringContent(tResquestJson, Encoding.UTF8, "application/json");
                HttpResponseMessage tResponse = await tHttpClient.SendAsync(tHttpRequest);
                using (StreamReader tStreamReader = new StreamReader(await tResponse.Content.ReadAsStreamAsync()))
                {
                    string tResponseJson = await tStreamReader.ReadToEndAsync();
                    rReturn = JsonConvert.DeserializeObject<NWDResponseCrucial>(tResponseJson)!;
                }
            }
            catch (Exception)
            {
                rReturn = new NWDResponseCrucial
                {
                    Status = NWDRequestStatus.NoNetwork
                };
            }

            rReturn.Logger(sRequest.Logger());
            
            return rReturn;
        }
    }
}