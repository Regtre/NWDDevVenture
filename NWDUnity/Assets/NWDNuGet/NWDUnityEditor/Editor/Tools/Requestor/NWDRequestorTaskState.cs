using System;

namespace NWDUnityEditor.Tools
{
    [Flags]
    public enum NWDRequestorTaskState
    {
        None =      0,
        Waiting =   1,
        Executing = 2,
        Success =      3,
        Error =     4,
    }
}

