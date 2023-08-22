using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDIp: NWDDataType
    {
        public int A { set; get; }
        public int B { set; get; }
        public int C { set; get; }
        public int D { set; get; }
    }
}
