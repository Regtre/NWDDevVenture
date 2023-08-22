using NWDFoundation.Exchanges;
using NWDRuntime.Exchanges;
using NWDUnityShared.Engine;
using NWDUnityShared.Exchanges;

namespace NWDUnityShared.Services
{
    public class NWDRuntimeService
    {
        static protected INWDExchanger<NWDRequestRuntime, NWDResponseRuntime> exchanger = new NWDHTTPRuntimeExchanger();
        static public void SetExchanger (INWDExchanger<NWDRequestRuntime, NWDResponseRuntime> sExchanger)
        {
            exchanger = sExchanger;
        }

        static protected string ForgeURL(string URI)
        {
            return NWDUnityEngine.Instance.Config.WebEditorURL() + URI;
        }

        static protected void CheckServerResponse (NWDResponseRuntime sResponse)
        {
            if (sResponse.Status != NWDRequestStatus.Ok)
            {
                throw new System.Exception("Server responded with status: " + sResponse.Status);
            }
        }
    }
}
