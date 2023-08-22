using System;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDCrucial.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;
using Org.BouncyCastle.Tls.Crypto;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDProjectService : NWDProjectSubObject
    {
        #region properties
        public NWDEnvironmentKind EnvironmentKind { set; get; }
        [NWDWebPropertyDescription("",NWDWebEditionStyle.Integer,false, "","","")]
        public NWDServiceStatus Status { set; get; } = NWDServiceStatus.Active;
        /// <summary>
        /// The readable name
        /// </summary>
        [NWDWebPropertyDescription("Name of service",NWDWebEditionStyle.Text,false, "","","", true,true,true)]
        public string Name  { set; get; } = string.Empty;
        /// <summary>
        /// The readable description
        /// </summary>
        [NWDWebPropertyDescription("Description of service",NWDWebEditionStyle.Text,false, "","","")]
        public string Description  { set; get; } = string.Empty;
        /// <summary>
        /// The Apple ProductId  
        /// </summary>
        // [NWDWebPropertyDescription("AppleProductId",NWDWebEditionStyle.Text, "","","")]
        // public string AppleProductId  { set; get; } = string.Empty;
        // /// <summary>
        // /// The Google ProductId  
        // /// </summary>
        // [NWDWebPropertyDescription("GoogleProductId",NWDWebEditionStyle.Text, "","","")]
        // public string GoogleProductId  { set; get; } = string.Empty;
        [NWDWebPropertyDescription("ServiceId",NWDWebEditionStyle.Integer,false, "","","", false, true, true)]
        public short ServiceId { set; get; }
        public NWDServiceKind ServiceKind { set; get; } = NWDServiceKind.Session;
        public NWDServiceOfflineUsage OfflineUsage { set; get; } = NWDServiceOfflineUsage.OffLineUnlimited;
        public uint OfflineCounterReserve { set; get; } = 0;
        #endregion

        #region constructors
        public NWDProjectService()
        {
        }
        public NWDProjectService(NWDProject sProject)
        {
            this.AssociateToProject(sProject);
        }
        public NWDProjectServiceKey ConvertToProjectServiceKey()
        {
            NWDProjectServiceKey rReturn = new()
            {
                ProjectId = this.ProjectId,
                Reference = this.Reference,
                Trashed = this.Trashed,
                EnvironmentKind = this.EnvironmentKind,
                Status = this.Status,
                Name = this.Name,
                Description = this.Description,
                // AppleProductId = this.AppleProductId,
                // GoogleProductId = this.GoogleProductId,
                ServiceId = this.ServiceId,
                ServiceKind = this.ServiceKind,
                OfflineUsage = this.OfflineUsage,
                OfflineCounterReserve = this.OfflineCounterReserve,
            };
         return  rReturn;
        }
        #endregion
    }
}
