using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDVersion : INWDSubModel
    {
        public int Major { set; get; }
        public int Minor { set; get; }
        public int Build { set; get; }
    }
}
