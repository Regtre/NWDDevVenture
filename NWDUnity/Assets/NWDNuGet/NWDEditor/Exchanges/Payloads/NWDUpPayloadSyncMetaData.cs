using System.Collections.Generic;
using NWDEditor;
using NWDEditor.Exchanges.Payloads;

namespace NWDHub.Models
{
    public class NWDUpPayloadSyncMetaData : NWDUpPayloadEditor
    {
        public List<NWDMetaData> MetaDataList { get; set; } = new List<NWDMetaData>();
        public ulong ProjectId;
    }
}