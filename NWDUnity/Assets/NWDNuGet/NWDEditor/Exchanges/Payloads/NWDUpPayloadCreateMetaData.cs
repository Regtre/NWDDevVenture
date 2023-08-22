using NWDEditor.Exchanges.Payloads;

namespace NWDEditor.Exchanges.Payloads
{
    public class NWDUpPayloadCreateMetaData : NWDUpPayloadEditor
    {
        #region properties
        public string TypeCLass { get; set; }

        #endregion

        public NWDUpPayloadCreateMetaData (string sTypeClass)
        {
            TypeCLass = sTypeClass;
        }
    }
}