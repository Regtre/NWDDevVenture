#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace NWDFoundation.Models
{
    [Serializable]
    public enum NWDConditional : int
    {
        EqualTo = 0,
        UpperThan = 1,
        UpperThanOrEqual = 2,
        LowerThan = 3,
        LowerThanOrEqual = 4,
        DifferentTo = 5,
    }

    [Serializable]
    // TODO change for structure
    public class NWDReferencesConditional<T>: NWDDataType where T : NWDBasicModel
    {
        public ulong Reference;
        public int Quantity;
        public NWDConditional Condition;
    }
}
#nullable disable