using NWDUnityShared.SQLite;
using System;

namespace NWDUnityShared.Tools
{
    public interface INWDDBStatement : IDisposable
    {
        public SQLite3.Result Exec(string sQuery);
        public SQLite3.Result GetResult();
    }
}
