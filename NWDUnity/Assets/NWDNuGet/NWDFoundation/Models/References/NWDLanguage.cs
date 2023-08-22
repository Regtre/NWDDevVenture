using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDLanguage: NWDDataType
    {
        public string Major { set; get; } = string.Empty;
        public string Minor { set; get; } = string.Empty;
        public string Value { set; get; } = string.Empty;
    }
}
