using NWDUnityShared.SQLite;
using System;

namespace NWDUnityShared.Tools
{
    public interface INWDDBTransaction : IDisposable
    {
        public INWDDBTransaction Open();
        public SQLite3.Result Rollback();
        public SQLite3.Result Commit();
        public SQLite3.Result GetResult();
    }
}
