using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDJson: NWDDataType
    {
        public string Value { set; get; } = string.Empty;
    }
}
