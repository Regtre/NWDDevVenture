using System.Text;
using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDMauiRuntime.Configurations;
using NWDRuntime.Exchanges;

namespace NWDMauiRuntime.Back;

public class NWDRuntimeCallback
{
    private HttpClient _HttpClient;
    private static NWDRuntimeCallback _Instance = null;

    private NWDRuntimeCallback()
    {
        _HttpClient = new HttpClient()
        {
            BaseAddress = new Uri(NWDMauiRuntimeConfiguration.KConfig.GetBestUrlForServer())
        };

    }


    public static NWDRuntimeCallback SharedInstance()
    {
        if (_Instance == null)
        {
            _Instance = new NWDRuntimeCallback();
        }

        return _Instance;
    }

    public NWDRequestPlayerToken GetRequestPlayerToken()
    {
        NWDRequestPlayerToken rRequestPlayerToken = new NWDRequestPlayerToken(
            NWDMauiRuntimeConfiguration.KConfig.ProjectId, NWDMauiRuntimeConfiguration.KConfig.Environment);
        if (IsKeyPresentAndNotEmpty(nameof(NWDRequestPlayerToken.PlayerReference)) &&
        IsKeyPresentAndNotEmpty(nameof(NWDRequestPlayerToken.AccountRange)) &&
        IsKeyPresentAndNotEmpty(nameof(NWDRequestPlayerToken.Token)))
        {
            
            ulong.TryParse(SecureStorage.GetAsync(nameof(NWDRequestPlayerToken.PlayerReference)).Result, out ulong playerReference);
            ushort.TryParse(SecureStorage.GetAsync(nameof(NWDRequestPlayerToken.AccountRange)).Result, out ushort accountRange);

            rRequestPlayerToken.PlayerReference = playerReference;
            rRequestPlayerToken.AccountRange = accountRange;
            rRequestPlayerToken.Token = SecureStorage.GetAsync(nameof(NWDRequestPlayerToken.Token)).Result;

        }

        return rRequestPlayerToken;
    }
    


    public void SetPreferences(NWDResponseRuntime sResponseRuntime)
    {
        if (sResponseRuntime.IsValid(NWDMauiRuntimeConfiguration.KConfig))
        {
            SetPlayerToken(sResponseRuntime);
            SetServices(sResponseRuntime);
        }
    }

    public void SetPlayerToken(NWDResponseRuntime sResponseRuntime)
    {
        SecureStorage.SetAsync(nameof(NWDRequestPlayerToken.PlayerReference),
            sResponseRuntime.PlayerToken.PlayerReference.ToString()); 
        
        SecureStorage.SetAsync(nameof(NWDRequestPlayerToken.AccountRange),
            sResponseRuntime.PlayerToken.AccountRange.ToString());
        
        SecureStorage.SetAsync(nameof(NWDRequestPlayerToken.Token), sResponseRuntime.PlayerToken.Token);
    }

    public void SetServices(NWDResponseRuntime sResponseRuntime)
    {
        List<NWDAccountService> tList = sResponseRuntime.GetPayload<NWDDownPayload>(NWDMauiRuntimeConfiguration.KConfig)
            .AccountServiceList;
        SecureStorage.SetAsync(nameof(NWDAccountService), JsonConvert.SerializeObject(tList));
    }

    private void DeletePlayerToken()
    {
        SecureStorage.Remove(nameof(NWDRequestPlayerToken.PlayerReference)); 
        SecureStorage.Remove(nameof(NWDRequestPlayerToken.AccountRange));
        SecureStorage.Remove(nameof(NWDRequestPlayerToken.Token));
    }

    public async Task<NWDResponseRuntime> PostRequest(NWDRequestRuntime sRequest)
    {
        NWDResponseRuntime rReturn;
        string tResquestJson = JsonConvert.SerializeObject(sRequest);
        string tUrl = NWDMauiRuntimeConfiguration.KConfig.GetBestUrlForServer() + "/NWDRuntime/";
        try
        {

            HttpRequestMessage tHttpRequest = new HttpRequestMessage(HttpMethod.Post, tUrl);
            tHttpRequest.Content = new StringContent(tResquestJson, Encoding.UTF8, "application/json");
            HttpResponseMessage tResponse = _HttpClient.Send(tHttpRequest);
            using (StreamReader tStreamReader = new StreamReader(await tResponse.Content.ReadAsStreamAsync()))
            {
                string tResponseJson = await tStreamReader.ReadToEndAsync();
                rReturn = JsonConvert.DeserializeObject<NWDResponseRuntime>(tResponseJson)!;
            }

            if (rReturn != null && rReturn.IsValid(NWDMauiRuntimeConfiguration.KConfig))
            {
                /*NWDSync.SetPlayerLastSyncInformation(sHttpContext, rReturn.PlayerToken.PlayerSyncInformation);
                NWDSync.SetStudioLastSyncInformation(sHttpContext, rReturn.PlayerToken.StudioSyncInformation);*/
                SetServices(rReturn);
            }
            else
            {
                rReturn = new NWDResponseRuntime() { Status = NWDRequestStatus.HashInvalid };
            }
        }
        catch (Exception)
        {
            rReturn = new NWDResponseRuntime
            {
                Status = NWDRequestStatus.NoNetwork
            };
        }

        if (rReturn.Status != NWDRequestStatus.Ok)
        {
            // NWDLogger.Critical(rReturn.Status.ToString());
            Console.WriteLine(rReturn.Status.ToString());
            DeletePlayerToken();
        }
        else
        {
            SetPlayerToken(rReturn);
        }

        rReturn.Logger(sRequest.Logger(tUrl));
        return rReturn;
    }

    public static bool IsKeyPresentAndNotEmpty(string sKey)
    {
        return !string.IsNullOrEmpty(SecureStorage.GetAsync(sKey).Result);
    }

    public void SetSessionPlayerSync(NWDSyncInformation sSync)
    {
        SecureStorage.SetAsync("Player_"+nameof(NWDSyncInformation), JsonConvert.SerializeObject(sSync));
    }

    public void SetSessionStudioSync(NWDSyncInformation sSync)
    {
        SecureStorage.SetAsync("Studio_"+nameof(NWDSyncInformation), JsonConvert.SerializeObject(sSync));
    }

    public NWDSyncInformation? GetSessionPlayerSync()
    {
        return JsonConvert.DeserializeObject<NWDSyncInformation>(SecureStorage.GetAsync(("Player_" + nameof(NWDSyncInformation))).Result);
    }

    public NWDSyncInformation? GetSessionStudioSync()
    {
        return JsonConvert.DeserializeObject<NWDSyncInformation>(SecureStorage.GetAsync(("Studio_" + nameof(NWDSyncInformation))).Result);
    }
}
    