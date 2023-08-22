using NWDFoundation.Models;

namespace NWDEditor
{
    public class NWDMetaData : NWDDatabaseWebBasicModel
    {

        public ulong ProjectUniqueId { set; get; }
        //public Dictionary<ushort, List<NWDStudioData>> DataByDataTrack { get; set; } = new Dictionary<ushort, List<NWDStudioData>>();
        public string DataByDataTrack { get; set; } = string.Empty;
        public string Title { set; get; } = string.Empty;
        public string Description { set; get; } = string.Empty;
        public string ClassName { set; get; } = string.Empty;
        public bool IsLocked { get; set; } = false;
        public int LockLimit { get; set; } = 0;
        public string LockerName { set; get; } = string.Empty;
        
        public int ModificationDate { set; get; }
        public bool AvailableForWeb { set; get; } = false;
        public bool AvailableForGame { set; get; } = false;
        public bool AvailableForApp { set; get; } = false;
        public NWDMetaData(){}

        public NWDMetaData(NWDMetaData sOther): base(sOther)
        {
            ProjectUniqueId = sOther.ProjectUniqueId;
            DataByDataTrack = sOther.DataByDataTrack;
            Title = sOther.Title;
            Description = sOther.Description;
            ClassName = sOther.ClassName;
            IsLocked = sOther.IsLocked;
            ModificationDate = sOther.ModificationDate;
            LockerName = sOther.LockerName;
            AvailableForWeb = sOther.AvailableForWeb;
            AvailableForGame = sOther.AvailableForGame;
            AvailableForApp = sOther.AvailableForApp;
        }
    }
}