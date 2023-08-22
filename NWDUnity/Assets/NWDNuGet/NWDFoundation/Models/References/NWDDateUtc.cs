using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDDateUtc: NWDDataType 
    {
        public int Value { set; get; } = 0;
    }
}
