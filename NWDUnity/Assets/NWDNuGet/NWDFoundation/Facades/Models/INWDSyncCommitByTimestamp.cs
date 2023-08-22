using System;
using System.Security.Cryptography.X509Certificates;

namespace NWDFoundation.Models
{
    public interface INWDSyncCommitByTimestamp 
    {
        public Int64 SyncDatetime { set; get; }
        public Int64 Commit { set; get; }

    }
}