namespace NWDWebTreat.Models
{
    [Serializable]
    public class NWDVoucherServiceName
    {
        public string Name { set; get; } = string.Empty;
        public long Id { set; get; }

        public NWDVoucherServiceName()
        {
            
        }
        public NWDVoucherServiceName(string sName, long sId)
        {
            Name = sName;
            Id = sId;
        }
    }
}