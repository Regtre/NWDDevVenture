using NWDEditor.Exchanges.Payloads;

namespace NWDEditor.Exchanges
{
    public class NWDDownPayloadUnlockMeta : NWDDownPayloadEditor
    {
        public NWDMetaData? MetaData { get; set; }

        public NWDDownPayloadUnlockMeta(NWDMetaData? sMetaData)
        {
            MetaData = sMetaData;
        }
    }
}