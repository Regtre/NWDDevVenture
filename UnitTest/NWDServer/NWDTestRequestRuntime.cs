using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;
using NWDWebRuntime.Configuration;

namespace UnitTest.NWDServer
{
    public class NWDTestRequestRuntime : NWDTestSetup
    {
        [Test]
        public void TestSerialization()
        {
            NWDRequestRuntime tRequestRuntime = new NWDRequestRuntime(NWDWebRuntimeConfiguration.KConfig, GetPlayerToken(NWDExchangeOrigin.Web), NWDExchangeRuntimeKind.Unknown, null,
                NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
            string tJsonA = JsonConvert.SerializeObject(tRequestRuntime);
            // NWDLogger.WriteLine(tJsonA);
            NWDRequestRuntime? tRequestDeserialize = JsonConvert.DeserializeObject<NWDRequestRuntime>(tJsonA);
            string tJsonB = JsonConvert.SerializeObject(tRequestDeserialize);
            // NWDLogger.WriteLine(tJsonB);
            Assert.That(tJsonB, Is.EqualTo(tJsonA));
        }

        [Test]
        public void TestIsValid()
        {
            NWDRequestRuntime tRequestRuntime = new NWDRequestRuntime(NWDWebRuntimeConfiguration.KConfig, GetPlayerToken(NWDExchangeOrigin.Web), NWDExchangeRuntimeKind.Unknown, null,
                NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
            Assert.IsTrue(tRequestRuntime.IsValid(NWDWebRuntimeConfiguration.KConfig));
        }

        [Test]
        public void TestIsValid_Hack()
        {
            NWDRequestRuntime tRequestRuntime = new NWDRequestRuntime(NWDWebRuntimeConfiguration.KConfig, GetPlayerToken(NWDExchangeOrigin.Web), NWDExchangeRuntimeKind.Unknown, null,
                NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
            tRequestRuntime.Payload = tRequestRuntime.Payload + "1";
            Assert.IsFalse(tRequestRuntime.IsValid(NWDWebRuntimeConfiguration.KConfig));
        }
        
        [Test]
        public void TestIsValid_Hack_Player()
        {
            NWDRequestRuntime tRequestRuntime = new NWDRequestRuntime(NWDWebRuntimeConfiguration.KConfig, GetPlayerToken(NWDExchangeOrigin.Web), NWDExchangeRuntimeKind.Unknown, null,
                NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
            tRequestRuntime.PlayerToken = GetPlayerToken(NWDExchangeOrigin.Web);
            Assert.IsFalse(tRequestRuntime.IsValid(NWDWebRuntimeConfiguration.KConfig));
        }
        
        [Test]
        public void TestIsValid_Hack_PublicKey()
        {
            NWDRequestRuntime tRequestRuntime = new NWDRequestRuntime(NWDWebRuntimeConfiguration.KConfig, GetPlayerToken(NWDExchangeOrigin.Web), NWDExchangeRuntimeKind.Unknown, null, NWDExchangeOrigin.Unknown, NWDExchangeDevice.Unknown);
            tRequestRuntime.ProjectId = NWDSetup.FalseProjectId;
            // tRequestRuntime.PlayerToken.ProjectId = NWDSetup.FalseProjectId;
            Assert.IsFalse(tRequestRuntime.IsValid(NWDWebRuntimeConfiguration.KConfig));
        }
    }
}