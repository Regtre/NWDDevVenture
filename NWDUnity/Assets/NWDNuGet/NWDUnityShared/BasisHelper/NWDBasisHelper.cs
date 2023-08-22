using NWDUnityShared.Engine;

namespace NWDUnityShared.BasisHelper
{
    public static partial class NWDBasisHelper
    {
        #region DataAccess
        public static T GetReachableData<T> (ulong sReference) where T : class
        {
            return NWDUnityEngine.Instance.DataManager.GetReachableDataByReference<T>(sReference);
        }
        #endregion
    }
}
