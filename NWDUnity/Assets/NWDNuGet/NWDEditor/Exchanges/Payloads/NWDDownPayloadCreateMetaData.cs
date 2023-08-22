
namespace NWDEditor.Exchanges.Payloads
{
    public class NWDDownPayloadCreateMetaData : NWDDownPayloadEditor
    {
        public NWDMetaData? MetaData { get; set; }
        
        public NWDDownPayloadCreateMetaData(NWDMetaData? sMetaData)
        {
            MetaData = sMetaData;
        }
        public NWDDownPayloadCreateMetaData(){}
    }
}

