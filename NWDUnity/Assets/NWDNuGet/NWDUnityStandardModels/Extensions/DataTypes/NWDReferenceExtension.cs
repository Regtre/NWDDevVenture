using NWDFoundation.Models;
using NWDUnityShared.BasisHelper;

namespace NWDUnityStandardModels.Extensions
{
    static public class NWDReferenceExtension
    {
        static public T GetReachableData<T> (this NWDReference<T> sReference) where T : NWDDatabaseBasicModel
        {
            T rReturn = null;

            if (sReference != null)
            {
                rReturn = NWDBasisHelper.GetReachableData<T>(sReference.Reference);
            }

            return rReturn;
        }

        static public T Get<T> (this NWDReference<T> sReference) where T : NWDDatabaseBasicModel
        {
            return GetReachableData(sReference);
        }
    }
}
