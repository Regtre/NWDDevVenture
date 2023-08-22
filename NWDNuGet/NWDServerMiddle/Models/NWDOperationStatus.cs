using System;

namespace NWDServerMiddle.Models
{
    [Serializable]
    public enum NWDOperationStatus : int
    {
        Ko = -1,
        Unknown = 0,
        Ok = 1,
    }
}