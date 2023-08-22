using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public abstract class NWDVolatileData : NWDBasicModel
    {
        /// <summary>
        /// Anonymous Unique Identity
        /// </summary>
        public string Anonymous { set; get; } = string.Empty;
    }
}