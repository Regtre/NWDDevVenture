using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDRect: NWDDataType 
    {
        public float X { set; get; }
        public float Y { set; get; }
        public float W { set; get; }
        public float H { set; get; }
    }
}
