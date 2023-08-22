using NWDEditor.Exchanges;
using NWDFoundation.Exchanges;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Exchanges;
using NWDUnityShared.Exchanges;

namespace NWDUnityEditor.Services
{
    public class NWDEditorService
    {
        static protected INWDExchanger<NWDRequestEditor, NWDResponseEditor> exchanger = new NWDHTTPEditorExchanger();
        static public void SetExchanger (INWDExchanger<NWDRequestEditor, NWDResponseEditor> sExchanger)
        {
            exchanger = sExchanger;
        }

        static protected string ForgeURL(string URI)
        {
            return NWDUnityEngineEditor.Instance.GetConfig().WebEditorURL() + URI;
        }

        static protected void CheckServerResponse (NWDResponseEditor sResponse)
        {
            if (sResponse.Status != NWDRequestStatus.Ok)
            {
                throw new System.Exception("Server responded with status: " + sResponse.Status);
            }
        }
    }
}
