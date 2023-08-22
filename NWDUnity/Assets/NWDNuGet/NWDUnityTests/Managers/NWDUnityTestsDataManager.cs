using NWDFoundation.Models;
using NWDUnityShared.Enumerations;
using NWDUnityShared.Facades;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;

namespace NWDUnityTests.Manager
{
    public class NWDUnityTestsDataManager : INWDUnityDataManager
    {
        public NWDDataManagerState State => throw new NotImplementedException();

        public object GetDataByReference(Type sDataType, ulong sReference)
        {
            return null;
        }

        public T GetDataByReference<T>(ulong sReference) where T : class
        {
            return null;
        }

        public object GetPlayerDataByReference(Type sDataType, ulong sReference)
        {
            return null;
        }

        public T GetPlayerDataByReference<T>(ulong sReference) where T : class
        {
            return null;
        }

        public object GetReachableDataByReference(Type sDataType, ulong sReference)
        {
            return null;
        }

        public T GetReachableDataByReference<T>(ulong sReference) where T : class
        {
            return null;
        }

        public object GetStudioDataByReference(Type sDataType, ulong sReference)
        {
            return null;
        }

        public T GetStudioDataByReference<T>(ulong sReference) where T : class
        {
            return null;
        }

        public List<object> GetAllData(Type sDataType)
        {
            return null;
        }

        public List<T> GetAllData<T>() where T : class
        {
            return null;
        }

        public List<object> GetAllPlayerData(Type sDataType)
        {
            return null;
        }

        public List<T> GetAllPlayerData<T>() where T : class
        {
            return null;
        }

        public List<object> GetAllStudioData(Type sDataType)
        {
            return null;
        }

        public List<T> GetAllStudioData<T>() where T : class
        {
            return null;
        }

        public void SavePlayerData<T>(T sPlayerData) where T : NWDPlayerData
        {

        }

        public NWDAsyncOperation Initialize()
        {
            return null;
        }

        public void Start() { }

        public void Stop() { }

        public NWDAsyncOperation Synchronize()
        {
            return null;
        }
    }
}
