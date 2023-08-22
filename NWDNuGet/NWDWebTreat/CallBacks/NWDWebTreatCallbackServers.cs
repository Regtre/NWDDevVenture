using System.Text;
using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDTreat.Exchanges;
using NWDWebRuntime.Configuration;
using NWDWebTreat.Configuration;

namespace NWDWebTreat.CallBacks
{
    public static class NWDWebTreatCallbackServers
    {

        public static async Task<NWDResponseTreat> PostRequest(NWDRequestTreat sRequest)
        {
            NWDResponseTreat rReturn;
            try
            {
                string tResquestJson = JsonConvert.SerializeObject(sRequest);
                // TODO : use servers definition to select one server!
                // use IP to be nearest the user
                HttpClient tHttpClient = new HttpClient();
//#if DEBUG
               // HttpRequestMessage tHttpRequest = new HttpRequestMessage(HttpMethod.Post, NWDWebRuntimeConfiguration.KConfig.GetDnsHttps()+"/NWDTreat/");
                
//#else
                HttpRequestMessage tHttpRequest = new HttpRequestMessage(HttpMethod.Post, NWDWebRuntimeConfiguration.KConfig.GetHubDnsHttps()+"/NWDTreat/");
//#endif
                tHttpRequest.Content = new StringContent(tResquestJson, Encoding.UTF8, "application/json");
                
                
                HttpResponseMessage tResponse = await tHttpClient.SendAsync(tHttpRequest);

                using (StreamReader tStreamReader = new StreamReader(await tResponse.Content.ReadAsStreamAsync()))
                {
                    
                    string tResponseJson = await tStreamReader.ReadToEndAsync();

                    rReturn = JsonConvert.DeserializeObject<NWDResponseTreat>(tResponseJson)!;

                }

                if (rReturn != null && rReturn.IsValid(NWDWebTreatConfiguration.KConfig))
                {
                }
                else
                {
                    rReturn = new NWDResponseTreat() { Status = NWDRequestStatus.TokenError };
                }
            }
            catch (Exception)
            {
                rReturn = new NWDResponseTreat
                {
                    Status = NWDRequestStatus.NoNetwork
                };
            }
            rReturn.Logger(sRequest.Logger());
            return rReturn;
        }
    }
}