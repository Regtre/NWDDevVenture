using NWDUnityShared.SQLite;
using System;

namespace NWDUnityShared.Tools
{
    public interface INWDDBRequest : INWDDBStatement
    {
        public INWDDBRequest Open(string sQuery);
        public SQLite3.Result Step();
        public SQLite3.Result Close();
    }
}
