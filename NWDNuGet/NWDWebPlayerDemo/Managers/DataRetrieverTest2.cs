using NWDFoundation.Models;
using NWDFoundation.WebEdition.Facades;
using NWDWebEditor.Managers;
using NWDWebRuntime.Models;

namespace NWDWebPlayerDemo.Managers;

public class DataRetrieverTest2 : IDataRetriever
{
    public List<T> GetAll<T>() where T : NWDDatabaseWebBasicModel
    {
        return NWDWebDBDataManager.GetAllData<T>(); 
    }

    public List<T?> GetAllByReference<T>(NWDReferencesArray<T>? sReferencesArray) where T : NWDDatabaseWebBasicModel
    {
        List<T?> rReturn = new List<T?>();
        if (sReferencesArray != null)
        {
            rReturn.AddRange(sReferencesArray.ReferencesList.Select(sS => NWDWebDBDataManager.GetDataByReference<T>(ulong.Parse(sS))));

            return rReturn;
        }
        else
        {
            return new List<T?>();
        }
    }
}