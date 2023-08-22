// using NWDFoundation.Exchanges;
// using NWDEditor.Exchanges;
// using NWDHub.Managers;
//
//
// namespace NWDServerTests.ServerRequestManager.Project
// {
//     public class NWDTestProjectCreate : NWDTestSetup
//     {
//         private static readonly NWDEditorManager KEditorManager = new NWDEditorManager();
//         [Test]
//         public void TestRequestBasic()
//         {
//             NWDRequestEditor tRequest = new NWDRequestEditor(NWDExchangeEditorKind.Unknown, null, NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
//             Assert.IsTrue(tRequest.IsValid());
//         }
//         
//         [Test]
//         public void TestRequestIsValid()
//         {
//             NWDRequestEditor tRequest = new NWDRequestEditor(NWDExchangeEditorKind.Unknown, null, NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
//             Assert.IsTrue(tRequest.IsValid());
//         }
//         
//         [Test]
//         public void TestRequestIsValidStudio()
//         {
//             NWDRequestEditor tRequest = new NWDRequestEditor(NWDExchangeEditorKind.Unknown, null, NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
//             Assert.IsTrue(tRequest.IsValid());
//         }
//         
//         [Test]
//         public void TestRequest_CreateProject()
//         {
//             // test request for new Project ID
//             NWDRequestEditor tRequest = new NWDRequestEditor(NWDExchangeEditorKind.ProjectCreate ,null, NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
//             NWDResponseEditor tResponse = KEditorManager.Process(tRequest);
//             Assert.IsTrue(tResponse.IsValid());
//             //TODO : test if project was created!? 
//         }
//     }
// }