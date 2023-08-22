using System;

namespace NWDUnityEditor.Exchanges
{
    [Serializable]
    public enum NWDMetaDataRequestAction : int
    {
        Lock,
        Unlock,
        Update,
    }
}