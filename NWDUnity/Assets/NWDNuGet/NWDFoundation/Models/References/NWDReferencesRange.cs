#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDRange
    {
        public float Min{ set; get; }
        public float Max{ set; get; }
        public float Average{ set; get; }

        public NWDRange()
        {
            Min = 0.0F;
            Max = 0.0F;
            Average = 0.0F;
        }
    }

    [Serializable]
    public class NWDReferencesRange<T>: NWDDataType where T : NWDBasicModel
    {
        public Dictionary<ulong, NWDRange> ReferenceRange { set; get; } = new Dictionary<ulong, NWDRange>();

    }
}
#nullable disable