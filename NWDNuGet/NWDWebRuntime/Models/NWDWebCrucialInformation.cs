using NWDFoundation.Models;

namespace NWDWebRuntime.Models
{
    [Serializable]
    public class NWDWebCrucialInformation : NWDDatabaseWebBasicModel
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

        public NWDWebCrucialInformation()
        {
        }

        #endregion
    }
}