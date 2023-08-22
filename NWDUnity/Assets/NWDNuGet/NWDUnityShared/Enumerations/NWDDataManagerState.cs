using System;

namespace NWDUnityShared.Enumerations
{
    [Flags]
    public enum NWDDataManagerState
    {
        Stopped = 0,
        Initializing = 1,
        Updating = 2,
        Ready = 3
    }
}
