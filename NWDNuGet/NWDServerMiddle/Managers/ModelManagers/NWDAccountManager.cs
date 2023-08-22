using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDServerBack;
using NWDServerMiddle.Models;
using NWDServerMiddle.Models.Enum;

namespace NWDServerMiddle.Managers.ModelManagers
{
    public static class NWDAccountManager
    {
        #region Static properties

        public static List<INWDAccountDao> DaoList = new List<INWDAccountDao>();
        public static readonly Dictionary<ushort, INWDAccountDao> DaoByRange = new Dictionary<ushort, INWDAccountDao>();

        #endregion

        #region Dao

        public static void Prepare()
        {
            DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDAccountDao>(NWDConfigurationDatabase.KConfig.DatabaseAccountArray);
            foreach (INWDAccountDao tDao in DaoList)
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
                        NWDLogger.TraceAttention("Must create table for  "+tDao.FingerPrintTableName(tEnvironment));
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
                foreach (INWDAccountDao tDao in DaoList)
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
                foreach (INWDAccountDao tDao in DaoList)
                {
                    tDao.DeleteTable(tEnvironment);
                    NWDCrucialInformationManager.ResetFingerPrintTable(tEnvironment, tDao);
                }
            }
        }

        public static INWDAccountDao? GetOneDao()
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

        public static INWDAccountDao? GetDaoByRange(ushort sRange)
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

        public static NWDAccount? CreateNewAccount(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId)
        {
            NWDAccount tAccount = new NWDAccount();
            INWDAccountDao? tDao = GetOneDao();
            if (tDao != null)
            {
                tAccount.Range = tDao.GetRange();
                return tDao.Create(sEnvironmentKind, sProjectId, tAccount);
            }
            else
            {
                return null;
            }
        }

        public static NWDAccount? GetAccountFor(NWDAccountSign sAccountSign, NWDRequestPlayerToken sPlayerToken)
        {
            NWDAccount rReturn = null;
            INWDAccountDao? tDao = GetDaoByRange(sAccountSign.Range);
            if (tDao != null)
            {
                List<NWDAccount> tAccountList = tDao.GetBy(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId,
                    new Dictionary<string, string>()
                    {
                        { nameof(NWDAccount.Reference), sAccountSign.Account.ToString() }
                    });
                if (tAccountList.Count == 1)
                {
                    rReturn = tAccountList[0];
                }
            }
            return rReturn;
        }

        public static NWDAccountInformation CheckRequest(NWDRequestPlayerToken? sPlayerToken)
        {
            if (sPlayerToken != null && NWDAccountTokenManager.DaoByRange.ContainsKey(sPlayerToken.AccountRange) == false)
            {
                if (sPlayerToken.PlayerReference > NWDConstants.K_REFERENCE_AREA_GLOBAL)
                {
                    sPlayerToken.AccountRange = NWDAccount.ExtractRange(sPlayerToken.PlayerReference);
                }
                // else
                // {
                //     List<ushort> tKeyList = new List<ushort>(NWDAccountTokenManager.DaoByRange.Keys);
                //     tKeyList.Shuffle();
                //     sPlayerToken.AccountRange = tKeyList[0];
                // }
            }

            NWDAccountInformation rReturn = new NWDAccountInformation()
            {
                Status = NWDAccountStatus.Unknown,
                Account = null,
                PlayerToken = null,
                RequestStatus = NWDRequestStatus.None
            };
            // Use Token
            NWDRequestPlayerToken tNewRequestPlayerToken = NWDAccountTokenManager.Use(sPlayerToken);
            rReturn.PlayerToken = tNewRequestPlayerToken;
            if (string.IsNullOrEmpty(tNewRequestPlayerToken.Token) == false)
            {
                // Get Account
                if (sPlayerToken != null)
                {
                    INWDAccountDao? tDao = GetDaoByRange(sPlayerToken.AccountRange);
                    if (tDao != null)
                    {
                        List<NWDAccount> tAccountList = tDao.GetBy(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId,
                            new Dictionary<string, string>()
                            {
                                { nameof(NWDAccount.Reference), sPlayerToken.PlayerReference.ToString() }
                            });
                        if (tAccountList.Count == 1)
                        {
                            rReturn.Account = tAccountList[0];
                            rReturn.Status = NWDAccountStatus.Valid;
                            rReturn.RequestStatus = NWDRequestStatus.None;
                            if (rReturn.Account.Trashed)
                            {
                                rReturn.Status = NWDAccountStatus.AccountTrashed;
                                rReturn.RequestStatus = NWDRequestStatus.AccountTrashed;
                            }
                            else
                            {
                                if (rReturn.Account.Ban)
                                {
                                    rReturn.Status = NWDAccountStatus.AccountBan;
                                    rReturn.RequestStatus = NWDRequestStatus.AccountBan;
                                }
                            }
                        }
                        else if (tAccountList.Count == 0)
                        {
                            rReturn.Status = NWDAccountStatus.AccountUnknown;
                            rReturn.RequestStatus = NWDRequestStatus.AccountUnknown;
                        }
                        else
                        {
                            rReturn.Status = NWDAccountStatus.AccountNotUnique;
                            rReturn.RequestStatus = NWDRequestStatus.AccountNotUnique;
                        }
                    }
                    else
                    {
                        rReturn.Status = NWDAccountStatus.DaoError;
                        rReturn.RequestStatus = NWDRequestStatus.DaoError;
                    }
                }
            }
            else
            {
                    rReturn.Status = NWDAccountStatus.TokenError;
                    rReturn.RequestStatus = NWDRequestStatus.TokenEmpty;
            }

            return rReturn;
        }

        #endregion
    }
}