// using NWDEditor.Exchanges;
// using NWDFoundation.Exchanges;
// using NWDFoundation.Logger;
// using NWDRuntime.Exchanges;
// using NWDServerFront.Controllers;
// using NWDServerMiddle.Models;
// using NWDServerShared.Configuration;
//
//
// namespace NWDServerTests;
//
// public class NWDTestServerStatus : NWDTestSetup
// {
//     [Test]
//     public void TestEnableDisableWithInterception()
//     {
//         
//         NWDOnOffController tOnOffController = new NWDOnOffController();
//         NWDOnOffRequest tRequestOn = new NWDOnOffRequest() { OnOff = NWDOnOffStatus.On, Key = NWDAdminConfiguration.KConfig.MonitoringSecretKey};
//         NWDOnOffRequest tRequestOff = new NWDOnOffRequest() { OnOff = NWDOnOffStatus.Off , Key = NWDAdminConfiguration.KConfig.MonitoringSecretKey};
//         
//         NWDLogger.Trace(NWDConfigurationServer.KConfig.Status.ToString());
//         
//         NWDResponseEditor tResponseEditor = EditorManager.Process(new NWDRequestEditor(NWDExchangeEditorKind.Test, null,
//             NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
//         Assert.That(tResponseEditor.Kind, Is.EqualTo(NWDExchangeEditorKind.Test));
//         Assert.That(tResponseEditor.Status, Is.EqualTo(NWDRequestStatus.Ok));
//         
//         NWDResponseRuntime  tResponseRuntime = RuntimeController.Post(new NWDRequestRuntime(NWDSetup.ProjectId, NWDTestSetup.GetPlayerToken(),NWDExchangeRuntimeKind.Test, null,
//             NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
//         Assert.That(tResponseRuntime.RuntimeKind, Is.EqualTo(NWDExchangeRuntimeKind.Test));
//         Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
//         
//         NWDLogger.Trace(NWDConfigurationServer.KConfig.Status.ToString());
//
//         tOnOffController.Post(tRequestOff);
//         
//         NWDLogger.Trace(NWDConfigurationServer.KConfig.Status.ToString());
//         
//         tResponseEditor = EditorManager.Process(new NWDRequestEditor(NWDExchangeEditorKind.Test, null,
//             NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
//         Assert.That(tResponseEditor.Kind, Is.EqualTo(NWDExchangeEditorKind.ServerIsDisabled));
//         Assert.That(tResponseEditor.Status, Is.EqualTo(NWDRequestStatus.Error));
//         
//         tResponseRuntime = RuntimeController.Post(new NWDRequestRuntime(NWDSetup.ProjectId, NWDTestSetup.GetPlayerToken(),NWDExchangeRuntimeKind.Test, null,
//             NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
//         Assert.That(tResponseRuntime.RuntimeKind, Is.EqualTo(NWDExchangeRuntimeKind.ServerIsDisabled));
//         Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
//         
//         NWDLogger.Trace(NWDConfigurationServer.KConfig.Status.ToString());
//         
//         tOnOffController.Post(tRequestOn);
//         
//         NWDLogger.Trace(NWDConfigurationServer.KConfig.Status.ToString());
//         
//         tResponseEditor = EditorManager.Process(new NWDRequestEditor(NWDExchangeEditorKind.Test, null,
//             NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
//         Assert.That(tResponseEditor.Kind, Is.EqualTo(NWDExchangeEditorKind.Test));
//         Assert.That(tResponseEditor.Status, Is.EqualTo(NWDRequestStatus.Ok));
//         
//         tResponseRuntime = RuntimeController.Post(new NWDRequestRuntime(NWDSetup.ProjectId, NWDTestSetup.GetPlayerToken(),NWDExchangeRuntimeKind.Test, null,
//             NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown));
//         Assert.That(tResponseRuntime.RuntimeKind, Is.EqualTo(NWDExchangeRuntimeKind.Test));
//         Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
//         
//         NWDLogger.Trace(NWDConfigurationServer.KConfig.Status.ToString());
//     }
// }