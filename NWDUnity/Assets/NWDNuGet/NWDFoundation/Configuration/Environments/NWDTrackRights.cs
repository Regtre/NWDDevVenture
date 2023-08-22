using System;
using NWDFoundation.Models;

namespace NWDFoundation.Configuration.Environments
{
    [Serializable]
    //[Flags]
    public enum NWDTrackRights 
    {
        /// <summary>
        /// No rights, not readable, not writable
        /// </summary>
        None = 0,
        /// <summary>
        /// Can read data in this environment and copy from this environment to a writable environment
        /// </summary>
        Read = 2,
        /// <summary>
        /// Can write data in this environment
        /// </summary>
        Write = 4,
    }
}