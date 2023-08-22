using NWDUnityShared.Engine;
using NWDTreat.Exchanges;
using NWDUnityShared.Exchanges;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDUnityTreat.Config;
using NWDUnityEditor.Engine;
using NWDFoundation.Configuration.Environments;
using NWDTreat.Exchanges.Payloads;

namespace NWDUnityTreat.Services
{
    public class NWDTreatService
    {
        static protected INWDExchanger<NWDRequestTreat, NWDResponseTreat> exchanger = new NWDHTTPTreatExchanger();
        static public void SetExchanger (INWDExchanger<NWDRequestTreat, NWDResponseTreat> sExchanger)
        {
            exchanger = sExchanger;
        }

        static protected string ForgeURL(string URI)
        {
            return NWDUnityEngine.Instance.Config.WebEditorURL() + URI;
        }

        static protected void CheckServerResponse (NWDResponseTreat sResponse)
        {
            if (sResponse.Status != NWDRequestStatus.Ok)
            {
                throw new System.Exception("Server responded with status: " + sResponse.Status);
            }
        }

        static public void AssociateService(NWDAccountService sAccountService)
        {
            ulong tProjectId = NWDUnityEngineEditor.Instance.Config.GetProjectId();
            NWDEnvironmentKind tEnvironment = NWDUnityEngineEditor.Instance.EnvironmentManager.GetCurrentEnvironment();
            NWDExchangeDevice tDevice = NWDUnityEngineEditor.Instance.Config.GetDeviceOS();

            NWDUpPayloadTreatService tPayload = new NWDUpPayloadTreatService(sAccountService);
            NWDRequestTreat tRequest = new NWDRequestTreat(NWDUnityTreatConfig.Instance, tProjectId, tEnvironment, NWDExchangeTreatKind.AssociateService, tPayload, NWDExchangeOrigin.UnityEditor, tDevice);
            NWDResponseTreat tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            CheckServerResponse(tResponse);
        }
    }
}
