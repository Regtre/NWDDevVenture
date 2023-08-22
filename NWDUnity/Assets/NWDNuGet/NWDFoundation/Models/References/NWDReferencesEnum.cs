#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace NWDFoundation.Models
{
    [Serializable]
    // TODO change for structure
    public class NWDReferencesEnum<T, TE>: NWDDataType  where TE : Enum where T : NWDBasicModel 
    {
        public Dictionary<ulong, TE> ReferenceEnum { set; get; } = new Dictionary<ulong, TE>();
    }
}

#nullable disable