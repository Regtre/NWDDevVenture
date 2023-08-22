using System;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Tools;

namespace NWDFoundation.Exchanges
{
    [Serializable]
    public abstract class NWDBasicExchange
    {
        public string Dll { set; get; } = NWDVersionDll.Version;
        public string IdName { set; get; } = NWDRandom.RandomString(8);
        public ulong ProjectId { set; get; }
        public NWDEnvironmentKind Environment { set; get; }
        public string Stamp { set; get; } = string.Empty;
        public string Hash { set; get; } = string.Empty;

        public string Payload { set; get; } = string.Empty;
        public int Timestamp { set; get; }
        
    }
}

