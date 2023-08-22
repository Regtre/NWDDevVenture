using Newtonsoft.Json;
using NWDEditor;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDRuntime.Exchanges.Payloads;
using NWDUnityEditor.Constants;
using NWDUnityEditor.Models;
using NWDUnityEditor.Services;
using NWDUnityEditor.TaskSchedulers;
using NWDUnityEditor.Windows;
using NWDUnityRuntime.Services;
using NWDUnityShared.Engine;
using NWDUnityShared.Managers;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWDUnityEditor.Managers
{
    public class NWDUnityEditorDataManager : NWDUnityDataManager<NWDMetaData, NWDUnityEditorDataScheduler>
    {
        #region MetaData methods
        public ulong GetForClass(ref List<NWDMetaData> sData, string sClassName)
        {
            return GetForClass(ref sData, sClassName, UpdateIndex + 1);
        }

        public ulong GetForClass(ref List<NWDMetaData> sData, string[] sClassNames)
        {
            return GetForClass(ref sData, sClassNames, UpdateIndex + 1);
        }

        public ulong GetForClass(ref List<NWDMetaData> sData, string sClassName, ulong sUpdateIndex)
        {
            lock (_lock)
            {
                if (sUpdateIndex != UpdateIndex)
                {
                    sData = StudioData.Where(x => x.ClassName == sClassName).ToList();
                }
                return UpdateIndex;
            }
        }

        public ulong GetForClass(ref List<NWDMetaData> sData, string[] sClassNames, ulong sUpdateIndex)
        {
            lock (_lock)
            {
                if (sUpdateIndex != UpdateIndex)
                {
                    sData = StudioData.Where(x => sClassNames.Contains(x.ClassName)).ToList();
                }
                return UpdateIndex;
            }
        }

        public NWDMetaData GetMetaDataByReference(ulong sReference)
        {
            lock (_lock)
            {
                return StudioData.FirstOrDefault(x => x.Reference == sReference);
            }
        }

        protected override IEnumerable<NWDMetaData> GetAllStudioDataStorage(Type sDataType)
        {
            lock (_lock)
            {
                return StudioData.Where(x => sDataType.AssemblyQualifiedName == x.ClassName);
            }
        }

        public override NWDMetaData GetStudioDataStorageByReference(ulong sReference)
        {
            lock (_lock)
            {
                return StudioData.FirstOrDefault(x => x.Reference == sReference);
            }
        }

        protected override object GetStudioDataFromStorage(NWDMetaData sStudioDataStorage, Type sDataType)
        {
            object rResult = null;
            if (sStudioDataStorage != null)
            {
                List<NWDSubMetaData> tSubMetaData = new List<NWDSubMetaData>();
                if (!string.IsNullOrEmpty(sStudioDataStorage.DataByDataTrack))
                {
                    tSubMetaData = JsonConvert.DeserializeObject<List<NWDSubMetaData>>(sStudioDataStorage.DataByDataTrack);
                }

                int tDataTrack = NWDUnityEngine.Instance.EnvironmentManager.GetCurrentDataTrack();
                NWDEnvironmentKind tKind = NWDUnityEngine.Instance.EnvironmentManager.GetCurrentEnvironment();

                NWDSubMetaData tCurrentSub = tSubMetaData.FirstOrDefault(x => x.TrackId == tDataTrack && x.TrackKind == tKind);
                if (tCurrentSub != null)
                {
                    rResult = JsonConvert.DeserializeObject(tCurrentSub.Data, sDataType);
                }
            }
            return rResult;
        }
        #endregion

        #region Life cycle methods
        public override void Stop()
        {
            Scheduler.StopRecurrentTasks();
            base.Stop();
        }
        #endregion

        #region Management methods
        public override NWDAsyncOperation Synchronize()
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                NWDRequestPlayerToken tPlayerToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
                List<NWDMetaData> tMetaData = NWDMetaDataEditorService.SyncAll(StudioData);
                NWDDownPayloadDataSyncByIncrement tResponse = NWDDataSyncService.Synchronize(tPlayerToken, ref sHandler, PlayerDataToSave, new List<NWDStudioDataStorage>());

                Internal_Synchronize(ref sHandler, tMetaData, tResponse?.PlayerDataList);
            });
        }

        public NWDAsyncOperation<NWDMetaDataResult> LockMetaData(NWDUnityEditorWindowData sWindow, NWDMetaData sMetaData)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                NWDMetaDataResult rResult = new NWDMetaDataResult
                {
                    MetaData = NWDMetaDataEditorService.Lock(sMetaData),
                    Window = sWindow
                };

                Internal_Synchronize(ref sHandler, new List<NWDMetaData>() { rResult.MetaData });

                return rResult;
            });
        }

        public NWDAsyncOperation<NWDMetaDataResult> UnlockMetaData(NWDUnityEditorWindowData sWindow, NWDMetaData sMetaData)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                NWDMetaDataResult rResult = new NWDMetaDataResult
                {
                    MetaData = NWDMetaDataEditorService.Unlock(sMetaData),
                    Window = sWindow
                };

                Internal_Synchronize(ref sHandler, new List<NWDMetaData>() { rResult.MetaData });

                return rResult;
            });
        }

        public NWDAsyncOperation<NWDMetaDataResult> CreateMetaData(NWDUnityEditorWindowData sWindow, Type sType)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                NWDMetaDataResult rResult = new NWDMetaDataResult
                {
                    MetaData = NWDMetaDataEditorService.Create(sType),
                    Window = sWindow
                };

                Internal_Synchronize(ref sHandler, new List<NWDMetaData>() { rResult.MetaData });

                return rResult;
            });
        }
        #endregion

        #region Internal
        protected override async Task Internal_Initialize()
        {
            await base.Internal_Initialize();

            Scheduler.StartRecurrentTasks();

            AutoSynOperation = NWDAsyncOperationFactory.NewReccurentTask(Scheduler, (sHandler, _) =>
            {
                NWDRequestPlayerToken tPlayerToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
                List<NWDMetaData> tMetaData = NWDMetaDataEditorService.SyncAll(new List<NWDMetaData>());
                NWDDownPayloadDataSyncByIncrement tResponse = NWDDataSyncService.Synchronize(tPlayerToken, ref sHandler, PlayerDataToSave, new List<NWDStudioDataStorage>());

                Internal_Synchronize(ref sHandler, tMetaData, tResponse?.PlayerDataList ?? new List<NWDPlayerDataStorage>());
            }, NWDEditorConstants.CheckDataRepeatEvery);

        }
        #endregion

    }
}
