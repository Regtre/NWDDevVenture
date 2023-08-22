using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDSprite: NWDDataType 
    {
        public string Asset { set; get; } = string.Empty;
    }
}
