using System;

namespace NWDFoundation.Logger
{
    [Serializable]
    public enum NWDLogCategory : short
    {
        No = 0,
        Todo = 1,
        Success = 2,
        Failed = 4,
        Error = 8,
        Attention = 16,
        Exception = 32,
    }
}