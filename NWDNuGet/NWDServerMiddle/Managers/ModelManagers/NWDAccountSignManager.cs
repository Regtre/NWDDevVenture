using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDServerBack;
using NWDServerMiddle.Configuration;
using NWDServerMiddle.Models;
using NWDServerShared.Configuration;

namespace NWDServerMiddle.Managers.ModelManagers;

public static class NWDAccountSignManager
{
    #region Static properties

    public static List<INWDAccountSignDao> DaoList = new List<INWDAccountSignDao>();

    private static readonly Dictionary<ushort, INWDAccountSignDao> DaoByRange = new Dictionary<ushort, INWDAccountSignDao>();

    #endregion

    #region Dao

    public static void Prepare()
    {
        DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDAccountSignDao>(NWDConfigurationDatabase.KConfig.DatabaseAccountArray);
        foreach (INWDAccountSignDao? tDao in DaoList)
        {
            if (DaoByRange.ContainsKey(tDao.GetRange()) == false)
            {
                DaoByRange.Add(tDao.GetRange(), tDao);
            }
        }

        CheckAllTables();
    }

    public static void CheckAllTables()
    {
        foreach (NWDEnvironmentKind tEnvironment in (NWDEnvironmentKind[])Enum.GetValues(typeof(NWDEnvironmentKind)))
        {
            foreach (INWDDao tDao in DaoList)
            {
                if (tDao.FingerPrintTable(tEnvironment) != NWDCrucialInformationManager.GetFingerPrintTable(tEnvironment, tDao))
                {
                    NWDLogger.TraceAttention("Must create table for  " + tDao.FingerPrintTableName(tEnvironment));
                    tDao.CreateTable(tEnvironment);
                    NWDCrucialInformationManager.SetFingerPrintTable(tEnvironment, tDao);
                }
                else
                {
                    //NWDLogger.TraceSuccess("Table is identical to finger print "+tDao.FingerPrintTableName(tEnvironment)); 
                }
            }
        }
    }

    public static void ForceCreateAllTables()
    {
        foreach (NWDEnvironmentKind tEnvironment in (NWDEnvironmentKind[])Enum.GetValues(typeof(NWDEnvironmentKind)))
        {
            foreach (INWDAccountSignDao? tDao in DaoList)
            {
                tDao.CreateTable(tEnvironment);
                NWDCrucialInformationManager.SetFingerPrintTable(tEnvironment, tDao);
            }
        }
    }

    public static void DeleteAllTables()
    {
        foreach (NWDEnvironmentKind tEnvironment in (NWDEnvironmentKind[])Enum.GetValues(typeof(NWDEnvironmentKind)))
        {
            foreach (INWDAccountSignDao tDao in DaoList)
            {
                tDao.DeleteTable(tEnvironment);
                NWDCrucialInformationManager.ResetFingerPrintTable(tEnvironment, tDao);
            }
        }
    }

    public static INWDAccountSignDao? GetOneDao()
    {
        if (DaoList.Count == 0)
        {
            return null;
        }
        else if (DaoList.Count == 1)
        {
            return DaoList[0];
        }
        else
        {
            return DaoList[NWDRandom.Random(0, DaoList.Count)];
        }
    }

    public static INWDAccountSignDao? GetDaoByRange(ushort sRange)
    {
        if (DaoByRange.ContainsKey(sRange))
        {
            return DaoByRange[sRange];
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region Methods

    public static bool CheckIfSignIsValid(NWDAccountSign sSign, ulong sProjectId)
    {
        bool rReturn = !(sSign.ProjectId != sProjectId
                         || string.IsNullOrEmpty(sSign.LoginHash)
                         || string.IsNullOrEmpty(sSign.RescueHash)
                         || string.IsNullOrEmpty(sSign.SignHash)
                         || sSign.LoginHash.Contains('%')
                         || sSign.LoginHash.Contains('*')
                         || sSign.LoginHash.Contains('_')
                         || sSign.RescueHash.Contains('%')
                         || sSign.RescueHash.Contains('*')
                         || sSign.RescueHash.Contains('_')
                         || sSign.SignHash.Contains('%')
                         || sSign.SignHash.Contains('*')
                         || sSign.SignHash.Contains('_')
            );
        return rReturn;
    }

    public static bool CheckIfSignLoginHashExists(NWDEnvironmentKind sEnvironmentKind, NWDAccountSign sSign)
    {
        List<NWDAccountSign> tList = new List<NWDAccountSign>();
        foreach (INWDAccountSignDao? tDao in DaoList)
        {
            tList.AddRange(tDao.GetBy(sEnvironmentKind, sSign.ProjectId, new Dictionary<string, string>()
            {
                { nameof(NWDAccountSign.LoginHash), sSign.LoginHash }
            }));
        }

        return tList.Count > 0;
    }
    public static bool CheckIfSignNewLoginHashExists(NWDEnvironmentKind sEnvironmentKind, NWDAccountSign sNewSign, NWDAccountSign sOldSign)
    {
        List<NWDAccountSign> tList = new List<NWDAccountSign>();
        foreach (INWDAccountSignDao? tDao in DaoList)
        {
            tList.AddRange(tDao.GetBy(sEnvironmentKind, sNewSign.ProjectId, new Dictionary<string, string>()
            {
                { nameof(NWDAccountSign.LoginHash), sNewSign.LoginHash }
            },  nameof(NWDAccountSign.Reference)+" != '"+sOldSign.Reference+"'"));
        }

        return tList.Count > 0;
    }

    private static List<NWDAccountSign> SignsForAccount(NWDRequestPlayerToken sPlayerToken)
    {
        List<NWDAccountSign> rReturn = new List<NWDAccountSign>();
        INWDAccountSignDao? tDao = GetDaoByRange(sPlayerToken.AccountRange);
        if (tDao != null)
        {
            rReturn = tDao.GetBy(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId,
                new Dictionary<string, string>()
                {
                    { nameof(NWDAccountSign.Account), sPlayerToken.PlayerReference.ToString() },
                    { nameof(NWDAccountSign.SignStatus), ((int)NWDAccountSignAction.Associated).ToString() },
                });
        }

        return rReturn;
    }

    public static NWDAccount? AccountForSign(NWDAccountSign sSign, NWDRequestPlayerToken sPlayerToken)
    {
        NWDAccount? rReturn = null;
        List<NWDAccountSign> tList = new List<NWDAccountSign>();
        foreach (INWDAccountSignDao? tDao in DaoList)
        {
            tList.AddRange(tDao.GetBy(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId, new Dictionary<string, string>()
            {
                { nameof(NWDAccountSign.LoginHash), sSign.LoginHash },
                { nameof(NWDAccountSign.SignHash), sSign.SignHash },
                { nameof(NWDAccountSign.RescueHash), sSign.RescueHash },
                { nameof(NWDAccountSign.SignType), ((int)sSign.SignType).ToString() },
                { nameof(NWDAccountSign.SignStatus), ((int)NWDAccountSignAction.Associated).ToString() },
            }));
        }

        if (tList.Count == 1)
        {
            rReturn = NWDAccountManager.GetAccountFor(tList[0], sPlayerToken);
        }

        return rReturn;
    }

    private static bool ReserveSignAdd(NWDRequestPlayerToken sPlayerToken, NWDAccountSign sSign)
    {
        bool rReturn = false;
        NWDAccountSign tAccountSign = new NWDAccountSign
        {
            Account = sPlayerToken.PlayerReference,
            Range = sPlayerToken.AccountRange,
            ProjectId = sPlayerToken.ProjectId,
            Name = sSign.Name,
            LoginHash = sSign.LoginHash,
            RescueHash = sSign.RescueHash,
            SignHash = sSign.SignHash,
            SignType = sSign.SignType,
            SignStatus = NWDAccountSignAction.TryToAssociate
        };
#if DEBUG
        tAccountSign.RescueVerifHash = sSign.RescueVerifHash;
#endif
        INWDAccountSignDao? tDao = GetDaoByRange(sPlayerToken.AccountRange);
        if (tDao != null)
        {
            tDao.Create(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId, tAccountSign);
            rReturn = true;
        }

        return rReturn;
    }

    public static NWDAccountSign? ReserveSignUp(NWDRequestPlayerToken sPlayerToken, NWDAccountSign sSign)
    {
        NWDAccountSign? rAccountSign = null;
        NWDAccount? tAccount = NWDAccountManager.CreateNewAccount(sPlayerToken.EnvironmentKind, sSign.ProjectId);
        if (tAccount != null)
        {
            NWDAccountSign tAccountSign = new NWDAccountSign
            {
                Account = tAccount.Reference,
                Range = tAccount.Range,
                ProjectId = sSign.ProjectId,
                Name = sSign.Name,
                LoginHash = sSign.LoginHash,
                RescueHash = sSign.RescueHash,
                SignHash = sSign.SignHash,
                SignType = sSign.SignType,
                SignStatus = NWDAccountSignAction.TryToAssociate
            };
#if DEBUG
            tAccountSign.RescueVerifHash = sSign.RescueVerifHash;
#endif
            INWDAccountSignDao? tDao = GetDaoByRange(tAccount.Range);
            if (tDao != null)
            {
                rAccountSign = tDao.Create(sPlayerToken.EnvironmentKind, tAccountSign.ProjectId, tAccountSign);
            }
        }

        return rAccountSign;
    }

    private static bool AccountSignDelete(NWDAccountSign sSign, NWDRequestPlayerToken sPlayerToken)
    {
        bool rReturn = false;
        INWDAccountSignDao? tDao = GetDaoByRange(sPlayerToken.AccountRange);
        if (tDao != null)
        {
            List<NWDAccountSign> tList = tDao.GetBy(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId, new Dictionary<string, string>()
            {
                { nameof(NWDAccountSign.Account), sPlayerToken.PlayerReference.ToString() },
                { nameof(NWDAccountSign.LoginHash), sSign.LoginHash },
                { nameof(NWDAccountSign.SignHash), sSign.SignHash },
                { nameof(NWDAccountSign.RescueHash), sSign.RescueHash },
                { nameof(NWDAccountSign.SignType), ((int)sSign.SignType).ToString() },
                { nameof(NWDAccountSign.SignStatus), ((int)NWDAccountSignAction.Associated).ToString() },
            });
            if (tList.Count == 1)
            {
                tDao.Delete(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId, tList[0].Reference);
                // NWDAccountSign tSignResult = tList[0];
                // tSignResult.SignStatus = NWDAccountSignAction.Dissociated;
                // tDao.Update(sPlayerToken.EnvironmentKind, sPlayerToken.GetProjectId, tSignResult);
                rReturn = true;
            }
            else
            {
            }
        }
        else
        {
        }

        return rReturn;
    }

    public static NWDAccountSign? CheckReservedSign(NWDRequestPlayerToken sPlayerToken, NWDAccountSign sSign)
    {
        NWDAccountSign? rReturn = null;
        List<NWDAccountSign> tList = new List<NWDAccountSign>();
        Dictionary<NWDAccountSign, INWDAccountSignDao> tDicoReverse = new Dictionary<NWDAccountSign, INWDAccountSignDao>();
        foreach (INWDAccountSignDao? tDao in DaoList)
        {
            List<NWDAccountSign> tListProvisoire = tDao.GetBy(sPlayerToken.EnvironmentKind, sSign.ProjectId,
                new Dictionary<string, string>()
                {
                    { nameof(NWDAccountSign.LoginHash), sSign.LoginHash }
                });
            tList.AddRange(tListProvisoire);
            foreach (NWDAccountSign tSign in tListProvisoire)
            {
                tDicoReverse.Add(tSign, tDao);
            }
        }

        if (tList.Count == 0)
        {
            // si elle a disparue ... je retourne une erreur de création de compte
        }
        else if (tList.Count == 1)
        {
            NWDAccountSign tSign = tList[0];
            if (tSign.SignStatus == NWDAccountSignAction.TryToAssociate)
            {
                tSign.SignStatus = NWDAccountSignAction.Associated;
                INWDAccountSignDao tDao = tDicoReverse[tSign];
                rReturn = tDao.Update(sPlayerToken.EnvironmentKind, tSign.ProjectId, tSign);
            }
        }
        else
        {
            // si elle est en plusieurs exemplaire je les detruit toutes et je retpourne une erreur de création de compte
            foreach (INWDAccountSignDao? tDao in DaoList)
            {
                List<NWDAccountSign> tListToDelete = tDao.GetBy(sPlayerToken.EnvironmentKind, sSign.ProjectId, new Dictionary<string, string>()
                {
                    { nameof(NWDAccountSign.LoginHash), sSign.LoginHash }
                });
                foreach (NWDAccountSign tSignToDelete in tListToDelete)
                {
                    tDao.Delete(sPlayerToken.EnvironmentKind, sSign.ProjectId, tSignToDelete.Reference);
                }
            }
        }

        return rReturn;
    }

    #endregion

    #region Process

    public static NWDResponseRuntime ProcessSignUp(NWDRequestRuntime sRequestRuntime /*, NWDAccountInformation sAccountInformation*/)
    {
        string tDebug =  string.Empty;
        NWDRequestStatus tRequestStatus = NWDRequestStatus.Error;
        NWDRequestPlayerToken tNewPlayerToken = new NWDRequestPlayerToken(sRequestRuntime.PlayerToken);
        NWDUpPayloadAccountSignUp tUpPayload = sRequestRuntime.GetPayload<NWDUpPayloadAccountSignUp>(NWDServerMiddleConfiguration.KConfig);
        if (tUpPayload != null)
        {
            NWDDownPayloadAccountSignUp tDownPayload = new NWDDownPayloadAccountSignUp();
            NWDAccountSign tAccountSign = tUpPayload.AccountSign;
            tAccountSign.SignStatus = NWDAccountSignAction.ErrorAssociated;
            tAccountSign.Account = 0;
            tAccountSign.TokenRescue = string.Empty;
            tAccountSign.TokenRescueLimit = 0;
            tAccountSign.TokenVerif = string.Empty;
            tAccountSign.TokenVerifLimit = 0;
            tAccountSign.SignVerifHash = string.Empty;
            tAccountSign.RescueVerifHash = string.Empty;
            tAccountSign.LoginVerifHash = string.Empty;
#if DEBUG
            tAccountSign.RescueVerifHash = "Sign Up";
#endif
            if (sRequestRuntime.PlayerToken != null && sRequestRuntime.PlayerToken.ProjectId == sRequestRuntime.ProjectId && CheckIfSignIsValid(tAccountSign, sRequestRuntime.ProjectId))
            {
                if (CheckIfSignLoginHashExists(sRequestRuntime.PlayerToken.EnvironmentKind, tAccountSign))
                {
                    tDownPayload.AccountSignList.Add(tAccountSign);
                }
                else
                {
                    NWDAccountSign? tAccountSignReserved = ReserveSignUp(sRequestRuntime.PlayerToken, tAccountSign);
                    if (tAccountSignReserved != null)
                    {
                        NWDAccountSign? tNewAccountSign = CheckReservedSign(sRequestRuntime.PlayerToken, tAccountSignReserved);
                        if (tNewAccountSign != null)
                        {
                            tDownPayload.AccountSignList.Add(tNewAccountSign);
                            NWDRequestPlayerToken tNewToken = new NWDRequestPlayerToken(sRequestRuntime.PlayerToken)
                            {
                                AccountRange = tNewAccountSign.Range,
                                PlayerReference = tNewAccountSign.Account
                            };
                            tNewPlayerToken = NWDAccountTokenManager.NewToken(tNewToken);
                            tRequestStatus = NWDRequestStatus.Ok;
                        }
                        else
                        {
                            tDebug += "tNewAccountSign = null, ";
                            tDownPayload.AccountSignList.Add(tAccountSign);
                            tNewPlayerToken.PlayerReference = 0;
                            tNewPlayerToken.AccountRange = 0;
                            tNewPlayerToken.Token = string.Empty;
                            tNewPlayerToken.OldToken = string.Empty;
                            tRequestStatus = NWDRequestStatus.AccountError;
                        }
                    }
                    else
                    {
                        tDebug += "account reserved = null, ";
                        tDownPayload.AccountSignList.Add(tAccountSign);
                        tNewPlayerToken.PlayerReference = 0;
                        tNewPlayerToken.AccountRange = 0;
                        tNewPlayerToken.Token = string.Empty;
                        tNewPlayerToken.OldToken = string.Empty;
                        tRequestStatus = NWDRequestStatus.AccountError;
                    }
                }
            }
            else
            {
                tDebug += "";
                // weird!  sign player and request are not constant in environment and project 
                tNewPlayerToken.PlayerReference = 0;
                tNewPlayerToken.AccountRange = 0;
                tNewPlayerToken.Token = string.Empty;
                tNewPlayerToken.OldToken = string.Empty;
            }

            // Add services to response
            tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(tNewPlayerToken);
            return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, tNewPlayerToken, sRequestRuntime.Kind, tDownPayload, tRequestStatus, tDebug);
        }

        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sRequestRuntime.PlayerToken, sRequestRuntime.Kind, null, NWDRequestStatus.Error, tDebug);
    }

    public static NWDResponseRuntime ProcessSignAdd(NWDRequestRuntime sRequestRuntime, NWDAccountInformation sAccountInformation)
    {
        string tDebug =  string.Empty;
        NWDRequestStatus tRequestStatus = NWDRequestStatus.Error;
        NWDUpPayloadAccountSignAdd tUpPayload = sRequestRuntime.GetPayload<NWDUpPayloadAccountSignAdd>(NWDServerMiddleConfiguration.KConfig);
        if (tUpPayload != null)
        {
            NWDDownPayloadAccountSignAdd tDownPayload = new NWDDownPayloadAccountSignAdd();
            NWDAccountSign tAccountSign = tUpPayload.AccountSign;
            tAccountSign.SignStatus = NWDAccountSignAction.ErrorAssociated;
            if (sAccountInformation.Account != null)
            {
                tAccountSign.Account = sAccountInformation.Account.Reference;
            }

            tAccountSign.TokenRescue = string.Empty;
            tAccountSign.TokenRescueLimit = 0;
            tAccountSign.TokenVerif = string.Empty;
            tAccountSign.TokenVerifLimit = 0;
            tAccountSign.SignVerifHash = string.Empty;
            tAccountSign.RescueVerifHash = string.Empty;
            tAccountSign.LoginVerifHash = string.Empty;
#if DEBUG
            tAccountSign.RescueVerifHash = "Sign Add";
#endif
            if (sRequestRuntime.PlayerToken != null && sRequestRuntime.PlayerToken.ProjectId == sRequestRuntime.ProjectId && CheckIfSignIsValid(tAccountSign, sRequestRuntime.ProjectId))
            {
                if (CheckIfSignLoginHashExists(sRequestRuntime.PlayerToken.EnvironmentKind, tAccountSign))
                {
                    tDownPayload.AccountSign = tAccountSign;
                }
                else
                {
                    if (ReserveSignAdd(sRequestRuntime.PlayerToken, tAccountSign))
                    {
                        NWDAccountSign? tNewAccountSign = CheckReservedSign(sRequestRuntime.PlayerToken, tAccountSign);
                        if (tNewAccountSign != null)
                        {
                            tDownPayload.AccountSign = tNewAccountSign;
                            tRequestStatus = NWDRequestStatus.Ok;
                        }
                        else
                        {
                            tDownPayload.AccountSign = tAccountSign;
                            tRequestStatus = NWDRequestStatus.AccountError;
                        }
                    }
                    else
                    {
                        tDownPayload.AccountSign = tAccountSign;
                        tRequestStatus = NWDRequestStatus.AccountError;
                    }
                }
            }
            else
            {
                // weird!  sign player and request are not constant in environment and project 
            }

            // Add services to response
            tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(sAccountInformation.PlayerToken);
            return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sAccountInformation.PlayerToken, sRequestRuntime.Kind, tDownPayload, tRequestStatus);
        }

        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sRequestRuntime.PlayerToken, sRequestRuntime.Kind, null, NWDRequestStatus.AccountUnknown, tDebug);
    }

    public static NWDResponseRuntime ProcessSignIn(NWDRequestRuntime sRequestRuntime /*, NWDAccountInformation sAccountInformation*/)
    {
        string tDebug =  string.Empty;
        NWDRequestStatus tRequestStatus = NWDRequestStatus.Error;
        NWDUpPayloadAccountSignIn tUpPayload = sRequestRuntime.GetPayload<NWDUpPayloadAccountSignIn>(NWDServerMiddleConfiguration.KConfig);
        NWDDownPayloadAccountSignIn tDownPayload = new NWDDownPayloadAccountSignIn();
        if (tUpPayload != null)
        {
            NWDRequestPlayerToken tNewPlayerToken = new NWDRequestPlayerToken(sRequestRuntime.PlayerToken);

            NWDAccountSign tAccountSign = tUpPayload.AccountSign;
            tAccountSign.SignStatus = NWDAccountSignAction.Associated;
            tAccountSign.Account = 0;
            tAccountSign.TokenRescue = string.Empty;
            tAccountSign.TokenRescueLimit = 0;
            tAccountSign.TokenVerif = string.Empty;
            tAccountSign.TokenVerifLimit = 0;
            tAccountSign.SignVerifHash = string.Empty;
            tAccountSign.RescueVerifHash = string.Empty;
            tAccountSign.LoginVerifHash = string.Empty;
            if (sRequestRuntime.PlayerToken != null && sRequestRuntime.PlayerToken.ProjectId == sRequestRuntime.ProjectId && CheckIfSignIsValid(tAccountSign, sRequestRuntime.ProjectId))
            {
                NWDAccount? tAccount = AccountForSign(tAccountSign, tNewPlayerToken);
                if (tAccount != null)
                {
                    tNewPlayerToken.AccountRange = tAccount.Range;
                    tNewPlayerToken.PlayerReference = tAccount.Reference;
                    tNewPlayerToken = NWDAccountTokenManager.NewToken(tNewPlayerToken);
                    tRequestStatus = NWDRequestStatus.Ok;
                    if (tAccount.Trashed)
                    {
                        tRequestStatus = NWDRequestStatus.AccountTrashed;
                    }
                    else
                    {
                        if (tAccount.Ban)
                        {
                            tRequestStatus = NWDRequestStatus.AccountBan;
                        }
                    }
                }
                else
                {
                    tNewPlayerToken.PlayerReference = 0;
                    tNewPlayerToken.AccountRange = 0;
                    tNewPlayerToken.Token = string.Empty;
                    tNewPlayerToken.OldToken = string.Empty;
                    tRequestStatus = NWDRequestStatus.AccountUnknown;
                }
            }
            else
            {
                // weird!  sign player and request are not constant in environment and project 
                tNewPlayerToken.PlayerReference = 0;
                tNewPlayerToken.AccountRange = 0;
                tNewPlayerToken.Token = string.Empty;
                tNewPlayerToken.OldToken = string.Empty;
            }

            // Add services to response
            tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(tNewPlayerToken);
            return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, tNewPlayerToken, sRequestRuntime.Kind, tDownPayload, tRequestStatus);
        }
        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sRequestRuntime.PlayerToken, sRequestRuntime.Kind, null, NWDRequestStatus.AccountUnknown, tDebug);
    }

    public static NWDResponseRuntime ProcessSignOut(NWDRequestRuntime sRequestRuntime, NWDAccountInformation sAccountInformation)
    {
        string tDebug =  string.Empty;
        NWDRequestStatus tRequestStatus;
        NWDUpPayloadAccountSignOut tUpPayload = sRequestRuntime.GetPayload<NWDUpPayloadAccountSignOut>(NWDServerMiddleConfiguration.KConfig);
        if (tUpPayload != null)
        {
            NWDRequestPlayerToken tNewPlayerToken = new NWDRequestPlayerToken(sAccountInformation.PlayerToken);
            tNewPlayerToken.PlayerReference = 0;
            tNewPlayerToken.AccountRange = 0;
            NWDDownPayloadAccountSignOut tDownPayload = new NWDDownPayloadAccountSignOut();
            // delete all token 
            NWDAccountTokenManager.DeleteAllToken(sRequestRuntime.PlayerToken);
            if (sAccountInformation.PlayerToken != null)
            {
                NWDAccountSign tDeviceSign = tUpPayload.DeviceSign;
                if (tDeviceSign != null)
                {
                    if (tDeviceSign.SignType == NWDAccountSignType.DeviceId)
                    {
                        tDeviceSign.SignStatus = NWDAccountSignAction.Associated;
                        tDeviceSign.Account = 0;
                        tDeviceSign.TokenRescue = string.Empty;
                        tDeviceSign.TokenRescueLimit = 0;
                        tDeviceSign.TokenVerif = string.Empty;
                        tDeviceSign.TokenVerifLimit = 0;
                        tDeviceSign.SignVerifHash = string.Empty;
                        tDeviceSign.RescueVerifHash = string.Empty;
                        tDeviceSign.LoginVerifHash = string.Empty;
                        if (sRequestRuntime.PlayerToken != null && sRequestRuntime.PlayerToken.ProjectId == sRequestRuntime.ProjectId && CheckIfSignIsValid(tDeviceSign, sRequestRuntime.ProjectId))
                        {
                            NWDAccount? tAccount = AccountForSign(tDeviceSign, tNewPlayerToken);
                            if (tAccount != null)
                            {
                                tNewPlayerToken.AccountRange = tAccount.Range;
                                tNewPlayerToken.PlayerReference = tAccount.Reference;
                                tNewPlayerToken = NWDAccountTokenManager.NewToken(tNewPlayerToken);
                                tRequestStatus = NWDRequestStatus.Ok;
                                if (tAccount.Trashed)
                                {
                                    tRequestStatus = NWDRequestStatus.AccountTrashed;
                                }
                                else
                                {
                                    if (tAccount.Ban)
                                    {
                                        tRequestStatus = NWDRequestStatus.AccountBan;
                                    }
                                }
                            }
                            else
                            {
                                tNewPlayerToken.PlayerReference = 0;
                                tNewPlayerToken.AccountRange = 0;
                                tNewPlayerToken.Token = string.Empty;
                                tNewPlayerToken.OldToken = string.Empty;
                                tRequestStatus = NWDRequestStatus.AccountUnknown;
                            }
                        }
                        else
                        {
                            tNewPlayerToken.PlayerReference = 0;
                            tNewPlayerToken.AccountRange = 0;
                            tNewPlayerToken.Token = string.Empty;
                            tNewPlayerToken.OldToken = string.Empty;
                            tRequestStatus = NWDRequestStatus.AccountError;
                            // weird!  sign device is not device? !
                        }
                    }
                    else
                    {
                        tNewPlayerToken.PlayerReference = 0;
                        tNewPlayerToken.AccountRange = 0;
                        tNewPlayerToken.Token = string.Empty;
                        tNewPlayerToken.OldToken = string.Empty;
                        tRequestStatus = NWDRequestStatus.AccountError;
                        // weird!  sign player and request are not constant in environment and project 
                    }

                    // Add services to response
                    tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(tNewPlayerToken);
                    return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, tNewPlayerToken, sRequestRuntime.Kind, tDownPayload, tRequestStatus);
                }
                else
                {
                    tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(tNewPlayerToken);
                    return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, tNewPlayerToken, sRequestRuntime.Kind, tDownPayload, NWDRequestStatus.Ok);
                }
            }

            return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, tNewPlayerToken, sRequestRuntime.Kind, tDownPayload, NWDRequestStatus.AccountError);
        }

        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sRequestRuntime.PlayerToken, sRequestRuntime.Kind, null, NWDRequestStatus.Error, tDebug);
    }

    public static NWDResponseRuntime ProcessSignModify(NWDRequestRuntime sRequestRuntime, NWDAccountInformation sAccountInformation)
    {
        string tDebug =  string.Empty;
        NWDRequestStatus tRequestStatus = NWDRequestStatus.Error;
        NWDUpPayloadAccountSignModify tUpPayload = sRequestRuntime.GetPayload<NWDUpPayloadAccountSignModify>(NWDServerMiddleConfiguration.KConfig);
        if (tUpPayload != null)
        {
            NWDDownPayloadAccountSignModify tDownPayload = new NWDDownPayloadAccountSignModify();
            NWDAccountSign tNewSign = tUpPayload.NewAccountSign;
            NWDAccountSign tOldSign = tUpPayload.OldAccountSign;
            if (tNewSign.SignHash != tOldSign.SignHash && CheckIfSignIsValid(tNewSign, sRequestRuntime.ProjectId) && CheckIfSignIsValid(tOldSign, sRequestRuntime.ProjectId))
            {
                tNewSign.SignStatus = NWDAccountSignAction.ErrorAssociated;
                if (sAccountInformation.Account != null)
                {
                    tNewSign.Account = sAccountInformation.Account.Reference;
                }

                tNewSign.TokenRescue = string.Empty;
                tNewSign.TokenRescueLimit = 0;
                tNewSign.TokenVerif = string.Empty;
                tNewSign.TokenVerifLimit = 0;
                tNewSign.SignVerifHash = string.Empty;
                tNewSign.RescueVerifHash = string.Empty;
                tNewSign.LoginVerifHash = string.Empty;
#if DEBUG
                tNewSign.RescueVerifHash = "Sign Add";
#endif
                if (sRequestRuntime.PlayerToken != null && sRequestRuntime.PlayerToken.ProjectId == sRequestRuntime.ProjectId && CheckIfSignIsValid(tNewSign, sRequestRuntime.ProjectId))
                {
                    if (tNewSign.SignType == tOldSign.SignType && tNewSign.LoginHash == tOldSign.LoginHash && sAccountInformation.PlayerToken != null)
                    {
                        if (AccountSignDelete(tOldSign, sAccountInformation.PlayerToken))
                        {
                            if (ReserveSignAdd(sRequestRuntime.PlayerToken, tNewSign))
                            {
                                NWDAccountSign? tNewAccountSign = CheckReservedSign(sRequestRuntime.PlayerToken, tNewSign);
                                if (tNewAccountSign != null)
                                {
                                    tDownPayload.AccountSignList.Add(tNewAccountSign);
                                    tRequestStatus = NWDRequestStatus.Ok;
                                }
                                else
                                {
                                    tDownPayload.AccountSignList.Add(tNewSign);
                                    tRequestStatus = NWDRequestStatus.AccountError;
                                }
                            }
                            else
                            {
                                tRequestStatus = NWDRequestStatus.AccountError;
                            }
                        }
                        else
                        {
                            tRequestStatus = NWDRequestStatus.Error;
                        }
                    }
                    else if (CheckIfSignNewLoginHashExists(sRequestRuntime.PlayerToken.EnvironmentKind,tNewSign,  tOldSign))
                    {
                        tRequestStatus = NWDRequestStatus.AccountError;
                        // tDownPayload.AccountSignList.Add(tNewSign);
                    }
                    else
                    {
                        if (ReserveSignAdd(sRequestRuntime.PlayerToken, tNewSign))
                        {
                            NWDAccountSign? tNewAccountSign = CheckReservedSign(sRequestRuntime.PlayerToken, tNewSign);
                            if (tNewAccountSign != null)
                            {
                                tDownPayload.AccountSignList.Add(tNewAccountSign);
                                if (sAccountInformation.PlayerToken != null)
                                {
                                    if (AccountSignDelete(tOldSign, sAccountInformation.PlayerToken))
                                    {
                                        tRequestStatus = NWDRequestStatus.Ok;
                                    }
                                }
                            }
                            else
                            {
                                tDownPayload.AccountSignList.Add(tNewSign);
                                tRequestStatus = NWDRequestStatus.AccountError;
                            }
                        }
                        else
                        {
                            tDownPayload.AccountSignList.Add(tNewSign);
                            tRequestStatus = NWDRequestStatus.AccountError;
                        }
                    }
                }
                else
                {
                    // weird!  sign player and request are not constant in environment and project 
                }
            }
            else
            {
                // weird!  signs are equal
            }

            // Add services to response
            tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(sAccountInformation.PlayerToken);
            return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sAccountInformation.PlayerToken, sRequestRuntime.Kind, tDownPayload, tRequestStatus);
        }
        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sAccountInformation.PlayerToken, sRequestRuntime.Kind, null, NWDRequestStatus.Error, tDebug);
    }

    public static NWDResponseRuntime ProcessSignDelete(NWDRequestRuntime sRequestRuntime, NWDAccountInformation sAccountInformation)
    {
        string tDebug =  string.Empty;
        NWDRequestStatus tRequestStatus;
        NWDUpPayloadAccountSignDelete tUpPayload = sRequestRuntime.GetPayload<NWDUpPayloadAccountSignDelete>(NWDServerMiddleConfiguration.KConfig);
        if (tUpPayload != null)
        {
            NWDDownPayloadAccountSignDelete tDownPayload = new NWDDownPayloadAccountSignDelete();

            NWDAccountSign tSign = tUpPayload.AccountSign;
            if (sAccountInformation.PlayerToken != null)
            {
                List<NWDAccountSign> tAllSign = SignsForAccount(sAccountInformation.PlayerToken);
                if (tAllSign.Count <= 1)
                {
                    tRequestStatus = NWDRequestStatus.Error;
                }
                else
                {
                    if (AccountSignDelete(tSign, sAccountInformation.PlayerToken))
                    {
                        tRequestStatus = NWDRequestStatus.Ok;
                    }
                    else
                    {
                        tRequestStatus = NWDRequestStatus.Error;
                    }
                }
            }
            else
            {
                tRequestStatus = NWDRequestStatus.TokenError;
            }

            // Add services to response
            tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(sAccountInformation.PlayerToken);
            return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sAccountInformation.PlayerToken, sRequestRuntime.Kind, tDownPayload, tRequestStatus);
        }
        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sAccountInformation.PlayerToken, sRequestRuntime.Kind, null, NWDRequestStatus.Error, tDebug);
    }

    public static NWDResponseRuntime ProcessTest(NWDRequestRuntime sRequestRuntime,
        NWDAccountInformation sAccountInformation)
    {
        string tDebug =  string.Empty;
        NWDDownPayloadAccountTest tDownPayload = new NWDDownPayloadAccountTest();
        NWDRequestStatus tRequestStatus = NWDRequestStatus.Ok;
        // Add services to response
        tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(sAccountInformation.PlayerToken);
        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sAccountInformation.PlayerToken, NWDExchangeRuntimeKind.Test, tDownPayload, tRequestStatus, tDebug);
    }

    public static NWDResponseRuntime ProcessSignAll(NWDRequestRuntime sRequestRuntime,
        NWDAccountInformation sAccountInformation)
    {
        string tDebug =  string.Empty;
        NWDDownPayloadAccountSignAll tDownPayload = new NWDDownPayloadAccountSignAll();
        if (sRequestRuntime.PlayerToken != null)
        {
            tDownPayload.AccountSignList = SignsForAccount(sRequestRuntime.PlayerToken);
        }

        NWDRequestStatus tRequestStatus = NWDRequestStatus.Ok;
        // Add services to response
        tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(sAccountInformation.PlayerToken);
        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sAccountInformation.PlayerToken, sRequestRuntime.Kind, tDownPayload, tRequestStatus, tDebug);
    }

    public static NWDResponseRuntime ProcessSignLost(NWDRequestRuntime sRequestRuntime /*, NWDAccountInformation sAccountInformation*/)
    {
        string tDebug =  string.Empty;
        NWDRequestStatus tRequestStatus = NWDRequestStatus.Error;
        NWDUpPayloadAccountSignLost tUpPayload = sRequestRuntime.GetPayload<NWDUpPayloadAccountSignLost>(NWDServerMiddleConfiguration.KConfig);
        if (tUpPayload != null)
        {
            NWDDownPayloadAccountSignLost tDownPayload = new NWDDownPayloadAccountSignLost();
            // TODO : todo
            NWDAccountSign tLost = NWDAccountSign.CreateEmailPassword(tUpPayload.RescueEmail, "none", sRequestRuntime.ProjectId);
            if (CheckIfSignIsValid(tLost, sRequestRuntime.ProjectId))
            {
                Dictionary<INWDAccountSignDao, List<NWDAccountSign>> tDictionaryOfSign = new Dictionary<INWDAccountSignDao, List<NWDAccountSign>>();
                // je cherche une signature de rescue qui correspond à cette donnée et génére un token de restauration 
                foreach (INWDAccountSignDao? tDao in DaoList)
                {
                    if (sRequestRuntime.PlayerToken != null)
                    {
                        List<NWDAccountSign> tListTemp = tDao.GetBy(sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.ProjectId, new Dictionary<string, string>()
                        {
                            { nameof(NWDAccountSign.RescueHash), tLost.RescueHash },
                            { nameof(NWDAccountSign.SignStatus), ((int)NWDAccountSignAction.Associated).ToString() },
                        });
                        foreach (NWDAccountSign tSign in tListTemp)
                        {
                            if (tSign.SignType == NWDAccountSignType.EmailPassword || tSign.SignType == NWDAccountSignType.LoginEmailPassword)
                            {
                                if (tDictionaryOfSign.ContainsKey(tDao) == false)
                                {
                                    tDictionaryOfSign.Add(tDao, new List<NWDAccountSign>());
                                }

                                tDictionaryOfSign[tDao].Add(tSign);
                            }
                        }
                    }
                }

                if (tDictionaryOfSign.Keys.Count == 1)
                {
                    foreach (KeyValuePair<INWDAccountSignDao, List<NWDAccountSign>> tKeyValue in tDictionaryOfSign)
                    {
                        if (tKeyValue.Value.Count == 1)
                        {
                            NWDAccountSign tSignRescue = tKeyValue.Value[0];
                            string tToken = tSignRescue.LoginHash + "" + NWDRandom.RandomStringBase64(64); // prefix by LoginHash to be sure that token is unique
                            tSignRescue.TokenRescue = tToken;
                            tSignRescue.TokenRescueLimit = NWDTimestamp.GetTimestampUnix() + NWDServerConfiguration.KConfig.RescueTokenDuration;
                            if (sRequestRuntime.PlayerToken != null)
                            {
                                tKeyValue.Key.Update(sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.ProjectId,
                                    tSignRescue);
                                tDownPayload.RescueTokenSecured = NWDSecurityTools.CryptSomething(tToken,
                                    sRequestRuntime.ProjectId, sRequestRuntime.PlayerToken.EnvironmentKind, NWDServerMiddleConfiguration.KConfig, NWDServerMiddleConfiguration.KConfig);
                            }

                            tDownPayload.SignType = tSignRescue.SignType;
                            tDownPayload.Limit = tSignRescue.TokenRescueLimit;
                            // je retourne un token secure à envoyer par email à la charge du service
                            tRequestStatus = NWDRequestStatus.Ok;
                        }
                        else
                        {
                            tRequestStatus = NWDRequestStatus.AccountNotUnique;
                        }
                    }
                }
                else
                {
                    tRequestStatus = NWDRequestStatus.AccountUnknown;
                }
            }
            else
            {
                tRequestStatus = NWDRequestStatus.Error;
            }

            return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), sRequestRuntime.Kind, tDownPayload, tRequestStatus);
        }
        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, new NWDRequestPlayerToken(sRequestRuntime.PlayerToken), sRequestRuntime.Kind, null, NWDRequestStatus.Error, tDebug);
    }

    public static NWDResponseRuntime ProcessSignRescue(NWDRequestRuntime sRequestRuntime/*, NWDAccountInformation sAccountInformation*/)
    {
        string tDebug =  string.Empty;
            // NWDRequestPlayerToken tNewPlayerToken = new NWDRequestPlayerToken(sAccountInformation.PlayerToken);
            NWDRequestPlayerToken tNewPlayerToken = new NWDRequestPlayerToken(sRequestRuntime.PlayerToken);
            tNewPlayerToken.PlayerReference = 0;
            tNewPlayerToken.AccountRange = 0;
            NWDRequestStatus tRequestStatus = NWDRequestStatus.Error;
            NWDUpPayloadAccountSignRescue tUpPayload = sRequestRuntime.GetPayload<NWDUpPayloadAccountSignRescue>(NWDServerMiddleConfiguration.KConfig);
            if (tUpPayload != null)
            {
                NWDDownPayloadAccountSignRescue tDownPayload = new NWDDownPayloadAccountSignRescue();
                NWDAccountSign tLost = tUpPayload.AccountSign;
                if (CheckIfSignIsValid(tLost, sRequestRuntime.ProjectId))
                {
                    int tTimeStamp = NWDTimestamp.GetTimestampUnix();
                    Dictionary<INWDAccountSignDao, List<NWDAccountSign>> tDictionaryOfSign = new Dictionary<INWDAccountSignDao, List<NWDAccountSign>>();
                    foreach (INWDAccountSignDao? tDao in DaoList)
                    {
                        if (sRequestRuntime.PlayerToken != null)
                        {
                            List<NWDAccountSign> tListTemp = tDao.GetBy(sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.ProjectId, new Dictionary<string, string>()
                            {
                                { nameof(NWDAccountSign.TokenRescue), tLost.TokenRescue },
                                { nameof(NWDAccountSign.RescueHash), tLost.RescueHash },
                                { nameof(NWDAccountSign.SignType), ((int)tLost.SignType).ToString() },
                                { nameof(NWDAccountSign.SignStatus), ((int)NWDAccountSignAction.Associated).ToString() },
                            });
                            foreach (NWDAccountSign tSign in tListTemp.Where(sSign => sSign.SignType is NWDAccountSignType.EmailPassword or NWDAccountSignType.LoginEmailPassword).Where(sSign => sSign.TokenRescueLimit > tTimeStamp))
                            {
                                if (tDictionaryOfSign.ContainsKey(tDao) == false)
                                {
                                    tDictionaryOfSign.Add(tDao, new List<NWDAccountSign>());
                                }

                                tDictionaryOfSign[tDao].Add(tSign);
                            }
                        }
                    }

                    if (tDictionaryOfSign.Keys.Count == 1)
                    {
                        foreach (KeyValuePair<INWDAccountSignDao, List<NWDAccountSign>> tKeyValue in tDictionaryOfSign)
                        {
                            if (tKeyValue.Value.Count == 1)
                            {
                                NWDAccountSign tSignRescue = tKeyValue.Value[0];
                                tSignRescue.TokenRescue = string.Empty;
                                tSignRescue.TokenRescueLimit = 0;
                                tSignRescue.RescueHash = tLost.RescueHash;
                                tSignRescue.SignHash = tLost.SignHash;
                                tSignRescue.LoginHash = tLost.LoginHash;
                                if (sRequestRuntime.PlayerToken != null)
                                    tKeyValue.Key.Update(sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.ProjectId,
                                        tSignRescue);

                                NWDAccount? tAccount = AccountForSign(tSignRescue, tNewPlayerToken);
                                if (tAccount != null)
                                {
                                    tNewPlayerToken.AccountRange = tAccount.Range;
                                    tNewPlayerToken.PlayerReference = tAccount.Reference;
                                    tNewPlayerToken = NWDAccountTokenManager.NewToken(tNewPlayerToken);
                                }

                                tRequestStatus = NWDRequestStatus.Ok;
                            }
                            else
                            {
                                tRequestStatus = NWDRequestStatus.AccountNotUnique;
                            }
                        }
                    }
                    else
                    {
                        tRequestStatus = NWDRequestStatus.AccountUnknown;
                    }
                }
                else
                {
                    tRequestStatus = NWDRequestStatus.Error;
                }

                // Add services to response
                tDownPayload.AccountServiceList = NWDAccountServiceManager.AddServices(tNewPlayerToken);
                return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, tNewPlayerToken, sRequestRuntime.Kind, tDownPayload, tRequestStatus);
            }

        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, new NWDRequestPlayerToken(sRequestRuntime.ProjectId, sRequestRuntime.Environment), sRequestRuntime.Kind, null, NWDRequestStatus.Error, tDebug);
    }

    public static NWDResponseRuntime ProcessAccountDelete(NWDRequestRuntime sRequestRuntime,
        NWDAccountInformation sAccountInformation)
    {
        string tDebug =  string.Empty;
        NWDDownPayloadAccountDelete tDownPayload = new NWDDownPayloadAccountDelete();
        // TODO : todo
        // je detruit l'account en le mettant en trash et je detruit toutes les tokens de ce compte
        // il ne pourra plus se conecter 
        // TODO : remove automatic   NWDRequestStatus.Ok;
        NWDRequestStatus tRequestStatus = NWDRequestStatus.Ok;
        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sAccountInformation.PlayerToken, sRequestRuntime.Kind, tDownPayload, tRequestStatus, tDebug);
    }

    public static NWDResponseRuntime ProcessAccountChangeRange(NWDRequestRuntime sRequestRuntime, NWDAccountInformation sAccountInformation)
    {
        string tDebug =  string.Empty;
        NWDDownPayloadAccountChangeRange tDownPayload = new NWDDownPayloadAccountChangeRange();
        // TODO : todo
        // je dois modifier le range pour le nouveau
        // je dois mettre le player en migration
        // je copie toutes données de lancien serveur vers le nouveau serveur
        // je change le range du account
        // je detruis toutes les données de l'ancien serveur
        // je mete le player en on ligne 
        // TODO : remove automatic   NWDRequestStatus.Ok;
        NWDRequestStatus tRequestStatus = NWDRequestStatus.Ok;
        return new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sAccountInformation.PlayerToken, sRequestRuntime.Kind, tDownPayload, tRequestStatus, tDebug);
    }

    #endregion
}