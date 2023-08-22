using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public abstract class NWDStudioData : NWDBasicModel, INWDAvailableForTarget, INWDSyncCommitByTimestamp, INWDDataTrack
    {
        public Int64 DataTrack { set; get; }
        public bool AvailableForWeb { set; get; } = false;
        public bool AvailableForGame { set; get; } = false;
        public bool AvailableForApp { set; get; } = false;
        public Int64 SyncDatetime { set; get; }
        public Int64 Commit { set; get; }
    }
}