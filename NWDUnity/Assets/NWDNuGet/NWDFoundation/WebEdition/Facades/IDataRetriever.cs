using System.Collections.Generic;
using NWDFoundation.Models;

namespace NWDFoundation.WebEdition.Facades
{
    public interface IDataRetriever
    {
        public List<T> GetAll<T>() where T : NWDDatabaseWebBasicModel;
        public List<T?> GetAllByReference<T>(NWDReferencesArray<T>? sReferencesArray) where T : NWDDatabaseWebBasicModel;
    }
}

