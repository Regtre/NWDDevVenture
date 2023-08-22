using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public abstract class NWDBundledData : NWDBasicModel
    {
        public int BundleId { set; get; }
        public string? ClassName { set; get; }
        public string? IndexOne { set; get; }
        public string? IndexTwo { set; get; }
        public string? IndexThree { set; get; }
        public string? IndexFour { set; get; }
    }
}