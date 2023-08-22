#nullable enable
using System.Collections.Generic;
using NWDFoundation.Models;
using NWDFoundation.WebEdition.Models;

namespace NWDFoundation.WebEdition
{
    public interface INWDWebEditionModelDataManager<T> where T : NWDBasicModel
    {
        public int PagesCount(int sItemPerPage);
        public void Add(T sModel);
        public T? Update<TW>(TW sModel) where TW : T;
        public void Delete(ulong sReference);
        public T? GetReal<TW>(ulong sReference) where TW : T;
        public int GetPageOfItem<TW>(T? sModel, NWDWebEditionPagination sPagination) where TW : T;
        public List<T> GetItemAtActualPage<TW>(NWDWebEditionPagination sPagination) where TW : T;
    }
}
#nullable disable