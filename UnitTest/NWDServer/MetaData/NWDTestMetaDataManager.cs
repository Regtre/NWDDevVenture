using Newtonsoft.Json;
using NWDEditor;
using NWDFoundation.Models;
using NWDHub.Configuration;
using NWDWebEditor.Managers;
using NWDWebRuntime.Models;

namespace UnitTest.NWDServer.MetaData
{

    public class NWDTestMetaDataManager : NWDTestSetup
    {
    public class NWDStudioDataTest : NWDStudioData
    {
    }
        [Test]
        public void TestCreateMetaData()
        {
            NWDMetaData? rResult = new NWDMetaData();
            rResult.ProjectUniqueId = NWDHubConfiguration.KConfig.GetCrucialProjectId();
            rResult.ClassName = nameof(NWDStudioDataTest);
            rResult.DataByDataTrack = JsonConvert.SerializeObject(new NWDSubMetaData[0]);
            rResult = NWDWebDBDataManager.SaveData(rResult, true);
            
           Assert.IsNotNull( NWDWebDBDataManager.GetDataByReference<NWDMetaData>(rResult!.Reference));
            
        }
    }
}