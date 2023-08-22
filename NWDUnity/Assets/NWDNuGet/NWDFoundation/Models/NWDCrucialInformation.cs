using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDCrucialInformation : NWDBasicModel
    {
        #region Properties

        /// <summary>
        /// Project Unique Identity
        /// </summary>
        public string Key { set; get; } = string.Empty;

        /// <summary>
        /// Unique Reference of instance
        /// </summary>
        public string Value { set; get; } = string.Empty;

        #endregion

        #region Constructors

        public NWDCrucialInformation()
        {
        }

        #endregion
    }
}