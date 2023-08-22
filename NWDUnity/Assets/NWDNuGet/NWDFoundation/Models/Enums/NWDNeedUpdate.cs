using System;

namespace NWDFoundation.Models.Enums
{
    /// <summary>
    /// Define the status of upgrade
    /// </summary>
    [Serializable]
    public enum NWDNeedUpdate
    {
        Unknow = 0,
        Update = 1,
        Upgrade = 2,
        UpgradeNow = 4,
    }
}