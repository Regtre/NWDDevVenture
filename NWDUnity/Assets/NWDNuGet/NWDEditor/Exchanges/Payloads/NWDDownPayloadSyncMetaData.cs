using System.Collections.Generic;
using NWDEditor.Exchanges.Payloads;

namespace NWDEditor.Exchanges
{
    public class NWDDownPayloadSyncMetaData : NWDDownPayloadEditor
    {
        public List<NWDMetaData> MetaDataList { get; set; } = new List<NWDMetaData>();

        public NWDDownPayloadSyncMetaData(List<NWDMetaData> sSyncMetaData)
        {
            MetaDataList = sSyncMetaData;
        }
    }
}