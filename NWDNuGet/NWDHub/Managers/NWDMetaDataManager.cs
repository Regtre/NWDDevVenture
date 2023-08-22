using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using NWDCrucial.Exchanges;
using NWDCrucial.Exchanges.Payloads;
using NWDEditor;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDHub.Configuration;
using NWDHub.Factories;
using NWDWebEditor.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Tools;

namespace NWDHub.Managers
{
    public static class NWDMetaDataManager
    {
        private const int _LOCKER_DELAY = 3600;

        public static List<NWDMetaData> SyncMetaData(List<NWDMetaData> sMetaDataList, NWDProjectDescription sProjectDescription)
        {
            Dictionary<NWDEnvironmentKind, Dictionary<NWDMetaData, List<NWDSubMetaData>>> tDataCache = new Dictionary<NWDEnvironmentKind, Dictionary<NWDMetaData, List<NWDSubMetaData>>> ();

            foreach (NWDMetaData tMetadata in sMetaDataList)
            {
                NWDMetaData? tProcessedMetaData = NWDWebDBDataManager.SaveData(tMetadata);

                if (tProcessedMetaData != null && !string.IsNullOrEmpty (tProcessedMetaData.DataByDataTrack))
                {
                    List<NWDSubMetaData>? tSubMetaDataList = JsonConvert.DeserializeObject<List<NWDSubMetaData>>(tProcessedMetaData.DataByDataTrack);
                    if (tSubMetaDataList != null)
                    {
                        foreach(NWDSubMetaData tSubMetaData in tSubMetaDataList)
                        {
                            if (!string.IsNullOrEmpty(tSubMetaData.Data))
                            {
                                if (!tDataCache.ContainsKey(tSubMetaData.TrackKind))
                                {
                                    tDataCache.Add(tSubMetaData.TrackKind, new Dictionary<NWDMetaData, List<NWDSubMetaData>>());
                                }

                                if (!tDataCache[tSubMetaData.TrackKind].ContainsKey(tProcessedMetaData))
                                {
                                    tDataCache[tSubMetaData.TrackKind].Add(tProcessedMetaData, new List<NWDSubMetaData>());
                                }

                                tDataCache[tSubMetaData.TrackKind][tProcessedMetaData].Add(tSubMetaData);
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<NWDEnvironmentKind, Dictionary<NWDMetaData, List<NWDSubMetaData>>>  tCache in tDataCache)
            {
                switch(tCache.Key)
                {
                    case NWDEnvironmentKind.Dev:
                        PublishDevStudioData(sProjectDescription, tCache.Value);
                        break;
                    case NWDEnvironmentKind.PlayTest:
                        PublishPlaytestStudioData(sProjectDescription, tCache.Value);
                        break;
                    case NWDEnvironmentKind.Qualification:
                        PublishQualificationStudioData(sProjectDescription, tCache.Value);
                        break;
                    case NWDEnvironmentKind.PreProduction:
                        PublishPreprodStudioData(sProjectDescription, tCache.Value);
                        break;
                    case NWDEnvironmentKind.Production:
                        PublishproductionStudioData(sProjectDescription, tCache.Value);
                        break;
                    default:
                        break;
                }
            }

            return NWDWebDBDataManager.GetBy<NWDMetaData>(new Dictionary<string, string>()
            {
                { nameof(NWDMetaData.ProjectUniqueId), sProjectDescription.ProjectId.ToString() }
            });
        }

        public static NWDMetaData? LockMetaData(ulong sReference, string sLockerName, NWDProjectDescription sProjectDescription, int sLockDelay = _LOCKER_DELAY)
        {
            NWDMetaData? tMetaData = NWDWebDBDataManager.GetDataByReference<NWDMetaData>(sReference);
            if (tMetaData != null)
            {
                if (tMetaData.LockLimit < NWDToolBox.ToTimestampUnix(DateTime.UtcNow))
                {
                    tMetaData.IsLocked = true;
                    tMetaData.LockLimit = NWDToolBox.ToTimestampUnix(DateTime.UtcNow.AddSeconds(sLockDelay));
                    tMetaData.LockerName = sLockerName;
                    NWDWebDBDataManager.SaveData(tMetaData);
                }
                else
                {
                    if (tMetaData.LockerName == sLockerName)
                    {
                        tMetaData.IsLocked = true;
                        tMetaData.LockLimit = NWDToolBox.ToTimestampUnix(DateTime.UtcNow.AddSeconds(sLockDelay));
                        tMetaData.LockerName = sLockerName;
                        NWDWebDBDataManager.SaveData(tMetaData);
                    }
                    else
                    {
                        // Do  nothing lock limit is active!
                    }
                }
            }
            //TODO verif rights of  sProjectDescription on this data 
            //TODO flush all datatrack non-authorized 
            return tMetaData;
        }

        public static NWDMetaData? UnlockMetaData(NWDMetaData sMetaData, string sLockerName, NWDProjectDescription sProjectDescription)
        {
            NWDMetaData? tMetaData = NWDWebDBDataManager.GetDataByReference<NWDMetaData>(sMetaData.Reference);
            if (tMetaData != null)
            {
                //TODO verif rights of  sProjectDescription on this data 
                //TODO modify only datatrack authorized 
                if (tMetaData.LockerName == sLockerName)
                {
                    tMetaData.IsLocked = false;
                    tMetaData.LockLimit = 0;
                    tMetaData.LockerName = string.Empty;
                    NWDWebDBDataManager.SaveData(tMetaData);
                }
            }
            return tMetaData;
        }

        public static NWDMetaData? CreateMetaData(string sTypeClass, NWDProjectDescription sProjectDescription)
        {
            //TODO verif rights of sProjectDescription on this data 
            
            NWDMetaData rResult = new NWDMetaData();
            rResult.ProjectUniqueId = sProjectDescription.ProjectId;
            rResult.ClassName = sTypeClass;
            rResult.DataByDataTrack = JsonConvert.SerializeObject(new NWDSubMetaData[0]);

            return NWDWebDBDataManager.SaveData(rResult, true);
        }

        public static void PublishDevStudioData(NWDProjectDescription sProjectDescription, Dictionary<NWDMetaData, List<NWDSubMetaData>> sSubMetaDataList)
        {
            List<NWDStudioDataStorage> tStudioStorageList = new List<NWDStudioDataStorage>();

            foreach (KeyValuePair<NWDMetaData, List<NWDSubMetaData>> tSubMetaDataItem in sSubMetaDataList)
            {
                foreach (NWDSubMetaData tSubMetaData in tSubMetaDataItem.Value)
                {
                    tStudioStorageList.Add(NWDStudioDataStorageFactory.FromMetaData(tSubMetaDataItem.Key, tSubMetaData));
                }
            }

            if (tStudioStorageList.Count > 0)
            {
                // Send request !
                NWDUpPayloadPublishStudioData? tPayloadPublishStudioData = new NWDUpPayloadPublishStudioData()
                {
                    Kind = NWDEnvironmentKind.Dev,
                    ProjectId = sProjectDescription.ProjectId,
                    StudioDataStorageList = tStudioStorageList
                };
                NWDRequestCrucial tRequest = new NWDRequestCrucial(NWDHubConfiguration.KConfig, NWDExchangeCrucialKind.PublishStudioData, tPayloadPublishStudioData, NWDExchangeOrigin.Web, NWDExchangeDevice.Unknown);
                NWDResponseCrucial? tResponseCrucial = NWDWebCrucialCallbackServers.PostRequest(tRequest).Result;
            }
        }

        public static void PublishPlaytestStudioData(NWDProjectDescription sProjectDescription, Dictionary<NWDMetaData, List<NWDSubMetaData>> sSubMetaDataList)
        {
            List<NWDStudioDataStorage> tStudioStorageList = new List<NWDStudioDataStorage>();

            foreach (KeyValuePair<NWDMetaData, List<NWDSubMetaData>> tSubMetaDataItem in sSubMetaDataList)
            {
                foreach (NWDSubMetaData tSubMetaData in tSubMetaDataItem.Value)
                {
                    tStudioStorageList.Add(NWDStudioDataStorageFactory.FromMetaData(tSubMetaDataItem.Key, tSubMetaData));
                }
            }

            if (tStudioStorageList.Count > 0)
            {
                // Send request !
                NWDUpPayloadPublishStudioData? tPayloadPublishStudioData = new NWDUpPayloadPublishStudioData()
                {
                    Kind = NWDEnvironmentKind.PlayTest,
                    ProjectId = sProjectDescription.ProjectId,
                    StudioDataStorageList = tStudioStorageList
                };
                NWDRequestCrucial tRequest = new NWDRequestCrucial(NWDHubConfiguration.KConfig, NWDExchangeCrucialKind.PublishStudioData, tPayloadPublishStudioData, NWDExchangeOrigin.Web, NWDExchangeDevice.Unknown);
                NWDResponseCrucial? tResponseCrucial = NWDWebCrucialCallbackServers.PostRequest(tRequest).Result;
            }
        }

        public static void PublishQualificationStudioData(NWDProjectDescription sProjectDescription, Dictionary<NWDMetaData, List<NWDSubMetaData>> sSubMetaDataList)
        {
            List<NWDStudioDataStorage> tStudioStorageList = new List<NWDStudioDataStorage>();

            foreach (KeyValuePair<NWDMetaData, List<NWDSubMetaData>> tSubMetaDataItem in sSubMetaDataList)
            {
                foreach (NWDSubMetaData tSubMetaData in tSubMetaDataItem.Value)
                {
                    tStudioStorageList.Add(NWDStudioDataStorageFactory.FromMetaData(tSubMetaDataItem.Key, tSubMetaData));
                }
            }

            if (tStudioStorageList.Count > 0)
            {
                // Send request !
                NWDUpPayloadPublishStudioData? tPayloadPublishStudioData = new NWDUpPayloadPublishStudioData()
                {
                    Kind = NWDEnvironmentKind.Qualification,
                    ProjectId = sProjectDescription.ProjectId,
                    StudioDataStorageList = tStudioStorageList
                };
                NWDRequestCrucial tRequest = new NWDRequestCrucial(NWDHubConfiguration.KConfig, NWDExchangeCrucialKind.PublishStudioData, tPayloadPublishStudioData, NWDExchangeOrigin.Web, NWDExchangeDevice.Unknown);
                NWDResponseCrucial? tResponseCrucial = NWDWebCrucialCallbackServers.PostRequest(tRequest).Result;
            }
        }

        public static void PublishPreprodStudioData(NWDProjectDescription sProjectDescription, Dictionary<NWDMetaData, List<NWDSubMetaData>> sSubMetaDataList)
        {
            List<NWDStudioDataStorage> tStudioStorageList = new List<NWDStudioDataStorage>();

            foreach (KeyValuePair<NWDMetaData, List<NWDSubMetaData>> tSubMetaDataItem in sSubMetaDataList)
            {
                foreach (NWDSubMetaData tSubMetaData in tSubMetaDataItem.Value)
                {
                    tStudioStorageList.Add(NWDStudioDataStorageFactory.FromMetaData(tSubMetaDataItem.Key, tSubMetaData));
                }
            }

            if (tStudioStorageList.Count > 0)
            {
                // Send request !
                NWDUpPayloadPublishStudioData? tPayloadPublishStudioData = new NWDUpPayloadPublishStudioData()
                {
                    Kind = NWDEnvironmentKind.PreProduction,
                    ProjectId = sProjectDescription.ProjectId,
                    StudioDataStorageList = tStudioStorageList
                };
                NWDRequestCrucial tRequest = new NWDRequestCrucial(NWDHubConfiguration.KConfig, NWDExchangeCrucialKind.PublishStudioData, tPayloadPublishStudioData, NWDExchangeOrigin.Web, NWDExchangeDevice.Unknown);
                NWDResponseCrucial? tResponseCrucial = NWDWebCrucialCallbackServers.PostRequest(tRequest).Result;
            }
        }

        public static void PublishproductionStudioData(NWDProjectDescription sProjectDescription, Dictionary<NWDMetaData, List<NWDSubMetaData>> sSubMetaDataList)
        {
            List<NWDStudioDataStorage> tStudioStorageList = new List<NWDStudioDataStorage>();

            foreach (KeyValuePair<NWDMetaData, List<NWDSubMetaData>> tSubMetaDataItem in sSubMetaDataList)
            {
                foreach (NWDSubMetaData tSubMetaData in tSubMetaDataItem.Value)
                {
                    tStudioStorageList.Add(NWDStudioDataStorageFactory.FromMetaData(tSubMetaDataItem.Key, tSubMetaData));
                }
            }

            if (tStudioStorageList.Count > 0)
            {
                // Send request !
                NWDUpPayloadPublishStudioData? tPayloadPublishStudioData = new NWDUpPayloadPublishStudioData()
                {
                    Kind = NWDEnvironmentKind.Production,
                    ProjectId = sProjectDescription.ProjectId,
                    StudioDataStorageList = tStudioStorageList
                };
                NWDRequestCrucial tRequest = new NWDRequestCrucial(NWDHubConfiguration.KConfig, NWDExchangeCrucialKind.PublishStudioData, tPayloadPublishStudioData, NWDExchangeOrigin.Web, NWDExchangeDevice.Unknown);
                NWDResponseCrucial? tResponseCrucial = NWDWebCrucialCallbackServers.PostRequest(tRequest).Result;
            }
        }
    }
}