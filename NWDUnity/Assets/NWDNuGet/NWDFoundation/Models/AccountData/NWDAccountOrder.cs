using System;
using System.Text;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDAccountOrder : NWDAccountData
    {
        #region properties

        /// <summary>
        /// Name of Invoices (to show in list)
        /// </summary>
        public string Name { set; get; } = string.Empty;

        /// <summary>
        /// Payload to use to see anonymized order
        /// </summary>
        public string Payload { set; get; } = string.Empty;

        /// <summary>
        /// Url to see non anonymized order, personal information are not recorded on Net-Worked-Data
        /// </summary>
        public string Url { set; get; } = string.Empty;

        #endregion
    }
}