using System;
using System.Runtime.Serialization;
using NWDFoundation.Facades;

namespace NWDFoundation.Exchanges
{
    [Serializable]
    public class NWDSyncInformation: INWDSerializable
    {
        #region properties

        public bool UseMe { set; get; } = true;

        /// <summary>
        /// Sync DateTime Last time
        /// </summary>
        public DateTime OldSyncDateTime { set; get; }

        /// <summary>
        /// Sync Commit Id of request/Transaction Last time
        /// </summary>
        public Int64 OldSyncCommitId { set; get; }

        /// <summary>
        /// Sync DateTime
        /// </summary>
        public DateTime SyncDateTime { set; get; }

        /// <summary>
        /// Sync Commit Id of request/Transaction
        /// </summary>
        public Int64  SyncCommitId { set; get; }

        #endregion

        #region Constructor

        public NWDSyncInformation()
        {
            SyncDateTime = DateTime.UnixEpoch;
            SyncCommitId = 0;
            OldSyncDateTime = DateTime.UnixEpoch;
            OldSyncCommitId = 0;
        }
        public NWDSyncInformation(NWDSyncInformation sSyncInformation)
        {
            SyncDateTime = sSyncInformation.SyncDateTime;
            SyncCommitId = sSyncInformation.SyncCommitId;
            OldSyncDateTime = sSyncInformation.OldSyncDateTime;
            OldSyncCommitId = sSyncInformation.OldSyncCommitId;
        }
        
        public static NWDSyncInformation Copy(NWDSyncInformation sSyncInformation)
        {
           return new NWDSyncInformation(sSyncInformation);
        }
        
        public static NWDSyncInformation From(NWDSyncInformation sSyncInformation, DateTime sNewSyncDateTime, Int64 sNewCommitId)
        {
            NWDSyncInformation rReturn = new NWDSyncInformation
            {
                OldSyncCommitId = sSyncInformation.SyncCommitId,
                OldSyncDateTime = sSyncInformation.SyncDateTime,
                SyncDateTime = sNewSyncDateTime,
                SyncCommitId = sNewCommitId
            };
            return rReturn;
        }
        #endregion
    }
}