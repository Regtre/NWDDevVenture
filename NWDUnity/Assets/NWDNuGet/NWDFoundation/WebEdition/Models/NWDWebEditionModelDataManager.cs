using System;
using System.Collections.Generic;
using System.Reflection;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;

namespace NWDFoundation.WebEdition.Models
{
    [Serializable]
    public class NWDWebEditionModelDataManager<T> : INWDWebEditionModelDataManager<T> where T : NWDBasicModel
    {
        private Dictionary<ulong, T>? DictionaryOfItems { set; get; } = new Dictionary<ulong, T>();
        private List<T>? ListOfItems { set; get; } = new List<T>();

        public int PagesCount(int sItemPerPage)
        {
            int rReturn = 0;
            if (DictionaryOfItems == null)
            {
                rReturn = 0;
            }
            else
            {
                rReturn = (int)Math.Ceiling((double)DictionaryOfItems.Count / (double)sItemPerPage);
            }

            return rReturn;
        }

        public void Add(T sModel)
        {
            if (DictionaryOfItems != null)
            {
                if (DictionaryOfItems.ContainsKey(sModel.Reference) == true)
                {
                    sModel.Reference = NWDRandom.UnsignedLongNumeric(16);
                    while (DictionaryOfItems.ContainsKey(sModel.Reference))
                    {
                        sModel.Reference = NWDRandom.UnsignedLongNumeric(16);
                    }

                    DictionaryOfItems[sModel.Reference] = sModel;
                }

                DictionaryOfItems.Add(sModel.Reference, sModel);
                if (ListOfItems != null)
                {
                    ListOfItems.Add(sModel);
                }
            }
        }

        public T? Update<TW>(TW sModel) where TW : T
        {
            T? tModel = sModel as T;
            if (DictionaryOfItems != null)
            {
                if (DictionaryOfItems.ContainsKey(sModel.Reference) == true)
                {
                    Delete(sModel.Reference);
                    if (tModel != null)
                    {
                        Add(tModel);
                    }
                }
                else
                {
                    sModel.Reference = NWDRandom.UnsignedLongNumeric(16);
                    while (DictionaryOfItems.ContainsKey(sModel.Reference))
                    {
                        sModel.Reference = NWDRandom.UnsignedLongNumeric(16);
                    }

                    Add(sModel as T);
                }
            }

            return tModel;
        }

        public void Delete(ulong sReference)
        {
            if (DictionaryOfItems != null)
            {
                if (DictionaryOfItems.ContainsKey(sReference) == true)
                {
                    T tItem = DictionaryOfItems[sReference];
                    DictionaryOfItems.Remove(tItem.Reference);
                    if (ListOfItems != null)
                    {
                        ListOfItems.Remove(tItem);
                    }
                }
            }
        }

        public T? GetReal<TW>(ulong sReference) where TW : T
        {
            T? rReturn = null;
            if (DictionaryOfItems != null)
            {
                if (DictionaryOfItems.ContainsKey(sReference) == true)
                {
                    rReturn = DictionaryOfItems[sReference] as T;
                }
            }

            return rReturn;
        }

        public int GetPageOfItem<TW>(T? sModel, NWDWebEditionPagination sPagination) where TW : T
        {
            int rReturn = 0;
            if (sModel != null && DictionaryOfItems != null)
            {
                if (DictionaryOfItems.ContainsKey(sModel.Reference) == true)
                {
                    T tItem = DictionaryOfItems[sModel.Reference];
                    if (ListOfItems != null)
                    {
                        rReturn = (int)Math.Ceiling((double)ListOfItems.IndexOf(tItem) / (double)sPagination.ItemPerPage);
                    }
                }
            }

            return rReturn;
        }

        public List<T> GetItemAtActualPage<TW>(NWDWebEditionPagination sPagination) where TW : T
        {
            List<T> tReturn = new List<T>();
            if (ListOfItems != null && DictionaryOfItems != null)
            {
                List<T>? tListOfItems = ListOfItems as List<T>;
                if (tListOfItems != null)
                {
                    tReturn.AddRange(tListOfItems);
                }

                int tPageMax = Math.Min(PagesCount(sPagination.ItemPerPage), sPagination.ActivePage);
                int tMin = Math.Max(Math.Max((tPageMax - 1) * sPagination.ItemPerPage, 0), 0);
                int tMax = Math.Max(Math.Min(tMin + sPagination.ItemPerPage, DictionaryOfItems.Count), 0);
                string tSortBy = sPagination.SortBy;
                if (string.IsNullOrEmpty(tSortBy) == false)
                {
                    Type tType = typeof(T);
                    PropertyInfo? tProp = tType.GetProperty(tSortBy);
                    if (tProp != null)
                    {
                        Comparison<T> tCompare = delegate(T a, T b)
                        {
                            bool tAsc = sPagination.SortDirection == NWDWebEditionSortDirection.Ascending;
                            object tValueA = tAsc ? tProp.GetValue(a, null) : tProp.GetValue(b, null);
                            object tValueB = tAsc ? tProp.GetValue(b, null) : tProp.GetValue(a, null);

                            return tValueA is IComparable ? ((IComparable)tValueA).CompareTo(tValueB) : 0;
                        };
                        tReturn.Sort(tCompare);
                    }
                }

                return tReturn.GetRange(tMin, Math.Min(tMax - tMin, sPagination.ItemPerPage));
            }
            else
            {
                return tReturn;
            }
        }
    }
}