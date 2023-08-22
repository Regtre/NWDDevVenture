using System;

namespace NWDUnityEditor.Windows
{
    [Serializable]
    public enum NWDMetaDataLockerState
    {
        Unlock = 0,
        LockByMe = 1,
        LockByAnother = 9,
    }
}