using System;
using NWDFoundation.Models.Enums;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDPlayerDataStorage : NWDDataBasicStorageModel, INWDAccountDependence
    {
        public ulong Account { set; get; }
        public ushort Range { set; get; }
        public NWDPlayerDataProcessKind Process { set; get; } = NWDPlayerDataProcessKind.None;
        public NWDPlayerDataStorage()
        {
        }
    }
}