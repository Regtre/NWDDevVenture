using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDFoundation.Configuration.Permissions
{
    [Serializable]
    public class NWDServiceKeyDescription
    {
        public string Name  { set; get; } = string.Empty;
        public string Description  { set; get; } = string.Empty;
        public long ServiceId { set; get; }
        public NWDServiceKind ServiceKind { set; get; } = NWDServiceKind.Session;
        public NWDServiceOfflineUsage OfflineUsage { set; get; } = NWDServiceOfflineUsage.OffLineUnlimited;
        public uint OfflineCounterReserve { set; get; } = 0;

        public NWDServiceKeyDescription(NWDProjectServiceKey sProjectServiceKey)
        {
            
        }
    }
}