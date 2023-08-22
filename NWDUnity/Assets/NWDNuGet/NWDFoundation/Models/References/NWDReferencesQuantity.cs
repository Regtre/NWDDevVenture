#nullable enable
using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    // TODO change for structure
    public class NWDReferencesQuantity : NWDDataType, INWDSubModel
    {
        public Dictionary<ulong, Int64> ReferenceAmount { set; get; } = new Dictionary<ulong, Int64>();

    }

    [Serializable]
    // TODO change for structure
    public class NWDReferencesQuantity<T> : NWDReferencesQuantity where T : NWDBasicModel
    {

    }
}
#nullable disable