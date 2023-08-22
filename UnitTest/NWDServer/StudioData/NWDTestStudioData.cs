// using NWDFoundation.Configuration.Environments;
// using NWDFoundation.Models;
// using NWDDevServerFrontBack.Tools;
// using NWDServerMiddle.Managers.ModelManagers;
//
//
// namespace NWDServerTests.StudioData;
//
// [TestFixture(NWDEnvironmentKind.Dev)]
// [TestFixture(NWDEnvironmentKind.PlayTest)]
// public class NWDTestStudioData : NWDTestSetup
// {
//     private static NWDEnvironmentKind _EnvironmentKind;
//     private static readonly ulong ProjectId = NWDSetup.ProjectId;
//
//     public NWDTestStudioData(NWDEnvironmentKind sEnvironmentKind)
//     {
//         _EnvironmentKind = sEnvironmentKind;
//     }
//
//     [TestCase(10UL)]
//     public void TestInsertStudioDataManager_Insert(ulong sReference)
//     {
//         int tCountBeforeInsert = NWDStudioDataManager.GetAllForProject(_EnvironmentKind, NWDSetup.ProjectId).Count;
//
//         NWDStudioDataStorage tStudioDataToCreate = new NWDStudioDataStorage
//         {
//             ProjectId = NWDSetup.ProjectId, 
//             Json = "Test",
//             Reference = sReference,
//         };
//         NWDStudioDataManager.InsertOrUpdate(_EnvironmentKind,ProjectId,new List<NWDStudioDataStorage>(){tStudioDataToCreate});
//         Assert.That(NWDStudioDataManager.GetAllForProject(_EnvironmentKind, NWDSetup.ProjectId).Count, Is.EqualTo(tCountBeforeInsert+1));
//     }
//
//     [TestCase(20UL)]
//     [TestCase(25UL)]
//     public void TestInsertStudioDataManager_Update(ulong sReference)
//     {
//         NWDStudioDataStorage tStudioDataToUpdate = new NWDStudioDataStorage  
//         {
//            ProjectId = NWDSetup.ProjectId, 
//             Json = "Test",
//             Reference = sReference,
//         };
//         NWDStudioDataManager.InsertOrUpdate(_EnvironmentKind,ProjectId,new List<NWDStudioDataStorage>(){tStudioDataToUpdate});
//
//         string tJson = "Test Update "+TimeTools.GetNowTimestampUnix();
//         tStudioDataToUpdate.Json = tJson;
//         
//         NWDStudioDataManager.InsertOrUpdate(_EnvironmentKind,ProjectId,new List<NWDStudioDataStorage>(){tStudioDataToUpdate});
//         Assert.That(tStudioDataToUpdate.Reference, Is.EqualTo(sReference));
//         Assert.That(NWDStudioDataManager.GetSingleByReference(_EnvironmentKind,ProjectId, sReference)?.Json, Is.EqualTo(tJson));
//     }
//
//     [TestCase(30UL)]
//     public void TestTrashStudioDataManager(ulong sReference)
//     {
//         NWDStudioDataStorage tStudioDataToTrashed = new  NWDStudioDataStorage     
//         {
//                 ProjectId = NWDSetup.ProjectId, 
//             Json = "Test",
//             Reference = sReference,
//         };
//         Assert.That(tStudioDataToTrashed.Trashed, Is.False);
//         NWDStudioDataManager.InsertOrUpdate(_EnvironmentKind,ProjectId,new List<NWDStudioDataStorage>(){tStudioDataToTrashed});
//         
//         NWDStudioDataManager.Trash(_EnvironmentKind,ProjectId,tStudioDataToTrashed);
//         
//         Assert.That(NWDStudioDataManager.GetSingleByReference(_EnvironmentKind,ProjectId,tStudioDataToTrashed.Reference)?.Trashed,Is.EqualTo(true));
//     }
//
//     [TestCase(50Ul)]
//     public void TestDeleteStudioDataManager(ulong sReference)
//     {
//         NWDStudioDataStorage tStudioDataToDelete = new NWDStudioDataStorage
//         {
//             ProjectId = NWDSetup.ProjectId, 
//             Json = "Test Delete",
//             Reference = sReference,
//         };
//         NWDStudioDataManager.InsertOrUpdate(_EnvironmentKind, ProjectId, new List<NWDStudioDataStorage>(){tStudioDataToDelete});
//         Assert.That(NWDStudioDataManager.GetSingleByReference(_EnvironmentKind,ProjectId,sReference), Is.Not.Null);
//         NWDStudioDataManager.DeleteByReference(_EnvironmentKind,ProjectId,sReference);
//         Assert.That(NWDStudioDataManager.GetSingleByReference(_EnvironmentKind,ProjectId,sReference), Is.Null);
//     }
//
//     [TestCase(60UL)]
//     public void TestGetDataForDataTrack(ulong sReference)
//     {
//         ushort tDataTrack = 1;
//         NWDStudioDataStorage tStudioData = new NWDStudioDataStorage()
//         {
//             Reference = sReference, DataTrack = tDataTrack, Json = "Data Track Test", ProjectId = NWDSetup.ProjectId
//         };
//         Assert.That(NWDStudioDataManager.GetSingleForProjectAndDataTrackAndReference(_EnvironmentKind, NWDSetup.ProjectId, tDataTrack, sReference), Is.Null);
//
//         NWDStudioDataManager.InsertOrUpdate(_EnvironmentKind, NWDSetup.ProjectId, new List<NWDStudioDataStorage>() { tStudioData
//     });
//
//     Assert.That(NWDStudioDataManager.GetSingleForProjectAndDataTrackAndReference(_EnvironmentKind,NWDSetup.ProjectId,tDataTrack,sReference), Is.Not.Null);
//     }
//     
//     [TestCase((ulong)71,(ushort)1,(ushort)2,20UL)]
//     [TestCase((ulong)72,(ushort)2,(ushort)7,52UL)]
//     public void TestGetDataBySyncIndex(ulong sReference,ushort sRange,ushort sDataTrack, ulong sSyncIndex)
//     {
//         //SetUp
//         NWDStudioDataStorage tStudioData = new NWDStudioDataStorage()
//         {
//             Reference = sReference, DataTrack = sDataTrack, Json ="Test Sync Index" , ProjectId = ProjectId, SyncIndex = sSyncIndex+3
//         };
//         //Process
//         NWDStudioDataManager.InsertOrUpdate(_EnvironmentKind, ProjectId,new List<NWDStudioDataStorage>(){tStudioData});
//         
//         //Assert
//         List<NWDStudioDataStorage> tStudioDatas =
//             NWDStudioDataManager.GetBySync(_EnvironmentKind, ProjectId, sSyncIndex);
//         Assert.That(tStudioDatas.Count, Is.GreaterThan(0));
//
//         ulong  tMaxSyncIndex = tStudioDatas.Max(sPlayerData => sPlayerData.SyncIndex); 
//         
//         foreach (NWDStudioDataStorage tStudio in tStudioDatas)
//         {
//             Assert.That(tStudio.SyncIndex, Is.EqualTo(tMaxSyncIndex));
//         }
//         
//     }
//
//     [TestCase(80UL,82UL)]
//     public void TestPublish(ulong sReference1, ulong sReference2)
//     {
//         List<NWDStudioDataStorage> tStudioDatas = new List<NWDStudioDataStorage>()
//         {
//              new NWDStudioDataStorage() { Reference = sReference1, DataTrack = 1, Json = "Data Publish", ProjectId = NWDSetup.ProjectId },
//              new NWDStudioDataStorage() { Reference = sReference2, DataTrack = 1, Json = "Data Publish", ProjectId = NWDSetup.ProjectId }
//         };
//       
//         NWDStudioDataManager.Publish(NWDSetup.ProjectId,tStudioDatas);
//         
//         Assert.That(NWDStudioDataManager.GetSingleByReference(NWDEnvironmentKind.Production,NWDSetup.ProjectId,sReference1), Is.Not.Null);
//         Assert.That(NWDStudioDataManager.GetSingleByReference(NWDEnvironmentKind.Production,NWDSetup.ProjectId,sReference2), Is.Not.Null);
//  
//     }
//     
//     [OneTimeTearDown]
//     public void TearDown()
//     {
//         List<NWDStudioDataStorage> tStudioDatas = NWDStudioDataManager.GetAllForProject(_EnvironmentKind,NWDSetup.ProjectId);
//         NWDStudioDataManager.DeleteRangeByReferences(_EnvironmentKind,NWDSetup.ProjectId,tStudioDatas.Select(sStudio => sStudio.Reference ).ToList());
//         List<NWDStudioDataStorage> tStudioDatasProd = NWDStudioDataManager.GetAllForProject(NWDEnvironmentKind.Production,NWDSetup.ProjectId);
//         NWDStudioDataManager.DeleteRangeByReferences(NWDEnvironmentKind.Production,NWDSetup.ProjectId,tStudioDatasProd.Select(sStudio => sStudio.Reference ).ToList());
//     }
//     
//
// }