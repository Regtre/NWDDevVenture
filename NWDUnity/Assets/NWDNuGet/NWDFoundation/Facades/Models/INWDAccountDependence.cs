using System;
using System.Security.Cryptography.X509Certificates;

namespace NWDFoundation.Models
{
    public interface INWDAccountDependence : INWDAccountRange
    {
        /// <summary>
        /// Reference to Account
        /// </summary>
        public ulong Account { set; get; }

    }
}