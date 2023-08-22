using NWDEditor.Exchanges;
using NWDUnityShared.Exchanges;

namespace NWDUnityEditor.Exchanges
{
    public class NWDHTTPEditorExchanger : NWDHTTPExchanger<NWDRequestEditor, NWDResponseEditor>
    {
        public override string DefaultURI => "/NWDEditor/";
    }
}

