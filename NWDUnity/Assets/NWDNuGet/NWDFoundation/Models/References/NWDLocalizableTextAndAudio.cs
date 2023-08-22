using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDLocalizableTextAndAudio: NWDDataType
    {
        public string Base { set; get; } = string.Empty;
        public Dictionary<string, string> Values { set; get; } =new Dictionary<string, string>();
        public Dictionary<string, string> Audios { set; get; } =new Dictionary<string, string>();
    }
}
