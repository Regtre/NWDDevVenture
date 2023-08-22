using NWDAppRuntime.Middle;
using NWDFoundation.Models;
using NWDMauiRuntime.Back;
using NWDRuntime.Models;
using NWDWebRuntime.Models;

namespace NWDMauiRuntime.Middle.Managers;

public class NWDDataManager
{
    private static readonly Dictionary<ulong, NWDDataInMemory> _PlayerDatas = new Dictionary<ulong, NWDDataInMemory>();
    private static readonly NWDDataInMemory _StudioData = new NWDDataInMemory();

    private static readonly Dictionary<ulong, List<NWDPlayerData>> _PlayerDatasToSave = new Dictionary<ulong, List<NWDPlayerData>>();

    private static readonly List<NWDStudioData> _StudioDataToSave = new List<NWDStudioData>();
    
    
    public static void FastSync()
    {
        NWDDataInMemory tDataInMemory = GetDataInMemoryForAccount();
            NWDDataInMemoryPlayerAndStudio tMemoryPlayerAndStudio = NWDSync.SyncByIncrement(
                tDataInMemory.GetAllData<NWDPlayerData>(),
                _StudioData.GetAllData<NWDStudioData>(), NWDAccountManager.GetAccount(), NWDAccountManager.GetDataTrack());
            tDataInMemory.AddDataInMemory(tMemoryPlayerAndStudio.PlayerData);
            _StudioData.AddDataInMemory(tMemoryPlayerAndStudio.StudioData);
        
    }
    
    private static NWDDataInMemory GetDataInMemoryForAccount()
    {
        NWDAccount tAccount = NWDAccountManager.GetAccount();
        if (_PlayerDatas.ContainsKey(tAccount.Reference) == false)
        {
            _PlayerDatas.TryAdd(tAccount.Reference, new NWDDataInMemory());
        }

        NWDDataInMemory tData = _PlayerDatas[tAccount.Reference];
        return tData;
    }
    
    public static List<T> GetAllData<T>() where T : NWDPlayerData
    {
        List<T> rReturn = new List<T>();
        if (NWDAccountManager.IsConnected())
        {
            NWDDataInMemory tData = GetDataInMemoryForAccount();
            rReturn = tData.GetDataByClass<T>();
        }

        return rReturn;
    }
    
    
}