using NWDEditor.Exchanges;
using NWDEditor.Exchanges.Payloads;
using NWDUnityEditor.Engine;

namespace NWDUnityEditor.Factories
{
    public class NWDResponseEditorFactory
    {
        static public T GetPayload<T> (NWDResponseEditor sResponse) where T : NWDDownPayloadEditor
        {
            return sResponse.GetPayload<T>(NWDUnityEngineEditor.Instance.GetConfig().GetPrivateRoleToken());
        }
    }
}

