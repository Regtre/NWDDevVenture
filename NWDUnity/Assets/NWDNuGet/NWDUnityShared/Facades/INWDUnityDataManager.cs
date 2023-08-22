using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Models;
using NWDUnityShared.Enumerations;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;

namespace NWDUnityShared.Facades
{
    public interface INWDUnityDataManager
    {
        #region States
        public NWDDataManagerState State { get; }
        #endregion

        #region Life cycle methods
        public void Start();
        public void Stop();
        public NWDAsyncOperation Initialize();
        #endregion

        #region Data access methods
        public object GetDataByReference(Type sDataType, ulong sReference);
        public T GetDataByReference<T>(ulong sReference) where T : class;
        public object GetReachableDataByReference(Type sDataType, ulong sReference);
        public T GetReachableDataByReference<T>(ulong sReference) where T : class;
        public object GetPlayerDataByReference(Type sDataType, ulong sReference);
        public T GetPlayerDataByReference<T>(ulong sReference) where T : class;
        public object GetStudioDataByReference(Type sDataType, ulong sReference);
        public T GetStudioDataByReference<T>(ulong sReference) where T : class;
        public List<object> GetAllData(Type sDataType);
        public List<T> GetAllData<T>() where T : class;
        public List<object> GetAllPlayerData(Type sDataType);
        public List<T> GetAllPlayerData<T>() where T : class;
        public List<object> GetAllStudioData(Type sDataType);
        public List<T> GetAllStudioData<T>() where T : class;
        public void SavePlayerData<T>(T sPlayerData) where T : NWDPlayerData;
        #endregion

        #region Management methods
        public NWDAsyncOperation Synchronize();
        #endregion
    }
}
