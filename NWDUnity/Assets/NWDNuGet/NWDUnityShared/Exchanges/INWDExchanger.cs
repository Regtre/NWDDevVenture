namespace NWDUnityShared.Exchanges
{
    public interface INWDExchanger <REQ, RES>
    {
        string DefaultURI { get; }

        RES SendSync (REQ sRequest, string sURL);

        RES SendAsync (REQ sRequest, string sURL);
    }
}

