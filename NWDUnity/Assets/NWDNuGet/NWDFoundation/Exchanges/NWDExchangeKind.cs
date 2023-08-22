using System;

namespace NWDFoundation.Exchanges
{
    [Serializable]
    public enum NWDExchangeKind
    {
        /// <summary>
        /// No exchange kind (!)
        /// </summary>
        None = -9,

        /// <summary>
        /// Use to test connection : ok or ko
        /// </summary>
        Test = -1,

        /// <summary>
        /// Not yet specified
        /// </summary>
        Unknown = 0,
    }
}