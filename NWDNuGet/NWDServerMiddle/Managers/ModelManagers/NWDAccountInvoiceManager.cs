using System;
using System.Collections.Generic;
using System.Globalization;
using NWDEditor.Exchanges.Payloads;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDServerBack;

namespace NWDServerMiddle.Managers.ModelManagers;

public static class NWDAccountInvoiceManager
{
    #region Static properties

    public static List<INWDAccountInvoiceDao> DaoList = new List<INWDAccountInvoiceDao>();
    public static readonly Dictionary<ushort, INWDAccountInvoiceDao> DaoByRange = new Dictionary<ushort, INWDAccountInvoiceDao>();

    #endregion

    #region Dao

    public static void Prepare()
    {
        DaoList = NWDDevServerBackStaticFactory.GetDaoList<INWDAccountInvoiceDao>(NWDConfigurationDatabase.KConfig.DatabaseAccountArray);
        foreach (INWDAccountInvoiceDao tDao in DaoList)
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
                    //NWDLogger.TraceSuccess("Table is identical to finger print " + tDao.FingerPrintTableName(tEnvironment));
                }
            }
        }
    }

    public static void ForceCreateAllTables()
    {
        foreach (NWDEnvironmentKind tEnvironment in (NWDEnvironmentKind[])Enum.GetValues(typeof(NWDEnvironmentKind)))
        {
            foreach (INWDAccountInvoiceDao tDao in DaoList)
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
            foreach (INWDAccountInvoiceDao tDao in DaoList)
            {
                tDao.DeleteTable(tEnvironment);
                NWDCrucialInformationManager.ResetFingerPrintTable(tEnvironment, tDao);
            }
        }
    }

    public static INWDAccountInvoiceDao? GetOneDao()
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

    public static INWDAccountInvoiceDao? GetDaoByRange(ushort sRange)
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
}