using NUnit.Framework;
using NWDFoundation.Config;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Exchanges;
using NWDUnityEditor.Managers;
using NWDUnityShared.Engine;
using NWDUnityShared.Managers;
using NWDUnityTests.Config;
using NWDUnityTests.Engine;
using NWDUnityTests.Initializer;
using NWDUnityTests.Manager;

namespace NWDUnityTests.F1_Accounts
{
    [SetUpFixture]
    public class NWDAccountTestsSetUp
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            INWDConfig tActualConfig = NWDUnityEngine.Instance.Config;

            string tDefaultWebEditor = tActualConfig.GetDefaultWebEditor();
            string tWebEditor = tActualConfig.WebEditorURL();
            NWDExchangeDevice tDeviceOs = tActualConfig.GetDeviceOS();
            ulong tProjectId = tActualConfig.GetProjectId();
            string tProjectKeyName = tActualConfig.GetProjectKeyInstanceName();
            NWDDataTrackDescription tDataTrack = tActualConfig.GetSelectedEnvironment();
            string tProjectKey = tActualConfig.GetProjectKey(tProjectId, tDataTrack.Kind);

            NWDConfigUnityTests tConfig = new NWDConfigUnityTests(tDefaultWebEditor, tWebEditor, tDeviceOs, tProjectId, tProjectKeyName, tProjectKey, tDataTrack);
            NWDUnityAccountManager tAccountManager = new NWDUnityAccountManager();
            NWDUnityTestsDataManager tDataManager = new NWDUnityTestsDataManager();
            NWDUnityTestsDeviceManager tDeviceManager = new NWDUnityTestsDeviceManager();
            NWDUnityTestsEnvironmentManager tEnvironmentManager = new NWDUnityTestsEnvironmentManager();
            NWDUnityEditorThreadManager tAsyncManager = new NWDUnityEditorThreadManager();

            NWDEngineUnityTests tEngine = new NWDEngineUnityTests(tConfig, tAccountManager, tDataManager, tDeviceManager, tEnvironmentManager, tAsyncManager);
            NWDEngineInitializerTests.StartTests(tEngine);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            NWDEngineInitializerTests.StopTests();
        }
    }
}
