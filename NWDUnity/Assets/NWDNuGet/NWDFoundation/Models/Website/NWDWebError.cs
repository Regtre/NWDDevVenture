using System;

namespace NWDFoundation.Models
{
[Serializable]
    public class NWDWebError
    {
        public int Error { set; get; } = 666;
        public string OriginalUrl { set; get; } = string.Empty;
        public string Title { set; get; } = string.Empty;
        public string Description { set; get; } = string.Empty;
        public string DebugLog { set; get; } = string.Empty;
    }
}