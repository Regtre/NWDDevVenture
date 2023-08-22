#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDAverage
    {
        public float Total{ set; get; }
        public float Counter{ set; get; }
        public float Average{ set; get; }

        public NWDAverage()
        {
            Total = 0.0F;
            Counter = 0.0F;
            Average = 0.0F;
        }
    }

    [Serializable]
    // TODO change for structure
    public class NWDReferencesAverage<T>: NWDDataType where T : NWDBasicModel
    {
        public Dictionary<ulong, NWDAverage> ReferenceAverage { set; get; } = new Dictionary<ulong, NWDAverage>();

    }
}
#nullable disable