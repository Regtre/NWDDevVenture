using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDAsset : INWDSubModel 
    {
        public string UnityAsset { set; get; } = string.Empty;
    }

    [Serializable]
    public class NWDAsset<T> : NWDAsset where T : INWDAsset
    {

    }
}
