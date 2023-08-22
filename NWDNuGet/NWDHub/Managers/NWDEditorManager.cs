using Newtonsoft.Json;
using NWDEditor;
using NWDEditor.Exchanges;
using NWDEditor.Exchanges.Payloads;
using NWDEditor.Facades;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDHub.Configuration;
using NWDHub.Models;
using NWDTreat.Configuration;
using NWDWebRuntime.Configuration;

namespace NWDHub.Managers
{
    public class NWDEditorManager : INWDEditorManager
    {
        #region Methods
        public NWDResponseEditor Process(NWDRequestEditor sRequest)
        {
            NWDResponseEditor rReturn = new NWDResponseEditor(NWDExchangeEditorKind.Unknown, null, NWDRequestStatus.Error);
            switch (sRequest.Kind)
            {
                case NWDExchangeEditorKind.Unknown:
                    rReturn = new NWDResponseEditor(NWDExchangeEditorKind.Unknown, null, NWDRequestStatus.Error);
                    break;
                case NWDExchangeEditorKind.GetProjectSettings:
                    rReturn = GetProjectSettings(sRequest);
                    break;
                case NWDExchangeEditorKind.MetaDataLock:
                    rReturn = MetaDataLock(sRequest);
                    break;
                case NWDExchangeEditorKind.MetaDataUnlock:
                    rReturn = MetaDataUnlock(sRequest);
                    break;
                case NWDExchangeEditorKind.SyncMetaData:
                    rReturn = SyncMetaData(sRequest);
                    break;
                case NWDExchangeEditorKind.CreateMetaData:
                    rReturn = CreateMetaData(sRequest);
                    break;
                case NWDExchangeEditorKind.Test:
                    rReturn = TestConnexion(sRequest);
                    break;
                default:
                    rReturn = new NWDResponseEditor(NWDExchangeEditorKind.Unknown, null, NWDRequestStatus.Error);
                    break;
            }
            return rReturn;
        }

        private NWDResponseEditor CreateMetaData(NWDRequestEditor sRequest)
        {
            NWDResponseEditor rReturn = new NWDResponseEditor(NWDExchangeEditorKind.None, null, NWDRequestStatus.Error);
            NWDProjectDescription? tProjectDescription = NWDProjectDescriptionManager.GetByPublicToken(sRequest.RolePublicKey);
            if (tProjectDescription != null && sRequest.IsValid(tProjectDescription.SecretToken) && tProjectDescription.CanCreateMetaData)
            {
                NWDUpPayloadCreateMetaData? tUpPayload = sRequest.GetPayload<NWDUpPayloadCreateMetaData>();
                if (tUpPayload != null)
                {
                    rReturn = new NWDResponseEditor(NWDExchangeEditorKind.CreateMetaData, new NWDDownPayloadCreateMetaData(NWDMetaDataManager.CreateMetaData(tUpPayload.TypeCLass, tProjectDescription)), NWDRequestStatus.Ok, tProjectDescription.SecretToken);
                }
            
                
            }
            return rReturn;
        }

        private NWDResponseEditor SyncMetaData(NWDRequestEditor sRequest)
        {
            NWDResponseEditor rReturn = new NWDResponseEditor(NWDExchangeEditorKind.None, null, NWDRequestStatus.Error);
            NWDProjectDescription? tProjectDescription = NWDProjectDescriptionManager.GetByPublicToken(sRequest.RolePublicKey);
            if(tProjectDescription != null && sRequest.IsValid(tProjectDescription.SecretToken))
            {
                NWDUpPayloadSyncMetaData? tUpPayload = sRequest.GetPayload<NWDUpPayloadSyncMetaData>();
                if (tUpPayload != null)
                {
                    rReturn = new NWDResponseEditor(NWDExchangeEditorKind.SyncMetaData, new NWDDownPayloadSyncMetaData(NWDMetaDataManager.SyncMetaData(tUpPayload.MetaDataList, tProjectDescription)), NWDRequestStatus.Ok, tProjectDescription.SecretToken);
                }
            }
            return rReturn; 
        }

        private NWDResponseEditor MetaDataUnlock(NWDRequestEditor sRequest)
        {
            NWDResponseEditor rReturn = new NWDResponseEditor(NWDExchangeEditorKind.None, null, NWDRequestStatus.Error);
            NWDProjectDescription? tProjectDescription = NWDProjectDescriptionManager.GetByPublicToken(sRequest.RolePublicKey);
            if(tProjectDescription != null && sRequest.IsValid(tProjectDescription.SecretToken))
            {
                NWDUpPayloadMetaDataUnlock? tUpPayload = sRequest.GetPayload<NWDUpPayloadMetaDataUnlock>();
                if (tUpPayload != null)
                {
                    rReturn = new NWDResponseEditor(NWDExchangeEditorKind.MetaDataUnlock, new NWDDownPayloadUnlockMeta(NWDMetaDataManager.UnlockMetaData(tUpPayload.MetaData, tUpPayload.LockerName, tProjectDescription)), NWDRequestStatus.Ok, tProjectDescription.SecretToken);
                }
            }
            return rReturn; 
        }
        private NWDResponseEditor MetaDataLock(NWDRequestEditor sRequest)
        {
            NWDResponseEditor rReturn = new NWDResponseEditor(NWDExchangeEditorKind.None, null, NWDRequestStatus.Error);
            NWDProjectDescription? tProjectDescription = NWDProjectDescriptionManager.GetByPublicToken(sRequest.RolePublicKey);
            if(tProjectDescription != null && sRequest.IsValid(tProjectDescription.SecretToken))
            {
                NWDUpPayloadMetaDataLock? tUpPayload = sRequest.GetPayload<NWDUpPayloadMetaDataLock>();
                if (tUpPayload != null)
                {
                    rReturn = new NWDResponseEditor(NWDExchangeEditorKind.MetaDataLock, new NWDDownPayloadLockMetaData(NWDMetaDataManager.LockMetaData(tUpPayload.MetaDataReference, tUpPayload.LockerName, tProjectDescription)), NWDRequestStatus.Ok, tProjectDescription.SecretToken);
                }
                
            }
            return rReturn; 
        }
        public NWDResponseEditor GetProjectSettings(NWDRequestEditor sRequest)
        {
            NWDResponseEditor rReturn = new NWDResponseEditor(NWDExchangeEditorKind.None, null, NWDRequestStatus.Error);
            // TODO : NWDProjectDescriptionManager is use twice to get the NWDProjectDescriptionStorage, here and at start of this fonction 
            NWDProjectDescriptionStorage? tProjectDescriptionStorage = NWDProjectDescriptionManager.GetProjectDescriptionStorageByPublicToken(sRequest.RolePublicKey);
            if (tProjectDescriptionStorage != null)
            {
                NWDProjectDescription? tProjectDescriptionWithoutKey = JsonConvert.DeserializeObject<NWDProjectDescription>(tProjectDescriptionStorage.Json);
                if (tProjectDescriptionWithoutKey != null && sRequest.IsValid(tProjectDescriptionWithoutKey.SecretToken))
                {
                    string tSecretToken = tProjectDescriptionWithoutKey.SecretToken;
                    // clean key for role request
                    tProjectDescriptionWithoutKey.SecretToken = string.Empty;
                    tProjectDescriptionWithoutKey.PublicToken = string.Empty;
                    // add info from hub and server
                    tProjectDescriptionWithoutKey.HubDnsHttps = "https://" + NWDWebRuntimeConfiguration.KConfig.GetHubDnsClean();
                    tProjectDescriptionWithoutKey.ServerDnsHttpsFormat = "https://" + NWDWebRuntimeConfiguration.KConfig.GetServerFormatDnsClean();
                    // clean key for role

                    // TODO: Début d'un fix temporaire.
                    // Comme les données de credentials ne sont pas sauvegardés au moment du "Publish" du projet
                    // Les données sont ajoutées manuellement ici.
                    List<NWDProjectTreatStorage>? tKeys = NWDProjectTreatManager.GetAllByProjectUniqueId(tProjectDescriptionWithoutKey.ProjectId);
                    tProjectDescriptionWithoutKey.Keys = tKeys.Select(x =>
                    {
                        NWDProjectCredentials tCredential = x.ConvertToProjectCredentials();
                        tCredential.SecretKey = string.Empty;
                        tCredential.Modification = tProjectDescriptionStorage.Modification;
                        tCredential.Creation = tProjectDescriptionStorage.Creation;
                        return tCredential;
                    }).ToArray();
                    // TODO: Fin du fix temporaire
                    
                    NWDDownPayloadGetProjectSettings tDownPayload = new NWDDownPayloadGetProjectSettings
                    {
                        ProjectDescription = tProjectDescriptionWithoutKey
                    };
                    rReturn = new NWDResponseEditor(NWDExchangeEditorKind.GetProjectSettings, tDownPayload, NWDRequestStatus.Ok, tSecretToken);
                }
            }

            return rReturn;
        }

        /// <summary>
        /// Simple method to test if a URL is a valid NWD server.
        /// This method should be accessed without any authentication or role.
        /// </summary>
        /// <param name="sRequest">The request recieved.</param>
        /// <returns>An OK response.</returns>
        public NWDResponseEditor TestConnexion(NWDRequestEditor sRequest)
        {
            NWDResponseEditor rReturn = new NWDResponseEditor(NWDExchangeEditorKind.Test, null, NWDRequestStatus.Ok);
            return rReturn;
        }
        #endregion

    }
}