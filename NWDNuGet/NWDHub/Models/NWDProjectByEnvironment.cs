using System;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDCrucial.Models;
using NWDFoundation.WebEdition.Attributes;

namespace NWDHub.Models
{
    /// <summary>
    /// Define Project information by environment for NWDServer
    /// </summary>
    [Serializable]
    //TODO rename NWDProjectEnvironment
    public class NWDProjectByEnvironment : NWDProjectSubObject //TODO It's Player Data transform to Project data in server
    {
        /// <summary>
        /// Customizable name of this environment 
        /// </summary>
        public string Name { set; get; } = string.Empty;
        /// <summary>
        /// Tracks number for this environment 
        /// </summary>
        public ulong[] Tracks { set; get; } = new ulong[]{0};
        public NWDEnvironmentKind EnvironmentKind { set; get; } = NWDEnvironmentKind.Dev;
        /// <summary>
        /// The project status : active, inactive, upgrading data
        /// </summary>
        public NWDEnvironmentStatus Status { set; get; } = NWDEnvironmentStatus.Active;
        /// <summary>
        /// The TreatKey for this project and this environment, use only by editor administration
        /// </summary>
        public string TreatKey { set; get; } = NWDRandom.RandomStringToken(128);
        /// <summary>
        /// The ProjectKey for this project and this environment, use by runtime player to update player's data 
        /// </summary>
        public string ProjectKey { set; get; } = NWDRandom.RandomStringToken(128);
        public string SecretKey { set; get; } = NWDRandom.RandomStringToken(128);
        
        public NWDProjectCredentials ConvertToProjectCredentials()
        {
            NWDProjectCredentials rReturn = new NWDProjectCredentials()
            {
                ProjectId = this.ProjectUniqueId,
                Reference = this.Reference,
                Trashed = this.Trashed,
                EnvironmentKind = this.EnvironmentKind,
                Status = this.Status,
                TreatKey = this.TreatKey,
                ProjectKey = this.ProjectKey,
                SecretKey = this.SecretKey,
            };
            return rReturn;
        }
        public NWDProjectByEnvironment()
        {
        }
        
        public NWDProjectByEnvironment(NWDProject sProject)
        {
            this.AssociateToProject(sProject);
        }
    }
}