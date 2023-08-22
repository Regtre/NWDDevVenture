using NWDFoundation.Configuration.Environments;
using System.Collections.Generic;

namespace NWDEditor
{
    public class NWDSubMetaData
    {
        public int TrackId { get; set; }
        public NWDEnvironmentKind TrackKind { get; set; }
        public NWDSubMetaDataState State { get; set; }
        public ulong Origin { get; set; }
        public string Data { get; set; } = string.Empty;
        public bool Trashed { get; set; }
    }
}
