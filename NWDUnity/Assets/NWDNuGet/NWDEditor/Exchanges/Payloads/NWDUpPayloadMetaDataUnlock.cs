using NWDEditor;
using NWDEditor.Exchanges.Payloads;

namespace NWDHub.Models
{
    public class NWDUpPayloadMetaDataUnlock : NWDUpPayloadEditor
    {
        public NWDMetaData? MetaData { get; set; }
        public string LockerName { get; set; } = NWDUpPayloadMetaDataLock.K_UNKNOWN;

    }
}
