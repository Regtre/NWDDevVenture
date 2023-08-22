using System.Reflection;
using Microsoft.AspNetCore.Http;
using NWDFoundation.Models;
using NWDFoundation.WebEdition.Enums;
using NWDFoundation.WebEdition.Models;

namespace NWDWebRuntime.Managers;

public class NWDWebEditionModelDataManagerStandard<T>  where T : NWDPlayerData

{
    public int PagesCount(HttpContext sHttpContext,int sItemPerPage, Func<T, bool> sFilter)
    {
        List<T> tItems = NWDWebDataManager.GetDataForPlayerByClass<T>(sHttpContext).Where(sFilter).ToList();
        return  (int)Math.Ceiling(tItems.Count / (double)sItemPerPage);
    }

    public void Add(HttpContext sHttpContext,T sModel)
    {
       NWDWebDataManager.SaveData(sHttpContext,sModel);
    }

    public TW? Update<TW>(HttpContext sHttpContext,TW sModel) where TW : NWDPlayerData
    {
        NWDWebDataManager.SaveData(sHttpContext,sModel);
        return NWDWebDataManager.GetDataByReference<TW>(sHttpContext,sModel.Reference);
    }

    public void Delete(HttpContext sHttpContext,ulong sReference)
    {
        T? tObject = NWDWebDataManager.GetDataByReference<T>(sHttpContext, sReference);
        if (tObject != null)
        {
            NWDWebDataManager.DeleteData<T>(sHttpContext,tObject);
        }
    }

    public W? GetReal<W>(HttpContext sHttpContext,ulong sReference) where W : NWDPlayerData
    {
        return NWDWebDataManager.GetDataByReference<W>(sHttpContext, sReference); 
    }

    public int GetPageOfItem<TW>(HttpContext sHttpContext,TW? sModel, NWDWebEditionPagination sPagination, Func<TW, bool> sFilter) where TW : NWDPlayerData
    {
        //TODO fix that
        int rReturn = 1;
        // NWDLogger.WriteLine(" sModel reference  = " + sModel.Reference);
        List<TW> tItems = NWDWebDataManager.GetDataForPlayerByClass<TW>(sHttpContext).Where(sFilter).ToList();
        if (sModel != null)
        { 
            TW? tItem = tItems.Find(sItem => sItem.Reference == sModel.Reference);
            if (tItem != null)
            {
                if (tItems.IndexOf(tItem) > 0)
                {
                    rReturn = (int)Math.Ceiling((double)tItems.IndexOf(tItem) / (double)sPagination.ItemPerPage);
                }
            }
        }
        return rReturn; 
    }
    public int GetPageOfNextNewItem<TW>(HttpContext sHttpContext,TW sModel, NWDWebEditionPagination sPagination, Func<TW, bool> sFilter) where TW : NWDPlayerData
    {
        int rReturn = 0;
        List<TW> tItems = NWDWebDataManager.GetDataForPlayerByClass<TW>(sHttpContext).Where(sFilter).ToList();
        rReturn = (int)Math.Ceiling(tItems.Count / (double)sPagination.ItemPerPage) +1 ;
        if (tItems.Count % sPagination.ItemPerPage == 0)
        {
            rReturn = rReturn + 1;
        }
        // if (sModel != null && tItems != null)
        // {
        //     W tItem = tItems.Find(item => item.Reference == sModel.Reference);
        //     NWDLogger.WriteLine(" tItem = " + tItem);
        //     if (tItem!=null)
        //     {
        //         rReturn = (int)Math.Ceiling((double)tItems.IndexOf(tItem) / (double)sPagination.ItemPerPage) + 1;
        //         NWDLogger.WriteLine(" 51 rReturn = " + rReturn);
        //     }
        // }
        return rReturn; 
    }

    public List<TW> GetItemAtActualPage<TW>(HttpContext sHttpContext,NWDWebEditionPagination sPagination, Func<TW, bool> sFilter) where TW : NWDPlayerData
    {
        List<TW> tReturn =  NWDWebDataManager.GetDataForPlayerByClass<TW>(sHttpContext).Where(sFilter).ToList();
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
                Comparison<TW> tCompare = delegate(TW sA, TW sB)
                {
                    bool tAsc = sPagination.SortDirection == NWDWebEditionSortDirection.Ascending;
                    object? tValueA = tAsc ? tProp.GetValue(sA, null) : tProp.GetValue(sB, null);
                    object? tValueB = tAsc ? tProp.GetValue(sB, null) : tProp.GetValue(sA, null);

                    return tValueA is IComparable ? ((IComparable)tValueA).CompareTo(tValueB) : 0;
                };
                tReturn.Sort(tCompare);
            }
        }

        return tReturn.GetRange(tMin, Math.Min(tMax - tMin, sPagination.ItemPerPage));
    }
}