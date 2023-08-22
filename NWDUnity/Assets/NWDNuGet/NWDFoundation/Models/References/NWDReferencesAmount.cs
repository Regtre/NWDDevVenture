#nullable enable
using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    // TODO change for structure
    public abstract class NWDReferencesAmount : NWDDataType, INWDSubModel
    {
        public Dictionary<ulong, float> ReferenceAmount { set; get; } = new Dictionary<ulong, float>();

    }

    [Serializable]
    // TODO change for structure
    public class NWDReferencesAmount<T>: NWDReferencesAmount where T : NWDBasicModel
    {

    }
}
#nullable disable