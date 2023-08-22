using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDTimeUtc: NWDDataType 
    {
        public int Value { set; get; } = 0;
    }
}
