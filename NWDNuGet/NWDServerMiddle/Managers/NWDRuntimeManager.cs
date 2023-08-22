using System;
using System.Linq;
using NWDFoundation.Exchanges;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDRuntime.Facades;
using NWDRuntime.Models;
using NWDServerMiddle.Configuration;
using NWDServerMiddle.Managers.ModelManagers;
using NWDServerMiddle.Models;
using NWDServerMiddle.Models.Enum;
using NWDServerShared.Configuration;

namespace NWDServerMiddle.Managers
{
    public class NWDRuntimeManager : INWDRuntimeManager
    {
        public NWDRuntimeManager()
        {
        }

        public NWDResponseRuntime Process(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn;
            switch (sRequestRuntime.Kind)
            {
                case NWDExchangeRuntimeKind.Test:
                    rReturn = Test(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.None:
                    rReturn = None(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.AccountDelete:
                    rReturn = AccountDelete(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.AccountChangeRange:
                    rReturn = AccountChangeRange(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.SignOut:
                    rReturn = SignOut(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.SignIn:
                    rReturn = SignIn(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.SignUp:
                    rReturn = SignUp(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.SignLost:
                    rReturn = SignLost(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.SignRescue:
                    rReturn = SignRescue(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.SignAdd:
                    rReturn = SignAdd(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.SignModify:
                    rReturn = SignModify(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.SignDelete:
                    rReturn = SignDelete(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.GetAllSign:
                    rReturn = SignAllSign(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.SyncDataByIncrement:
                    rReturn = SyncDataByIncrement(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.GetAllData:
                    rReturn = AllData(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.GetAllPlayerData:
                    rReturn = AllPlayerData(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.GetPlayerDataByReferences:
                    rReturn = PlayerDataByReferences(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.GetPlayerDataByBundle:
                    rReturn = PlayerDataByBundle(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.GetAllStudioData:
                    rReturn = AllStudioData(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.GetStudioDataByReferences:
                    rReturn = StudioDataByReferences(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.GetStudioDataByBundle:
                    rReturn = StudioDataByBundle(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.CreateRelationship:
                    rReturn = CreateRelationship(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.GetRelationship:
                    rReturn = GetRelationship(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.LinkRelationship:
                    rReturn = LinkRelationship(sRequestRuntime);
                    break;
                case NWDExchangeRuntimeKind.FinalizeRelationship:
                    rReturn = FinalizeRelationship(sRequestRuntime);
                    break;
                default:
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        NWDRequestStatus.Error);
                    break;
            }

            return rReturn;
        }

        #region NWDRelationship

        private NWDResponseRuntime UpdateRelationship(NWDRequestRuntime sRequestRuntime)
        {
            throw new NotImplementedException();
        }

        private NWDResponseRuntime GetRelationship(NWDRequestRuntime sRequestRuntime)
        {
            throw new NotImplementedException();
        }

        private NWDResponseRuntime CreateRelationship(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    NWDRelationship tRelationship = NWDRelationshipManager.CreateRelationship(sRequestRuntime);
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken,
                        sRequestRuntime.Kind,
                        null, NWDRequestStatus.Ok);
                }
            }

            return rReturn;
        }
        private NWDResponseRuntime LinkRelationship(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    NWDDownPayloadLinkRelationship tRelationship = NWDRelationshipManager.LinkRelationship(sRequestRuntime);
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken,
                        sRequestRuntime.Kind,
                        tRelationship, NWDRequestStatus.Ok);
                }
            }

            return rReturn;
            
        }
        private NWDResponseRuntime FinalizeRelationship(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    NWDRelationshipManager.FinalizeRelationship(sRequestRuntime);
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken,
                        sRequestRuntime.Kind,
                        null, NWDRequestStatus.Ok);
                }
            }

            return rReturn;
        }

        #endregion

        #region Test

        private NWDResponseRuntime Test(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    rReturn = NWDAccountSignManager.ProcessTest(sRequestRuntime, tAccountInformation);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }
            else
            {
                rReturn.Status = NWDRequestStatus.HashInvalid;
            }

            return rReturn;
        }

        #endregion

        #region None

        private NWDResponseRuntime None(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    NWDDownPayload? tDownPayload = new NWDDownPayload();
                    // Add services to response
                    //tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(tAccountInformation.PlayerToken);
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken, sRequestRuntime.Kind, tDownPayload, NWDRequestStatus.Error);
                }
            }

            return rReturn;
        }

        #endregion

        #region Sign

        private NWDResponseRuntime SignOut(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    rReturn = NWDAccountSignManager.ProcessSignOut(sRequestRuntime, tAccountInformation);
                }
            }

            return rReturn;
        }

        private NWDResponseRuntime SignIn(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                // NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                // if (tAccountInformation.Status == NWDAccountStatus.Valid ||
                //     tAccountInformation.Status == NWDAccountStatus.TokenError)
                // {
                rReturn = NWDAccountSignManager.ProcessSignIn(sRequestRuntime /*, tAccountInformation*/);
                // }
            }

            return rReturn;
        }

        private NWDResponseRuntime SignUp(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            rReturn.Debug = "SignUp()";
            rReturn.Debug += "- Before IsValid()";

            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                rReturn.Debug += "- Is Valid pass";

                // NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                // if (tAccountInformation.Status == NWDAccountStatus.Valid ||
                //     tAccountInformation.Status == NWDAccountStatus.TokenError)
                // {
                rReturn = NWDAccountSignManager.ProcessSignUp(sRequestRuntime /*, tAccountInformation*/);
                // }
            }
            else
            {
            }

            return rReturn;
        }

        private NWDResponseRuntime SignLost(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                // NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                // if (tAccountInformation.Status == NWDAccountStatus.Valid ||
                //     tAccountInformation.Status == NWDAccountStatus.TokenError)
                // {
                rReturn = NWDAccountSignManager.ProcessSignLost(sRequestRuntime /*, tAccountInformation*/);
                // }
                // else
                // {
                //     rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                //         tAccountInformation.RequestStatus);
                // }
            }

            return rReturn;
        }

        private NWDResponseRuntime SignRescue(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                // NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                // if (tAccountInformation.Status == NWDAccountStatus.Valid ||
                //     tAccountInformation.Status == NWDAccountStatus.TokenError)
                // {
                rReturn = NWDAccountSignManager.ProcessSignRescue(sRequestRuntime /*, tAccountInformation*/);
                // }
                // else
                // {
                //     rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                //         tAccountInformation.RequestStatus);
                // }
            }

            return rReturn;
        }

        private NWDResponseRuntime SignAdd(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    rReturn = NWDAccountSignManager.ProcessSignAdd(sRequestRuntime, tAccountInformation);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }

            return rReturn;
        }

        private NWDResponseRuntime SignModify(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    rReturn = NWDAccountSignManager.ProcessSignModify(sRequestRuntime, tAccountInformation);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }

            return rReturn;
        }

        private NWDResponseRuntime SignAllSign(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    rReturn = NWDAccountSignManager.ProcessSignAll(sRequestRuntime, tAccountInformation);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }
            else
            {
            }

            return rReturn;
        }

        private NWDResponseRuntime SignDelete(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    rReturn = NWDAccountSignManager.ProcessSignDelete(sRequestRuntime, tAccountInformation);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }

            return rReturn;
        }

        #endregion

        #region Account

        private NWDResponseRuntime AccountDelete(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    rReturn = NWDAccountSignManager.ProcessAccountDelete(sRequestRuntime, tAccountInformation);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }

            return rReturn;
        }

        private NWDResponseRuntime AccountChangeRange(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    rReturn = NWDAccountSignManager.ProcessAccountChangeRange(sRequestRuntime, tAccountInformation);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }

            return rReturn;
        }

        #endregion

        #region Sync player only

        private NWDResponseRuntime AllPlayerData(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            NWDDownPayloadAllPlayerData? tDownPayloadAllPlayerData = new NWDDownPayloadAllPlayerData();
            // return response
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    NWDUpPayloadAllPlayerData tUpPayloadAllPlayerData =
                        sRequestRuntime.GetPayload<NWDUpPayloadAllPlayerData>(NWDServerMiddleConfiguration.KConfig);

                    if (sRequestRuntime.PlayerToken != null)
                    {
                        tDownPayloadAllPlayerData.PlayerDataList = NWDPlayerDataManager.GetAllForProject(
                            sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.PlayerToken.AccountRange,
                            sRequestRuntime.ProjectId, sRequestRuntime.PlayerToken.PlayerReference);
                        tDownPayloadAllPlayerData.AccountServiceList =
                            NWDAccountServiceManager.AddServices(tAccountInformation.PlayerToken);
                    }

                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken,
                        sRequestRuntime.Kind,
                        tDownPayloadAllPlayerData, NWDRequestStatus.Ok);
                }
            }

            return rReturn;
        }

        private NWDResponseRuntime PlayerDataByReferences(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            NWDDownPayloadPlayerDataByReferences? tDownPayloadPlayerDataByReferences =
                new NWDDownPayloadPlayerDataByReferences();
            // return response
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    NWDUpPayloadPlayerDataByReferences tUpPayloadPlayerDataByReferences =
                        sRequestRuntime.GetPayload<NWDUpPayloadPlayerDataByReferences>(NWDServerMiddleConfiguration
                            .KConfig);
                    if (sRequestRuntime.PlayerToken != null)
                    {
                        tDownPayloadPlayerDataByReferences.PlayerDataList =
                            NWDPlayerDataManager.GetByReferences(
                                sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.ProjectId,
                                tUpPayloadPlayerDataByReferences.PlayerDataReferencesList,
                                sRequestRuntime.PlayerToken.AccountRange);
                    }

                    tDownPayloadPlayerDataByReferences.AccountServiceList =
                        NWDAccountServiceManager.AddServices(tAccountInformation.PlayerToken);

                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken,
                        sRequestRuntime.Kind,
                        tDownPayloadPlayerDataByReferences, NWDRequestStatus.Ok);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }

            return rReturn;
        }

        private NWDResponseRuntime PlayerDataByBundle(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            NWDDownPayloadAccountSignAdd? tDownPayloadAccountSignAdd = new NWDDownPayloadAccountSignAdd();
            // return response
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    NWDUpPayloadAccountSignAdd tUpPayloadAccountSignAdd =
                        sRequestRuntime.GetPayload<NWDUpPayloadAccountSignAdd>(NWDServerMiddleConfiguration.KConfig);
                    tDownPayloadAccountSignAdd.AccountServiceList =
                        NWDAccountServiceManager.AddServices(tAccountInformation.PlayerToken);
                    //bool tValid = false;
                    // TODO : process
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken,
                        sRequestRuntime.Kind,
                        tDownPayloadAccountSignAdd, NWDRequestStatus.Ok);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }

            return rReturn;
        }

        #endregion

        #region Sync all data

        private NWDResponseRuntime SyncDataByIncrement(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            NWDDownPayloadDataSyncByIncrement? tDownPayloadDataSyncByIncrement =
                new NWDDownPayloadDataSyncByIncrement();
            // return response
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);

                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    NWDUpPayloadDataSyncByIncrement tUpPayloadDataSyncByIncrement =
                        sRequestRuntime.GetPayload<NWDUpPayloadDataSyncByIncrement>(
                            NWDServerMiddleConfiguration.KConfig);
                    if (tUpPayloadDataSyncByIncrement is { PlayerDataList: { }, StudioDataList: { } })
                    {
                        if (sRequestRuntime.PlayerToken != null)
                        {
                            NWDNewSyncInformation tNewSyncInfo =
                                NWDPlayerDataManager.GenerateNewSyncInformation(
                                    sRequestRuntime.PlayerToken.AccountRange);
                            NWDPlayerDataManager.InsertOrUpdate(sRequestRuntime.PlayerToken.EnvironmentKind,
                                sRequestRuntime.ProjectId, sRequestRuntime.PlayerToken.AccountRange,
                                tUpPayloadDataSyncByIncrement.PlayerDataList, tNewSyncInfo);
                            //NWDStudioDataManager.InsertOrUpdate(sRequestRuntime.PlayerToken.EnvironmentKind,
                            //    sRequestRuntime.ProjectId, tUpPayloadDataSyncByIncrement.StudioDataList);
                        }
                    }

                    if (sRequestRuntime.PlayerToken != null)
                    {
                        tDownPayloadDataSyncByIncrement.PlayerDataList =
                            NWDPlayerDataManager.GetBySync(sRequestRuntime.PlayerToken.EnvironmentKind,
                                sRequestRuntime.ProjectId,
                                sRequestRuntime.PlayerToken.AccountRange,
                                tUpPayloadDataSyncByIncrement.PlayerDataSyncInformation,
                                sRequestRuntime.PlayerToken.PlayerReference);
                        tDownPayloadDataSyncByIncrement.StudioDataList =
                            NWDStudioDataManager.GetBySync(sRequestRuntime.PlayerToken.EnvironmentKind,
                                sRequestRuntime.ProjectId,
                                tUpPayloadDataSyncByIncrement.StudioDataSyncInformation);
                    }

                    rReturn.Debug += "GetBySync StudioDataList";
                    rReturn.Debug = "AddServices " +
                                    NWDAccountServiceManager.AddServices(tAccountInformation.PlayerToken).Count;
                    tDownPayloadDataSyncByIncrement.AccountServiceList =
                        NWDAccountServiceManager.AddServices(tAccountInformation.PlayerToken);
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken, sRequestRuntime.Kind, tDownPayloadDataSyncByIncrement,
                        NWDRequestStatus.Ok);
                }
            }

            return rReturn;
        }

        private NWDResponseRuntime AllData(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            NWDDownPayloadAllData? tDownPayloadAllData = new NWDDownPayloadAllData();
            // return response
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    if (sRequestRuntime.PlayerToken != null)
                    {
                        tDownPayloadAllData.PlayerDataList = NWDPlayerDataManager.GetAllForProject(
                            sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.PlayerToken.AccountRange,
                            sRequestRuntime.ProjectId, sRequestRuntime.PlayerToken.PlayerReference);
                        tDownPayloadAllData.StudioDataList =
                            NWDStudioDataManager.GetAllForProject(sRequestRuntime.PlayerToken.EnvironmentKind,
                                sRequestRuntime.ProjectId);
                    }

                    tDownPayloadAllData.AccountServiceList =
                        NWDAccountServiceManager.AddServices(tAccountInformation.PlayerToken);
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken,
                        sRequestRuntime.Kind,
                        tDownPayloadAllData, NWDRequestStatus.Ok);
                }
            }

            return rReturn;
        }

        #endregion

        #region Sync studio only

        private NWDResponseRuntime AllStudioData(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            NWDDownPayloadAllStudioData? tDownPayloadAllStudioData = new NWDDownPayloadAllStudioData();
            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    if (sRequestRuntime.PlayerToken != null)
                        tDownPayloadAllStudioData.StudioDataList = NWDStudioDataManager.GetAllForProject(
                            sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.PlayerToken.AccountRange,
                            sRequestRuntime.ProjectId);
                    tDownPayloadAllStudioData.AccountServiceList =
                        NWDAccountServiceManager.AddServices(tAccountInformation.PlayerToken);

                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken,
                        sRequestRuntime.Kind,
                        tDownPayloadAllStudioData, NWDRequestStatus.Ok);
                }
            }

            return rReturn;
        }


        private NWDResponseRuntime StudioDataByReferences(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Error);
            NWDDownPayloadStudioDataByReferences? tDownPayloadStudioDataByReferences =
                new NWDDownPayloadStudioDataByReferences();

            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    NWDUpPayloadStudioDataByReferences tUpPayloadStudioDataByReferences =
                        sRequestRuntime.GetPayload<NWDUpPayloadStudioDataByReferences>(NWDServerMiddleConfiguration
                            .KConfig);

                    if (sRequestRuntime.PlayerToken != null)
                        tDownPayloadStudioDataByReferences.StudioDataList =
                            NWDStudioDataManager.GetByReferences(
                                sRequestRuntime.PlayerToken.EnvironmentKind,
                                sRequestRuntime.ProjectId, sRequestRuntime.PlayerToken.AccountRange,
                                tUpPayloadStudioDataByReferences.StudioDataReferenceList);
                    tDownPayloadStudioDataByReferences.AccountServiceList =
                        NWDAccountServiceManager.AddServices(tAccountInformation.PlayerToken);

                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken,
                        sRequestRuntime.Kind,
                        tDownPayloadStudioDataByReferences, NWDRequestStatus.Ok);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }

            return rReturn;
        }


        private NWDResponseRuntime StudioDataByBundle(NWDRequestRuntime sRequestRuntime)
        {
            NWDResponseRuntime rReturn;
            NWDDownPayloadStudioDataByBundle? tDownPayloadStudioDataByBundle =
                new NWDDownPayloadStudioDataByBundle();

            if (sRequestRuntime.IsValid(NWDServerMiddleConfiguration.KConfig))
            {
                NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(sRequestRuntime.PlayerToken);
                if (tAccountInformation.Status == NWDAccountStatus.Valid)
                {
                    // TODO : process
                    tDownPayloadStudioDataByBundle.AccountServiceList =
                        NWDAccountServiceManager.AddServices(tAccountInformation.PlayerToken);

                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        tAccountInformation.PlayerToken,
                        sRequestRuntime.Kind,
                        tDownPayloadStudioDataByBundle, NWDRequestStatus.Ok);
                }
                else
                {
                    rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                        new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                        tAccountInformation.RequestStatus);
                }
            }
            else
            {
                rReturn = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig,
                    new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), NWDExchangeRuntimeKind.Unknown, null,
                    NWDRequestStatus.Error);
            }

            return rReturn;
        }

        #endregion
    }
}