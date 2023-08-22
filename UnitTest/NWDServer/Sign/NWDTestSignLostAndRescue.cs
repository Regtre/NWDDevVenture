using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;

namespace UnitTest.NWDServer.Sign
{
    public class NWDTestSignLostAndRescue: NWDTestSign
    {
        [Test]
        public void TestSignLostAndRescue_AccountSignManager()
        {
            string tEmailFake = NWDRandom.RandomStringBase64(28);
            
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountSign tAccountSign = NewFakeAccountSignWithEmail(tEmailFake);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            tRequestRuntime = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            tRequestRuntime = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            tRequestRuntime = NWDRequestRuntime.CreateRequestSignLost(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tEmailFake+"kkk", NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
            
            tRequestRuntime = NWDRequestRuntime.CreateRequestSignLost(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tEmailFake, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            NWDDownPayloadAccountSignLost tInfos = tResponseRuntime.GetPayload<NWDDownPayloadAccountSignLost>(NWDHubConfiguration.KConfig);
            NWDLogger.Trace(nameof(tInfos.RescueTokenSecured)+ "  = " + tInfos.RescueTokenSecured);

            string tTokenDecrypt = NWDSecurityTools.DecryptSomething(tInfos.RescueTokenSecured, NWDHubConfiguration.KConfig.GetCrucialProjectId(), tRequestPlayerToken.EnvironmentKind,NWDHubConfiguration.KConfig,NWDHubConfiguration.KConfig);
            NWDLogger.Trace(nameof(tTokenDecrypt)+ "  = " + tTokenDecrypt);
            
            NWDAccountSign tAccountSignRescue = NewFakeAccountSignWithEmail(tEmailFake);
            tAccountSignRescue.TokenRescue = tTokenDecrypt;
            
            tRequestRuntime = NWDRequestRuntime.CreateRequestSignRescue(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSignRescue, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSignRescue, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            
            tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
            
            
        }
        
        
        [OneTimeTearDown]
        public void TestClean()
        {
            
        }
    }
}