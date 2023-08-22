using NWDFoundation.Models;
using NWDUnityShared.Tools;
using System.Collections.Generic;

namespace NWDUnityShared.Facades
{
    public interface INWDUnityLocalDBManager
    {
        public void Start();
        public void Stop();
        public NWDAsyncOperation Initialiaze();
        public NWDAsyncOperation CreateDatabase<T>() where T : NWDDatabaseBasicModel;
        public NWDAsyncOperation RecordData<T>(List<T> sDataList) where T : NWDDatabaseBasicModel;
        public NWDAsyncOperation<List<T>> GetAll<T>() where T : NWDDatabaseBasicModel;
        public NWDAsyncOperation<List<NWDPlayerDataStorage>> GetAll(ulong sAccountReference);
    }
}
