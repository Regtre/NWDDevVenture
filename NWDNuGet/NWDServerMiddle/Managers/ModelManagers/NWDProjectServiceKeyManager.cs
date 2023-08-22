using System;
using System.Collections.Generic;
using NWDCrucial.Facades;
using NWDCrucial.Models;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDServerBack;

namespace NWDServerMiddle.Managers.ModelManagers
{

    public static class NWDProjectServiceKeyManager
    {
        #region Static properties

        public static List<INWDProjectServiceKeyDao> DaoList = new List<INWDProjectServiceKeyDao>();
        public static readonly Dictionary<ushort, INWDProjectServiceKeyDao> DaoByRange = new Dictionary<ushort, INWDProjectServiceKeyDao>();

        #endregion

        #region Dao

        public static void Prepare()
        {
            DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDProjectServiceKeyDao>(NWDConfigurationDatabase.KConfig.DatabaseStudioArray);
            foreach (INWDProjectServiceKeyDao tDao in DaoList)
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
                foreach (INWDProjectServiceKeyDao tDao in DaoList)
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
                foreach (INWDProjectServiceKeyDao tDao in DaoList)
                {
                    tDao.DeleteTable(tEnvironment);
                    NWDCrucialInformationManager.ResetFingerPrintTable(tEnvironment, tDao);
                }
            }
        }

        public static INWDProjectServiceKeyDao? GetOneDao()
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

        public static INWDProjectServiceKeyDao? GetDaoByRange(ushort sRange)
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


        //
        // public static NWDDownPayloadServiceCreate ProcessCreate(NWDUpPayloadServiceCreate? sService)
        // {
        //     NWDDownPayloadServiceCreate rResponse = new NWDDownPayloadServiceCreate();
        //     rResponse.ServiceList = new List<NWDProjectServiceKey>();
        //     if (sService != null)
        //     {
        //         foreach (NWDProjectServiceKey tService in sService.ServiceList)
        //         {
        //             if (UpdateOrRecord(tService) == false)
        //             {
        //                 rResponse.Success = false;
        //             }
        //             else
        //             {
        //                 rResponse.ServiceList.Add(tService);
        //             }
        //         }
        //     }
        //
        //     return rResponse;
        // }
        //
        // public static NWDDownPayloadServiceUpdate ProcessUpdate(NWDUpPayloadServiceUpdate? sService)
        // {
        //     NWDDownPayloadServiceUpdate rResponse = new NWDDownPayloadServiceUpdate();
        //     rResponse.ServiceList = new List<NWDProjectServiceKey>();
        //     if (sService is { ServiceList: { } })
        //         foreach (NWDProjectServiceKey tService in sService.ServiceList)
        //         {
        //             if (UpdateOrRecord(tService) == false)
        //             {
        //                 rResponse.Success = false;
        //             }
        //             else
        //             {
        //                 rResponse.ServiceList.Add(tService);
        //             }
        //         }
        //
        //     return rResponse;
        // }
        //
        // public static NWDDownPayloadServiceDelete ProcessDelete(NWDUpPayloadServiceDelete? sService)
        // {
        //     NWDDownPayloadServiceDelete rResponse = new NWDDownPayloadServiceDelete
        //     {
        //         ServiceList = new List<NWDProjectServiceKey>()
        //     };
        //     if (sService != null)
        //     {
        //         foreach (NWDProjectServiceKey tService in sService.ServiceList)
        //         {
        //             if (Delete(tService) == false)
        //             {
        //                 rResponse.Success = false;
        //             }
        //             else
        //             {
        //                 rResponse.ServiceList.Add(tService);
        //             }
        //         }
        //     }
        //
        //     return rResponse;
        // }

        // public static NWDDownPayloadGetServiceForProject GetServiceForProject(NWDUpPayloadGetServiceForProject? sService)
        // {
        //     if (sService != null)
        //     {
        //         NWDDownPayloadGetServiceForProject rResponse = new NWDDownPayloadGetServiceForProject
        //         {
        //             ServiceList = GetAllByProjectIdAndEnvironment(sService.GetProjectId, sService.Environment)
        //         };
        //         return rResponse;
        //     }
        //
        //     return new NWDDownPayloadGetServiceForProject(); 
        // }

        #endregion
        #region Get
        public static bool UpdateOrRecord(NWDProjectServiceKey sProjectServiceKey)
        {
            bool rReturn = true;
            if (DaoList.Count > 0)
            {
                //List<NWDProjectServiceKey> tResultList = new List<NWDProjectServiceKey>();
                foreach (INWDProjectServiceKeyDao tDao in DaoList)
                {
                    List<NWDProjectServiceKey> tResultListInThisDao = tDao.GetBy(sProjectServiceKey.EnvironmentKind, sProjectServiceKey.ProjectId,
                        new Dictionary<string, string>()
                        {
                            { nameof(NWDProjectServiceKey.ProjectId), sProjectServiceKey.ProjectId.ToString() },
                            { nameof(NWDProjectServiceKey.ServiceId), sProjectServiceKey.ServiceId.ToString() },
                        });
                    if (tResultListInThisDao.Count == 0)
                    {
                        tDao.Create(sProjectServiceKey.EnvironmentKind, sProjectServiceKey.ProjectId, sProjectServiceKey);
                    }
                    else if (tResultListInThisDao.Count == 1)
                    {
                        tDao.Update(sProjectServiceKey.EnvironmentKind, sProjectServiceKey.ProjectId, sProjectServiceKey);
                    }
                    else
                    {
                        
                        NWDServerHookSlack tSlack = new NWDServerHookSlack();
                        tSlack.Send("Alert! more than one " + nameof(NWDProjectCredentials) + " (" +
                                    tResultListInThisDao.Count + " results on " + tDao.GetInfos() + ") for " +
                                    nameof(NWDProjectCredentials.ProjectId) + " : " + sProjectServiceKey.ProjectId +
                                    " and " + nameof(NWDProjectCredentials.EnvironmentKind) + " : " +
                                    sProjectServiceKey.EnvironmentKind.ToString() + "(" +
                                    ((int)sProjectServiceKey.EnvironmentKind).ToString() + ") ");
                        rReturn = false;
                    }
                }
            }
            else
            {
                rReturn = false;
                /*NWDSlackWebhook tSlack = new NWDSlackWebhook();
                tSlack.Send("Alert! no DOA for " + nameof(NWDProjectServiceKey) + " ");*/
            }

            return rReturn;
        }

        public static bool Delete(NWDProjectServiceKey sService)
        {
            bool rReturn = true;
            if (DaoList.Count > 0)
            {
                //List<NWDProjectServiceKey> tResultList = new List<NWDProjectServiceKey>();
                foreach (INWDProjectServiceKeyDao tDao in DaoList)
                {
                    List<NWDProjectServiceKey> tResultListInThisDao = tDao.GetBy(sService.EnvironmentKind, sService.ProjectId,
                        new Dictionary<string, string>()
                        {
                            { nameof(NWDProjectServiceKey.ProjectId), sService.ProjectId.ToString() },
                            { nameof(NWDProjectServiceKey.ServiceId), sService.ServiceId.ToString() },
                        });
                    if (tResultListInThisDao.Count == 0)
                    {
                    }
                    else if (tResultListInThisDao.Count == 1)
                    {
                        NWDProjectServiceKey tResult = tResultListInThisDao[0];
                        // update this environment in this database
                        tDao.Delete(sService.EnvironmentKind, sService.ProjectId, tResult.Reference);
                        //tResultList.Add(tResult);
                    }
                    else
                    {
                        foreach (NWDProjectServiceKey tEnv in tResultListInThisDao)
                        {
                            tDao.Delete(sService.EnvironmentKind, sService.ProjectId, tEnv.Reference);
                        }

                        /*NWDSlackWebhook tSlack = new NWDSlackWebhook();
                        tSlack.Send("Alert! more than one " + nameof(NWDProjectServiceKey) + " (" +
                                    tResultListInThisDao.Count + " results on " + tDao.GetInfos() + ") for " +
                                    nameof(NWDProjectServiceKey.GetProjectId) + " : " + sService.GetProjectId +
                                    " and " + nameof(NWDProjectServiceKey.Environment) + " : " +
                                    sService.Environment.ToString() + "(" +
                                    ((int)sService.Environment).ToString() + ") ");*/
                        // Error : more than one GetProjectId reference for this environment!
                        rReturn = false;
                    }
                }
            }
            else
            {
                rReturn = false;
                /*NWDSlackWebhook tSlack = new NWDSlackWebhook();
                tSlack.Send("Alert! no DOA for " + nameof(NWDProjectServiceKey) + " ");*/
            }

            return rReturn;
        }

        public static List<NWDProjectServiceKey> GetAllByProjectIdAndEnvironment(ulong sProjectId,
            NWDEnvironmentKind sEnvironment)
        {
            List<NWDProjectServiceKey> rReturn = new List<NWDProjectServiceKey>();
            foreach (INWDProjectServiceKeyDao tDao in DaoList)
            {
                List<NWDProjectServiceKey> tResultListInThisDao = tDao.GetBy(sEnvironment, sProjectId,
                    new Dictionary<string, string>()
                    {
                        { nameof(NWDProjectServiceKey.ProjectId), sProjectId.ToString() },
                        // { nameof(NWDProjectServiceKey.Environment), ((int)sEnvironment).ToString() },
                    });
                if (tResultListInThisDao.Count == 0)
                {
                    /*NWDSlackWebhook tSlack = new NWDSlackWebhook();
                    tSlack.Send("Alert! no " + nameof(NWDProjectServiceKey) + " (on " + tDao.GetInfos() + ") for " +
                                nameof(NWDProjectServiceKey.GetProjectId) + " : " + sProjectId + " and " +
                                nameof(NWDProjectServiceKey.Environment) + " : " + sEnvironment.ToString() + "(" +
                                ((int)sEnvironment).ToString() + ") ");*/
                }
                else if (tResultListInThisDao.Count == 1)
                {
                    rReturn.Add(tResultListInThisDao[0]);
                }
                else
                {
                    rReturn.AddRange(tResultListInThisDao);
                    /*NWDSlackWebhook tSlack = new NWDSlackWebhook();
                    tSlack.Send("Alert! more than one " + nameof(NWDProjectServiceKey) + " (" +
                                tResultListInThisDao.Count + " results on " + tDao.GetInfos() + ") for " +
                                nameof(NWDProjectServiceKey.GetProjectId) + " : " + sProjectId + " and " +
                                nameof(NWDProjectServiceKey.Environment) + " : " + sEnvironment.ToString() + "(" +
                                ((int)sEnvironment).ToString() + ") ");*/
                }
            }

            return rReturn;
        }

        public static NWDProjectServiceKey? GetOneByProjectIdAndEnvironment(ulong sProjectId, ulong sServiceId,
            NWDEnvironmentKind sEnvironment)
        {
            NWDProjectServiceKey? rReturn = null;
            if (DaoList.Count > 0)
            {
                INWDProjectServiceKeyDao tDao = DaoList[0];
                List<NWDProjectServiceKey> tResultListInThisDao = tDao.GetBy(sEnvironment, sProjectId,
                    new Dictionary<string, string>()
                    {
                        { nameof(NWDProjectServiceKey.ProjectId), sProjectId.ToString() },
                        { nameof(NWDProjectServiceKey.ServiceId), sServiceId.ToString() },
                        // { nameof(NWDProjectServiceKey.Environment), ((int)sEnvironment).ToString() },
                    });
                if (tResultListInThisDao.Count == 0)
                {
                    /*NWDSlackWebhook tSlack = new NWDSlackWebhook();
                    tSlack.Send("Alert! no " + nameof(NWDProjectServiceKey) + " (on " + tDao.GetInfos() + ") for " +
                                nameof(NWDProjectServiceKey.GetProjectId) + " : " + sProjectId + " and " +
                                nameof(NWDProjectServiceKey.Environment) + " : " + sEnvironment.ToString() + "(" +
                                ((int)sEnvironment).ToString() + ") ");*/
                }
                else if (tResultListInThisDao.Count == 1)
                {
                    rReturn = tResultListInThisDao[0];
                }
                else
                {
                    /*NWDSlackWebhook tSlack = new NWDSlackWebhook();
                    tSlack.Send("Alert! more than one " + nameof(NWDProjectServiceKey) + " (" +
                                tResultListInThisDao.Count + " results on " + tDao.GetInfos() + ") for " +
                                nameof(NWDProjectServiceKey.GetProjectId) + " : " + sProjectId + " and " +
                                nameof(NWDProjectServiceKey.Environment) + " : " + sEnvironment.ToString() + "(" +
                                ((int)sEnvironment).ToString() + ") ");*/
                }
            }
            else
            {
                /*NWDSlackWebhook tSlack = new NWDSlackWebhook();
                tSlack.Send("Alert! no DOA for " + nameof(NWDProjectServiceKey) + " ");*/
            }

            return rReturn;
        }
        #endregion
    }
}