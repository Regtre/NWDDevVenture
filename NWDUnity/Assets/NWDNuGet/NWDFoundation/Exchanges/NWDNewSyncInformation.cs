using System;

namespace NWDFoundation.Exchanges
{
    [Serializable]
    public class NWDNewSyncInformation
    {
        #region properties
        
        /// <summary>
        /// Sync DateTime Last time
        /// </summary>
        public DateTime SyncDateTime { set; get; }

        /// <summary>
        /// Sync Commit Id of request/Transaction Last time
        /// </summary>
        public Int64 SyncCommitId { set; get; }

        #endregion
    }
}