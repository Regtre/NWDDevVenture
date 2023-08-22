using System;
using NWDFoundation.Exchanges;

namespace NWDRuntime.Exchanges
{
    [Serializable]
    public enum NWDExchangeLicenseKind
    {
        /// <summary>
        /// No exchange kind (!)
        /// </summary>
        None = NWDExchangeKind.None,

        /// <summary>
        /// Use to test connection : ok or ko
        /// </summary>
        Test = NWDExchangeKind.Test,

        /// <summary>
        /// Not yet specified
        /// </summary>
        Unknown = NWDExchangeKind.Unknown,

        CheckLicense = 110,
    }
}