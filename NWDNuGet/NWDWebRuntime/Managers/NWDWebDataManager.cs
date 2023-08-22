using Microsoft.AspNetCore.Http;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDRuntime.Models;
using NWDWebRuntime.Back;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebRuntime.Tools.Sessions;

namespace NWDWebRuntime.Managers;

/// <summary>
/// Manager for NWD Data
/// </summary>
public static class NWDWebDataManager
{
    private static readonly Dictionary<ulong, NWDDataInMemory> _PlayerDatas = new Dictionary<ulong, NWDDataInMemory>();
    private static readonly NWDDataInMemory _StudioData = new NWDDataInMemory();

    private static readonly Dictionary<ulong, List<NWDPlayerData>> _PlayerDatasToSave = new Dictionary<ulong, List<NWDPlayerData>>();

    private static readonly List<NWDStudioData> _StudioDataToSave = new List<NWDStudioData>();

    public static List<T> GetAllData<T>(HttpContext sHttpContext) where T : NWDPlayerData
    {
        List<T> rReturn = new List<T>();
        if (NWDAccountWebManager.AccountIsConnected(sHttpContext))
        {
            NWDDataInMemory tData = GetDataInMemoryForAccount(sHttpContext);
            rReturn = tData.GetDataByClass<T>();
        }

        return rReturn;
    }

    /// <summary>
    /// Get NWD Data By Reference
    /// </summary>
    /// <param name="sHttpContext"></param>
    /// <param name="sReference"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetDataByReference<T>(HttpContext sHttpContext, ulong sReference) where T : NWDPlayerData
    {
        T? rReturn = null;
        if (NWDAccountWebManager.AccountIsConnected(sHttpContext))
        {
            NWDDataInMemory tData = GetDataInMemoryForAccount(sHttpContext);
            rReturn = tData.GetDataByReference<T>(sReference);
        }

        return rReturn;
    }


    /// <summary>
    /// Save NWD Data and Sync DB 
    /// </summary>
    /// <param name="sHttpContext"/>
    /// <param name="sObject"></param>
    public static void SaveData(HttpContext sHttpContext, NWDPlayerData sObject)
    {
        sObject.Account = NWDAccountWebManager.GetAccountInContext(sHttpContext).Reference;
        if (sObject.Account != 0)
        {
            NWDDataInMemory tData = GetDataInMemoryForAccount(sHttpContext);
            sObject.AvailableForWeb = true;
            sObject.Modification = NWDTimestamp.ToTimestamp(DateTime.UtcNow);
            if (sObject.Reference == 0)
            {
                sObject.DataTrack = NWDWebRuntimeConfiguration.KConfig.DataTrackActive; // Why are types not matchnig ? Is this Ok ?
                sObject.Creation = NWDTimestamp.ToTimestamp(DateTime.UtcNow);
                sObject.Reference = NewValidReference(sObject.GetType(), false, sHttpContext);
            }
            tData.AddDataInMemory(sObject);
            ulong tAccountReference = NWDAccountWebManager.GetAccountInContext(sHttpContext).Reference;
            if (_PlayerDatasToSave.ContainsKey(tAccountReference) == true)
            {
                _PlayerDatasToSave[tAccountReference].Add(sObject);
            }
        }
    }

    /// <summary>
    /// Save NWD Data and Sync DB 
    /// </summary>
    /// <param name="sHttpContext"/>
    /// <param name="sObject"></param>
    public static T SaveDataModel<T>(HttpContext sHttpContext, T sObject) where T : NWDPlayerData
    {
        SaveData(sHttpContext, sObject);
        return sObject;
    }

    /// <summary>
    /// Sync DB
    /// </summary>
    /// <param name="sHttpContext"></param>
    public static void SyncPlayerData(HttpContext sHttpContext)
    {
        if (NWDAccountWebManager.AccountIsConnected(sHttpContext))
        {
            SyncData(sHttpContext);
        }
    }

    /// <summary>
    /// Fast Sync of DB. Retrieve Data from NWD Server but doesn't send data from Web Database
    /// </summary>
    /// <param name="sHttpContext"></param>
    public static void FastSync(HttpContext? sHttpContext)
    {
        if (sHttpContext != null)
        {
            NWDDataInMemory tDataInMemory = GetDataInMemoryForAccount(sHttpContext);
            NWDDataInMemoryPlayerAndStudio tMemoryPlayerAndStudio = NWDSync.SyncByIncrement(
                tDataInMemory.GetAllData<NWDPlayerData>(),
                _StudioData.GetAllData<NWDStudioData>(), NWDAccountWebManager.GetAccountInContext(sHttpContext), NWDAccountWebManager.GetDataTrack(sHttpContext),
                sHttpContext);
            tDataInMemory.AddDataInMemory(tMemoryPlayerAndStudio.PlayerData);
            _StudioData.AddDataInMemory(tMemoryPlayerAndStudio.StudioData);
        }
    }

    public static bool IsDataToSave(HttpContext? sHttpContext)
    {
        if (sHttpContext != null)
        {
            NWDAccount tAccount = NWDAccountWebManager.GetAccountInContext(sHttpContext);
            if (!_PlayerDatasToSave.ContainsKey(tAccount.Reference))
            {
                
                _PlayerDatasToSave.Add(tAccount.Reference, new List<NWDPlayerData>());
            }

            return _PlayerDatasToSave[tAccount.Reference] is { Count: > 0 };
        }
        else
        {
            return false;
        }
    }

    #region private

    private static List<NWDPlayerData> GetAllPlayerDatasFromMemory(HttpContext sHttpContext)
    {
        return GetDataInMemoryForAccount(sHttpContext).DataMemory.OfType<NWDPlayerData>().ToList();
    }
    
    private static NWDDataInMemory GetDataInMemoryForAccount(HttpContext sHttpContext)
    {
        NWDAccount tAccount = NWDAccountWebManager.GetAccountInContext(sHttpContext);
        if (_PlayerDatas.ContainsKey(tAccount.Reference) == false)
        {
            _PlayerDatas.TryAdd(tAccount.Reference, new NWDDataInMemory());
        }

        NWDDataInMemory tData = _PlayerDatas[tAccount.Reference];
        return tData;
    }

    public static List<T> GetDataForPlayerByClass<T>(HttpContext sHttpContext) where T : NWDPlayerData
    {
        NWDDataInMemory tData = GetDataInMemoryForAccount(sHttpContext);
        if (tData != null)
        {
            return tData.GetDataByClass<T>().Where(sData => sData.Trashed == false).ToList();
        }
        else
        {
            return new List<T>();
        }
    }

    private static void SyncData(HttpContext sHttpContext)
    {
        NWDDataInMemory tDataInMemory = GetDataInMemoryForAccount(sHttpContext);
        NWDDataInMemoryPlayerAndStudio tMemoryPlayerAndStudio = NWDSync.SyncByIncrement(
            _PlayerDatasToSave[NWDAccountWebManager.GetAccountInContext(sHttpContext).Reference],
            _StudioDataToSave, NWDAccountWebManager.GetAccountInContext(sHttpContext),NWDAccountWebManager.GetDataTrack(sHttpContext),sHttpContext);
        tDataInMemory.AddDataInMemory(tMemoryPlayerAndStudio.PlayerData);
        _StudioData.AddDataInMemory(tMemoryPlayerAndStudio.StudioData);
        _PlayerDatasToSave[NWDAccountWebManager.GetAccountInContext(sHttpContext).Reference].Clear();
        _StudioDataToSave.Clear();
    }

    #region Reference

    private static ulong NewValidReference(Type sType, bool sRangeDependent, HttpContext sHttpContext)
    {
        ulong rReturn = NewReference(sRangeDependent, sHttpContext);
        while (TestIfReferenceExists(sType, rReturn, sHttpContext))
        {
            rReturn = NewReference(sRangeDependent, sHttpContext);
        }

        return rReturn;
    }

    private static ulong NewReference(bool sRangeDependent, HttpContext sHttpContext)
    {
        ulong rReturn = 0;
        if (sRangeDependent)
        {
            ulong tRef = NWDRandom.UnsignedLongNumeric(12);
            rReturn = tRef + NWDAccountWebManager.GetAccountInContext(sHttpContext).Range * (ulong)(100000000000000) +
                      (ulong)(10000000000000000000);
        }
        else
        {
            rReturn = NWDRandom.UnsignedLongNumeric(12);
        }

        return rReturn;
    }

    private static bool TestIfReferenceExists(Type sType, ulong sReference, HttpContext sHttpContext)
    {
        //SyncData(sHttpContext);
        if (sType.IsAssignableFrom(typeof(NWDPlayerData)))
        {
            List<NWDPlayerData> tPlayerDatas = GetAllPlayerDatasFromMemory(sHttpContext);
            return tPlayerDatas.Any(sPlayerData => sPlayerData.Reference == sReference);
        }

        if (sType.IsAssignableFrom(typeof(NWDPlayerData)))
        {
            return _StudioData.GetAllData<NWDPlayerData>().Any(sStudioData => sStudioData.Reference == sReference);
        }

        return false;
    }

    #endregion

    #endregion

    public static void DeleteData<T>(HttpContext sHttpContext, T sModel) where T : NWDPlayerData
    {
        sModel.Trashed = true;
        SaveData(sHttpContext, sModel);
    }

    public static void SaveDataIfNeeded(HttpContext sHttpContext)
    {
        if (IsDataToSave(sHttpContext))
        {
            SyncData(sHttpContext);
        }
    }


    public static void FlushAllForAccount(HttpContext? sHttpContext)
    {
        if (sHttpContext != null)
        {
            NWDAccount tAccount = NWDAccountWebManager.GetAccountInContext(sHttpContext);
            if (_PlayerDatas.ContainsKey(tAccount.Reference))
            {
                _PlayerDatas.Remove(tAccount.Reference);
            }
        }
    }
}