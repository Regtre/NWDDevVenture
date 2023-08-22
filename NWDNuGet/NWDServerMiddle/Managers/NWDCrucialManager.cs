using System;
using NWDCrucial.Configuration;
using NWDCrucial.Exchanges;
using NWDCrucial.Exchanges.Payloads;
using NWDCrucial.Facades;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models.Enums;
using NWDServerMiddle.Configuration;
using NWDServerMiddle.Managers.ModelManagers;
using NWDServerShared.Configuration;

namespace NWDServerMiddle.Managers
{
    public class NWDCrucialManager : INWDCrucialManager
    {
        #region Methods

        public NWDResponseCrucial Process(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (NWDServerConfiguration.KConfig.IsOverFlow() == false)
            {
                if (NWDServerConfiguration.KConfig.Status == NWDServerStatus.Active)
                {
                    switch (sRequest.Kind)
                    {
                        case NWDExchangeCrucialKind.None:
                            rReturn = None(sRequest);
                            break;
                        case NWDExchangeCrucialKind.Test:
                            rReturn = Test(sRequest);
                            break;
                        case NWDExchangeCrucialKind.Unknown:
                            rReturn = Unknown(sRequest);
                            break;
                        case NWDExchangeCrucialKind.ProjectPublish:
                            rReturn = ProjectPublish(sRequest);
                            break;
                        case NWDExchangeCrucialKind.ProjectClean:
                            rReturn = ProjectClean(sRequest);
                            break;
                        case NWDExchangeCrucialKind.ProjectDelete:
                            rReturn = ProjectDelete(sRequest);
                            break;
                        case NWDExchangeCrucialKind.PublishStudioData:
                            rReturn = PublishStudioData(sRequest);
                            break;
                        case NWDExchangeCrucialKind.AlterAccountData:
                            rReturn = AlterAccountData(sRequest);
                            break;
                        case NWDExchangeCrucialKind.AlterPlayerData:
                            rReturn = AlterPlayerData(sRequest);
                            break;
                        case NWDExchangeCrucialKind.DisableAllServers:
                            rReturn = DisableAllServers(sRequest);
                            break;
                        case NWDExchangeCrucialKind.EnableAllServers:
                            rReturn = EnableAllServers(sRequest);
                            break;
                        case NWDExchangeCrucialKind.DisableThisServer:
                            rReturn = DisableAllServers(sRequest);
                            break;
                        case NWDExchangeCrucialKind.EnableThisServer:
                            rReturn = EnableAllServers(sRequest);
                            break;
                        case NWDExchangeCrucialKind.UpgradeAllBases:
                            rReturn = UpgradeAllBases(sRequest);
                            break;
                        case NWDExchangeCrucialKind.OptimizeAllBases:
                            rReturn = OptimizeAllBases(sRequest);
                            break;
                        case NWDExchangeCrucialKind.CleanAllBases:
                            rReturn = CleanAllBases(sRequest);
                            break;
                        case NWDExchangeCrucialKind.AssociateService:
                            rReturn = AssociateService(sRequest);
                            break;
                        case NWDExchangeCrucialKind.AssociateSubService:
                            rReturn = AssociateSubService(sRequest);
                            break;
                        case NWDExchangeCrucialKind.DissociateServiceAndSubServices:
                            rReturn = DissociateServiceAndSubServices(sRequest);
                            break;
                        case NWDExchangeCrucialKind.GetSubServicesFromAccount:
                            rReturn = GetSubServicesFromAccount(sRequest);
                            break;
                        // default:
                        //     rReturn = new NWDResponseCrucial(NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
                        //     rReturn.ServerIdentity = NWDConfigurationServer.KConfig.GetServerIdentity();
                        //     throw new ArgumentOutOfRangeException();
                        //     break;
                    }
                }
                else
                {
                    rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig, NWDExchangeCrucialKind.None,
                        null, NWDRequestStatus.ServerIsDisabled);
                    rReturn.ServerIdentity = NWDServerConfiguration.KConfig.GetServerIdentity();
                }
            }
            else
            {
                rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig, NWDExchangeCrucialKind.None,
                    null, NWDRequestStatus.PleaseChangeServer);
                rReturn.ServerIdentity = NWDServerConfiguration.KConfig.GetServerIdentity();
            }

            return rReturn;
        }

       

        private NWDResponseCrucial AssociateService(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn;
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig, sRequest.Kind,
                    NWDAccountServiceManager.AssociateService(
                        sRequest.GetPayload<NWDUpPayloadAssociateService>(NWDServerMiddleConfiguration.KConfig)),
                    NWDRequestStatus.Ok);
            }
            else
            {
                rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig, NWDExchangeCrucialKind.Unknown,
                    null, NWDRequestStatus.Error);
            }

            return rReturn;
        }

        private NWDResponseCrucial AssociateSubService(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn;
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig, sRequest.Kind,
                    NWDAccountServiceManager.AssociateSubService(
                        sRequest.GetPayload<NWDUpPayloadAssociateSubService>(NWDServerMiddleConfiguration.KConfig)),
                    NWDRequestStatus.Ok);
            }
            else
            {
                rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig, NWDExchangeCrucialKind.Unknown,
                    null, NWDRequestStatus.Error);
            }

            return rReturn;
        }
        private NWDResponseCrucial GetSubServicesFromAccount(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn;
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig, sRequest.Kind,
                    NWDAccountServiceManager.GetSubServicesFromAccount(
                        sRequest.GetPayload<NWDUpPayloadGetSubServicesFromAccount>(NWDServerMiddleConfiguration.KConfig)),
                    NWDRequestStatus.Ok);
            }
            else
            {
                rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig, NWDExchangeCrucialKind.Unknown,
                    null, NWDRequestStatus.Error);
            }

            return rReturn;
        }

        private NWDResponseCrucial DissociateServiceAndSubServices(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn;
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig, sRequest.Kind,
                    NWDAccountServiceManager.DissociateServiceAndSubServices(
                        sRequest.GetPayload<NWDUpPayloadDissociateServiceAndSubService>(NWDServerMiddleConfiguration
                            .KConfig),sRequest.ProjectId,sRequest.Environment),
                    NWDRequestStatus.Ok);
            }
            else
            {
                rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig, NWDExchangeCrucialKind.Unknown,
                    null, NWDRequestStatus.Error);
            }

            return rReturn;
        }

        #endregion

        #region Process

        private NWDResponseCrucial None(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                //TODO Implement method code here
                NWDLogger.TraceTodo("⚠️ Alert " + nameof(NWDCrucialManager) + "." + nameof(None) +
                                    "() NOT IMPLEMENTED !");
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(None) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial Test(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                //TODO Implement method code here
                NWDLogger.TraceTodo("⚠️ Alert " + nameof(NWDCrucialManager) + "." + nameof(Test) +
                                    "() NOT IMPLEMENTED !");
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(Test) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial Unknown(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                //TODO Implement method code here
                NWDLogger.TraceTodo("⚠️ Alert " + nameof(NWDCrucialManager) + "." + nameof(Unknown) +
                                    "() NOT IMPLEMENTED !");
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(Unknown) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial ProjectPublish(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                rReturn = NWDProjectCredentialManager.ProcessPublish(
                    sRequest.GetPayload<NWDUpPayloadProjectPublish>(NWDServerMiddleConfiguration.KConfig));
            }
            else
            {
                NWDLogger.TraceAttention("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(ProjectPublish) +
                                         "() ! " + nameof(sRequest) + " " + nameof(sRequest.IsValid) +
                                         " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial ProjectClean(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                //TODO Implement method code here
                NWDLogger.TraceTodo("⚠️ Alert " + nameof(NWDCrucialManager) + "." + nameof(ProjectClean) +
                                    "() NOT IMPLEMENTED !");
                rReturn = NWDProjectCredentialManager.ProcessClean(
                    sRequest.GetPayload<NWDUpPayloadProjectClean>(NWDServerMiddleConfiguration.KConfig));
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(ProjectClean) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial ProjectDelete(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                //TODO Implement method code here
                NWDLogger.TraceTodo("⚠️ Alert " + nameof(NWDCrucialManager) + "." + nameof(ProjectDelete) +
                                    "() NOT IMPLEMENTED !");
                rReturn = NWDProjectCredentialManager.ProcessDelete(
                    sRequest.GetPayload<NWDUpPayloadProjectDelete>(NWDServerMiddleConfiguration.KConfig));
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(ProjectDelete) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial PublishStudioData(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                //TODO Implement method code here
                NWDLogger.TraceTodo("⚠️ Alert " + nameof(NWDCrucialManager) + "." + nameof(PublishStudioData) +
                                    "() NOT IMPLEMENTED !");

                NWDUpPayloadPublishStudioData? tPayloadPublishStudioData =
                    sRequest.GetPayload<NWDUpPayloadPublishStudioData>(NWDServerMiddleConfiguration.KConfig);
                if (tPayloadPublishStudioData != null && tPayloadPublishStudioData.StudioDataStorageList != null)
                {
                    NWDStudioDataManager.Publish(tPayloadPublishStudioData.Kind, tPayloadPublishStudioData.ProjectId, tPayloadPublishStudioData.StudioDataStorageList);
                }
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(PublishStudioData) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial AlterAccountData(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                //TODO Implement method code here
                NWDLogger.TraceTodo("⚠️ Alert " + nameof(NWDCrucialManager) + "." + nameof(AlterAccountData) +
                                    "() NOT IMPLEMENTED !");
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(AlterAccountData) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial AlterPlayerData(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                //TODO Implement method code here
                NWDLogger.TraceTodo("⚠️ Alert " + nameof(NWDCrucialManager) + "." + nameof(AlterPlayerData) +
                                    "() NOT IMPLEMENTED !");
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(AlterPlayerData) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial DisableAllServers(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDServerConfiguration.KConfig.Status = NWDServerStatus.Inactive;
                NWDServerConfiguration.KConfig.ClusterStatus = NWDServerStatus.Inactive;
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(DisableAllServers) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial EnableAllServers(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDServerConfiguration.KConfig.Status = NWDServerStatus.Active;
                NWDServerConfiguration.KConfig.ClusterStatus = NWDServerStatus.Active;
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(EnableAllServers) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial DisableThisServer(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDServerConfiguration.KConfig.Status = NWDServerStatus.Inactive;
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(DisableThisServer) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial EnableThisServer(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDServerConfiguration.KConfig.Status = NWDServerStatus.Active;
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(EnableThisServer) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial UpgradeAllBases(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDServerStatus tOldStatus = NWDServerConfiguration.KConfig.Status;
                NWDServerConfiguration.KConfig.Status = NWDServerStatus.Upgrading;
                CheckAllTables();
                NWDServerConfiguration.KConfig.Status = tOldStatus;
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(UpgradeAllBases) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial OptimizeAllBases(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                //TODO Implement method code here
                NWDLogger.TraceTodo("⚠️ Alert " + nameof(NWDCrucialManager) + "." + nameof(OptimizeAllBases) +
                                    "() NOT IMPLEMENTED !");
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(OptimizeAllBases) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        private NWDResponseCrucial CleanAllBases(NWDRequestCrucial sRequest)
        {
            NWDResponseCrucial rReturn = new NWDResponseCrucial(NWDServerMiddleConfiguration.KConfig,
                NWDExchangeCrucialKind.Unknown, null, NWDRequestStatus.Error);
            if (sRequest.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                //TODO Implement method code here
                NWDLogger.TraceTodo("⚠️ Alert " + nameof(NWDCrucialManager) + "." + nameof(CleanAllBases) +
                                    "() NOT IMPLEMENTED !");
            }
            else
            {
                NWDLogger.Error("⚠️ Alert in " + nameof(NWDCrucialManager) + "." + nameof(CleanAllBases) + "() ! " +
                                nameof(sRequest) + " " + nameof(sRequest.IsValid) + " return false!");
            }

            return rReturn;
        }

        #endregion

        #region Database management

        public static void CheckAllTables()
        {
            NWDCrucialInformationManager.Prepare();

            NWDProjectCredentialManager.CheckAllTables();
            NWDProjectServiceKeyManager.CheckAllTables();

            NWDAccountManager.CheckAllTables();
            NWDAccountSignManager.CheckAllTables();
            NWDAccountTokenManager.CheckAllTables();
            NWDAccountServiceManager.CheckAllTables();
            NWDAccountOrderManager.CheckAllTables();
            NWDAccountInvoiceManager.CheckAllTables();

            NWDPlayerDataManager.CheckAllTables();
            NWDStudioDataManager.CheckAllTables();
        }

        public static void ForceCreateAllTables()
        {
            NWDCrucialInformationManager.Prepare();

            NWDCrucialInformationManager.ForceCreateAllTables();

            NWDProjectCredentialManager.ForceCreateAllTables();
            NWDProjectServiceKeyManager.ForceCreateAllTables();

            NWDAccountManager.ForceCreateAllTables();
            NWDAccountSignManager.ForceCreateAllTables();
            NWDAccountTokenManager.ForceCreateAllTables();
            NWDAccountServiceManager.ForceCreateAllTables();
            NWDAccountOrderManager.ForceCreateAllTables();
            NWDAccountInvoiceManager.ForceCreateAllTables();

            NWDPlayerDataManager.ForceCreateAllTables();
            NWDStudioDataManager.ForceCreateAllTables();
        }

        public static void DeleteAllTables()
        {
            NWDCrucialInformationManager.DeleteAllTables();

            NWDProjectCredentialManager.DeleteAllTables();
            NWDProjectServiceKeyManager.DeleteAllTables();

            NWDAccountManager.DeleteAllTables();
            NWDAccountSignManager.DeleteAllTables();
            NWDAccountTokenManager.DeleteAllTables();
            NWDAccountServiceManager.DeleteAllTables();
            NWDAccountOrderManager.DeleteAllTables();
            NWDAccountInvoiceManager.DeleteAllTables();

            NWDPlayerDataManager.DeleteAllTables();
            NWDStudioDataManager.DeleteAllTables();
        }

        #endregion
    }
}