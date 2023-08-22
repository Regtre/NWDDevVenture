using System.Reflection;
using NWDFoundation.Models;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models;

namespace NWDWebEditor.Managers;

public class NWDWebDBEditionDataManager<T>  where T : NWDDatabaseWebBasicModel
{
    private bool Create = false;
    public void CreateTable()
    {
        if (Create == false)
        {
            Create = true;
            NWDWebDBDataManager.CreateTable<T>();
        }
    }

    public List<T> GetBy(Dictionary<string, string>? sDictionary, string sAndWhere = "")
    {
        return NWDWebDBDataManager.GetBy<T>( sDictionary, sAndWhere);
    }
    
    public int PagesCount(int sItemPerPage, Func<T, bool> sFilter)
    {
        List<T> items = NWDWebDBDataManager.GetAllData<T>().Where(sFilter).ToList();
        return  (int)Math.Ceiling(items.Count / (double)sItemPerPage);
    }

    public void Add(T sModel)
    {
        NWDWebDBDataManager.SaveData(sModel);
    }

    public W? Update<W>(W sModel) where W : NWDDatabaseWebBasicModel
    {
        NWDWebDBDataManager.SaveData(sModel);
        return NWDWebDBDataManager.GetDataByReference<W>(sModel.Reference);
    }

    public void Delete(ulong sReference)
    {
        T? tItem = NWDWebDBDataManager.GetDataByReference<T>(sReference);
        if (tItem != null)
        {
            NWDWebDBDataManager.DeleteData<T>(tItem);
        }
    }

    public W? GetReal<W>(ulong sReference) where W : NWDDatabaseWebBasicModel
    {
        return NWDWebDBDataManager.GetDataByReference<W>(sReference);
    }

    public int GetPageOfItem<W>(W sModel, NWDWebSqlEditionPagination sPagination, Func<W, bool> sFilter) where W : NWDDatabaseWebBasicModel
    {
        //TODO fix that
        int rReturn = 1;
        List<W> items = NWDWebDBDataManager.GetAllData<W>().Where(sFilter).ToList();
        if (sModel != null && items != null)
        { 
            W? tItem = items.Find(item => item.Reference == sModel.Reference);
            if (tItem != null)
            {
                if (items.IndexOf(tItem) > 0)
                {
                    rReturn = (int)Math.Ceiling((double)items.IndexOf(tItem) / (double)sPagination.ItemPerPage);
                }
            }
        }
        return rReturn; 
    }
    public int GetPageOfNextNewItem<W>(W sModel, NWDWebSqlEditionPagination sPagination, Func<W, bool> sFilter) where W : NWDDatabaseWebBasicModel
    {
        int rReturn = 0;
        List<W> tItems = NWDWebDBDataManager.GetAllData<W>().Where(sFilter).ToList();
        rReturn = (int)Math.Ceiling(tItems.Count / (double)sPagination.ItemPerPage) +1 ;
        if (tItems.Count % sPagination.ItemPerPage == 0)
        {
            rReturn = rReturn + 1;
        }
        return rReturn; 
    }

    public List<W> GetItemAtActualPage<W>(NWDWebSqlEditionPagination sPagination, Func<W, bool> sFilter) where W : NWDDatabaseWebBasicModel
    {
        List<W> tReturn =  NWDWebDBDataManager.GetAllData<W>().Where(sFilter).ToList();
        int tPagesCount = (int)Math.Ceiling(tReturn.Count / (double)sPagination.ItemPerPage);
        int tPageMax = Math.Min(tPagesCount, sPagination.ActivePage);
        int tMin = Math.Max(Math.Max((tPageMax - 1) * sPagination.ItemPerPage, 0), 0);
        int tMax = Math.Max(Math.Min(tMin + sPagination.ItemPerPage, tReturn.Count), 0);
        string tSortBy = sPagination.SortBy;
        if (string.IsNullOrEmpty(tSortBy) == false)
        {
            Type tType = typeof(T);
            PropertyInfo? tProp = tType.GetProperty(tSortBy);
            if (tProp != null)
            {
                Comparison<W> tCompare = delegate(W a, W b)
                {
                    bool tAsc = sPagination.SortDirection == NWDWebEditionSortDirection.Ascending;
                    object? tValueA = tAsc ? tProp.GetValue(a, null) : tProp.GetValue(b, null);
                    object? tValueB = tAsc ? tProp.GetValue(b, null) : tProp.GetValue(a, null);

                    return tValueA is IComparable ? ((IComparable)tValueA).CompareTo(tValueB) : 0;
                };
                tReturn.Sort(tCompare);
            }
        }

        return tReturn.GetRange(tMin, Math.Min(tMax - tMin, sPagination.ItemPerPage));
    }
}