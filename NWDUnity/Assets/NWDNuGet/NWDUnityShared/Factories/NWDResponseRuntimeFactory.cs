using NWDFoundation.Exchanges.Payloads;
using NWDRuntime.Exchanges;
using NWDUnityShared.Engine;

namespace NWDUnityShared.Factories
{
    public class NWDResponseRuntimeFactory
    {
        static public T GetPayload<T> (NWDResponseRuntime sResponse) where T : NWDDownPayload
        {
            return sResponse.GetPayload<T>(NWDUnityEngine.Instance.Config);
        }
    }
}

