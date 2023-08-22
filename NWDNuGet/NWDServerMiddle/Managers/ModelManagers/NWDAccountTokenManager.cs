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

namespace NWDServerMiddle.Managers.ModelManagers;

public static class NWDAccountTokenManager
{
    #region Static properties

    public static List<INWDAccountTokenDao> DaoList = new List<INWDAccountTokenDao>();

    public static readonly Dictionary<ushort, INWDAccountTokenDao> DaoByRange = new Dictionary<ushort, INWDAccountTokenDao>();

    private const int _TOKEN_LENGHT = 64;

    #endregion

    #region Dao

    public static void Prepare()
    {
        DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDAccountTokenDao>(NWDConfigurationDatabase.KConfig.DatabaseAccountArray);
        foreach (INWDAccountTokenDao tDao in DaoList)
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
            foreach (INWDAccountTokenDao tDao in DaoList)
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
            foreach (INWDAccountTokenDao tDao in DaoList)
            {
                tDao.DeleteTable(tEnvironment);
                NWDCrucialInformationManager.ResetFingerPrintTable(tEnvironment, tDao);
            }
        }
    }

    private static INWDAccountTokenDao? GetOneDao()
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

    private static INWDAccountTokenDao? GetDaoByRange(ushort sRange)
    {
        if (DaoByRange.ContainsKey(sRange))
        {
            return DaoByRange[sRange];
        }
        else
        {
            return GetOneDao();
        }
    }

    #endregion

    #region Methods to use and generate tokens

    public static bool Test(NWDRequestPlayerToken sPlayerToken)
    {
        bool rReturn = false;
        INWDAccountTokenDao? tDao = GetDaoByRange(sPlayerToken.AccountRange);
        if (tDao != null)
        {
            if (sPlayerToken.Token != null)
            {
                List<NWDAccountToken> tTokenList = tDao.GetBy(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId,
                    new Dictionary<string, string>()
                    {
                        { nameof(NWDAccountToken.Account), sPlayerToken.PlayerReference.ToString() },
                        { nameof(NWDAccountToken.Token), sPlayerToken.Token },
                        { nameof(NWDAccountToken.ExchangeOrigin), ((int)sPlayerToken.ExchangeOrigin).ToString() }
                    });
                if (tTokenList.Count == 1)
                {
                    rReturn = true;
                }
            }
        }

        return rReturn;
    }

    public static NWDRequestPlayerToken Use(NWDRequestPlayerToken? sPlayerToken)
    {
        NWDRequestPlayerToken rReturn = new NWDRequestPlayerToken(sPlayerToken);
        if (sPlayerToken != null)
        {
            rReturn = new NWDRequestPlayerToken(sPlayerToken)
            {
                Token = string.Empty,
                OldToken = sPlayerToken?.Token
            };
            if (sPlayerToken != null)
            {
                INWDAccountTokenDao? tDao = GetDaoByRange(sPlayerToken.AccountRange);
                if (tDao != null)
                {
                    if (sPlayerToken.Token != null)
                    {
                        List<NWDAccountToken> tTokenList = tDao.GetBy(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId,
                            new Dictionary<string, string>
                            {
                                { nameof(NWDAccountToken.Account), sPlayerToken.PlayerReference.ToString() },
                                { nameof(NWDAccountToken.Token), sPlayerToken.Token },
                                { nameof(NWDAccountToken.ExchangeOrigin), ((int)sPlayerToken.ExchangeOrigin).ToString() }
                            });
                        if (tTokenList.Count == 1)
                        {
                            switch (sPlayerToken.ExchangeOrigin)
                            {
                                case NWDExchangeOrigin.Web:
                                {
                                    NWDAccountToken tToken = tTokenList[0];
                                    rReturn.Token = tToken.Token;
                                    rReturn.AccountRange = tToken.Range;
                                }
                                    break;
                                case NWDExchangeOrigin.App:
                                case NWDExchangeOrigin.Game:
                                case NWDExchangeOrigin.Unknown:
                                default:
                                {
                                    NWDAccountToken tToken = tTokenList[0];
                                    tToken.Token = NWDRandom.RandomStringToken(_TOKEN_LENGHT);
                                    tDao.Update(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId, tToken);
                                    rReturn.Token = tToken.Token;
                                    rReturn.AccountRange = tToken.Range;
                                }
                                    break;
                            }
                        }
                        else
                        {
                            //NWDLogger.Critical("tTokenList.Count  = " +tTokenList.Count);
                            DeleteAllToken(sPlayerToken);
                            // TODO Disconnect when token is empty!
                            rReturn.Token = string.Empty;
                        }
                    }
                }
                else
                {
                    NWDLogger.Critical("DAO error");
                }
            }
        }

        return rReturn;
    }

    public static NWDRequestPlayerToken NewToken(NWDRequestPlayerToken sPlayerToken)
    {
        // NWDLogger.WriteLine(nameof(NWDRequestPlayerToken) + "." + nameof(NewToken) + "()");
        // NWDLogger.WriteLine(nameof(NWDRequestPlayerToken) + "." + nameof(NewToken) + "() => sPlayerToken => EnvironmentKind " + sPlayerToken.EnvironmentKind);
        // NWDLogger.WriteLine(nameof(NWDRequestPlayerToken) + "." + nameof(NewToken) + "() => sPlayerToken => GetProjectId " + sPlayerToken.GetProjectId);
        // NWDLogger.WriteLine(nameof(NWDRequestPlayerToken) + "." + nameof(NewToken) + "() => sPlayerToken => PlayerReference " + sPlayerToken.PlayerReference);
        // NWDLogger.WriteLine(nameof(NWDRequestPlayerToken) + "." + nameof(NewToken) + "() => sPlayerToken => AccountRange " + sPlayerToken.AccountRange);
        // NWDLogger.WriteLine(nameof(NWDRequestPlayerToken) + "." + nameof(NewToken) + "() => sPlayerToken => ExchangeOrigin " + sPlayerToken.ExchangeOrigin);
        // NWDLogger.WriteLine(nameof(NWDRequestPlayerToken) + "." + nameof(NewToken) + "() => sPlayerToken => Token " + sPlayerToken.Token);
        // NWDLogger.WriteLine(nameof(NWDRequestPlayerToken) + "." + nameof(NewToken) + "() => sPlayerToken => OldToken " + sPlayerToken.OldToken);
        NWDRequestPlayerToken rReturn = new NWDRequestPlayerToken(sPlayerToken)
        {
            AccountRange = sPlayerToken.AccountRange,
            PlayerReference = sPlayerToken.PlayerReference,
            ProjectId = sPlayerToken.ProjectId
        };
        INWDAccountTokenDao? tDao = GetDaoByRange(rReturn.AccountRange);
        if (tDao != null)
        {
            rReturn.Token = NWDRandom.RandomStringToken(_TOKEN_LENGHT);
            rReturn.OldToken = string.Empty;
            rReturn.EnvironmentKind = sPlayerToken.EnvironmentKind;
            rReturn.ExchangeOrigin = sPlayerToken.ExchangeOrigin;
            DeleteAllToken(rReturn);
            NWDAccountToken tToken = new NWDAccountToken(rReturn);
            NWDAccountToken tTokenCreated = tDao.Create(rReturn.EnvironmentKind, rReturn.ProjectId, tToken);
        }
        else
        {
        }
        
        return rReturn;
    }

    public static bool DeleteAllToken(NWDRequestPlayerToken? sPlayerToken)
    {
        bool rReturn = true;
        if (sPlayerToken != null)
        {
            INWDAccountTokenDao? tDao = GetDaoByRange(sPlayerToken.AccountRange);
            if (tDao != null)
            {
                List<NWDAccountToken> tTokenList = tDao.GetBy(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId,
                    new Dictionary<string, string>()
                    {
                        { nameof(NWDAccountToken.Account), sPlayerToken.PlayerReference.ToString() },
                        { nameof(NWDAccountToken.ExchangeOrigin), ((int)sPlayerToken.ExchangeOrigin).ToString() }
                    });
                foreach (NWDAccountToken tToken in tTokenList)
                {
                    tDao.Delete(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId, tToken.Reference);
                }
            }
            else
            {
                rReturn = false;
            }
        }

        return rReturn;
    }

    public static bool DeleteAllTokenForAllOrigin(NWDRequestPlayerToken sPlayerToken)
    {
        bool rReturn = true;
        INWDAccountTokenDao? tDao = GetDaoByRange(sPlayerToken.AccountRange);
        if (tDao != null)
        {
            List<NWDAccountToken> tTokenList = tDao.GetBy(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId,
                new Dictionary<string, string>()
                {
                    { nameof(NWDAccountToken.Account), sPlayerToken.PlayerReference.ToString() },
                });
            foreach (NWDAccountToken tToken in tTokenList)
            {
                tDao.Delete(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId, tToken.Reference);
            }
        }
        else
        {
            rReturn = false;
        }

        return rReturn;
    }

    #endregion
}