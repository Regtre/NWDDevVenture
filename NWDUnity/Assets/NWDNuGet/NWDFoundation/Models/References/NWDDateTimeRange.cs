using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDDateTimeRange: NWDDataType 
    {
        public int Min { set; get; } = 0;
        public int Max { set; get; } = 0;
    }
}
