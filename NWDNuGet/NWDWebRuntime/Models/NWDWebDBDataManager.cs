using System.Reflection;
using MySqlX.XDevAPI;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Factories;

namespace NWDWebRuntime.Models;

/// <summary>
/// Manager for NWD Data
/// </summary>
// TODO :  rename this
public static class NWDWebDBDataManager
{
    //NWDDatabaseConnectorConfiguration.KConfig.Credentials
    public static void CreateAllTables()
    {
        INWDWebDBByReflexion? tManager = GetManager();
        if (tManager != null)
        {
            tManager.CreateTable<NWDWebCrucialInformation>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment());
        }

        foreach (Type tType in NWDReflexion.GetAllTypesInAllAssembliesSubclassOf(typeof(NWDDatabaseWebBasicModel)))
        {
            NWDLogger.Information("Need to create table for "+tType.Name);
            MethodInfo? tMethod = typeof(NWDWebDBDataManager).GetMethod(nameof(CreateTable), BindingFlags.Public | BindingFlags.Static);
            if (tMethod != null)
            {
                tMethod = tMethod.MakeGenericMethod(tType);
                tMethod.Invoke(null, null);
            }
        }
    }

    private static INWDWebDBByReflexion? GetManager()
    {
        return NWDWebDbByReflexionFactory.GetNWDWebDBByReflexion(NWDDatabaseConnectorConfiguration.KConfig.Credentials.Kind);
    }

    private static bool TableExists<T>() where T : NWDDatabaseWebBasicModel
    {
        bool rReturn = false;
        INWDWebDBByReflexion? tManager = GetManager();
        if (tManager != null)
        {
            tManager.TableExists<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment());
        }

        return rReturn;
    }

    public static void CreateTable<T>() where T : NWDDatabaseWebBasicModel
    {
        INWDWebDBByReflexion? tManager = GetManager();
        if (tManager != null)
        {
            bool tNeedUpdate = true;
            List<NWDWebCrucialInformation> tListOfInfo = GetBy<NWDWebCrucialInformation>(new Dictionary<string, string>() { { nameof(NWDWebCrucialInformation.Key), tManager.FingerPrintTableName<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment()) } });
            if (tListOfInfo.Count > 0)
            {
                if (tListOfInfo[0].Value == tManager.FingerPrintTable<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment()))
                {
                    tNeedUpdate = false;
                }
            }

            if (tNeedUpdate)
            {
                NWDLogger.TraceAttention("Must create table for  " + tManager.FingerPrintTableName<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment()));
                tManager.CreateTable<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment());
                foreach (NWDWebCrucialInformation tInformation in tListOfInfo)
                {
                    DeleteData<NWDWebCrucialInformation>(tInformation);
                }

                NWDWebCrucialInformation tInfo = new NWDWebCrucialInformation();
                tInfo.Key = tManager.FingerPrintTableName<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment());
                tInfo.Value = tManager.FingerPrintTable<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment());
                SaveData(tInfo);
            }
            else
            {
                //NWDLogger.TraceSuccess("Table is identical to finger print "+tManager.FingerPrintTableName<T>(NWDWebRuntimeConfiguration.KConfig.Environment)); 
            }
        }
        else
        {
            throw new Exception(nameof(INWDWebDBByReflexion) + " manager not found to " + nameof(CreateTable) + "<" + typeof(T).Name + ">()!");
        }
    }

    public static void DeleteTable<T>() where T : NWDDatabaseWebBasicModel
    {
        INWDWebDBByReflexion? tManager = GetManager();
        if (tManager != null)
        {
            tManager.DeleteTable<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment());
            List<NWDWebCrucialInformation> tListOfInfo = GetBy<NWDWebCrucialInformation>(new Dictionary<string, string>() { { nameof(NWDWebCrucialInformation.Key), tManager.FingerPrintTableName<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment()) } });
            foreach (NWDWebCrucialInformation tInformation in tListOfInfo)
            {
                DeleteData<NWDWebCrucialInformation>(tInformation);
            }
        }
    }

    public static List<T> GetAllData<T>() where T : NWDDatabaseWebBasicModel
    {
        List<T> tAllData = new List<T>();
        INWDWebDBByReflexion? tManager = GetManager();
        if (tManager != null)
        {
            tAllData = tManager.FindAll<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDWebRuntimeConfiguration.KConfig.GetProjectId());
        }

        return tAllData;
    }

    public static List<T> GetBy<T>(Dictionary<string, string>? sDictionary, string sAndWhere = "") where T : NWDDatabaseWebBasicModel
    {
        List<T> tDataList = new List<T>();
        INWDWebDBByReflexion? tManager = GetManager();
        if (tManager != null)
        {
            tDataList = tManager.GetBy<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDWebRuntimeConfiguration.KConfig.GetProjectId(), sDictionary, sAndWhere);
        }

        return tDataList;
    }

    /// <summary>
    /// Get NWD Data By Reference
    /// </summary>
    /// <param name="sHttpContext"></param>
    /// <param name="sReference"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetDataByReference<T>(ulong sReference) where T : NWDDatabaseWebBasicModel
    {
        T? tData = null;
        INWDWebDBByReflexion? tManager = GetManager();
        if (tManager != null)
        {
            tData = tManager.GetByReference<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDWebRuntimeConfiguration.KConfig.GetProjectId(), sReference.ToString());
        }

        return tData;
    }

    public static T? SaveData<T>(T sObject, bool sWithNewReference = false) where T : NWDDatabaseWebBasicModel
    {
        INWDWebDBByReflexion? tManager = GetManager();
        T? rReturn = null;
        if (tManager != null)
        {
            rReturn = tManager.InsertOrUpdate(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDWebRuntimeConfiguration.KConfig.GetProjectId(), sObject, sWithNewReference);
        }
        else
        {
            throw new Exception(nameof(NWDWebDBDataManager) + " " + nameof(SaveData) + " () no manager found!");
        }

        return rReturn;
    }

    #region Reference

    private static ulong NewValidReference<T>() where T : NWDDatabaseWebBasicModel
    {
        ulong tReference = 0;
        INWDWebDBByReflexion? tManager = GetManager();
        if (tManager != null)
        {
            tReference = tManager.NewValidReference<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDWebRuntimeConfiguration.KConfig.GetProjectId());
        }

        return tReference;
    }

    private static bool TestIfReferenceExists<T>(ulong sReference) where T : NWDDatabaseWebBasicModel
    {
        bool tReferenceExists = false;
        INWDWebDBByReflexion? tManager = GetManager();
        if (tManager != null)
        {
            tReferenceExists = tManager.TestIfReferenceExists<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDWebRuntimeConfiguration.KConfig.GetProjectId(), sReference);
        }

        return tReferenceExists;
    }

    #endregion

    public static void DeleteData<T>(T sModel) where T : NWDDatabaseWebBasicModel
    {
        INWDWebDBByReflexion? tManager = GetManager();
        if (tManager != null)
        {
            tManager.Delete<T>(NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), NWDWebRuntimeConfiguration.KConfig.GetProjectId(), sModel.Reference);
        }
    }
}