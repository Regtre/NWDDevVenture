// using Newtonsoft.Json;
// using NWDFoundation.Configuration.Environments;
// using NWDFoundation.Exchanges;
// using NWDFoundation.Logger;
// using NWDFoundation.Models;
// using NWDFoundation.Tools;
// using NWDRuntime.Exchanges;
// using NWDRuntime.Exchanges.Payloads;
// using NWDServerMiddle.Managers.ModelManagers;
// using NWDServerMiddle.Models;
// using JsonSerializer = System.Text.Json.JsonSerializer;
//
// namespace NWDServerTests.ServerRequestManager;
//
//
// public class NWDTestRuntimeManager : NWDTestSetup {
//     
//     private static  ushort _Range;
//     private static  uint _Counter;
//     private const NWDEnvironmentKind K_EnvironmentKind = NWDEnvironmentKind.Dev;
//     private static readonly ulong ProjectId = NWDSetup.ProjectId;
//
//     private static NWDRequestPlayerToken? _PlayerToken;
//     private NWDAccountSign NewFakeAccountSign()
//     {
//         string tLoginHash = NWDRandom.RandomStringBase64(28) + (_Counter++).ToString("0000");
//         string tSignHash = NWDRandom.RandomStringBase64(28) + (_Counter++).ToString("0000");
//         string tRescueHash = NWDRandom.RandomStringBase64(28) + (_Counter++).ToString("0000");
//         NWDLogger.Trace(nameof(NewFakeAccountSign) + " create with : " +
//                         " SignHash:" + tSignHash + " " +
//                         " RescueEmail:" + tRescueHash + " " +
//                         " LoginHash:" + tLoginHash + " " +
//                         " for project " + NWDSetup.ProjectId);
//         return new NWDAccountSign()
//         {
//             ProjectId = NWDSetup.ProjectId,
//             LoginHash = tLoginHash,
//             SignHash = tSignHash,
//             RescueHash = tRescueHash,
//         };
//     }
//
//     [SetUp]
//     public void SetUp()
//     {
//         NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
//         NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
//         NWDAccountSign tAccountSign = NewFakeAccountSign();
//         NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
//         NWDResponseRuntime tResponseRuntimeSignUp = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUp, tAccountInformation);
//         _PlayerToken = tResponseRuntimeSignUp.PlayerToken;
//         _Range = tResponseRuntimeSignUp.PlayerToken.AccountRange;
//     }
//
//
//     [TestCase(17UL, 78UL, 20UL,47UL,100UL,101UL)]
//     public void TestSyncDataByIncrement(ulong sSyncIndexStudioRequest, ulong sSyncIndexStudioDb,ulong sSyncIndexPlayerRequest, ulong sSyncIndexPlayerDb, ulong sReferencePlayer, ulong sReferenceStudio)
//     {
//         
//         NWDStudioDataStorage tStudioData = new NWDStudioDataStorage()
//         {
//             DataTrack =  0,
//             Json = "Test Request SyncData",
//             ProjectId = NWDSetup.ProjectId,
//             Reference = sReferenceStudio,
//             SyncIndex = sSyncIndexStudioDb,
//         };
//         NWDStudioDataManager.InsertOrUpdate(K_EnvironmentKind, ProjectId, new List<NWDStudioDataStorage>(){tStudioData});
//
//         if (_PlayerToken != null)
//         {
//             NWDPlayerDataStorage tPlayerData = new NWDPlayerDataStorage()
//             {
//                 DataTrack = 0,
//                 Json = "Test Request SyncData",
//                 ProjectId = NWDSetup.ProjectId,
//                 Reference = sReferencePlayer,
//                 SyncIndex = sSyncIndexPlayerDb,
//                 Account = _PlayerToken.PlayerReference
//             };
//             NWDPlayerDataManager.InsertOrUpdate(K_EnvironmentKind, ProjectId, _Range, new List<NWDPlayerDataStorage>(){tPlayerData});
//         }
//
//
//         NWDUpPayloadDataSyncByIncrement tPayloadDataSyncByIncrement = new NWDUpPayloadDataSyncByIncrement()
//         {
//             PlayerLastSync = sSyncIndexStudioRequest,
//             StudioDataSyncInformation = sSyncIndexPlayerRequest
//         };
//
//         NWDRequestRuntime tRequestRuntimeSyncData = new NWDRequestRuntime()
//         {
//             ProjectId = 9UL,
//             Kind = NWDExchangeRuntimeKind.SyncDataByIncrement,
//             PlayerToken =_PlayerToken,
//             Payload = JsonSerializer.Serialize(tPayloadDataSyncByIncrement),
//         };
//         tRequestRuntimeSyncData.Secure(NWDRandom.RandomStringCypher(32));
//         Assert.That(tRequestRuntimeSyncData.IsValid(), Is.True);
//
//         
//         NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntimeSyncData);
//         NWDDownPayloadDataSyncByIncrement? tDownPayloadDataSyncByIncrement =
//             JsonConvert.DeserializeObject<NWDDownPayloadDataSyncByIncrement>(tResponseRuntime.Payload);
//
//         Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
//         if (tDownPayloadDataSyncByIncrement != null)
//         {
//             Assert.Multiple(() =>
//             {
//                 Assert.That(tDownPayloadDataSyncByIncrement.StudioLastSync, Is.EqualTo(sSyncIndexStudioDb));
//                 Assert.That(tDownPayloadDataSyncByIncrement.PlayerLastSync, Is.EqualTo(sSyncIndexPlayerDb));
//             });
//         }
//     }
//
//     [TestCase(200UL,201UL)]
//     public void TestAllData(ulong sReferencePlayer, ulong sReferenceStudio)
//     {
//         NWDStudioDataStorage tStudioData = new NWDStudioDataStorage()
//         {
//             DataTrack =  0,
//             Json = "Test Request SyncData",
//             ProjectId = NWDSetup.ProjectId,
//             Reference = sReferenceStudio,
//         };
//         NWDStudioDataManager.InsertOrUpdate(K_EnvironmentKind, ProjectId,new List<NWDStudioDataStorage>() { tStudioData });
//         if (_PlayerToken != null)
//         {
//             NWDPlayerDataStorage tPlayerData = new NWDPlayerDataStorage()
//             {
//                 DataTrack = 0,
//                 Json = "Test Request SyncData",
//                 ProjectId = NWDSetup.ProjectId,
//                 Reference = sReferencePlayer,
//                 Account = _PlayerToken.PlayerReference
//
//             };
//             NWDPlayerDataManager.InsertOrUpdate(K_EnvironmentKind, ProjectId, _Range,new List<NWDPlayerDataStorage>() { tPlayerData });
//         }
//
//         NWDRequestRuntime tRequestRuntime = new NWDRequestRuntime()
//         {
//             ProjectId = 9UL,
//             Kind = NWDExchangeRuntimeKind.GetAllData,
//             PlayerToken =_PlayerToken,
//             Payload = string.Empty,
//         };
//         tRequestRuntime.Secure(NWDRandom.RandomStringAlpha(12));
//         Assert.That(tRequestRuntime.IsValid(), Is.True);
//
//         NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime); 
//         NWDDownPayloadAllData? tPayloadAllData = JsonConvert.DeserializeObject<NWDDownPayloadAllData>(tResponseRuntime.Payload);
//         Assert.That(tResponseRuntime.Status,Is.EqualTo(NWDRequestStatus.Ok));
//         Assert.That(tPayloadAllData?.PlayerDataList.Count,Is.GreaterThan(0));
//         Assert.That(tPayloadAllData?.StudioDataList.Count,Is.GreaterThan(0));
//
//     }
//
//     [TestCase(300UL,301UL,302UL)]
//     public void TestAllPlayerData(params ulong[] sReferences)
//     {
//         foreach (ulong tReference in sReferences)
//         {
//             if (_PlayerToken != null)
//             {
//                 NWDPlayerDataStorage tPlayerData = new NWDPlayerDataStorage()
//                 {
//                     Json = "TestAllPlayerData",
//                     Reference = tReference,
//                     DataTrack = 0,
//                     Account = _PlayerToken.PlayerReference
//
//                 };
//                 NWDPlayerDataManager.InsertOrUpdate(K_EnvironmentKind, ProjectId, _Range, new List<NWDPlayerDataStorage>() { tPlayerData });
//             }
//         }
//         NWDRequestRuntime tRequestRuntime = new NWDRequestRuntime()
//         {
//             ProjectId = 9UL,
//             Kind = NWDExchangeRuntimeKind.GetAllPlayerData,
//             PlayerToken =_PlayerToken,
//             Payload = string.Empty,
//             
//         };
//         tRequestRuntime.Secure(NWDRandom.RandomStringAlpha(12));
//         Assert.That(tRequestRuntime.IsValid(), Is.True);
//
//         NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime); 
//         NWDDownPayloadAllPlayerData? tPayloadAllData = JsonConvert.DeserializeObject<NWDDownPayloadAllPlayerData>(tResponseRuntime.Payload);
//         Assert.That(tResponseRuntime.Status,Is.EqualTo(NWDRequestStatus.Ok));
//         Assert.That(tPayloadAllData?.PlayerDataList.Count,Is.EqualTo(sReferences.ToList().Count));
//     }
//
//     
//     [TestCase( 15UL,45UL,78UL)]
//     public void TestPlayerDataByReferences(params ulong[] sReferences)
//     {
//         foreach (ulong tReference in sReferences)
//         {
//             NWDPlayerDataStorage tPlayerData = new NWDPlayerDataStorage()
//             {
//                 Json = "TestPlayerDataByReferences",
//                 Reference = tReference,
//                 DataTrack = 0
//             };
//             NWDPlayerDataManager.InsertOrUpdate(K_EnvironmentKind, ProjectId, _Range,new List<NWDPlayerDataStorage>() { tPlayerData });
//         }
//         
//         NWDUpPayloadPlayerDataByReferences tUpPayloadPlayerDataByReferences = new NWDUpPayloadPlayerDataByReferences()
//         {
//             PlayerDataReferencesList = sReferences.ToList()
//         };
//         
//         NWDRequestRuntime tRequestRuntime = new NWDRequestRuntime()
//         {
//             ProjectId = 9UL,
//             Kind = NWDExchangeRuntimeKind.GetPlayerDataByReferences,
//             PlayerToken =_PlayerToken,
//             Payload = JsonConvert.SerializeObject(tUpPayloadPlayerDataByReferences),
//         };
//         tRequestRuntime.Secure(NWDRandom.RandomStringAlpha(12));
//         Assert.That(tRequestRuntime.IsValid(), Is.True);
//
//         NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime); 
//         NWDDownPayloadAllPlayerData? tPayloadAllData = JsonConvert.DeserializeObject<NWDDownPayloadAllPlayerData>(tResponseRuntime.Payload);
//         
//         Assert.That(tResponseRuntime.Status,Is.EqualTo(NWDRequestStatus.Ok));
//         Assert.That(tPayloadAllData?.PlayerDataList.Count,Is.EqualTo(tUpPayloadPlayerDataByReferences.PlayerDataReferencesList.Count));
//     }
//
//     
//     [TestCase (400UL,4010UL,420UL,430UL)]
//     public void TestAllStudioData(params ulong[] sReferences)
//     {
//         foreach (ulong tReference in sReferences)
//         {
//             NWDStudioDataStorage tPlayerData = new NWDStudioDataStorage()
//             {
//                 Json = "TestAllStudioData",
//                 Reference = tReference,
//                 DataTrack = 0
//             };
//             NWDStudioDataManager.InsertOrUpdate(K_EnvironmentKind, ProjectId,new List<NWDStudioDataStorage>() { tPlayerData });
//         }
//         
//         NWDRequestRuntime tRequestRuntime = new NWDRequestRuntime()
//         {
//             ProjectId = 9UL,
//             Kind = NWDExchangeRuntimeKind.GetAllStudioData,
//             PlayerToken =_PlayerToken,
//             Payload = string.Empty,
//         };
//         tRequestRuntime.Secure(NWDRandom.RandomStringAlpha(12));
//         Assert.That(tRequestRuntime.IsValid(), Is.True);
//
//         NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime); 
//         NWDDownPayloadAllStudioData? tPayloadAllData = JsonConvert.DeserializeObject<NWDDownPayloadAllStudioData>(tResponseRuntime.Payload);
//         Assert.That(tResponseRuntime.Status,Is.EqualTo(NWDRequestStatus.Ok));
//         Assert.That(tPayloadAllData?.StudioDataList.Count,Is.EqualTo(sReferences.ToList().Count));
//     }
//
//     [TestCase (500UL,5010UL,520UL,530UL)]
//     public void TestStudioDataByReferences(params ulong[] sReferences)
//     {
//         foreach (ulong tReference in sReferences)
//         {
//             NWDStudioDataStorage tStudioData = new NWDStudioDataStorage()
//             {
//                 Json = "TestPlayerDataByReferences",
//                 Reference = tReference,
//                 DataTrack = 0
//             };
//             NWDStudioDataManager.InsertOrUpdate(K_EnvironmentKind, ProjectId,new List<NWDStudioDataStorage>() { tStudioData });
//         }
//         
//         NWDUpPayloadStudioDataByReferences tUpPayloadStudioDataByReferences = new NWDUpPayloadStudioDataByReferences()
//         {
//            StudioDataReferenceList = sReferences.ToList()
//         };
//         
//         NWDRequestRuntime tRequestRuntime = new NWDRequestRuntime()
//         {
//             ProjectId = 9UL,
//             Kind = NWDExchangeRuntimeKind.GetStudioDataByReferences,
//             PlayerToken =_PlayerToken,
//             Payload = JsonConvert.SerializeObject(tUpPayloadStudioDataByReferences),
//         };
//         tRequestRuntime.Secure(NWDRandom.RandomStringAlpha(12));
//         Assert.That(tRequestRuntime.IsValid(), Is.True);
//
//         NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime); 
//         NWDDownPayloadAllStudioData? tPayloadAllData = JsonConvert.DeserializeObject<NWDDownPayloadAllStudioData>(tResponseRuntime.Payload);
//         
//         Assert.That(tResponseRuntime.Status,Is.EqualTo(NWDRequestStatus.Ok));
//         Assert.That(tPayloadAllData?.StudioDataList.Count,Is.EqualTo(tUpPayloadStudioDataByReferences.StudioDataReferenceList.Count));
//     }
//
//     [TearDown]
//     public void TearDown()
//     {
//         List<NWDStudioDataStorage> tStudioDatas = NWDStudioDataManager.GetAllForProject(K_EnvironmentKind,NWDSetup.ProjectId);
//         NWDStudioDataManager.DeleteRangeByReferences(K_EnvironmentKind,NWDSetup.ProjectId,tStudioDatas.Select(sStudio => sStudio.Reference ).ToList());
//         List<NWDPlayerDataStorage> tPlayerDatas = NWDPlayerDataManager.GetAllForProject(K_EnvironmentKind,_Range,NWDSetup.ProjectId);
//         NWDPlayerDataManager.DeleteRangeByReferences(K_EnvironmentKind,NWDSetup.ProjectId,_Range,tPlayerDatas.Select(sPlayer => sPlayer.Reference ).ToList());
//     }
// }