using System;

namespace NWDFoundation.Models.Enums
{
    /// <summary>
    /// Define the Environment status in NWDServer : active, inactive
    /// </summary>
    [Serializable]
    public enum NWDEnvironmentStatus
    {
        /// <summary>
        /// Environment is active
        /// </summary>
        Active = 0,
        /// <summary>
        /// Environment is inactive
        /// </summary>
        Inactive = 2,
    }
}