using NWDRuntime.Exchanges;

namespace NWDUnityShared.Exchanges
{
    public class NWDHTTPRuntimeExchanger : NWDHTTPExchanger<NWDRequestRuntime, NWDResponseRuntime>
    {
        public override string DefaultURI => "/NWDRuntime/";
    }
}

