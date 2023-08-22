using NWDFoundation.Localization;
using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public abstract class NWDLocalizable : INWDSubModel
    {
        public string Context { set; get; } = string.Empty;
        public Dictionary<NWDLangage, NWDAsset> Values { set; get; } = new Dictionary<NWDLangage, NWDAsset>();
    }

    [Serializable]
    public class NWDLocalizable<T> : NWDLocalizable where T : NWDAsset
    {
    }
}
