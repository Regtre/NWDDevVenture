using NWDEditor;
using NWDEditor.Exchanges;
using NWDFoundation.Exchanges;
using NWDUnityEditor.Engine;

namespace NWDUnityEditor.Services
{
    public class NWDProjectService : NWDEditorService
    {
        static public void Authenticate (string sPrivateToken, string sPublicToken, bool sForceUpdate)
        {
            // You cannot use the NWDRequestEditorFactory and NWDResponseEditorFactory as the global config has NWDUnityEngineEditor.Instance.GetConfig( not been configured yet.
            NWDRequestEditor tRequest = new NWDRequestEditor (sPublicToken, NWDExchangeEditorKind.GetProjectSettings, null, NWDExchangeOrigin.UnityEditor, NWDUnityEngineEditor.Instance.GetConfig().GetDeviceOS(), sPrivateToken);
            NWDResponseEditor tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            CheckServerResponse(tResponse);

            NWDDownPayloadGetProjectSettings tProjectSettings = tResponse.GetPayload<NWDDownPayloadGetProjectSettings>(sPrivateToken);

            NWDUnityEngineEditor.Instance.GetConfig().UpdateConfig(tProjectSettings, sPublicToken, sPrivateToken, sForceUpdate);
        }

        static public string TestConnexion (string sURL)
        {
            // Makes sure the URL is usable one.
            sURL = sURL.Trim().TrimEnd('/');

            // You cannot use the NWDRequestEditorFactory and NWDResponseEditorFactory as the global config has NWDUnityEngineEditor.Instance.GetConfig( not been configured yet.
            NWDRequestEditor tRequest = new NWDRequestEditor(string.Empty, NWDExchangeEditorKind.Test, null, NWDExchangeOrigin.UnityEditor, NWDUnityEngineEditor.Instance.GetConfig().GetDeviceOS(), string.Empty);
            NWDResponseEditor tResponse = exchanger.SendSync(tRequest, sURL + exchanger.DefaultURI);

            CheckServerResponse(tResponse);

            return sURL;
        }
    }
}
