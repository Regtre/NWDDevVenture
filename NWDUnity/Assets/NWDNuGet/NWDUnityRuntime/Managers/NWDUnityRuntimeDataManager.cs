using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDRuntime.Exchanges.Payloads;
using NWDRuntime.Factories;
using NWDUnityRuntime.Services;
using NWDUnityRuntime.TaskSchedulers;
using NWDUnityShared.Engine;
using NWDUnityShared.Managers;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NWDUnityRuntime.Managers
{
    public class NWDUnityRuntimeDataManager : NWDUnityDataManager<NWDStudioDataStorage, NWDUnityRuntimeDataScheduler>
    {
        #region Data access methods
        public override NWDStudioDataStorage GetStudioDataStorageByReference(ulong sReference)
        {
            int tDataTrack = NWDUnityEngine.Instance.EnvironmentManager.GetCurrentDataTrack();
            lock (_lock)
            {
                return StudioData.FirstOrDefault(x => x.Reference == sReference && x.DataTrack == tDataTrack);
            }
        }

        protected override IEnumerable<NWDStudioDataStorage> GetAllStudioDataStorage(Type sDataType)
        {
            int tDataTrack = NWDUnityEngine.Instance.EnvironmentManager.GetCurrentDataTrack();
            lock (_lock)
            {
                return StudioData.Where(x => sDataType.AssemblyQualifiedName == x.ClassName && tDataTrack == x.DataTrack);
            }
        }

        protected override object GetStudioDataFromStorage(NWDStudioDataStorage sStudioDataStorage, Type _)
        {
            object rResult = null;
            if (sStudioDataStorage != null)
            {
                rResult = NWDStudioDataFactory.FromStudioDataStorage(sStudioDataStorage);
            }
            return rResult;
        }

        protected override void Internal_InsertOrReplaceStudioData(NWDStudioDataStorage sStudioData)
        {
            lock (_lock)
            {
                int tIndex = StudioData.FindIndex(x => x.Reference == sStudioData.Reference && x.DataTrack == sStudioData.DataTrack);
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
        #endregion

        #region Management methods
        public override NWDAsyncOperation Synchronize()
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                NWDRequestPlayerToken tPlayerToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
                NWDDownPayloadDataSyncByIncrement tResponse = null;
                lock (_lock)
                {
                    tResponse = NWDDataSyncService.Synchronize(tPlayerToken, ref sHandler, PlayerDataToSave, new List<NWDStudioDataStorage>());
                }
                Internal_Synchronize(ref sHandler, tResponse.StudioDataList, tResponse.PlayerDataList);
            });
        }
        #endregion
    }
}
