using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDFoundation.Configuration.Permissions
{
    [Serializable]
    public class NWDProjectDescription
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public ulong Project { get; set; }
        public ulong ProjectId { get; set; }
        public string HubDnsHttps { get; set; } = string.Empty;
        public string ServerDnsHttpsFormat { get; set; } = string.Empty;

        public List<NWDServiceKeyDescription> ServiceKeys { set; get; } = new List<NWDServiceKeyDescription>();
        public NWDProjectCredentials[] Keys { get; set; } = new NWDProjectCredentials[]{};
        public string PublicToken { set; get; } = string.Empty;
        public string SecretToken { set; get; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public bool CanEditMetaDataInfos { set; get; } = false;
        public bool CanCreateMetaData { set; get; } = false;
        public NWDDataTrackDescription[]? Track { get; set; }
        public Dictionary<int, NWDTrackRights> TracksRights { get; set; } = new Dictionary<int, NWDTrackRights>();
        
        public string BaseLanguage { set; get; } = "en-US";
        public string[] SupportLanguages { set; get; } = new string[] { }; // must stay empty else it's populated by adds
    }
}