// using NWDFoundation.Exchanges;
// using Newtonsoft.Json;
// using NWDEditor.Exchanges;
// using NWDFoundation.Configuration;
// using NWDFoundation.Tools;
// using NWDServerShared.Configuration;
//
// namespace NWDServerTests
// {
//     public class NWDTestRequestEditor : NWDTestSetup
//     {
//         [Test]
//         public void TestSerialization()
//         {
//             NWDRequestEditor tRequestRuntime = new NWDRequestEditor(NWDExchangeEditorKind.Unknown, null,
//                 NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
//             string tJsonA = JsonConvert.SerializeObject(tRequestRuntime);
//             // NWDLogger.WriteLine(tJsonA);
//             NWDRequestEditor? tRequestDeserialize = JsonConvert.DeserializeObject<NWDRequestEditor>(tJsonA);
//             string tJsonB = JsonConvert.SerializeObject(tRequestDeserialize);
//             // NWDLogger.WriteLine(tJsonB);
//             Assert.That(tJsonB, Is.EqualTo(tJsonA));
//         }
//
//         [Test]
//         public void TestIsValid()
//         {
//             NWDRequestEditor tRequestRuntime = new NWDRequestEditor(NWDExchangeEditorKind.Unknown, null,
//                 NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
//             Assert.IsTrue(tRequestRuntime.IsValid());
//         }
//
//         [Test]
//         public void TestIsValid_Hack()
//         {
//             NWDRequestEditor tRequestRuntime = new NWDRequestEditor(NWDExchangeEditorKind.Unknown, null,
//                 NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
//             tRequestRuntime.Payload = tRequestRuntime.Payload + "1";
//             Assert.IsFalse(tRequestRuntime.IsValid());
//         }
//         
//         [Test]
//         public void TestIsValid_Hack_StudioKey()
//         {
//             string tOld = NWDEditorKeyManager.EditorManager.GetTreatKey();
//             NWDEditorKeyManager.EditorManager.SetTreatKey(NWDRandom.RandomStringAlpha(16));
//             NWDRequestEditor tRequestEditor = new NWDRequestEditor(NWDExchangeEditorKind.Unknown, null, NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
//             NWDEditorKeyManager.EditorManager.SetTreatKey(tOld);
//             Assert.IsFalse(tRequestEditor.IsValid());
//         }
//     }
// }