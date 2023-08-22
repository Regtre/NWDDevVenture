using System;

namespace NWDFoundation.Models.Enums
{
    /// <summary>
    /// Define the project part status in NWDServer : new, active, upgrading, inactive
    /// </summary>
    [Serializable]
    public enum NWDProjectPartStatus
    {
        /// <summary>
        /// This part is new and no publish
        /// </summary>
        New = 0,
        /// <summary>
        /// This part is active so published
        /// </summary>
        Active = 1,
        /// <summary>
        /// This part is upgrading need to be publish
        /// </summary>
        Upgrading = 2,
        /// <summary>
        /// This part is inactive
        /// </summary>
        Inactive = 3,
    }
}