using NWDFoundation.Localization;
using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDLocalizableText : INWDSubModel
    {
        public string Context { set; get; } = string.Empty;
        public Dictionary<NWDLangage, string> Values { set; get; } = new Dictionary<NWDLangage, string>();
    }
}
