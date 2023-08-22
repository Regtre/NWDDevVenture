// using NWDCrucial.Exchanges.Payloads;
// using NWDCrucial.Models;
// using NWDEditor.Exchanges;
// using NWDEditor.Exchanges.Payloads;
// using NWDFoundation.Configuration.Environments;
// using NWDFoundation.Exchanges;
// using NWDFoundation.Logger;
// using NWDFoundation.Models;
// using NWDFoundation.Tools;
// using NWDHub.Models;
// using NWDServerMiddle.Managers.ModelManagers;
//
// namespace NWDServerTests.ProjectByEnvironment
// {
//     public class NWDTestProjectByEnvironment : NWDTestSetup
//     {
//         private static NWDEnvironmentKind EnvironmentKind = NWDEnvironmentKind.Dev;
//             
//         private static readonly NWDProjectByEnvironment ProjectToTest = new NWDProjectByEnvironment()
//         {
//             Reference = (ulong)NWDRandom.LongNumeric(14),
//             ProjectId = (uint)NWDRandom.Random(111,999),
//             TreatKey = NWDRandom.RandomStringToken(32),
//             ProjectKey = NWDRandom.RandomStringToken(32),
//             EnvironmentKind = EnvironmentKind,
//         };
//
//         private static readonly NWDProjectByEnvironment ProjectUpdatedToTest = new NWDProjectByEnvironment()
//         {
//             Reference = (ulong)NWDRandom.LongNumeric(14),
//             ProjectId = ProjectToTest.ProjectId,
//             TreatKey = NWDRandom.RandomStringToken(32) + "A",
//             ProjectKey = NWDRandom.RandomStringToken(32) + "B",
//             EnvironmentKind = EnvironmentKind,
//         };
//         
//         
//         [SetUp]
//         public void TestProjectSetUp()
//         {
//             NWDProjectCredentialManager.Delete(ProjectToTest, ProjectToTest.ProjectId);
//             NWDProjectCredentialManager.Delete(ProjectUpdatedToTest, ProjectUpdatedToTest.ProjectId);
//         }
//
//         [Test]
//         public void TestProjectPublicKey()
//         {
//             foreach (NWDEnvironmentKind tEnvironment in (NWDEnvironmentKind[])Enum.GetValues(typeof(NWDEnvironmentKind)))
//             {
//                 NWDProjectByEnvironment? tProject = NWDProjectCredentialManager.GetOneByProjectIdAndEnvironment(NWDSetup.ProjectId, tEnvironment);
//                 if (tProject != null)
//                 {
//                     if (NWDSetup.ProjectByEnvironments.ContainsKey(tEnvironment))
//                     {
//                         Assert.That(NWDSetup.ProjectByEnvironments[tEnvironment].ProjectKey,
//                             Is.EqualTo(tProject.ProjectKey));
//                     }
//                     else
//                     {
//                         Assert.Fail(nameof(NWDSetup.ProjectByEnvironments) + "doesn't contains " + tEnvironment.ToString() + " environment!");
//                     }
//                 }
//                 else
//                 {
//                     Assert.Fail("Database doesn't contains " + tEnvironment.ToString() + " environment! for project ID = " + NWDSetup.ProjectId);
//                 }
//             }
//         }
//
//         [Test]
//         public void TestProjectPrivateKey()
//         {
//             foreach (NWDEnvironmentKind tEnvironment in
//                      (NWDEnvironmentKind[])Enum.GetValues(typeof(NWDEnvironmentKind)))
//             {
//                 NWDProjectByEnvironment? tProject =
//                     NWDProjectCredentialManager.GetOneByProjectIdAndEnvironment(NWDSetup.ProjectId, tEnvironment);
//                 if (tProject != null)
//                 {
//                     if (NWDSetup.ProjectByEnvironments.ContainsKey(tEnvironment))
//                     {
//                         Assert.That(NWDSetup.ProjectByEnvironments[tEnvironment].TreatKey,
//                             Is.EqualTo(tProject.TreatKey));
//                     }
//                     else
//                     {
//                         Assert.Fail(nameof(NWDSetup.ProjectByEnvironments) + "doesn't contains " +
//                                     tEnvironment.ToString() + " environment!");
//                     }
//                 }
//                 else
//                 {
//                     Assert.Fail("Database doesn't contains " + tEnvironment.ToString() +
//                                 " environment! for project ID = " + NWDSetup.ProjectId);
//                 }
//             }
//         }
//
//         [Test]
//         public void TestProjectCreate()
//         {
//             NWDLogger.Trace("test " + nameof(TestProjectCreate) + " with " +
//                             nameof(NWDProjectByEnvironment.ProjectId) + " = " + ProjectToTest.ProjectId);
//             NWDUpPayloadProjectCreate tUpPayload = new NWDUpPayloadProjectCreate();
//             tUpPayload.ProjectByEnvironmentList.Add(ProjectToTest);
//             NWDResponseEditor tResponse = EditorManager.Process(new NWDRequestEditor(
//                 NWDExchangeEditorKind.ProjectCreate, tUpPayload,
//                 NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
//             Assert.IsTrue(tResponse.IsValid());
//             NWDDownPayloadProjectCreate tDownPayload = tResponse.GetPayload<NWDDownPayloadProjectCreate>();
//             Assert.NotNull(tDownPayload);
//             // test if Project is in database 
//             NWDProjectByEnvironment? tResult =
//                 NWDProjectCredentialManager.GetOneByProjectIdAndEnvironment(ProjectToTest.ProjectId,
//                     ProjectToTest.EnvironmentKind);
//             Assert.NotNull(tResult);
//             if (tResult != null)
//             {
//                 Assert.That(ProjectToTest.TreatKey, Is.EqualTo(tResult.TreatKey));
//                 Assert.That(ProjectToTest.ProjectKey, Is.EqualTo(tResult.ProjectKey));
//                 Assert.That(ProjectToTest.ProjectId, Is.EqualTo(tResult.ProjectId));
//                 Assert.That(ProjectToTest.EnvironmentKind, Is.EqualTo(tResult.EnvironmentKind));
//             }
//
//             List<NWDProjectByEnvironment> tResultList =
//                 NWDProjectCredentialManager.GetAllByProjectIdAndEnvironment(ProjectToTest.ProjectId,
//                     ProjectToTest.EnvironmentKind);
//             Assert.IsTrue(tResultList.Count == NWDProjectCredentialManager.DaoList.Count);
//             foreach (NWDProjectByEnvironment tEnv in tResultList)
//             {
//                 Assert.That(ProjectToTest.TreatKey, Is.EqualTo(tEnv.TreatKey));
//                 Assert.That(ProjectToTest.ProjectKey, Is.EqualTo(tEnv.ProjectKey));
//                 Assert.That(ProjectToTest.ProjectId, Is.EqualTo(tEnv.ProjectId));
//                 Assert.That(ProjectToTest.EnvironmentKind, Is.EqualTo(tEnv.EnvironmentKind));
//             }
//         }
//
//         [Test]
//         public void TestProjectUpdate()
//         {
//             TestProjectCreate();
//             NWDLogger.Trace("test " + nameof(TestProjectUpdate) + " with " +
//                             nameof(NWDProjectByEnvironment.ProjectId) + " = " + ProjectToTest.ProjectId);
//             NWDUpPayloadProjectCreate tUpPayload = new NWDUpPayloadProjectCreate();
//             tUpPayload.ProjectByEnvironmentList.Add(ProjectUpdatedToTest);
//             NWDResponseEditor tResponse = EditorManager.Process(new NWDRequestEditor(
//                 NWDExchangeEditorKind.ProjectUpdate, tUpPayload,
//                 NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
//             Assert.IsTrue(tResponse.IsValid());
//             NWDDownPayloadProjectUpdate tDownPayload = tResponse.GetPayload<NWDDownPayloadProjectUpdate>();
//             Assert.NotNull(tDownPayload);
//             // test if Project is in database 
//             NWDProjectByEnvironment? tResult =
//                 NWDProjectCredentialManager.GetOneByProjectIdAndEnvironment(ProjectToTest.ProjectId,
//                     ProjectToTest.EnvironmentKind);
//             Assert.NotNull(tResult);
//             if (tResult != null)
//             {
//                 Assert.That(ProjectUpdatedToTest.TreatKey, Is.EqualTo(tResult.TreatKey));
//                 Assert.That(ProjectUpdatedToTest.ProjectKey, Is.EqualTo(tResult.ProjectKey));
//                 Assert.That(ProjectToTest.ProjectId, Is.EqualTo(tResult.ProjectId));
//                 Assert.That(ProjectToTest.EnvironmentKind, Is.EqualTo(tResult.EnvironmentKind));
//             }
//
//             List<NWDProjectByEnvironment> tResultList =
//                 NWDProjectCredentialManager.GetAllByProjectIdAndEnvironment(ProjectToTest.ProjectId,
//                     ProjectToTest.EnvironmentKind);
//             Assert.IsTrue(tResultList.Count == NWDProjectCredentialManager.DaoList.Count);
//             foreach (NWDProjectByEnvironment tEnv in tResultList)
//             {
//                 Assert.That(ProjectUpdatedToTest.TreatKey, Is.EqualTo(tEnv.TreatKey));
//                 Assert.That(ProjectUpdatedToTest.ProjectKey, Is.EqualTo(tEnv.ProjectKey));
//                 Assert.That(ProjectToTest.ProjectId, Is.EqualTo(tEnv.ProjectId));
//                 Assert.That(ProjectToTest.EnvironmentKind, Is.EqualTo(tEnv.EnvironmentKind));
//             }
//         }
//
//         [Test]
//         public void TestProjectDelete()
//         {
//             TestProjectCreate();
//             NWDLogger.Trace("test " + nameof(TestProjectDelete) + " with " + nameof(NWDProjectByEnvironment.ProjectId) + " = " + ProjectToTest.ProjectId);
//             NWDUpPayloadProjectDelete tUpPayload = new NWDUpPayloadProjectDelete();
//             tUpPayload.ProjectByEnvironmentList.Add(ProjectToTest);
//             NWDResponseEditor tResponse = EditorManager.Process(new NWDRequestEditor(
//                 NWDExchangeEditorKind.ProjectDelete, tUpPayload,
//                 NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
//             Assert.IsTrue(tResponse.IsValid());
//             NWDDownPayloadProjectDelete tDownPayload = tResponse.GetPayload<NWDDownPayloadProjectDelete>();
//             Assert.NotNull(tDownPayload);
//             // test if Project is delete from database 
//             NWDProjectByEnvironment? tResult =
//                 NWDProjectCredentialManager.GetOneByProjectIdAndEnvironment(ProjectToTest.ProjectId,
//                     ProjectToTest.EnvironmentKind);
//             Assert.IsNull(tResult);
//             List<NWDProjectByEnvironment> tResultList =
//                 NWDProjectCredentialManager.GetAllByProjectIdAndEnvironment(ProjectToTest.ProjectId,
//                     ProjectToTest.EnvironmentKind);
//             Assert.IsTrue(tResultList.Count == 0);
//         }
//
//         [OneTimeTearDown]
//         public void TestProjectClean()
//         {
//             NWDProjectCredentialManager.Delete(ProjectToTest, ProjectToTest.ProjectId);
//             NWDProjectCredentialManager.Delete(ProjectUpdatedToTest, ProjectUpdatedToTest.ProjectId);
//         }
//     }
// }