using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDDateTime : INWDSubModel
    {
        public long Timstamp { set; get; } = 0;
    }
}
