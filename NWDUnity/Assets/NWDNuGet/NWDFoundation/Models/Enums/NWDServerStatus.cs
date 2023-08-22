using System;

namespace NWDFoundation.Models.Enums
{
    /// <summary>
    /// Define the Server status in NWDServer : active, inactive, upgrading database
    /// </summary>
    [Serializable]
    public enum NWDServerStatus
    {
        /// <summary>
        /// Server is active
        /// </summary>
        Active = 0,
        /// <summary>
        /// Server is upgrading databases ... come back later
        /// </summary>
        Upgrading = 1,
        /// <summary>
        /// Server is inactive
        /// </summary>
        Inactive = 2,
    }
}