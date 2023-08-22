using System;
using System.Collections.Generic;
using System.Linq;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDServerBack;

namespace NWDServerMiddle.Managers.ModelManagers;

public static class NWDStudioDataManager
{
    #region Static properties

    public static List<INWDStudioDataDao> DaoList = new List<INWDStudioDataDao>();
    private static readonly Dictionary<long, INWDStudioDataDao> DaoByRange = new Dictionary<long, INWDStudioDataDao>();

    #endregion

    #region Dao

    public static void Prepare()
    {
        DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDStudioDataDao>(NWDConfigurationDatabase.KConfig.DatabaseStudioArray);
        foreach (INWDStudioDataDao tDao in DaoList)
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
            foreach (INWDStudioDataDao tDao in DaoList)
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
            foreach (INWDStudioDataDao tDao in DaoList)
            {
                tDao.DeleteTable(tEnvironment);
                NWDCrucialInformationManager.ResetFingerPrintTable(tEnvironment, tDao);
            }
        }
    }

    public static INWDStudioDataDao? GetOneDao()
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

    public static INWDStudioDataDao? GetDaoByRange(ushort sRange)
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

    public static List<NWDStudioDataStorage> GetAllForProject(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId)
    {
        List<NWDStudioDataStorage> rList = new List<NWDStudioDataStorage>();
        if (DaoList.Count > 0)
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string> { { nameof(NWDStudioDataStorage.ProjectId), sProjectId.ToString() } };
            rList = DaoList.First().GetBy(sEnvironmentKind, sProjectId, tDictionary);
        }
        else
        {
        }

        return rList;
    }

    public static List<NWDStudioDataStorage> GetAllForProject(NWDEnvironmentKind sEnvironmentKind, int sRange, ulong sProjectId)
    {
        List<NWDStudioDataStorage> rList = new List<NWDStudioDataStorage>();
        if (DaoList.Count > 0)
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string> { { nameof(NWDStudioDataStorage.ProjectId), sProjectId.ToString() } };
            rList = DaoList.First().GetBy(sEnvironmentKind, sProjectId, tDictionary);
        }
        else
        {
        }

        return rList;
    }

    public static List<NWDStudioDataStorage> GetByReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId,
        ulong sReference)
    {
        Dictionary<string, string> tDictionary = new Dictionary<string, string> { { nameof(NWDStudioDataStorage.RowId), sReference.ToString() } };
        List<NWDStudioDataStorage> rList = DaoList.First().GetBy(sEnvironmentKind, sProjectId, tDictionary);
        return rList;
    }

    public static List<NWDStudioDataStorage> GetByReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, int sRange,
        ulong sReference)
    {
        Dictionary<string, string> tDictionary = new Dictionary<string, string> { { nameof(NWDStudioDataStorage.RowId), sReference.ToString() } };
        List<NWDStudioDataStorage> rList = DaoByRange[sRange].GetBy(sEnvironmentKind, sProjectId, tDictionary);
        return rList;
    }
    
    public static List<NWDStudioDataStorage> GetByReferences(NWDEnvironmentKind sEnvironmentKind,
        ulong sProjectId, ushort sRange, List<ulong> sReferences)
    {
        List<NWDStudioDataStorage> rDatPlayers = new List<NWDStudioDataStorage>();
        foreach (ulong tReference in sReferences)
        {
            List<NWDStudioDataStorage>? tPlayerData = GetByReference(sEnvironmentKind, sProjectId, sRange, tReference);
            if (tPlayerData != null)
            {
                rDatPlayers.AddRange(tPlayerData);
            }
        }

        return rDatPlayers;
    }

    public static NWDStudioDataStorage? GetSingleByReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId,
        ulong sReference)
    {
        Dictionary<string, string> tDictionary = new Dictionary<string, string> { { nameof(NWDStudioDataStorage.Reference), sReference.ToString() } };
        List<NWDStudioDataStorage> tStudioDatas = DaoList.First().GetBy(sEnvironmentKind, sProjectId, tDictionary);
        NWDStudioDataStorage? rResult = null;
        if (tStudioDatas.Count == 1)
        {
            rResult = tStudioDatas.First();
        }
        else
        {
        }

        return rResult;
    }
    
    public static List<NWDStudioDataStorage> GetBySync(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, NWDSyncInformation sSync)
    {
        List<NWDStudioDataStorage> rStudioDatas = new List<NWDStudioDataStorage>(); 
        if (DaoList.Count > 0 )
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string>();
            string tAndWhereSync = " AND `" + nameof(NWDStudioDataStorage.SyncDatetime) + "` >=" + NWDTimestamp.ToTimestamp(sSync.OldSyncDateTime)+" AND `" + nameof(NWDStudioDataStorage.Commit) + "` != " + sSync.OldSyncCommitId+" ";
            rStudioDatas= DaoList.First().GetBy(sEnvironmentKind, sProjectId, tDictionary, tAndWhereSync);
        }
        else
        {
        }

        return rStudioDatas; 
    }


    #endregion

    #region Create/Update

    public static void InsertOrUpdate(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId,
        List<NWDStudioDataStorage> sData)
    {
        foreach (INWDStudioDataDao tStudioDataDao in DaoList)
        {
            foreach (NWDStudioDataStorage tStudioData in sData)
            {
                //tStudioData.Reference = DaoList.First().NewValidReference(sEnvironmentKind, sProjectId);
                tStudioDataDao.InsertOrUpdate(sEnvironmentKind, sProjectId, tStudioData);
            }
        }
    }

    private static void InsertOrUpdate(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId,
        NWDStudioDataStorage sData)
    {
        foreach (INWDStudioDataDao tStudioDataDao in DaoList)
        {
            tStudioDataDao.InsertOrUpdate(sEnvironmentKind, sProjectId, sData);
        }
    }

    #endregion

    #region Trash/Delete

    public static void Trash(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, NWDStudioDataStorage sStudioData)
    {
        if (!sStudioData.Trashed)
        {
            sStudioData.Trashed = true;
            foreach (INWDStudioDataDao tStudioDataDao in DaoList)
            {
                tStudioDataDao.Update(sEnvironmentKind, sProjectId, sStudioData);
            }
        }
        else
        {
        }
    }

    public static void DeleteByReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, ulong sReference)
    {
        foreach (INWDStudioDataDao tStudioDataDao in DaoList)
        {
            tStudioDataDao.Delete(sEnvironmentKind, sProjectId, sReference);
        }
    }

    public static void DeleteRangeByReferences(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId,
        List<ulong> sReferences)
    {
        foreach (INWDStudioDataDao tStudioDataDao in DaoList)
        {
            foreach (ulong tReference in sReferences)
            {
                tStudioDataDao.Delete(sEnvironmentKind, sProjectId, tReference);
            }
        }
    }

    #endregion

    #region Publish
    public static void Publish(ulong sProjectId, List<NWDStudioDataStorage> sStudioDatas)
    {
        foreach (NWDStudioDataStorage tStudioData in sStudioDatas)
        {
            foreach (INWDStudioDataDao tStudioDataDao in DaoList)
            {
                tStudioDataDao.InsertOrUpdate(NWDEnvironmentKind.Production, sProjectId, tStudioData);
            }
        }
        
    }
    
    public static void Publish(NWDEnvironmentKind sKind, ulong sProjectId, List<NWDStudioDataStorage> sStudioDatas)
    {
        foreach (NWDStudioDataStorage tStudioData in sStudioDatas)
        {
            foreach (INWDStudioDataDao tStudioDataDao in DaoList)
            {
                tStudioDataDao.InsertOrUpdate(sKind, sProjectId, tStudioData);
            }
        }
        
    }
    #endregion
}