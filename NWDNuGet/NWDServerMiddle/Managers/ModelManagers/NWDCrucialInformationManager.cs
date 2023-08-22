using System;
using System.Collections.Generic;
using System.Linq;
using NWDCrucial.Configuration;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDServerBack;
using NWDServerMiddle.Configuration;
using NWDServerShared.Configuration;

namespace NWDServerMiddle.Managers.ModelManagers;

public static class NWDCrucialInformationManager
{
    #region Static properties

    public static List<INWDCrucialInformationDao> DaoList = new List<INWDCrucialInformationDao>();
    private static readonly Dictionary<long, INWDCrucialInformationDao> DaoByRange = new Dictionary<long, INWDCrucialInformationDao>();

    #endregion

    #region Dao

    public static void Prepare()
    {
        DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDCrucialInformationDao>(NWDConfigurationDatabase.KConfig.DatabaseStudioArray);
        foreach (INWDCrucialInformationDao tDao in DaoList)
        {
            if (DaoByRange.ContainsKey(tDao.GetRange()) == false)
            {
                DaoByRange.Add(tDao.GetRange(), tDao);
            }
        }

        // only for this table
        foreach (NWDEnvironmentKind tEnvironment in (NWDEnvironmentKind[])Enum.GetValues(typeof(NWDEnvironmentKind)))
        {
            foreach (INWDCrucialInformationDao tDao in DaoList)
            {
                tDao.CreateTable(tEnvironment);
            }
        }

        // check any way
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
                    tDao.CreateTable(tEnvironment);
                    NWDCrucialInformationManager.SetFingerPrintTable(tEnvironment, tDao);
                }
            }
        }
    }

    public static void ForceCreateAllTables()
    {
        foreach (NWDEnvironmentKind tEnvironment in (NWDEnvironmentKind[])Enum.GetValues(typeof(NWDEnvironmentKind)))
        {
            foreach (INWDCrucialInformationDao tDao in DaoList)
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
            foreach (INWDCrucialInformationDao tDao in DaoList)
            {
                tDao.DeleteTable(tEnvironment);
                NWDCrucialInformationManager.ResetFingerPrintTable(tEnvironment, tDao);
            }
        }
    }

    public static INWDCrucialInformationDao? GetOneDao()
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

    public static INWDCrucialInformationDao? GetDaoByRange(ushort sRange)
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

    public static List<NWDCrucialInformation> GetAllForProject(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId)
    {
        List<NWDCrucialInformation> rList = new List<NWDCrucialInformation>();
        if (DaoList.Count > 0)
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string> { { nameof(NWDCrucialInformation.ProjectId), sProjectId.ToString() } };
            rList = DaoList.First().GetBy(sEnvironmentKind, sProjectId, tDictionary);

            foreach (NWDCrucialInformation tCrucialInformation in rList)
            {
                DaoList.First().Update(sEnvironmentKind, sProjectId, tCrucialInformation);
            }
        }
        else
        {
        }

        return rList;
    }

    public static List<NWDCrucialInformation> GetAllForProject(NWDEnvironmentKind sEnvironmentKind, int sRange, ulong sProjectId)
    {
        List<NWDCrucialInformation> rList = new List<NWDCrucialInformation>();
        if (DaoList.Count > 0)
        {
            Dictionary<string, string> tDictionary = new Dictionary<string, string> { { nameof(NWDCrucialInformation.ProjectId), sProjectId.ToString() } };
            rList = DaoList.First().GetBy(sEnvironmentKind, sProjectId, tDictionary);

            foreach (NWDCrucialInformation tCrucialInformation in rList)
            {
                DaoByRange[sRange].Update(sEnvironmentKind, sProjectId, tCrucialInformation);
            }
        }
        else
        {
        }

        return rList;
    }

    public static List<NWDCrucialInformation> GetByReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, ulong sReference)
    {
        Dictionary<string, string> tDictionary = new Dictionary<string, string> { { nameof(NWDCrucialInformation.RowId), sReference.ToString() } };
        List<NWDCrucialInformation> rList = DaoList.First().GetBy(sEnvironmentKind, sProjectId, tDictionary);
        return rList;
    }

    public static List<NWDCrucialInformation> GetByReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, int sRange, ulong sReference)
    {
        Dictionary<string, string> tDictionary = new Dictionary<string, string> { { nameof(NWDCrucialInformation.RowId), sReference.ToString() } };
        List<NWDCrucialInformation> rList = DaoByRange[sRange].GetBy(sEnvironmentKind, sProjectId, tDictionary);
        return rList;
    }

    public static NWDCrucialInformation? GetSingleByReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, ulong sReference)
    {
        Dictionary<string, string> tDictionary = new Dictionary<string, string> { { nameof(NWDCrucialInformation.Reference), sReference.ToString() } };
        List<NWDCrucialInformation> tCrucialInformation = DaoList.First().GetBy(sEnvironmentKind, sProjectId, tDictionary);
        NWDCrucialInformation? rResult = null;
        if (tCrucialInformation.Count == 1)
        {
            rResult = tCrucialInformation.First();
        }
        else
        {
        }

        return rResult;
    }

    #endregion

    #region Create/Update

    public static void InsertOrUpdate(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, List<NWDCrucialInformation> sData)
    {
        foreach (INWDCrucialInformationDao tCrucialInformationDao in DaoList)
        {
            foreach (NWDCrucialInformation tCrucialInformation in sData)
            {
                tCrucialInformationDao.InsertOrUpdate(sEnvironmentKind, sProjectId, tCrucialInformation);
            }
        }
    }

    private static void InsertOrUpdate(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, NWDCrucialInformation sData)
    {
        foreach (INWDCrucialInformationDao tCrucialInformationDao in DaoList)
        {
            tCrucialInformationDao.InsertOrUpdate(sEnvironmentKind, sProjectId, sData);
        }
    }

    #endregion

    #region Trash/Delete

    public static void Trash(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, NWDCrucialInformation sCrucialInformation)
    {
        if (!sCrucialInformation.Trashed)
        {
            sCrucialInformation.Trashed = true;
            foreach (INWDCrucialInformationDao tCrucialInformationDao in DaoList)
            {
                tCrucialInformationDao.Update(sEnvironmentKind, sProjectId, sCrucialInformation);
            }
        }
        else
        {
        }
    }

    public static void DeleteByReference(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId, ulong sReference)
    {
        foreach (INWDCrucialInformationDao tCrucialInformationDao in DaoList)
        {
            tCrucialInformationDao.Delete(sEnvironmentKind, sProjectId, sReference);
        }
    }

    public static void DeleteRangeByReferences(NWDEnvironmentKind sEnvironmentKind, ulong sProjectId,
        List<ulong> sReferences)
    {
        foreach (INWDCrucialInformationDao tCrucialInformationDao in DaoList)
        {
            foreach (ulong tReference in sReferences)
            {
                tCrucialInformationDao.Delete(sEnvironmentKind, sProjectId, tReference);
            }
        }
    }

    #endregion

    #region methodfs

    public static string GetFingerPrintTable(NWDEnvironmentKind sEnvironmentKind, INWDDao sDao)
    {
        string rReturn = string.Empty;
        string tTableName = sDao.FingerPrintTableName(sEnvironmentKind);
        INWDCrucialInformationDao? tDaoCrucial = GetOneDao();
        if (tDaoCrucial != null)
        {
            NWDCrucialInformation? tItem = null;
            List<NWDCrucialInformation> tReturnList = tDaoCrucial.GetBy(sEnvironmentKind, NWDServerMiddleConfiguration.KConfig.GetCrucialProjectId(), new Dictionary<string, string>() { { nameof(NWDCrucialInformation.Key), tTableName } });
            if (tReturnList.Count > 0)
            {
                tItem = tReturnList[0];
            }

            if (tItem != null)
            {
                rReturn = tItem.Value;
            }
        }
        else
        {
        }

        return rReturn;
    }

    public static void SetFingerPrintTable(NWDEnvironmentKind sEnvironmentKind, INWDDao sDao)
    {
        string tTableName = sDao.FingerPrintTableName(sEnvironmentKind);
        string tTable = sDao.FingerPrintTable(sEnvironmentKind);
        foreach (INWDCrucialInformationDao? tDaoCrucial in DaoList)
        {
            List<NWDCrucialInformation> tReturnList = tDaoCrucial.GetBy(sEnvironmentKind, NWDServerMiddleConfiguration.KConfig.GetCrucialProjectId(), new Dictionary<string, string>() { { nameof(NWDCrucialInformation.Key), tTableName } });
            if (tReturnList.Count > 0)
            {
                foreach (NWDCrucialInformation tItem in tReturnList)
                {
                    tItem.Value = tTable;
                    tDaoCrucial.InsertOrUpdate(sEnvironmentKind, NWDServerMiddleConfiguration.KConfig.GetCrucialProjectId(), tItem);
                }
            }
            else
            {
                NWDCrucialInformation? tItem = new NWDCrucialInformation() { ProjectId = NWDServerMiddleConfiguration.KConfig.GetCrucialProjectId() };
                tItem.Value = tTable;
                tItem.Key = tTableName;
                tItem.Reference = tDaoCrucial.NewValidReference(sEnvironmentKind, NWDServerMiddleConfiguration.KConfig.GetCrucialProjectId());
                tDaoCrucial.InsertOrUpdate(sEnvironmentKind, NWDServerMiddleConfiguration.KConfig.GetCrucialProjectId(), tItem);
            }
        }
    }

    public static void ResetFingerPrintTable(NWDEnvironmentKind sEnvironmentKind, INWDDao sDao)
    {
        string tTableName = sDao.FingerPrintTableName(sEnvironmentKind);
        foreach (INWDCrucialInformationDao? tDaoCrucial in DaoList)
        {
            List<NWDCrucialInformation> tReturnList = tDaoCrucial.GetBy(sEnvironmentKind, NWDServerMiddleConfiguration.KConfig.GetCrucialProjectId(), new Dictionary<string, string>() { { nameof(NWDCrucialInformation.Key), tTableName } });
            foreach (NWDCrucialInformation tItem in tReturnList)
            {
                tItem.Value = string.Empty;
                tDaoCrucial.InsertOrUpdate(sEnvironmentKind, NWDServerMiddleConfiguration.KConfig.GetCrucialProjectId(), tItem);
            }
        }
    }

    #endregion
}