using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDColorHex: NWDDataType 
    {
        public int Red { set; get; }
        public int Green { set; get; }
        public int Blue { set; get; }
        public int Alpha { set; get; }
    }
}
