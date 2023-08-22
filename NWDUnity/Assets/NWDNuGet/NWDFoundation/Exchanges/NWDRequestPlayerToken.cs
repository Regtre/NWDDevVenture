using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades;

namespace NWDFoundation.Exchanges
{
    [Serializable]
    public class NWDRequestPlayerToken
    {
        #region properties

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Ran")]
        public ushort AccountRange { set; get; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Prj")]
        public ulong ProjectId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Pla")]
        public ulong PlayerReference { set; get; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Tok")]
        public string? Token { set; get; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Oto")]
        public string? OldToken { set; get; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Env")]
        public NWDEnvironmentKind EnvironmentKind { set; get; }
        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Exo")]
        public NWDExchangeOrigin ExchangeOrigin { set; get; }
        public NWDSyncInformation PlayerSyncInformation { set; get; } = new NWDSyncInformation();
        public NWDSyncInformation StudioSyncInformation { set; get; } = new NWDSyncInformation();

        #endregion

        #region constructor

        [Obsolete("NEVER USE! RESERVED TO JSON CONVERT!")]
        public NWDRequestPlayerToken()
        {
        }

        public NWDRequestPlayerToken(INWDProjectInformation sProjectInformation)
        {
            AccountRange = 0;
            PlayerReference = 0;
            ProjectId = sProjectInformation.GetProjectId();
            Token = string.Empty;
            OldToken = string.Empty;
            EnvironmentKind = sProjectInformation.GetProjectEnvironment();
            ExchangeOrigin = NWDExchangeOrigin.Unknown;
        }

        public NWDRequestPlayerToken(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            AccountRange = 0;
            PlayerReference = 0;
            ProjectId = sProjectId;
            Token = string.Empty;
            OldToken = string.Empty;
            EnvironmentKind = sEnvironmentKind;
            ExchangeOrigin = NWDExchangeOrigin.Unknown;
        }

        public NWDRequestPlayerToken(NWDRequestPlayerToken? sToCopy)
        {
            if (sToCopy != null)
            {
                AccountRange = sToCopy.AccountRange;
                PlayerReference = sToCopy.PlayerReference;
                ProjectId = sToCopy.ProjectId;
                Token = sToCopy.Token;
                OldToken = sToCopy.OldToken;
                PlayerSyncInformation = new NWDSyncInformation(sToCopy.PlayerSyncInformation);
                StudioSyncInformation = new NWDSyncInformation(sToCopy.StudioSyncInformation);
                EnvironmentKind = sToCopy.EnvironmentKind;
                ExchangeOrigin = sToCopy.ExchangeOrigin;
            }
        }

        #endregion
    }
}