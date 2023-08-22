namespace UnitTest.NWDServer.Service
{
    // public class NWDTestService : NWDTestSetup
    // {
    //     private static readonly NWDService ServiceToTest = new NWDService()
    //     {
    //         Reference = (ulong)NWDRandom.LongNumeric(14),
    //         GetProjectId = (uint)NWDRandom.Random(111,999),
    //         ServiceId = (ushort)NWDRandom.Random(111,999),
    //         ServiceName = NWDRandom.RandomStringToken(8),
    //         OfflineUsage = NWDServiceOfflineUsage.OffLineUnlimited,
    //         OfflineCounterReserve = 0,
    //         ServiceKind = NWDServiceKind.Cookie,
    //     };
    //
    //     private static readonly NWDService ServiceUpdatedToTest = new NWDService()
    //     {
    //         Reference = (ulong)NWDRandom.LongNumeric(14),
    //         GetProjectId = ServiceToTest.GetProjectId, ServiceId = ServiceToTest.ServiceId,
    //         ServiceName = NWDRandom.RandomStringToken(8) + "B",
    //         OfflineUsage = NWDServiceOfflineUsage.OffLineLimited,
    //         OfflineCounterReserve = 10000,
    //         ServiceKind = NWDServiceKind.Session,
    //     };
    //
    //     [Test]
    //     public void TestServiceCreate()
    //     {
    //         NWDLogger.WriteLine("test " + nameof(TestServiceCreate) + " with " +
    //                           nameof(NWDService.GetProjectId) + " = " + ServiceToTest.GetProjectId + " " +
    //                           nameof(NWDService.Reference) + " = " + ServiceToTest.Reference 
    //                           );
    //         NWDUpPayloadServiceCreate tUpPayload = new NWDUpPayloadServiceCreate();
    //         tUpPayload.ServiceList.Add(ServiceToTest);
    //         NWDResponseEditor tResponse = EditorManager.Process(new NWDRequestEditor(
    //             NWDExchangeEditorKind.ServiceCreate, tUpPayload,
    //             NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
    //         Assert.IsTrue(tResponse.IsValid());
    //         NWDDownPayloadServiceCreate tDownPayload = tResponse.GetPayload<NWDDownPayloadServiceCreate>();
    //         Assert.NotNull(tDownPayload);
    //         // test if Project is in database 
    //         NWDService? tResult =
    //             NWDServiceManager.GetOneByProjectIdAndEnvironment(ServiceToTest.GetProjectId,
    //                 ServiceToTest.ServiceId,
    //                 ServiceToTest.Environment);
    //         Assert.NotNull(tResult);
    //         if (tResult != null)
    //         {
    //             Assert.That(ServiceToTest.ServiceName, Is.EqualTo(tResult.ServiceName));
    //             Assert.That(ServiceToTest.OfflineUsage, Is.EqualTo(tResult.OfflineUsage));
    //             Assert.That(ServiceToTest.OfflineCounterReserve, Is.EqualTo(tResult.OfflineCounterReserve));
    //             Assert.That(ServiceToTest.ServiceKind, Is.EqualTo(tResult.ServiceKind));
    //             Assert.That(ServiceToTest.GetProjectId, Is.EqualTo(tResult.GetProjectId));
    //             Assert.That(ServiceToTest.Environment, Is.EqualTo(tResult.Environment));
    //         }
    //
    //         List<NWDService> tResultList =
    //             NWDServiceManager.GetAllByProjectIdAndEnvironment(ServiceToTest.GetProjectId,
    //                 ServiceToTest.Environment);
    //         Assert.IsTrue(tResultList.Count == NWDServiceManager.DaoList.Count);
    //         foreach (NWDService tEnv in tResultList)
    //         {
    //             Assert.That(ServiceToTest.ServiceName, Is.EqualTo(tEnv.ServiceName));
    //             Assert.That(ServiceToTest.OfflineUsage, Is.EqualTo(tEnv.OfflineUsage));
    //             Assert.That(ServiceToTest.OfflineCounterReserve, Is.EqualTo(tEnv.OfflineCounterReserve));
    //             Assert.That(ServiceToTest.ServiceKind, Is.EqualTo(tEnv.ServiceKind));
    //             Assert.That(ServiceToTest.GetProjectId, Is.EqualTo(tEnv.GetProjectId));
    //             Assert.That(ServiceToTest.Environment, Is.EqualTo(tEnv.Environment));
    //         }
    //     }
    //
    //     [Test]
    //     public void TestServiceUpdate()
    //     {
    //         TestServiceCreate();
    //         NWDLogger.WriteLine("test " + nameof(TestServiceUpdate) + " with " +
    //                           nameof(NWDService.GetProjectId) + " = " + ServiceToTest.GetProjectId+ " " +
    //                           nameof(NWDService.Reference) + " = " + ServiceToTest.Reference );
    //         NWDUpPayloadServiceCreate tUpPayload = new NWDUpPayloadServiceCreate();
    //         tUpPayload.ServiceList.Add(ServiceUpdatedToTest);
    //         NWDResponseEditor tResponse = EditorManager.Process(new NWDRequestEditor(
    //             NWDExchangeEditorKind.ServiceUpdate, tUpPayload,
    //             NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
    //         Assert.IsTrue(tResponse.IsValid());
    //         NWDDownPayloadServiceUpdate tDownPayload = tResponse.GetPayload<NWDDownPayloadServiceUpdate>();
    //         Assert.NotNull(tDownPayload);
    //         // test if Project is in database 
    //         NWDService? tResult =
    //             NWDServiceManager.GetOneByProjectIdAndEnvironment(ServiceToTest.GetProjectId,
    //                 ServiceToTest.ServiceId,
    //                 ServiceToTest.Environment);
    //         Assert.NotNull(tResult);
    //         if (tResult != null)
    //         {
    //             Assert.That(ServiceUpdatedToTest.ServiceName, Is.EqualTo(tResult.ServiceName));
    //             Assert.That(ServiceUpdatedToTest.OfflineUsage, Is.EqualTo(tResult.OfflineUsage));
    //             Assert.That(ServiceUpdatedToTest.OfflineCounterReserve, Is.EqualTo(tResult.OfflineCounterReserve));
    //             Assert.That(ServiceUpdatedToTest.ServiceKind, Is.EqualTo(tResult.ServiceKind));
    //             Assert.That(ServiceToTest.GetProjectId, Is.EqualTo(tResult.GetProjectId));
    //             Assert.That(ServiceToTest.Environment, Is.EqualTo(tResult.Environment));
    //         }
    //
    //         List<NWDService> tResultList =
    //             NWDServiceManager.GetAllByProjectIdAndEnvironment(ServiceToTest.GetProjectId,
    //                 ServiceToTest.Environment);
    //         Assert.IsTrue(tResultList.Count == NWDServiceManager.DaoList.Count);
    //         foreach (NWDService tEnv in tResultList)
    //         {
    //             Assert.That(ServiceUpdatedToTest.ServiceName, Is.EqualTo(tEnv.ServiceName));
    //             Assert.That(ServiceUpdatedToTest.OfflineUsage, Is.EqualTo(tEnv.OfflineUsage));
    //             Assert.That(ServiceUpdatedToTest.OfflineCounterReserve, Is.EqualTo(tEnv.OfflineCounterReserve));
    //             Assert.That(ServiceUpdatedToTest.ServiceKind, Is.EqualTo(tEnv.ServiceKind));
    //             Assert.That(ServiceToTest.GetProjectId, Is.EqualTo(tEnv.GetProjectId));
    //             Assert.That(ServiceToTest.Environment, Is.EqualTo(tEnv.Environment));
    //         }
    //     }
    //
    //     [Test]
    //     public void TestServiceDelete()
    //     {
    //         TestServiceCreate();
    //         NWDLogger.WriteLine("test " + nameof(TestServiceDelete) + " with " + 
    //                           nameof(NWDService.GetProjectId) + " = " + ServiceToTest.GetProjectId+ " " +
    //                           nameof(NWDService.Reference) + " = " + ServiceToTest.Reference );
    //         NWDUpPayloadServiceDelete tUpPayload = new NWDUpPayloadServiceDelete();
    //         tUpPayload.ServiceList.Add(ServiceToTest);
    //         NWDResponseEditor tResponse = EditorManager.Process(new NWDRequestEditor(
    //             NWDExchangeEditorKind.ServiceDelete, tUpPayload,
    //             NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
    //         Assert.IsTrue(tResponse.IsValid());
    //         NWDDownPayloadServiceDelete tDownPayload = tResponse.GetPayload<NWDDownPayloadServiceDelete>();
    //         Assert.NotNull(tDownPayload);
    //         // test if Project is delete from database 
    //         NWDService? tResult =
    //             NWDServiceManager.GetOneByProjectIdAndEnvironment(ServiceToTest.GetProjectId,
    //                 ServiceToTest.ServiceId,
    //                 ServiceToTest.Environment);
    //         Assert.IsNull(tResult);
    //         List<NWDService> tResultList =
    //             NWDServiceManager.GetAllByProjectIdAndEnvironment(ServiceToTest.GetProjectId,
    //                 ServiceToTest.Environment);
    //         Assert.IsTrue(tResultList.Count == 0);
    //     }
    //
    //     [OneTimeTearDown]
    //     public void TestProjectClean()
    //     {
    //         NWDServiceManager.Delete(ServiceToTest); 
    //         NWDServiceManager.Delete(ServiceUpdatedToTest);
    //     }
    // }
}