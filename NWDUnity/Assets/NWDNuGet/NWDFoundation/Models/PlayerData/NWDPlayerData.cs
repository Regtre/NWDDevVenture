using System;
using NWDFoundation.Exchanges;
using NWDFoundation.Models.Enums;

namespace NWDFoundation.Models
{
    [Serializable]
    public abstract class NWDPlayerData : NWDBasicModel, INWDAccountDependence, INWDAvailableForTarget, INWDDataTrack, INWDSyncCommitByTimestamp
    {
        public ulong Account { set; get; }
        public ushort Range { set; get; }
        public Int64 DataTrack { set; get; }
        public bool AvailableForWeb { set; get; } = false;
        public bool AvailableForGame { set; get; } = false;
        public bool AvailableForApp { set; get; } = false;
        public Int64 SyncDatetime { set; get; }
        public Int64 Commit { set; get; }
        public NWDPlayerDataProcessKind Process { set; get; } = NWDPlayerDataProcessKind.None;
        
        public virtual void CopyFrom(NWDPlayerData sOther)
        {
            // Console.WriteLine("CopyFrom NWDPlayerData");
            base.CopyFrom(sOther);
            Account = sOther.Account;
            Range = sOther.Range;
            DataTrack = sOther.DataTrack;
            AvailableForWeb = sOther.AvailableForWeb;
            AvailableForGame = sOther.AvailableForGame;
            AvailableForApp = sOther.AvailableForApp;
            SyncDatetime = sOther.SyncDatetime;
            Commit = sOther.Commit;
            Process = sOther.Process;
        }
    }
}