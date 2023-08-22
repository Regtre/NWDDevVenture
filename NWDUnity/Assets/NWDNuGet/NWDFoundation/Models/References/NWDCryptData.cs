using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDCryptData: NWDDataType 
    {
        public string Value { set; get; } = string.Empty;
    }
}
