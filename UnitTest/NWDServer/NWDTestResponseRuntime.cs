using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;

namespace UnitTest.NWDServer
{
    public class NWDTestResponseRuntime : NWDTestSetup
    {
        [Test]
        public void TestSerialization()
        {
            NWDResponseRuntime tResponseRuntime = new NWDResponseRuntime(NWDHubConfiguration.KConfig, GetPlayerToken(NWDExchangeOrigin.Web), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Ok);
            string tJsonA = JsonConvert.SerializeObject(tResponseRuntime);
            // NWDLogger.WriteLine(tJsonA);
            NWDResponseRuntime? tResponseDeserialize = JsonConvert.DeserializeObject<NWDResponseRuntime>(tJsonA);
            string tJsonB = JsonConvert.SerializeObject(tResponseDeserialize);
            // NWDLogger.WriteLine(tJsonB);
            Assert.That(tJsonB, Is.EqualTo(tJsonA));
        }

        [Test]
        public void TestIsValid()
        {
            NWDResponseRuntime tResponseRuntime = new NWDResponseRuntime(NWDHubConfiguration.KConfig, GetPlayerToken(NWDExchangeOrigin.Web), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Ok);
            Assert.IsTrue(tResponseRuntime.IsValid(NWDHubConfiguration.KConfig));
        }
        
        [Test]
        public void TestIsValid_Hack()
        {
            NWDResponseRuntime tResponseRuntime = new NWDResponseRuntime(NWDHubConfiguration.KConfig, GetPlayerToken(NWDExchangeOrigin.Web), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Ok);
            tResponseRuntime.Payload = tResponseRuntime.Payload + "1";
            Assert.IsFalse(tResponseRuntime.IsValid(NWDHubConfiguration.KConfig));
        }
        
        [Test]
        public void TestIsValid_Hack_Player()
        {
            NWDResponseRuntime tResponseRuntime = new NWDResponseRuntime(NWDHubConfiguration.KConfig, GetPlayerToken(NWDExchangeOrigin.Web), NWDExchangeRuntimeKind.Unknown, null,
                NWDRequestStatus.Ok);
            tResponseRuntime.PlayerToken = GetPlayerToken(NWDExchangeOrigin.Web);
            Assert.IsFalse(tResponseRuntime.IsValid(NWDHubConfiguration.KConfig));
        }
    }
}