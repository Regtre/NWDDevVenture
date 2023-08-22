using System;
using NWDFoundation.Exchanges;

namespace NWDEditor.Exchanges
{
    [Serializable]
    public enum NWDExchangeEditorKind
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

        /// <summary>
        /// Authenticate by role or account and get project settings json with rights
        /// </summary>
        GetProjectSettings = 100,

        /// <summary>
        /// Lock metadata in hub for specific nickname
        /// </summary>
        SyncMetaData = 200,

        /// <summary>
        /// Lock metadata in hub for specific nickname
        /// </summary>
        MetaDataLock = 201,

        /// <summary>
        /// Unlock metadata in hub for specific nickname (metadata sync at same time)
        /// </summary>
        MetaDataUnlock = 202,
        
        /// <summary>
        /// Create metadata 
        /// </summary>
        CreateMetaData = 203,
    }
}