using System;
using System.Collections.Generic;
using System.Linq;
using NWDDevServerMiddleBack.Tools;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDRuntime.Models;
using NWDServerBack;
using NWDServerMiddle.Configuration;

namespace NWDServerMiddle.Managers.ModelManagers;

public class NWDRelationshipManager
{
    #region Static properties

    private static List<INWDRelationshipDao> DaoList = new List<INWDRelationshipDao>();

    private static readonly Dictionary<ushort, INWDRelationshipDao> DaoByRange =
        new Dictionary<ushort, INWDRelationshipDao>();

    #endregion

    #region Dao

    public static void Prepare()
    {
        DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDRelationshipDao>(NWDConfigurationDatabase.KConfig
            .DatabaseAccountArray);
        foreach (INWDRelationshipDao tDao in DaoList)
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
                if (tDao.FingerPrintTable(tEnvironment) !=
                    NWDCrucialInformationManager.GetFingerPrintTable(tEnvironment, tDao))
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
            foreach (INWDRelationshipDao tDao in DaoList)
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
            foreach (INWDRelationshipDao tDao in DaoList)
            {
                tDao.DeleteTable(tEnvironment);
                NWDCrucialInformationManager.ResetFingerPrintTable(tEnvironment, tDao);
            }
        }
    }

    public static INWDRelationshipDao? GetOneDao()
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

    public static INWDRelationshipDao? GetDaoByRange(ushort sRange)
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


    public static NWDRelationship CreateRelationship(NWDRequestRuntime sRequestRuntime)
    {
        NWDRelationship rRelationship = new NWDRelationship()
        {
            AccountA = new NWDReference<NWDAccount>(sRequestRuntime.PlayerToken.PlayerReference),
            RelationshipState = NWDRelationshipState.Pending,
            ModificationDate = TimeTools.GetNowTimestampUnix(),
        };
        string code = NWDRandom.RandomStringNoMistake(NWDServerMiddleConfiguration.KConfig.RelationshipCodeLength);

        INWDRelationshipDao? tDao = GetDaoByRange(NWDAccount.ExtractRange(sRequestRuntime.PlayerToken.PlayerReference));
        if (
            tDao?.GetBy(sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.PlayerToken.ProjectId,
                new Dictionary<string, string>()
                {
                    { nameof(NWDRelationship.Code), code }
                }).Count == 0)
        {
            rRelationship.Code = code;
            rRelationship.CodeExpiryDate = TimeTools.GetNowTimestampUnix() +
                                           NWDServerMiddleConfiguration.KConfig
                                               .RelationshipCodeValidationLengthInSeconds;

            tDao.InsertOrUpdate(sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.PlayerToken.ProjectId,
                rRelationship);
        }

        return rRelationship;
    }

    public static NWDDownPayloadLinkRelationship LinkRelationship(NWDRequestRuntime sRequestRuntime)
    {
        INWDRelationshipDao? tDao = GetDaoByRange(NWDAccount.ExtractRange(sRequestRuntime.PlayerToken.PlayerReference));
        NWDUpPayloadLinkRelationship tPayload =
            sRequestRuntime.GetPayload<NWDUpPayloadLinkRelationship>(NWDServerMiddleConfiguration.KConfig);
        NWDDownPayloadLinkRelationship rPayload = new NWDDownPayloadLinkRelationship();
        foreach (INWDRelationshipDao tRelationshipDao in DaoList)
        {
            List<NWDRelationship>? tRelationships = tRelationshipDao?.GetBy(sRequestRuntime.PlayerToken.EnvironmentKind,
                sRequestRuntime.PlayerToken.ProjectId,
                new Dictionary<string, string>()
                {
                    { nameof(NWDRelationship.Code), tPayload.code }
                });
            if (tRelationships != null && tRelationships.Count == 1)
            {
                NWDRelationship tRelation = tRelationships.First();

                if (sRequestRuntime.PlayerToken.PlayerReference == tRelation.AccountA.Reference)
                {
                    rPayload.LinkStatus = NWDRelationshipLinkStatus.SelfAssociation;
                }
                else if (tRelation.AccountB.Reference != 0)
                {
                    rPayload.LinkStatus = NWDRelationshipLinkStatus.AlreadyExisting;
                }
                else if (tRelation.CodeExpiryDate <
                         (TimeTools.GetNowTimestampUnix() + NWDServerMiddleConfiguration.KConfig.GraceTimeInSeconds))
                {
                    rPayload.LinkStatus = NWDRelationshipLinkStatus.Expired;
                }
                else
                {
                    tRelation.AccountB = new NWDReference<NWDAccount>(sRequestRuntime.PlayerToken.PlayerReference);
                    tRelation.RelationshipState = NWDRelationshipState.WaitingConfirmation;
                    tRelation.ModificationDate = TimeTools.GetNowTimestampUnix();

                    tDao?.InsertOrUpdate(sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.PlayerToken.ProjectId,
                        tRelationships.First());
                    rPayload.LinkStatus = NWDRelationshipLinkStatus.Valid;
                }
            }
        }
        return rPayload;  
    }

    public static void FinalizeRelationship(NWDRequestRuntime sRequestRuntime)
    {
        INWDRelationshipDao? tDao = GetDaoByRange(NWDAccount.ExtractRange(sRequestRuntime.PlayerToken.PlayerReference));
        NWDUpPayloadFinalizeRelationship tPayload =
            sRequestRuntime.GetPayload<NWDUpPayloadFinalizeRelationship>(NWDServerMiddleConfiguration.KConfig);
        List<NWDRelationship>? tRelationships = tDao?.GetBy(sRequestRuntime.PlayerToken.EnvironmentKind,
            sRequestRuntime.PlayerToken.ProjectId,
            new Dictionary<string, string>()
            {
                { nameof(NWDRelationship.Reference), tPayload.RelationshipReference.ToString() }
            });
        NWDDownPayloadFinalizeRelationship rPayload = new NWDDownPayloadFinalizeRelationship();
        
        if (tRelationships != null && tRelationships.Count == 1)
        {
            NWDRelationship tRelation = tRelationships.First();
            if (tPayload.IsAccepted)
            {
                tRelation.RelationshipState = NWDRelationshipState.Accepted;   
            }
            else
            {
                tRelation.RelationshipState = NWDRelationshipState.Refused;
            }
            tRelation.ModificationDate = TimeTools.GetNowTimestampUnix();

            tDao?.InsertOrUpdate(sRequestRuntime.PlayerToken.EnvironmentKind, sRequestRuntime.PlayerToken.ProjectId,
                tRelationships.First());
        }
    }
}