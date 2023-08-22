using NWDEditor.Exchanges;
using NWDEditor.Exchanges.Payloads;
using NWDFoundation.Exchanges;
using NWDUnityEditor.Config;
using NWDUnityEditor.Engine;

namespace NWDUnityEditor.Factories
{
    public static class NWDRequestEditorFactory
    {
        static public NWDRequestEditor New (NWDExchangeEditorKind sKind, NWDUpPayloadEditor? sUpPayload = null)
        {
            NWDConfigUnityEditor tConfig = NWDUnityEngineEditor.Instance.GetConfig();
            return new NWDRequestEditor(tConfig.GetPublicRoleToken(), sKind, sUpPayload, NWDExchangeOrigin.UnityEditor, tConfig.GetDeviceOS(), tConfig.GetPrivateRoleToken());
        }
    }
}