using System;

namespace NWDFoundation.Models.Enums
{
    /// <summary>
    /// Define the status of upgrade
    /// </summary>
    [Serializable]
    public enum NWDLicenseStatus
    {
        Unknow = 0,
        Valid = 1,
        Invalid = 2,
    }
}