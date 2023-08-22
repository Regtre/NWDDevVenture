using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NWDCrucial.Exchanges.Payloads;
using NWDDevServerMiddleBack.Tools;
using NWDEditor.Exchanges.Payloads;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDServerBack;
using Org.BouncyCastle.Asn1.X509;

namespace NWDServerMiddle.Managers.ModelManagers;

public static class NWDAccountServiceManager
{
    #region Static properties

    public static List<INWDAccountServiceDao> DaoList = new List<INWDAccountServiceDao>();

    public static readonly Dictionary<ushort, INWDAccountServiceDao> DaoByRange =
        new Dictionary<ushort, INWDAccountServiceDao>();

    #endregion

    #region Dao

    public static void Prepare()
    {
        DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDAccountServiceDao>(NWDConfigurationDatabase.KConfig
            .DatabaseAccountArray);
        foreach (INWDAccountServiceDao tDao in DaoList)
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
            foreach (INWDAccountServiceDao tDao in DaoList)
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
            foreach (INWDAccountServiceDao tDao in DaoList)
            {
                tDao.DeleteTable(tEnvironment);
                NWDCrucialInformationManager.ResetFingerPrintTable(tEnvironment, tDao);
            }
        }
    }

    public static INWDAccountServiceDao? GetOneDao()
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

    public static INWDAccountServiceDao? GetDaoByRange(ushort sRange)
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

    #region methods

    public static List<NWDAccountService> AddServices(NWDRequestPlayerToken? sPlayerToken)
    {
        List<NWDAccountService> rReturn = new List<NWDAccountService>();
        if (sPlayerToken != null && sPlayerToken.PlayerReference != 0)
        {
            INWDAccountServiceDao? tDao = GetDaoByRange(sPlayerToken.AccountRange);
            if (tDao != null)
            {
                rReturn = tDao.GetBy(sPlayerToken.EnvironmentKind, sPlayerToken.ProjectId,
                    new Dictionary<string, string>()
                    {
                        {
                            nameof(NWDAccountService.Account),
                            sPlayerToken.PlayerReference.ToString(CultureInfo.InvariantCulture)
                        }
                    });
            }
            // TODO Add services from another Account ... mistake if account range is not the same! ... SHIT !
            // TODO need to use webtreat to manage services to dupplicate from relation ship.
        }

        return rReturn;
    }

    public static NWDDownPayloadAssociateService AssociateService(
        NWDUpPayloadAssociateService? sPayloadAssociateService)
    {
        NWDDownPayloadAssociateService rReturn = new NWDDownPayloadAssociateService();
        if (sPayloadAssociateService is { AccountService: { } /*, PlayerToken: { }*/ })
        {
            // INWDAccountServiceDao? tDao = GetDaoByRange(sPayloadAssociateService.PlayerToken.AccountRange);
            // tDao?.InsertOrUpdate(sPayloadAssociateService.PlayerToken.EnvironmentKind,sPayloadAssociateService.PlayerToken.ProjectId,sPayloadAssociateService.AccountService);
            INWDAccountServiceDao? tDao =
                GetDaoByRange(NWDAccount.ExtractRange(sPayloadAssociateService.AccountService.Account));
            tDao?.InsertOrUpdate(sPayloadAssociateService.AccountService.EnvironmentKind,
                sPayloadAssociateService.AccountService.ProjectId, sPayloadAssociateService.AccountService);
        }

        return rReturn;
    }

    public static NWDDownPayloadAssociateSubService AssociateSubService(
        NWDUpPayloadAssociateSubService? sPayloadAssociateService)
    {
        NWDDownPayloadAssociateSubService rReturn = new NWDDownPayloadAssociateSubService();
        if (sPayloadAssociateService is { OfferByAccount: { }, OfferToAccount: { } })
        {
            INWDAccountServiceDao? tDaoAccountTo =
                GetDaoByRange(NWDAccount.ExtractRange(sPayloadAssociateService.OfferToAccount.Reference));

            Dictionary<string, string> tByReferenceService = new Dictionary<string, string>()
            {
                { nameof(NWDAccountService.Service), sPayloadAssociateService.OfferByService.Service.ToString() },
                {
                    nameof(NWDAccountService.FromAccountService),
                    sPayloadAssociateService.OfferByAccount.Reference.ToString()
                }
            };

            NWDAccountService? isAlreadyInsertedofThisService = tDaoAccountTo.GetBy(
                sPayloadAssociateService.OfferByService.EnvironmentKind,
                sPayloadAssociateService.OfferByService.ProjectId, tByReferenceService).FirstOrDefault();
            NWDAccountService? sInsertedService = null;
            if (isAlreadyInsertedofThisService == null)
            {
                NWDAccountService tNewService = NWDAccountService.CreateSubService(
                    sPayloadAssociateService.OfferByService,
                    sPayloadAssociateService.OfferToAccount);
                tNewService.Modification = TimeTools.DateTimeToTimestampUnix(DateTime.Now);
                sInsertedService = tDaoAccountTo?.InsertOrUpdate(
                    sPayloadAssociateService.OfferByService.EnvironmentKind,
                    sPayloadAssociateService.OfferByService.ProjectId,
                    tNewService
                );
            }

            if (sInsertedService != null)
            {
                INWDAccountServiceDao? tDaoAccountFrom =
                    GetDaoByRange(NWDAccount.ExtractRange(sPayloadAssociateService.OfferByAccount.Reference));
                sPayloadAssociateService.OfferByService.ToAccount =
                    new NWDReference<NWDAccount>(sPayloadAssociateService.OfferToAccount.Reference);
                sPayloadAssociateService.OfferByService.ToAccountService =
                    new NWDReference<NWDAccountService>(sInsertedService.Reference);
                sPayloadAssociateService.OfferByService.Modification = TimeTools.DateTimeToTimestampUnix(DateTime.Now);
                tDaoAccountFrom?.InsertOrUpdate(
                    sPayloadAssociateService.OfferByService.EnvironmentKind,
                    sPayloadAssociateService.OfferByService.ProjectId,
                    sPayloadAssociateService.OfferByService); //TODO NWD Account reveive only have ref valid ?? 
            }
        }

        return rReturn;
    }

    public static NWDDownPayloadCrucial DissociateServiceAndSubServices(
        NWDUpPayloadDissociateServiceAndSubService? sPayload, ulong sProjectId, NWDEnvironmentKind sEnvironment)
    {
        NWDDownPayloadAssociateSubService rReturn = new NWDDownPayloadAssociateSubService();
        if (sPayload != null)
        {
            Dictionary<string, string> tByReferenceService = new Dictionary<string, string>()
            {
                { nameof(NWDAccountService.Reference), sPayload.ServiceReference.Reference.ToString() },
            };

            NWDAccountService? tService =
                GetDaoByRange(NWDAccount.ExtractRange(sPayload.AccountReference.Reference))?
                    .GetBy(sEnvironment, sProjectId, tByReferenceService).FirstOrDefault();
            
            
            tByReferenceService[nameof(NWDAccountService.Reference)] =
                tService.FromAccountService.Reference.ToString();
            NWDAccountService? tSourceService =
                GetDaoByRange(NWDAccount.ExtractRange(tService.OfferByAccount.Reference))?
                    .GetBy(sEnvironment, sProjectId, tByReferenceService).FirstOrDefault();
            if (tSourceService != null)
            {
                tSourceService.ToAccountService = new NWDReference<NWDAccountService>()
                {
                    Reference = 0
                };
                tSourceService.ToAccount = new NWDReference<NWDAccount>()
                {
                    Reference = 0
                };
                tSourceService.Modification = TimeTools.DateTimeToTimestampUnix(DateTime.Now);
            }

            GetDaoByRange(NWDAccount.ExtractRange(tService.OfferByAccount.Reference))?
                .InsertOrUpdate(sEnvironment, sProjectId, tSourceService);
            GetDaoByRange(NWDAccount.ExtractRange(sPayload.AccountReference.Reference))?
                .Delete(sEnvironment, sProjectId, sPayload.ServiceReference.Reference);
            /*
            if (tAccountService != null)
            {
                

            }*/

            /*while (tAccountService != null)
            {
             
               

                if (tAccountService.ToAccountService.Reference != 0)
                {
                    Console.WriteLine("The service has sub service");
                    Dictionary<string, string> tByReference = new Dictionary<string, string>()
                    {
                        { nameof(NWDAccountService.Reference), tAccountService.ToAccountService.Reference.ToString() },
                    };
                    tAccountService = GetDaoByRange(NWDAccount.ExtractRange(tAccountService.ToAccount.Reference))
                        ?.GetBy(tAccountService.EnvironmentKind, tAccountService.ProjectId, tByReference).FirstOrDefault();
                    Console.WriteLine("Sub Service to dissociate : " + tAccountService?.Reference);
                }
                else
                {
                    Console.WriteLine("No sub service");
                    break; // no more service to dissociate
                }
              
            }*/
        }

        return rReturn;
    }

    #endregion


    public static NWDDownPayloadCrucial? GetSubServicesFromAccount(NWDUpPayloadGetSubServicesFromAccount? sPayload)
    {
        NWDDownPayloadGetSubServicesFromAccount rReturn = new NWDDownPayloadGetSubServicesFromAccount();
        if (sPayload is { Account: { } })
        {
            List<NWDAccountService> tAccountServices = new List<NWDAccountService>();
            Dictionary<string, string> tGetBy = new Dictionary<string, string>()
            {
                { nameof(NWDAccountService.OfferByAccount), sPayload.Account.Reference.ToString() },
            };
            foreach (INWDAccountServiceDao tDao in DaoList)
            {
                tAccountServices.AddRange(tDao.GetBy(sPayload.Environment, sPayload.Account.ProjectId, tGetBy));
            }

            rReturn.SubServices = tAccountServices;
        }

        return rReturn;
    }
}