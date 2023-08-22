using System;
using NWDFoundation.Exchanges;

namespace NWDFoundation.Models
{
    [Serializable]
    public abstract class NWDDataBasicStorageModel : NWDBasicModel, INWDAvailableForTarget, INWDSyncCommitByTimestamp,INWDDataTrack
    {
        public Int64 DataTrack { set; get; }
        public bool AvailableForWeb { set; get; } = false;
        public bool AvailableForGame { set; get; } = false;
        public bool AvailableForApp { set; get; } = false;
        public Int64 SyncDatetime { set; get; }
        public Int64 Commit { set; get; }
        public string ClassName { set; get; } = string.Empty;
        public string Json { set; get; } = string.Empty;
        public string IndexOne { set; get; }= string.Empty;
        public string IndexTwo { set; get; }= string.Empty;
        public string IndexThree { set; get; }= string.Empty;
        public string IndexFour { set; get; }= string.Empty;
    }   


}