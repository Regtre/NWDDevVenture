using System;
using System.Text;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDAccountInvoice : NWDAccountData
    {
        #region properties

        /// <summary>
        /// Name of Invoices (to show in list)
        /// </summary>
        public string Name { set; get; } = string.Empty;

        /// <summary>
        /// Payload to use to see anonymized invoice 
        /// </summary>
        public string Payload { set; get; } = string.Empty;
        /// <summary>
        /// Url to see non anonymized invoice, personal information are not recorded on Net-Worked-Data
        /// </summary>
        public string Url { set; get; } = string.Empty;

        #endregion

    }
}