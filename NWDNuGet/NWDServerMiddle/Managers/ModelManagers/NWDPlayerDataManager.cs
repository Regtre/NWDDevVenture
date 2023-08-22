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

public static class NWDPlayerDataManager
{
    #region Static properties

    public static List<INWDPlayerDataDao> DaoList = new List<INWDPlayerDataDao>();

    private static readonly Dictionary<ushort, INWDPlayerDataDao> DaoByRange =
        new Dictionary<ushort, INWDPlayerDataDao>();

    #endregion

    #region Dao

    public static void Prepare()
    {
        DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDPlayerDataDao>(NWDConfigurationDatabase.KConfig
            .DatabasePlayerArray);
        foreach (INWDPlayerDataDao tDao in DaoList)
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
            foreach (INWDPlayerDataDao tDao in DaoList)
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
            foreach (INWDPlayerDataDao tDao in DaoList)
            {
                tDao.DeleteTable(tEnvironment);
                NWDCrucialInformationManager.ResetFingerPrintTable(tEnvironment, tDao);
            }
        }
    }

    public static INWDPlayerDataDao? GetOneDao()
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

    public static INWDPlayerDataDao? GetDaoByRange(ushort sRange)
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

    public static List<NWDPlayerDataStorage> GetAllForProject(NWDEnvironmentKind sEnvironmentKind, ushort sRange,
        ulong sProjectId)
    {
        List<NWDPlayerDataStorage> rList = new List<NWDPlayerDataStorage>();
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string>
            {
                { nameof(NWDPlayerDataStorage.ProjectId), sProjectId.ToString() },
            };
            rList = DaoByRange[sRange].GetBy(sEnvironmentKind, sProjectId, tDictionary);
            foreach (NWDPlayerDataStorage tPlayerData in rList)
            {
                DaoList.First().Update(sEnvironmentKind, sProjectId, tPlayerData);
            }
        }
        else
        {
        }

        return rList;
    }

    public static List<NWDPlayerDataStorage> GetAllForProject(NWDEnvironmentKind sEnvironmentKind, ushort sRange,
        ulong sProjectId, ulong sAccountReference)
    {
        List<NWDPlayerDataStorage> rList = new List<NWDPlayerDataStorage>();
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string>
            {
                { nameof(NWDPlayerDataStorage.ProjectId), sProjectId.ToString() },
                { nameof(NWDPlayerDataStorage.Account), sAccountReference.ToString() }
            };
            rList = DaoByRange[sRange].GetBy(sEnvironmentKind, sProjectId, tDictionary);
            foreach (NWDPlayerDataStorage tPlayerData in rList)
            {
                DaoList.First().Update(sEnvironmentKind, sProjectId, tPlayerData);
            }
        }
        else
        {
        }

        return rList;
    }

    public static NWDPlayerDataStorage? GetSingleByReference(NWDEnvironmentKind sEnvironmentKind,
        ulong sProjectId, ulong sReference, ushort sRange)
    {
        NWDPlayerDataStorage? rPlayerData = null;
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string>
            {
                { nameof(NWDPlayerDataStorage.Reference), sReference.ToString() },
            };

            List<NWDPlayerDataStorage> tPlayerDatas =
                DaoByRange[sRange].GetBy(sEnvironmentKind, sProjectId, tDictionary);
            if (tPlayerDatas.Count == 1)
            {
                rPlayerData = tPlayerDatas.First();
            }
        }
        else
        {
        }

        return rPlayerData;
    }

    public static List<NWDPlayerDataStorage> GetByReferences(NWDEnvironmentKind sEnvironmentKind,
        ulong sProjectId, List<ulong> sReferences, ushort sRange)
    {
        List<NWDPlayerDataStorage> rDatPlayers = new List<NWDPlayerDataStorage>();
        foreach (ulong tReference in sReferences)
        {
            NWDPlayerDataStorage? tPlayerData = GetSingleByReference(sEnvironmentKind, sProjectId, tReference, sRange);
            if (tPlayerData != null)
            {
                rDatPlayers.Add(tPlayerData);
            }
        }

        return rDatPlayers;
    }

    public static List<NWDPlayerDataStorage> GetAllByAccountReferenceAndProjectIdAndDataTrack(
        NWDEnvironmentKind sEnvironmentKind,
        ulong sProjectId, ulong sAccountReference, ushort sDataTrack, ushort sRange)
    {
        List<NWDPlayerDataStorage> rPlayerDatas = new List<NWDPlayerDataStorage>();
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string>
            {
                { nameof(NWDPlayerDataStorage.Account), sAccountReference.ToString() },
                { nameof(NWDPlayerDataStorage.DataTrack), sDataTrack.ToString() }
            };
            rPlayerDatas = DaoByRange[sRange].GetBy(sEnvironmentKind, sProjectId, tDictionary);
        }
        else
        {
        }

        return rPlayerDatas;
    }

    public static List<NWDPlayerDataStorage> GetDataByAccountReferenceAndProjectIdAndType(
        NWDEnvironmentKind sEnvironmentKind,
        ulong sProjectId, ulong sAccountReference, ushort sRange, string sAssemblyQualifiedName)
    {
        List<NWDPlayerDataStorage> rPlayerDatas = new List<NWDPlayerDataStorage>();
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string>
            {
                { nameof(NWDPlayerDataStorage.Account), sAccountReference.ToString() },
                { nameof(NWDPlayerDataStorage.ClassName), sAssemblyQualifiedName }
            };
            rPlayerDatas = DaoByRange[sRange].GetBy(sEnvironmentKind, sProjectId, tDictionary);
        }
        else
        {
        }

        return rPlayerDatas;
    }

    private static void InsertOrUpdate(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, ushort sRange,
        NWDPlayerDataStorage sData)
    {
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            DaoByRange[sRange].InsertOrUpdate(sEnvironmentKind, sProjectId, sData);
        }
        else
        {
        }
    }

    public static NWDNewSyncInformation GenerateNewSyncInformation(ushort sRange)
    {
        NWDNewSyncInformation rReturn = new NWDNewSyncInformation();
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            rReturn.SyncDateTime = DaoByRange[sRange].GetCurrentDatetime();
            rReturn.SyncCommitId = DaoByRange[sRange].GenerateNewCommitId();
        }

        return rReturn;
    }

    public static List<NWDPlayerDataStorage> InsertOrUpdate(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId,
        ushort sRange, List<NWDPlayerDataStorage> sData, NWDNewSyncInformation sNewSyncInformation)
    {
        foreach (NWDPlayerDataStorage tPlayerData in sData)
        {
            tPlayerData.Range = sRange;
            tPlayerData.Commit = sNewSyncInformation.SyncCommitId;
            tPlayerData.SyncDatetime = NWDTimestamp.ToTimestamp(sNewSyncInformation.SyncDateTime);
            if (tPlayerData.Modification > tPlayerData.SyncDatetime)
            {
                tPlayerData.Modification = tPlayerData.SyncDatetime;
            }

            if (tPlayerData.Creation > tPlayerData.SyncDatetime)
            {
                tPlayerData.Creation = tPlayerData.SyncDatetime;
            }

            if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
            {
                DaoByRange[sRange].InsertOrUpdate(sEnvironmentKind, sProjectId, tPlayerData);
            }
            else
            {
            }
        }

        return sData;
    }

    public static void TrashByReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, ushort sRange,
        NWDPlayerDataStorage sData)
    {
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            if (!sData.Trashed)
            {
                sData.Trashed = true;

                DaoByRange[sRange].Update(sEnvironmentKind, sProjectId, sData);
            }
            else
            {
            }
        }
        else
        {
        }
    }

    public static void DeleteByReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, ushort sRange,
        ulong sReference)
    {
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            DaoByRange[sRange].Delete(sEnvironmentKind, sProjectId, sReference);
        }
        else
        {
        }
    }

    public static void DeleteRangeByReferences(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, ushort sRange,
        List<ulong> sReferences)
    {
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            foreach (ulong tReference in sReferences)
            {
                DeleteByReference(sEnvironmentKind, sProjectId, sRange, tReference);
            }
        }
        else
        {
        }
    }

    public static List<NWDPlayerDataStorage> GetBySync(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId,
        ushort sRange, NWDSyncInformation sSyncIndex, ulong sAccountReference)
    {
        List<NWDPlayerDataStorage> rPlayerDatas = new List<NWDPlayerDataStorage>();
        if (DaoByRange.Count > 0 && DaoByRange.ContainsKey(sRange))
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string>
            {
                { nameof(NWDPlayerDataStorage.Account), sAccountReference.ToString() }
            };
            string tAndWhereSync = " AND `" + nameof(NWDStudioDataStorage.SyncDatetime) + "` >=" +
                                   NWDTimestamp.ToTimestamp(sSyncIndex.OldSyncDateTime) + " AND `" +
                                   nameof(NWDStudioDataStorage.Commit) + "` != " + sSyncIndex.OldSyncCommitId + " ";
            rPlayerDatas = DaoByRange[sRange].GetBy(sEnvironmentKind, sProjectId, tDictionary, tAndWhereSync);
        }
        else
        {
        }

        return rPlayerDatas;
    }

    #endregion
}