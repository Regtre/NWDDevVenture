using System;

namespace NWDEditor
{
    [Serializable]
    public enum NWDSubMetaDataState : int
    {
        None =              0,
        VisibleInBuild =    1 << 0,
        Overriden =         1 << 1,
        Outdated =          1 << 2,
        All =               ~0
    }
}