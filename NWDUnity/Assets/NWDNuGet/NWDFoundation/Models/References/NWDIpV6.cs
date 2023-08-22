using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDIpV6: NWDDataType
    {
        public uint A { set; get; }
        public uint B { set; get; }
        public uint C { set; get; }
        public uint D { set; get; }
        public uint E { set; get; }
        public uint F { set; get; }
        public uint G { set; get; }
        public uint H { set; get; }
    }
}
