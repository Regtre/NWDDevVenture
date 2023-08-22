using System;
using System.Collections.Generic;
using NWDCrucial.Exchanges;
using NWDCrucial.Exchanges.Payloads;
using NWDCrucial.Facades;
using NWDCrucial.Models;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDServerBack;

namespace NWDServerMiddle.Managers.ModelManagers
{
    public static class NWDProjectCredentialManager
    {
        #region Static properties

        public static List<INWDProjectCredentialsDao> DaoList = new List<INWDProjectCredentialsDao>();
        private static readonly Dictionary<ushort, INWDProjectCredentialsDao> DaoByRange = new Dictionary<ushort, INWDProjectCredentialsDao>();

        #endregion

        #region Dao

        public static void Prepare()
        {
            DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDProjectCredentialsDao>(NWDConfigurationDatabase
                .KConfig.DatabaseStudioArray);
            foreach (INWDProjectCredentialsDao tDao in DaoList)
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

        // public static void CreateQuicklyTables()
        // {
        //     foreach (NWDEnvironmentKind tEnvironment in (NWDEnvironmentKind[])Enum.GetValues(typeof(NWDEnvironmentKind)))
        //     {
        //         foreach (INWDProjectCredentialsDao tDao in DaoList)
        //         {
        //             tDao.CreateTable(tEnvironment);
        //         }
        //     }
        // }

        public static void ForceCreateAllTables()
        {
            foreach (NWDEnvironmentKind tEnvironment in (NWDEnvironmentKind[])Enum.GetValues(typeof(NWDEnvironmentKind)))
            {
                foreach (INWDProjectCredentialsDao tDao in DaoList)
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
                foreach (INWDProjectCredentialsDao tDao in DaoList)
                {
                    tDao.DeleteTable(tEnvironment);
                }
            }
        }

        public static INWDProjectCredentialsDao? GetOneDao()
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

        public static INWDProjectCredentialsDao? GetDaoByRange(ushort sRange)
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

        #region Get

        public static NWDResponseCrucial ProcessPublish(NWDUpPayloadProjectPublish? sProject)
        {
            NWDResponseCrucial rResponse = new NWDResponseCrucial() { Status = NWDRequestStatus.Ok };
            NWDDownPayloadCrucial tPayloadCrucial = new NWDDownPayloadCrucial() { Success = true };
            if (sProject != null)
            {
                foreach (NWDProjectCredentials tProjectCredentials in sProject.ProjectCredentialsList)
                {
                    List<NWDProjectCredentials> tProjectIdList = GetAllByProjectIdAndEnvironment(tProjectCredentials.ProjectId, tProjectCredentials.EnvironmentKind);
                    if (tProjectIdList.Count > 0)
                    {
                        foreach (NWDProjectCredentials tProj in tProjectIdList)
                        {
                            tProj.ProjectKey = tProjectCredentials.ProjectKey;
                            tProj.SecretKey = tProjectCredentials.SecretKey;
                            tProj.TreatKey = tProjectCredentials.TreatKey;
                            tProj.Status = tProjectCredentials.Status;
                            if (UpdateOrRecord(tProj) == false)
                            {
                                tPayloadCrucial.Success = false;
                                rResponse.Debug = rResponse.Debug + "Error in " + nameof(NWDProjectCredentialManager) + "." + nameof(ProcessPublish) + " : " + tProjectCredentials.ProjectId + " " + tProjectCredentials.EnvironmentKind;
                            }
                        }
                    }
                    else
                    {
                        INWDProjectCredentialsDao? tDao = GetOneDao();
                        //TODO insert unique Reference ? !
                        if (UpdateOrRecord(tProjectCredentials) == false)
                        {
                            tPayloadCrucial.Success = false;
                            rResponse.Debug = rResponse.Debug + "Error in " + nameof(NWDProjectCredentialManager) + "." + nameof(ProcessPublish) + " : " + tProjectCredentials.ProjectId + " " + tProjectCredentials.EnvironmentKind;
                        }
                    }
                }

                foreach (NWDProjectServiceKey tProjectServiceKey in sProject.ProjectServiceKeyList)
                {
                    if (NWDProjectServiceKeyManager.UpdateOrRecord(tProjectServiceKey) == false)
                    {
                        tPayloadCrucial.Success = false;
                        rResponse.Debug = rResponse.Debug + "Error in " + nameof(NWDProjectCredentialManager) + "." + nameof(ProcessPublish) + " : " + tProjectServiceKey.ProjectId + " " + tProjectServiceKey.EnvironmentKind;
                    }
                }

                foreach (NWDProjectCredentials tProjectCredentials in sProject.ProjectCredentialsList)
                {
//                     foreach (NWDDefaultCrucialAccount tCrucialAccount in sProject.AccountList)
//                     {
//                         string tPassword = NWDRandom.RandomStringNoMistake(24);
// #if DEBUG
//                         tPassword = "azertyuiop";
// #endif
//                         NWDAccountSign? tAccountSign = NWDAccountSign.CreateEmailPassword(tCrucialAccount.Email, tPassword, tProjectCredentials.ProjectId);
//                         if (NWDAccountSignManager.CheckIfSignLoginHashExists(tProjectCredentials.EnvironmentKind, tAccountSign) == false)
//                         {
//                             NWDAccount? tAccount = NWDAccountManager.CreateNewAccount(tProjectCredentials.EnvironmentKind, tProjectCredentials.ProjectId);
//                             if (tAccount != null)
//                             {
//                                 tAccountSign.Account = tAccount.Reference;
//                                 tAccountSign.Range = tAccount.Range;
//                                 tAccountSign.SignStatus = NWDAccountSignAction.Associated;
//                                 tAccountSign = NWDAccountSignManager.GetDaoByRange(tAccount.Range)?.Create(tProjectCredentials.EnvironmentKind, tProjectCredentials.ProjectId, tAccountSign);
//                                 NWDLogger.Critical("Create account and sign for "+tCrucialAccount.Email+" with password = "+tPassword);
//                             }
//                         }
//                         else
//                         {
//                             NWDLogger.Information("Account "+tCrucialAccount.Email+" already exists!");
//                         }
//
//                         // Find account for this project
//                         if (tAccountSign != null)
//                         {
//                             foreach (NWDProjectServiceKey tProjectServiceKey in sProject.ProjectServiceKeyList)
//                             {
//                                 NWDAccountService tAccountService = new NWDAccountService()
//                                 {
//                                     ProjectId = tAccountSign.ProjectId,
//                                     EnvironmentKind = tProjectCredentials.EnvironmentKind,
//                                     Account = tAccountSign.Account,
//                                     Range = tAccountSign.Range,
//                                     Service = tProjectServiceKey.ServiceId,
//                                     Start = NWDTimestamp.ToTimestampUnix(DateTime.UtcNow),
//                                     End = NWDTimestamp.ToTimestampUnix(DateTime.UtcNow.AddYears(10)),
//                                     OfflineCounterDown = 10000,
//                                     Status = NWDAccountServiceStatus.IsActive,
//                                 };
//                                 INWDAccountServiceDao? tDao = NWDAccountServiceManager.GetDaoByRange(tAccountSign.Range);
//                                 tDao?.InsertOrUpdate(tProjectCredentials.EnvironmentKind, tProjectCredentials.ProjectId, tAccountService);
//                             }
//                         }
//                     }
                }
            }

            return rResponse;
        }

        public static NWDResponseCrucial ProcessClean(NWDUpPayloadProjectClean? sProject)
        {
            NWDResponseCrucial rResponse = new NWDResponseCrucial() { Status = NWDRequestStatus.Ok };
            NWDDownPayloadCrucial tPayloadCrucial = new NWDDownPayloadCrucial() { Success = true };
            if (sProject != null)
            {
                foreach (NWDProjectCredentials tProjectCredentials in sProject.ProjectCredentialsList)
                {
                    //TODO CLEAN ALL PLAYER DATA !
                    //TODO CLEAN ALL STUDIO DATA !
                }

                foreach (NWDProjectAbstractDataTrack tProjectDataTrack in sProject.ProjectDataTrackList)
                {
                    //TODO CLEAN ALL PLAYER DATA !
                    //TODO CLEAN ALL STUDIO DATA !
                }
            }

            return rResponse;
        }

        public static NWDResponseCrucial ProcessDelete(NWDUpPayloadProjectDelete? sProject)
        {
            NWDResponseCrucial rResponse = new NWDResponseCrucial() { Status = NWDRequestStatus.Ok };
            NWDDownPayloadCrucial tPayloadCrucial = new NWDDownPayloadCrucial() { Success = true };
            if (sProject != null)
            {
                foreach (NWDProjectAbstractDataTrack tProjectDataTrack in sProject.ProjectDataTrackList)
                {
                    //TODO DELETE ALL PLAYER DATA !
                    //TODO DELETE ALL STUDIO DATA !
                }

                foreach (NWDProjectCredentials tProjectCredentials in sProject.ProjectCredentialsList)
                {
                    //TODO DELETE ALL PLAYER DATA !
                    //TODO DELETE ALL STUDIO DATA !
                    if (Delete(tProjectCredentials, tProjectCredentials.ProjectId) == false)
                    {
                        tPayloadCrucial.Success = false;
                    }
                    else
                    {
                        rResponse.Debug = rResponse.Debug + "Error in " + nameof(NWDProjectCredentialManager) + "." + nameof(ProcessDelete) + " : " + tProjectCredentials.ProjectId + " " + tProjectCredentials.EnvironmentKind;
                    }
                }
            }

            return rResponse;
        }

        public static bool UpdateOrRecord(NWDProjectCredentials sProjectCredentials)
        {
            bool rReturn = true;
            if (DaoList.Count > 0)
            {
                foreach (INWDProjectCredentialsDao tDao in DaoList)
                {
                    List<NWDProjectCredentials> tResultListInThisDao = tDao.GetBy(sProjectCredentials.EnvironmentKind, sProjectCredentials.ProjectId,
                        new Dictionary<string, string>()
                        {
                            { nameof(NWDProjectCredentials.ProjectId), sProjectCredentials.ProjectId.ToString() },
                            { nameof(NWDProjectCredentials.EnvironmentKind), ((int)sProjectCredentials.EnvironmentKind).ToString() },
                        });
                    if (tResultListInThisDao.Count == 0)
                    {
                        tDao.Create(sProjectCredentials.EnvironmentKind, sProjectCredentials.ProjectId, sProjectCredentials);
                    }
                    else if (tResultListInThisDao.Count == 1)
                    {
                        NWDProjectCredentials tResult = tResultListInThisDao[0];
                        tDao.Update(sProjectCredentials.EnvironmentKind, sProjectCredentials.ProjectId, sProjectCredentials);
                    }
                    else
                    {
                        NWDServerHookSlack tSlack = new NWDServerHookSlack();
                        tSlack.Send("Alert! more than one " + nameof(NWDProjectCredentials) + " (" +
                                    tResultListInThisDao.Count + " results on " + tDao.GetInfos() + ") for " +
                                    nameof(NWDProjectCredentials.ProjectId) + " : " + sProjectCredentials.ProjectId +
                                    " and " + nameof(NWDProjectCredentials.EnvironmentKind) + " : " +
                                    sProjectCredentials.EnvironmentKind.ToString() + "(" +
                                    ((int)sProjectCredentials.EnvironmentKind).ToString() + ") ");
                        rReturn = false;
                    }
                }
            }
            else
            {
                rReturn = false;
                NWDServerHookSlack tSlack = new NWDServerHookSlack();
                tSlack.Send("Alert! no DOA for " + nameof(NWDProjectCredentials) + " ");
            }

            return rReturn;
        }

        public static bool Delete(NWDProjectCredentials sProjectCredentials, ulong sProjectId)
        {
            bool rReturn = true;
            if (DaoList.Count > 0)
            {
                //List<NWDProjectCredentials> tResultList = new List<NWDProjectCredentials>();
                foreach (INWDProjectCredentialsDao tDao in DaoList)
                {
                    List<NWDProjectCredentials> tResultListInThisDao = tDao.GetBy(sProjectCredentials.EnvironmentKind, sProjectId,
                        new Dictionary<string, string>()
                        {
                            { nameof(NWDProjectCredentials.ProjectId), sProjectCredentials.ProjectId.ToString() },
                            { nameof(NWDProjectCredentials.EnvironmentKind), ((int)sProjectCredentials.EnvironmentKind).ToString() },
                        });
                    if (tResultListInThisDao.Count == 0)
                    {
                    }
                    else if (tResultListInThisDao.Count == 1)
                    {
                        NWDProjectCredentials tResult = tResultListInThisDao[0];
                        // update this environment in this database
                        tDao.Delete(sProjectCredentials.EnvironmentKind, sProjectId, tResult.Reference);
                        //tResultList.Add(tResult);
                    }
                    else
                    {
                        foreach (NWDProjectCredentials tEnv in tResultListInThisDao)
                        {
                            tDao.Delete(sProjectCredentials.EnvironmentKind, sProjectId, tEnv.Reference);
                        }
                        NWDServerHookSlack tSlack = new NWDServerHookSlack();
                        tSlack.Send("Alert! more than one " + nameof(NWDProjectCredentials) + " (" +
                                    tResultListInThisDao.Count + " results on " + tDao.GetInfos() + ") for " +
                                    nameof(NWDProjectCredentials.ProjectId) + " : " + sProjectCredentials.ProjectId +
                                    " and " + nameof(NWDProjectCredentials.EnvironmentKind) + " : " +
                                    sProjectCredentials.EnvironmentKind.ToString() + "(" +
                                    ((int)sProjectCredentials.EnvironmentKind).ToString() + ") ");
                        // Error : more than one ProjectID reference for this environment!
                        rReturn = false;
                    }
                }
            }
            else
            {
                rReturn = false;
                NWDServerHookSlack tSlack = new NWDServerHookSlack();
                tSlack.Send("Alert! no DOA for " + nameof(NWDProjectCredentials) + " ");
            }

            return rReturn;
        }

        public static List<NWDProjectCredentials> GetAllByProjectIdAndEnvironment(ulong sProjectId,
            NWDEnvironmentKind sEnvironment)
        {
            List<NWDProjectCredentials> rReturn = new List<NWDProjectCredentials>();
            foreach (INWDProjectCredentialsDao tDao in DaoList)
            {
                List<NWDProjectCredentials> tResultListInThisDao = tDao.GetBy(sEnvironment, sProjectId,
                    new Dictionary<string, string>()
                    {
                        { nameof(NWDProjectCredentials.ProjectId), sProjectId.ToString() },
                        // { nameof(NWDProjectCredentials.Environment), ((int)sEnvironment).ToString() },
                    });
                if (tResultListInThisDao.Count == 0)
                {
                    NWDServerHookSlack tSlack = new NWDServerHookSlack();
                    tSlack.Send("Alert! no " + nameof(NWDProjectCredentials) + " (on " + tDao.GetInfos() + ") for " +
                                nameof(NWDProjectCredentials.ProjectId) + " : " + sProjectId + " and " +
                                nameof(NWDProjectCredentials.EnvironmentKind) + " : " + sEnvironment.ToString() + "(" +
                                ((int)sEnvironment).ToString() + ") ");
                }
                else if (tResultListInThisDao.Count == 1)
                {
                    rReturn.Add(tResultListInThisDao[0]);
                }
                else
                {
                    rReturn.AddRange(tResultListInThisDao);
                    NWDServerHookSlack tSlack = new NWDServerHookSlack();
                    tSlack.Send("Alert! more than one " + nameof(NWDProjectCredentials) + " (" +
                                tResultListInThisDao.Count + " results on " + tDao.GetInfos() + ") for " +
                                nameof(NWDProjectCredentials.ProjectId) + " : " + sProjectId + " and " +
                                nameof(NWDProjectCredentials.EnvironmentKind) + " : " + sEnvironment.ToString() + "(" +
                                ((int)sEnvironment).ToString() + ") ");
                }
            }

            return rReturn;
        }

        public static NWDProjectCredentials? GetOneByProjectIdAndEnvironment(ulong sProjectId,
            NWDEnvironmentKind sEnvironment)
        {
            NWDProjectCredentials? rReturn = null;
            if (DaoList.Count > 0)
            {
                INWDProjectCredentialsDao tDao = DaoList[0];
                List<NWDProjectCredentials> tResultListInThisDao = tDao.GetBy(sEnvironment, sProjectId,
                    new Dictionary<string, string>()
                    {
                        { nameof(NWDProjectCredentials.ProjectId), sProjectId.ToString() },
                        // { nameof(NWDProjectCredentials.Environment), ((int)sEnvironment).ToString() },
                    });
                if (tResultListInThisDao.Count == 0)
                {
                    NWDServerHookSlack tSlack = new NWDServerHookSlack();
                    tSlack.Send("Alert! no " + nameof(NWDProjectCredentials) + " (on " + tDao.GetInfos() + ") for " +
                                nameof(NWDProjectCredentials.ProjectId) + " : " + sProjectId + " and " +
                                nameof(NWDProjectCredentials.EnvironmentKind) + " : " + sEnvironment.ToString() + "(" +
                                ((int)sEnvironment).ToString() + ") ");
                }
                else if (tResultListInThisDao.Count == 1)
                {
                    rReturn = tResultListInThisDao[0];
                }
                else
                {
                    NWDServerHookSlack tSlack = new NWDServerHookSlack();
                    tSlack.Send("Alert! more than one " + nameof(NWDProjectCredentials) + " (" +
                                tResultListInThisDao.Count + " results on " + tDao.GetInfos() + ") for " +
                                nameof(NWDProjectCredentials.ProjectId) + " : " + sProjectId + " and " +
                                nameof(NWDProjectCredentials.EnvironmentKind) + " : " + sEnvironment.ToString() + "(" +
                                ((int)sEnvironment).ToString() + ") ");
                }
            }
            else
            {
                NWDServerHookSlack tSlack = new NWDServerHookSlack();
                tSlack.Send("Alert! no DOA for " + nameof(NWDProjectCredentials) + " ");
            }

            return rReturn;
        }

        #endregion
    }
}