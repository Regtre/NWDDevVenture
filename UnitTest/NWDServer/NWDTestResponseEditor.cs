// using NWDFoundation.Exchanges;
// using Newtonsoft.Json;
// using NWDEditor.Exchanges;
// using NWDFoundation.Configuration;
// using NWDFoundation.Tools;
// using NWDServerShared.Configuration;
//
// namespace NWDServerTests
// {
//     public class NWDTestResponseEditor : NWDTestSetup
//     {
//         [Test]
//         public void TestSerialization()
//         {
//             NWDResponseEditor tResponse = new NWDResponseEditor(NWDExchangeEditorKind.Unknown, null,
//                 NWDRequestStatus.Ok);
//             string tJsonA = JsonConvert.SerializeObject(tResponse);
//             // NWDLogger.WriteLine(tJsonA);
//             NWDResponseEditor? tResponseDeserialize = JsonConvert.DeserializeObject<NWDResponseEditor>(tJsonA);
//             string tJsonB = JsonConvert.SerializeObject(tResponseDeserialize);
//             // NWDLogger.WriteLine(tJsonB);
//             Assert.That(tJsonB, Is.EqualTo(tJsonA));
//         }
//
//         [Test]
//         public void TestIsValid()
//         {
//             NWDResponseEditor tResponse = new NWDResponseEditor(NWDExchangeEditorKind.Unknown, null,
//                 NWDRequestStatus.Ok);
//             Assert.IsTrue(tResponse.IsValid());
//         }
//
//         [Test]
//         public void TestIsValid_Hack()
//         {
//             NWDResponseEditor tResponse = new NWDResponseEditor(NWDExchangeEditorKind.Unknown, null,
//                 NWDRequestStatus.Ok);
//             tResponse.Payload = tResponse.Payload + "1";
//             Assert.IsFalse(tResponse.IsValid());
//         }
//
//         [Test]
//         public void TestIsValid_Hack_StudioKey()
//         {
//             string tOld = NWDEditorKeyManager.EditorManager.GetTreatKey();
//             NWDEditorKeyManager.EditorManager.SetTreatKey(NWDRandom.RandomStringAlpha(16));
//             NWDResponseEditor tResponse = new NWDResponseEditor(NWDExchangeEditorKind.Unknown, null, NWDRequestStatus.Ok);
//             NWDEditorKeyManager.EditorManager.SetTreatKey(tOld);
//             Assert.IsFalse(tResponse.IsValid());
//         }
//     }
// }