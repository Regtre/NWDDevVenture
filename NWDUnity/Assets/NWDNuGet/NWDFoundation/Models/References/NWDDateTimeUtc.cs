using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDDateTimeUtc: NWDDataType 
    {
        public int Value { set; get; } = 0;
    }
}
