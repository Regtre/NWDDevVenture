using NWDEditor.Exchanges.Payloads;

namespace NWDHub.Models
{
    public class NWDUpPayloadMetaDataLock : NWDUpPayloadEditor
    {
        public const string K_UNKNOWN = "unknown";
        public ulong MetaDataReference { get; set; }
        public string LockerName { get; set; } = K_UNKNOWN;
    }
}
