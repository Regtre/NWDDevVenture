using NWDEditor.Exchanges.Payloads;

namespace NWDEditor.Exchanges
{
    public class NWDDownPayloadLockMetaData : NWDDownPayloadEditor
    {
        public NWDMetaData? MetaData { get; set; }
        public NWDDownPayloadLockMetaData(NWDMetaData? sMetaData)
        {
            MetaData = sMetaData;
        }
    }
}