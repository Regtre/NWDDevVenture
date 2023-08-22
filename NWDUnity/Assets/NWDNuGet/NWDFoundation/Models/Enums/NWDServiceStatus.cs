using System;

namespace NWDFoundation.Models.Enums
{
    /// <summary>
    /// Define the Service status in NWDServer : active, inactive
    /// </summary>
    [Serializable]
    public enum NWDServiceStatus
    {
        /// <summary>
        /// Service is active
        /// </summary>
        Active = 0,
        /// <summary>
        /// Service is inactive
        /// </summary>
        Inactive = 2,
    }
}