using System;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDProjectCredentials : NWDBasicModel
    {
        /// <summary>
        /// Environment of this instance (use to change the table usage)
        /// </summary>
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
        /// </summary>
        public string SecretKey { set; get; } = NWDRandom.RandomStringToken(128);

        public NWDProjectCredentials()
        {
            
        }
        //
        // public NWDProjectCredentials(NWDProjectByEnvironment sProjectByEnvironment)
        // {
        //     GetProjectId = sProjectByEnvironment.GetProjectId;
        //     Reference = sProjectByEnvironment.Reference;
        //     Trashed = sProjectByEnvironment.Trashed;
        //     EnvironmentKind = sProjectByEnvironment.EnvironmentKind;
        //     Status = sProjectByEnvironment.Status;
        //     TreatKey = sProjectByEnvironment.TreatKey;
        //     ProjectKey = sProjectByEnvironment.ProjectKey;
        // }
    }
}