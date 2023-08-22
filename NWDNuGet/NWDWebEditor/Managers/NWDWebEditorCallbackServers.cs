using System.Text;
using Newtonsoft.Json;
using NWDEditor.Exchanges;
using NWDFoundation.Exchanges;
using NWDWebRuntime.Configuration;

namespace NWDWebEditor.Managers
{
    
    public static class NWDWebEditorCallbackServers
    {

        public static async Task<NWDResponseEditor?> PostRequest(NWDRequestEditor sRequest)
        {
            NWDResponseEditor? rReturn;
            try
            {
                string tResquestJson = JsonConvert.SerializeObject(sRequest);
                string tUrl = NWDWebRuntimeConfiguration.KConfig.GetBestUrlForServer() + "/NWDEditor/";

                HttpClient tHttpClient = new HttpClient();
                HttpRequestMessage tHttpRequest = new HttpRequestMessage(HttpMethod.Post, tUrl);
                tHttpRequest.Content = new StringContent(tResquestJson, Encoding.UTF8, "application/json");
                
                
                HttpResponseMessage tResponse = await tHttpClient.SendAsync(tHttpRequest);

                using (StreamReader tStreamReader = new StreamReader(await tResponse.Content.ReadAsStreamAsync()))
                {
                    
                    string tResponseJson = await tStreamReader.ReadToEndAsync();

                    rReturn = JsonConvert.DeserializeObject<NWDResponseEditor>(tResponseJson)!;

                }
            }
            catch (Exception)
            {
                rReturn = new NWDResponseEditor
                {
                    Status = NWDRequestStatus.NoNetwork
                };
            }
            return rReturn;
        }
    }
}