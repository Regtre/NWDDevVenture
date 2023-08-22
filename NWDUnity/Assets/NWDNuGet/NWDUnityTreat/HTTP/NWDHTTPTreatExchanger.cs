using NWDTreat.Exchanges;

namespace NWDUnityShared.Exchanges
{
    public class NWDHTTPTreatExchanger : NWDHTTPExchanger<NWDRequestTreat, NWDResponseTreat>
    {
        public override string DefaultURI => "/NWDTreat/";
    }
}

