using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDLocalizablePrefab: NWDDataType
    {
        public string Base { set; get; } = string.Empty;
        public Dictionary<string, string> Values { set; get; } =new Dictionary<string, string>();
    }
}
