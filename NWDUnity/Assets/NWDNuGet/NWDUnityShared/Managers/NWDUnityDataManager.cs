using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDRuntime.Factories;
using NWDUnityShared.Engine;
using NWDUnityShared.Enumerations;
using NWDUnityShared.Facades;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace NWDUnityShared.Managers
{
    public abstract class NWDUnityDataManager<SD, TS> : INWDUnityDataManager where SD : NWDDatabaseBasicModel where TS : NWDTaskScheduler<TS>, new()
    {
        #region Properties
        protected object _lock = new object();

        protected TS Scheduler = new TS();
        private NWDAsyncOperation autoSynOperation = null;

        protected List<NWDPlayerDataStorage> PlayerDataToSave = new List<NWDPlayerDataStorage>();
        protected List<NWDPlayerDataStorage> PlayerData = new List<NWDPlayerDataStorage>();
        protected List<SD> StudioData = new List<SD>();

        private NWDDataManagerState state = NWDDataManagerState.Stopped;
        private ulong updateIndex = 0;
        public ulong UpdateIndex
        {
            get
            {
                lock (_lock)
                {
                    return updateIndex;
                }
            }
            set
            {
                lock (_lock)
                {
                    updateIndex = value;
                }
            }
        }

        public NWDAsyncOperation AutoSynOperation
        {
            get
            {
                lock (_lock)
                {
                    return autoSynOperation;
                }
            }
            protected set
            {
                lock (_lock)
                {
                    autoSynOperation = value;
                }
            }
        }
        #endregion

        #region States
        public NWDDataManagerState State
        {
            get
            {
                lock (_lock)
                {
                    return state;
                }
            }
            private set
            {
                lock (_lock)
                {
                    state = value;
                }
            }
        }
        #endregion

        #region Data access methods
        public object GetDataByReference(Type sDataType, ulong sReference)
        {
            object rResult;
            if (IsPlayerDataType(sDataType))
            {
                rResult = GetPlayerDataByReference(sDataType, sReference);
            }
            else
            {
                rResult = GetStudioDataByReference(sDataType, sReference);
            }
            return rResult;
        }
        public T GetDataByReference<T>(ulong sReference) where T : class
        {
            return GetDataByReference(typeof(T), sReference) as T;
        }
        public object GetReachableDataByReference(Type sDataType, ulong sReference)
        {
            return GetDataByReference(sDataType, sReference);
        }
        public T GetReachableDataByReference<T>(ulong sReference) where T : class
        {
            return GetReachableDataByReference(typeof(T), sReference) as T;
        }
        public object GetPlayerDataByReference(Type sDataType, ulong sReference)
        {
            NWDPlayerDataStorage tPlayerDataStorage = GetPlayerDataStorageByReference(sReference);
            object rResult = GetPlayerDataFromStorage(tPlayerDataStorage);
            //if (rResult == null)
            //{
            //    rResult = sDataType.GetConstructor(new Type[0]).Invoke(new object[0]);
            //}
            return rResult;
        }
        public T GetPlayerDataByReference<T>(ulong sReference) where T : class
        {
            return GetPlayerDataByReference(typeof(T), sReference) as T;
        }
        public object GetStudioDataByReference(Type sDataType, ulong sReference)
        {
            SD tStudioDataStorage = GetStudioDataStorageByReference(sReference);
            object rResult = GetStudioDataFromStorage(tStudioDataStorage, sDataType);
            //if (rResult == null)
            //{
            //    rResult = sDataType.GetConstructor(new Type[0]).Invoke(new object[0]);
            //}
            return rResult;
        }
        public T GetStudioDataByReference<T>(ulong sReference) where T : class
        {
            return GetStudioDataByReference(typeof(T), sReference) as T;
        }

        public List<object> GetAllData(Type sDataType)
        {
            lock (_lock)
            {
                return Internal_GetAllData(sDataType).ToList();
            }
        }
        public List<T> GetAllData<T>() where T : class
        {
            lock (_lock)
            {
                return Internal_GetAllData<T>().ToList();
            }
        }
        public List<object> GetAllPlayerData(Type sDataType)
        {
            lock (_lock)
            {
                return Internal_GetAllPlayerData(sDataType).ToList();
            }
        }
        public List<T> GetAllPlayerData<T>() where T : class
        {
            lock (_lock)
            {
                return Internal_GetAllPlayerData<T>().ToList();
            }
        }
        public List<object> GetAllStudioData(Type sDataType)
        {
            lock (_lock)
            {
                return Internal_GetAllStudioData(sDataType).ToList();
            }
        }
        public List<T> GetAllStudioData<T>() where T : class
        {
            lock (_lock)
            {
                return Internal_GetAllStudioData<T>().ToList();
            }
        }

        public void SavePlayerData<T>(T sPlayerData) where T : NWDPlayerData
        {
            ulong sAccountReference = NWDUnityEngine.Instance.AccountManager.GetPlayerToken().PlayerReference;
            int tDataTrack = NWDUnityEngine.Instance.EnvironmentManager.GetCurrentDataTrack();
            sPlayerData.Account = sAccountReference;
            if (sPlayerData.Account != 0)
            {
                sPlayerData.AvailableForWeb = true;
                sPlayerData.Modification = NWDTimestamp.ToTimestamp(DateTime.UtcNow);
                if (sPlayerData.Reference == 0)
                {
                    sPlayerData.DataTrack = tDataTrack; // Why are types not matchnig ? Is it Ok ?
                    sPlayerData.Creation = NWDTimestamp.ToTimestamp(DateTime.UtcNow);
                    sPlayerData.Reference = NewPlayerReference();
                }
                ulong tAccountReference = sAccountReference;
                lock (_lock)
                {
                    NWDPlayerDataStorage rResult = PlayerData.FirstOrDefault(x => x.Reference == sPlayerData.Reference && x.DataTrack == tDataTrack);
                    if (rResult == null)
                    {
                        rResult = NWDPlayerDataFactory.ToDataPlayerStorage(sPlayerData, sAccountReference);
                    }
                    else
                    {
                        rResult.Json = JsonConvert.SerializeObject(sPlayerData);
                    }
                    if (!PlayerDataToSave.Any(x => x.Reference == sAccountReference))
                    {
                        PlayerDataToSave.Add(rResult);
                    }
                }
            }
        }

        protected ulong NewPlayerReference()
        {
            ulong rResult;
            do
            {
                rResult = NWDRandom.UnsignedLongNumeric(12);
            }
            while (PlayerReferenceExist(rResult));
            return rResult;
        }
        protected bool PlayerReferenceExist(ulong sReference)
        {
            lock (_lock)
            {
                return PlayerData.Any(x => x.Reference == sReference);
            }
        }

        protected bool IsPlayerDataType(Type sDataType)
        {
            return sDataType.IsSubclassOf(typeof(NWDPlayerData));
        }

        protected IEnumerable<NWDPlayerDataStorage> GetAllPlayerDataStorage(Type sDataType)
        {
            int tDataTrack = NWDUnityEngine.Instance.EnvironmentManager.GetCurrentDataTrack();
            lock (_lock)
            {
                return PlayerData.Where(x => sDataType.AssemblyQualifiedName == x.ClassName && tDataTrack == x.DataTrack);
            }
        }
        protected abstract IEnumerable<SD> GetAllStudioDataStorage(Type sDataType);

        public abstract SD GetStudioDataStorageByReference(ulong sReference);

        public NWDPlayerDataStorage GetPlayerDataStorageByReference(ulong sReference)
        {
            int tDataTrack = NWDUnityEngine.Instance.EnvironmentManager.GetCurrentDataTrack();
            lock (_lock)
            {
                return PlayerData.FirstOrDefault(x => x.Reference == sReference && x.DataTrack == tDataTrack);
            }
        }

        protected abstract object GetStudioDataFromStorage(SD sStudioDataStorage, Type sDataType);

        protected object GetPlayerDataFromStorage(NWDPlayerDataStorage sPlayerDataStorage)
        {
            object rResult = null;
            if (sPlayerDataStorage != null)
            {
                rResult = NWDPlayerDataFactory.FromPlayerDataStorage(sPlayerDataStorage);
            }
            return rResult;
        }
        #endregion


        #region Life cycle methods
        public void Start()
        {
            State = NWDDataManagerState.Stopped;
        }

        public virtual void Stop()
        {
            State = NWDDataManagerState.Stopped;
        }

        public NWDAsyncOperation Initialize()
        {
            State = NWDDataManagerState.Initializing;
            return NWDAsyncOperationFactory.NewTask(Scheduler, async (_, _) =>
            {
                await Internal_Initialize();
            });
        }
        #endregion

        #region Management methods
        public abstract NWDAsyncOperation Synchronize();

        #endregion

        #region Internal
        protected void Internal_Synchronize(ref NWDAsyncHandler sHandler, List<SD> tStudioData, List<NWDPlayerDataStorage> tPlayerData)
        {
            lock (_lock)
            {
                PlayerDataToSave.Clear();
            }

            State = NWDDataManagerState.Updating;

            if (tPlayerData != null && tPlayerData.Count > 0)
            {
                foreach (NWDPlayerDataStorage tData in tPlayerData)
                {
                    Internal_InsertOrReplacePlayerData(tData);
                }

                NWDUnityEngine.Instance.LocalDBManager.RecordData(tPlayerData);
            }

            if (tStudioData != null && tStudioData.Count > 0)
            {
                UpdateIndex++;
                foreach (SD tData in tStudioData)
                {
                    Internal_InsertOrReplaceStudioData(tData);
                }
                NWDUnityEngine.Instance.LocalDBManager.RecordData(tStudioData);
            }

            State = NWDDataManagerState.Ready;
        }

        protected void Internal_Synchronize(ref NWDAsyncHandler sHandler, List<SD> tStudioData)
        {
            State = NWDDataManagerState.Updating;

            if (tStudioData != null && tStudioData.Count > 0)
            {
                UpdateIndex++;
                foreach (SD tData in tStudioData)
                {
                    Internal_InsertOrReplaceStudioData(tData);
                }
                NWDUnityEngine.Instance.LocalDBManager.RecordData(tStudioData);
            }

            State = NWDDataManagerState.Ready;
        }

        protected virtual async Task Internal_Initialize ()
        {
            // Start loading player data from local DB
            NWDRequestPlayerToken tPlayerToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            NWDAsyncOperation<List<NWDPlayerDataStorage>> tPlayerOperation = NWDUnityEngine.Instance.LocalDBManager.GetAll(tPlayerToken.PlayerReference);

            // Start loading studio data from local DB
            NWDAsyncOperation<List<SD>> tStudioOperation = NWDUnityEngine.Instance.LocalDBManager.GetAll<SD>();

            // Wait for all data to be loaded
            await Task.WhenAll(tPlayerOperation.Wait(), tStudioOperation.Wait());

            // Add data in cache
            List<NWDPlayerDataStorage> tPlayerData = tPlayerOperation.Result;
            if (tPlayerData.Count > 0)
            {
                for (int i = 0; i < tPlayerData.Count; i++)
                {
                    Internal_InsertOrReplacePlayerData(tPlayerOperation.Result[i]);
                }
            }

            List<SD> tStudioData = tStudioOperation.Result;
            if (tStudioData.Count > 0)
            {
                for (int i = 0; i < tStudioData.Count; i++)
                {
                    Internal_InsertOrReplaceStudioData(tStudioOperation.Result[i]);
                }
            }

            State = NWDDataManagerState.Ready;
        }

        private void Internal_InsertOrReplacePlayerData(NWDPlayerDataStorage sPlayerData)
        {
            lock (_lock)
            {
                int tIndex = PlayerData.FindIndex(x => x.Reference == sPlayerData.Reference && x.DataTrack == sPlayerData.DataTrack);
                if (tIndex < 0)
                {
                    PlayerData.Add(sPlayerData);
                }
                else
                {
                    PlayerData[tIndex] = sPlayerData;
                }
            }
        }

        protected virtual void Internal_InsertOrReplaceStudioData(SD sStudioData)
        {
            lock (_lock)
            {
                int tIndex = StudioData.FindIndex(x => x.Reference == sStudioData.Reference);
                if (tIndex < 0)
                {
                    StudioData.Add(sStudioData);
                }
                else
                {
                    StudioData[tIndex] = sStudioData;
                }
            }
        }

        protected IEnumerable<object> Internal_GetAllData(Type sDataType)
        {
            IEnumerable<object> rResult;
            if (IsPlayerDataType(sDataType))
            {
                rResult = Internal_GetAllPlayerData(sDataType);
            }
            else
            {
                rResult = Internal_GetAllStudioData(sDataType);
            }
            return rResult;
        }
        protected IEnumerable<T> Internal_GetAllData<T>() where T : class
        {
            return Internal_GetAllData(typeof(T)) as IEnumerable<T>;
        }
        protected IEnumerable<object> Internal_GetAllPlayerData(Type sDataType)
        {
            IEnumerable<NWDPlayerDataStorage> tSelection = GetAllPlayerDataStorage(sDataType);
            lock (_lock)
            {
                return tSelection.Select(x => GetPlayerDataFromStorage(x)).Where(x => x != null);
            }
        }
        protected IEnumerable<T> Internal_GetAllPlayerData<T>() where T : class
        {
            IEnumerable<NWDPlayerDataStorage> tSelection = GetAllPlayerDataStorage(typeof(T));
            lock (_lock)
            {
                return tSelection.Select(x => GetPlayerDataFromStorage(x) as T).Where(x => x != null);
            }
        }
        protected IEnumerable<object> Internal_GetAllStudioData(Type sDataType)
        {
            IEnumerable<SD> tSelection = GetAllStudioDataStorage(sDataType);
            lock (_lock)
            {
                return tSelection.Select(x => GetStudioDataFromStorage(x, sDataType)).Where(x => x != null);
            }
        }
        protected IEnumerable<T> Internal_GetAllStudioData<T>() where T : class
        {
            IEnumerable<SD> tSelection = GetAllStudioDataStorage(typeof(T));
            lock (_lock)
            {
                return tSelection.Select(x => GetStudioDataFromStorage(x, typeof(T)) as T).Where(x => x != null);
            }
        }
        #endregion
    }
}
