// using Newtonsoft.Json;
// using NWDCrucial.Exchanges.Payloads;
// using NWDEditor.Exchanges;
// using NWDEditor.Exchanges.Payloads;
// using NWDFoundation.Configuration.Environments;
// using NWDFoundation.Exchanges;
// using NWDFoundation.Logger;
// using NWDFoundation.Models;
// using NWDFoundation.Tools;
// using NWDServerMiddle.Managers.ModelManagers;
//
//
// namespace NWDServerTests;
//
// public class NWDTestEditorManager : NWDTestSetup
// {
//     private const NWDEnvironmentKind K_EnvironmentKind = NWDEnvironmentKind.Dev;
//     private static readonly ulong ProjectId = NWDSetup.ProjectId;
//
//     private static NWDRequestPlayerToken? _PlayerToken = new NWDRequestPlayerToken();
//     private NWDAccountSign NewFakeAccountSign()
//     {
//         uint _Counter = 0;
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
//     [TestCase(10UL,11UL,12UL,13UL)]
//     public void TestPublishDataRequest(params ulong[] sReferences)
//     {
//         NWDUpPayloadPublishStudioData tPayloadPublishStudioData = new NWDUpPayloadPublishStudioData();
//         tPayloadPublishStudioData.StudioDataList = new List<NWDStudioDataStorage>();
//         foreach (ulong tReference in sReferences)
//         {
//             NWDStudioDataStorage tStudioData = new NWDStudioDataStorage()
//             {
//                 ProjectId = ProjectId,
//                 Json = "Test Publish Data",
//                 Reference = tReference,
//                 DataTrack = 0
//             };
//             tPayloadPublishStudioData.StudioDataList.Add(tStudioData);
//         }
//         
//         NWDRequestEditor tRequestEditor = new NWDRequestEditor()
//         {
//             Kind = NWDExchangeEditorKind.PublishStudioData,
//             Payload = JsonConvert.SerializeObject(tPayloadPublishStudioData),
//         };
//         tRequestEditor.Secure(NWDRandom.RandomStringAlpha(12));
//         Assert.That(tRequestEditor.IsValid(), Is.True);
//
//         NWDResponseEditor tResponseEditor = EditorController.Post(tRequestEditor); 
//         NWDDownPayloadPublishStudioData? tPayload = JsonConvert.DeserializeObject<NWDDownPayloadPublishStudioData>(tResponseEditor.Payload);
//         Assert.That(tResponseEditor.Status,Is.EqualTo(NWDRequestStatus.Ok));
//
//
//         foreach (ulong tReference in sReferences)
//         {
//             Assert.That(NWDStudioDataManager.GetSingleByReference(NWDEnvironmentKind.Production,ProjectId,tReference),Is.Not.Null);
//         }
//     }
// }