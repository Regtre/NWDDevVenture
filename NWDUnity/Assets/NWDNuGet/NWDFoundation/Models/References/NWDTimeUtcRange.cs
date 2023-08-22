using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDTimeUtcRange: NWDDataType 
    {
        public int Min { set; get; } = 0;
        public int Max { set; get; } = 0;
    }
}
