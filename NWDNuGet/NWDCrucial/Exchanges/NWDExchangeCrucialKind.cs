using System;
using NWDFoundation.Exchanges;

namespace NWDCrucial.Exchanges
{
    [Serializable]
    public enum NWDExchangeCrucialKind
    {
        /// <summary>
        /// No exchange kind (!)
        /// </summary>
        None = NWDExchangeKind.None,

        /// <summary>
        /// Use to test connection : ok or ko
        /// </summary>
        Test = NWDExchangeKind.Test,

        /// <summary>
        /// Not yet specified
        /// </summary>
        Unknown = NWDExchangeKind.Unknown,

        /// <summary>
        /// Publish project CRUD Environment by Project, CRUD Track associated, CRUD ServiceKey etc.
        /// </summary>
        // TODO : Implement in manager
        ProjectPublish = 110,

        /// <summary>
        /// Clean project : Remove old player from legacy date and delete old Data Model 
        /// </summary>
        // TODO : Implement in manager
        ProjectClean = 120,

        /// <summary>
        /// Delete project : Remove all PlayerData, StudioData, Environment, Service, etc ... use GetProjectId to "Tag Delete" in AllBases
        /// </summary>
        // TODO : Implement in manager
        ProjectDelete = 130,

        /// <summary>
        /// Publish StudioData, authorized StudioData is managed by Hub (Hub keep save of previous version of data)
        /// </summary>
        // TODO : Implement in manager
        PublishStudioData = 200,

        /// <summary>
        /// Publish AccountData, authorized AccountData is managed by Hub
        /// </summary>
        // TODO : Implement in manager
        AlterAccountData = 300,

        /// <summary>
        /// Publish PlayerData, authorized PlayerData is managed by Hub
        /// </summary>
        // TODO : Implement in manager
        AlterPlayerData = 400,
        
        /// <summary>
        /// Associate a service to a Account
        /// </summary>
        AssociateService = 500,
        
        /// <summary>
        /// Create a service for a project
        /// </summary>
        CreateService = 501,
        
        AssociateSubService = 502,
        
        DissociateServiceAndSubServices = 503,
        GetSubServicesFromAccount = 504,
        

        /// <summary>
        /// Operation Disable on Servers and AllBases
        /// </summary>
        // TODO : Implement in manager
        DisableAllServers = 900,

        /// <summary>
        /// Operation Enable on Servers and AllBases
        /// </summary>
        // TODO : Implement in manager
        EnableAllServers = 901,

        /// <summary>
        /// Operation Disable on this Server
        /// </summary>
        // TODO : Implement in manager
        DisableThisServer = 910,

        /// <summary>
        /// Operation Enable on this Server
        /// </summary>
        // TODO : Implement in manager
        EnableThisServer = 911,

        /// <summary>
        /// Operation Upgrade on Servers and AllBases
        /// </summary>
        // TODO : Implement in manager
        UpgradeAllBases = 990,

        /// <summary>
        /// Operation Optimize on Servers and AllBases
        /// </summary>
        // TODO : Implement in manager
        OptimizeAllBases = 991,

        /// <summary>
        /// Operation Clean : Remove old player from legacy date and delete old Data Model
        /// </summary>
        // TODO : Implement in manager
        CleanAllBases = 992,
    }
}