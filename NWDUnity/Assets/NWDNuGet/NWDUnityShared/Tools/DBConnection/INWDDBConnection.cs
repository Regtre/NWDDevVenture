using NWDUnityShared.SQLite;

namespace NWDUnityShared.Tools
{
    public interface INWDDBConnection : INWDDBStatement
    {
        public INWDDBConnection Open();
        public INWDDBRequest GetRequest();
        public INWDDBRequest StartRequest(string sQuery);
        public INWDDBTransaction GetTransaction();
        public INWDDBTransaction StartTransaction();
        public SQLite3.Result Close();
    }
}
