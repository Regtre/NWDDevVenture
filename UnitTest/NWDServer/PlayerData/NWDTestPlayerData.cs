// using NWDFoundation.Configuration.Environments;
// using NWDFoundation.Models;
// using NWDDevServerFrontBack.Tools;
// using NWDServerMiddle.Managers.ModelManagers;
//
// namespace NWDServerTests.PlayerData;
//
// [TestFixture(NWDEnvironmentKind.Dev)]
// [TestFixture(NWDEnvironmentKind.PlayTest)]
//
// public class NWDTestPlayerData : NWDTestSetup
// {
//     private static NWDEnvironmentKind _EnvironmentKind;
//
//     public NWDTestPlayerData(NWDEnvironmentKind sEnvironmentKind)
//     {
//         _EnvironmentKind = sEnvironmentKind;
//     }
//     private static readonly ulong ProjectId = NWDSetup.ProjectId;
//
//     [TestCase(01UL, (ushort)1)]
//     [TestCase(02UL, (ushort)2)]
//     public void TestInsertStudioDataManager_Insert(ulong sReference, ushort sRange)
//     {
//         NWDPlayerDataStorage tStudioData = new NWDPlayerDataStorage()
//         {
//             Reference = sReference, Json = "Data Track Test", ProjectId = ProjectId
//         };
//         Assert.That(
//             NWDPlayerDataManager.GetSingleByReference(_EnvironmentKind, ProjectId, sReference, sRange),
//             Is.Null);
//         NWDPlayerDataManager.InsertOrUpdate(_EnvironmentKind, ProjectId, sRange,new List<NWDPlayerDataStorage>() { tStudioData });
//         Assert.That(
//             NWDPlayerDataManager.GetSingleByReference(_EnvironmentKind, ProjectId, sReference, sRange),
//             Is.Not.Null);
//     }
//
//     [TestCase(11UL,  (ushort)1)]
//     [TestCase(12UL,  (ushort)2)]
//     public void TestInsertStudioDataManager_Update(ulong sReference, ushort sRange)
//     {
//         //SetUp
//         NWDPlayerDataStorage tStudioDataToUpdate = new NWDPlayerDataStorage()
//             { Reference = sReference, Json = "Test Trash", ProjectId = ProjectId };
//         NWDPlayerDataManager.InsertOrUpdate(_EnvironmentKind, ProjectId, sRange,new List<NWDPlayerDataStorage>() { tStudioDataToUpdate });
//
//         //Process        
//         string tJson = "Test Update " + TimeTools.GetNowTimestampUnix();
//         tStudioDataToUpdate.Json = tJson;
//         Assert.That(
//             NWDPlayerDataManager.GetSingleByReference(_EnvironmentKind, ProjectId, sReference, sRange),
//             Is.Not.Null);
//         List<NWDPlayerDataStorage> tReferenceList = NWDPlayerDataManager
//             .InsertOrUpdate(_EnvironmentKind, ProjectId, sRange, new List<NWDPlayerDataStorage>() { tStudioDataToUpdate });
//
//         //Assert
//         Assert.That(tReferenceList[0].Reference, Is.EqualTo(sReference));
//         Assert.That(
//             NWDPlayerDataManager.GetSingleByReference(_EnvironmentKind, ProjectId, tReferenceList[0].Reference, sRange)
//                 ?.Json, Is.EqualTo(tJson));
//
//     }
//
//     [TestCase(21UL , (ushort)1)]
//     [TestCase(22UL, (ushort)1)]
//     public void TestTrashStudioDataManager(ulong sReference, ushort sRange)
//     {
//         //SetUp
//         NWDPlayerDataStorage tStudioDataToTrashed = new NWDPlayerDataStorage()
//             { Reference = sReference, Json = "Test Trash", ProjectId = ProjectId };
//         NWDPlayerDataManager.InsertOrUpdate(_EnvironmentKind, ProjectId, sRange,new List<NWDPlayerDataStorage>() { tStudioDataToTrashed });
//
//         Assert.That(tStudioDataToTrashed.Trashed, Is.False);
//         Assert.That(
//             NWDPlayerDataManager.GetSingleByReference(_EnvironmentKind, tStudioDataToTrashed.ProjectId, sReference,sRange), Is.Not.Null);
//
//         //Process
//         NWDPlayerDataManager.TrashByReference(_EnvironmentKind, ProjectId, sRange, tStudioDataToTrashed);
//
//         //Assert
//         Assert.That(
//             NWDPlayerDataManager.GetSingleByReference(_EnvironmentKind, tStudioDataToTrashed.ProjectId, sReference, sRange)?.Trashed, Is.EqualTo(true));
//     }
//
//     [TestCase(31UL, (ushort)1)]
//     [TestCase(32UL, (ushort)1)]
//     public void TestDeleteStudioDataManager(ulong sReference, ushort sRange)
//     {
//         //SetUp
//         NWDPlayerDataStorage tStudioDataDelete = new NWDPlayerDataStorage()
//         {
//             Reference = sReference, Json = "Test Trash", ProjectId = ProjectId
//         };
//         NWDPlayerDataManager.InsertOrUpdate(_EnvironmentKind, ProjectId, sRange,new List<NWDPlayerDataStorage>() { tStudioDataDelete });
//
//         Assert.That(
//             NWDPlayerDataManager.GetSingleByReference(_EnvironmentKind, tStudioDataDelete.ProjectId, sReference, sRange), Is.Not.Null);
//
//         //Process
//         NWDPlayerDataManager.DeleteByReference(_EnvironmentKind, tStudioDataDelete.ProjectId, sRange, sReference);
//
//         //Assert
//         Assert.That(
//             NWDPlayerDataManager.GetSingleByReference(_EnvironmentKind, tStudioDataDelete.ProjectId, sReference, sRange), Is.Null);
//     }
//
//
//     [TestCase((ulong)41,  "Data Track 1 Test", (ushort)1)]
//     [TestCase((ulong)42,  "Data Track 2 Test", (ushort)1)]
//     public void TestGetDataForDataTrack(ulong sReference, string sJson, ushort sRange)
//     {
//         //SetUp
//         NWDPlayerDataStorage tPlayerData = new NWDPlayerDataStorage()
//         {
//             Reference = sReference, DataTrack = sDataTrack, Json = sJson, ProjectId = ProjectId
//         };
//         Assert.That(
//             NWDPlayerDataManager.GetSingleByReference(_EnvironmentKind, ProjectId, sReference, sRange),
//             Is.Null);
//
//         //Process
//         NWDPlayerDataManager.InsertOrUpdate(_EnvironmentKind, ProjectId, sRange,new List<NWDPlayerDataStorage>() { tPlayerData });
//
//         //Asert
//         Assert.That(
//             NWDPlayerDataManager.GetSingleByReference(_EnvironmentKind, ProjectId, sReference, sRange),
//             Is.Not.Null);
//     }
//
//     [TestCase((ulong)51 , 20UL,300UL,(ushort)1)]
//     [TestCase((ulong)52 , 52UL,401UL,(ushort)2)]
//     public void TestGetDataBySyncIndex(ulong sReference, ulong sSyncIndex, ulong sAccountReference,ushort sRange)
//     {
//         //SetUp
//         NWDPlayerDataStorage tPlayerData = new NWDPlayerDataStorage()
//         {
//             Reference = sReference,
//             Json = "Test Sync Index",
//             ProjectId = ProjectId,
//             Account = sAccountReference
//         };
//         //Process
//         NWDPlayerDataManager.InsertOrUpdate(_EnvironmentKind, ProjectId, sRange, new List<NWDPlayerDataStorage>() { tPlayerData });
//
//         //Assert
//         List<NWDPlayerDataStorage> tPlayerDatas =
//             NWDPlayerDataManager.GetBySync(_EnvironmentKind, ProjectId, sRange, sSyncIndex,sAccountReference);
//         Assert.That(tPlayerDatas.Count, Is.GreaterThan(0));
//
//         ulong tMaxSyncIndex = tPlayerDatas.Max(sPlayerData => sPlayerData.SyncIndex);
//
//         foreach (NWDPlayerDataStorage tPlayer in tPlayerDatas)
//         {
//             Assert.That(tPlayer.SyncIndex, Is.EqualTo(tMaxSyncIndex));
//         }
//
//     }
//
//     [OneTimeTearDown]
//     public void TearDown()
//     {
//         List<ushort> tRanges = new List<ushort>()
//             { 1, 2 };
//         foreach (ushort tUshort in tRanges)
//         {
//             List<NWDPlayerDataStorage> tStudioDatas =
//                 NWDPlayerDataManager.GetAllForProject(_EnvironmentKind, tUshort, ProjectId);
//             NWDPlayerDataManager.DeleteRangeByReferences(_EnvironmentKind, ProjectId, tUshort,
//                 tStudioDatas.Select(sStudio => sStudio.Reference).ToList());
//         }
//     }
// }