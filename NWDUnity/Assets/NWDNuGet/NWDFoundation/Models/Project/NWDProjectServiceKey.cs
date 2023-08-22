using System;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDProjectServiceKey : NWDBasicModel
    {
        /// <summary>
        /// Environment of this instance (use to change the table usage)
        /// </summary>
        public NWDEnvironmentKind EnvironmentKind { set; get; } = NWDEnvironmentKind.Dev;
        /// <summary>
        /// The project status : active, inactive, upgrading data
        /// </summary>
        public NWDServiceStatus Status { set; get; } = NWDServiceStatus.Active;
        /// <summary>
        /// The readable name
        /// </summary>
        public string Name  { set; get; } = string.Empty;
        /// <summary>
        /// The readable description
        /// </summary>
        public string Description  { set; get; } = string.Empty;
        // /// <summary>
        // /// The Apple ProductId  
        // /// </summary>
        // public string AppleProductId  { set; get; } = string.Empty;
        // /// <summary>
        // /// The Google ProductId  
        // /// </summary>
        // public string GoogleProductId  { set; get; } = string.Empty;
        public long ServiceId { set; get; }
        public NWDServiceKind ServiceKind { set; get; } = NWDServiceKind.Session;
        public NWDServiceOfflineUsage OfflineUsage { set; get; } = NWDServiceOfflineUsage.OffLineUnlimited;
        public uint OfflineCounterReserve { set; get; } = 0;
        public NWDProjectServiceKey()
        {
            
        }
    }
}