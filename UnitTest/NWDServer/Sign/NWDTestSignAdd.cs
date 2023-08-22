using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;

namespace UnitTest.NWDServer.Sign
{
    public class NWDTestSignAdd : NWDTestSign
    {
        [Test]
        public void TestSignAdd_AccountSignManager()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignUp = RuntimeController.Post(tRequestRuntimeSignUp);
            Assert.That(tResponseRuntimeSignUp.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeOut = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntimeSignUp.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeOut = RuntimeController.Post(tRequestRuntimeOut);
            Assert.That(tResponseRuntimeOut.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeSignIn = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignIn = RuntimeController.Post(tRequestRuntimeSignIn);
            Assert.That(tResponseRuntimeSignIn.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDAccountSign tAccountSignSecond = NewFakeAccountSign();
            NWDRequestRuntime tRequestRuntimeAdd = NWDRequestRuntime.CreateRequestSignAdd(NWDHubConfiguration.KConfig,tResponseRuntimeSignIn.PlayerToken, tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeAdd = RuntimeController.Post(tRequestRuntimeAdd);
            Assert.That(tResponseRuntimeAdd.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeOutSecond = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntimeAdd.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeOutSecond = RuntimeController.Post(tRequestRuntimeOutSecond);
            Assert.That(tResponseRuntimeOutSecond.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeSignInSecond = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignInSecond = RuntimeController.Post(tRequestRuntimeSignInSecond);
            Assert.That(tResponseRuntimeSignInSecond.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            
            Assert.That(tResponseRuntimeSignInSecond.PlayerToken.PlayerReference, Is.EqualTo(tResponseRuntimeSignUp.PlayerToken.PlayerReference));
            
            NWDRequestRuntime tRequestRuntimeUseToken = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntimeSignInSecond.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseToken = RuntimeController.Post(tRequestRuntimeUseToken);
            Assert.That(tResponseRuntimeUseToken.Status, Is.EqualTo(NWDRequestStatus.Ok));
        }
        
        
        [OneTimeTearDown]
        public void TestClean()
        {
            
        }
    }
}