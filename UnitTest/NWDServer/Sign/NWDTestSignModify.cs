using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;

namespace UnitTest.NWDServer.Sign
{
    public class NWDTestSignModify : NWDTestSign
    {
        [Test]
        public void TestSignModify_AccountSignManager()
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
            NWDRequestRuntime tRequestRuntimeModify = NWDRequestRuntime.CreateRequestSignModify(NWDHubConfiguration.KConfig,tResponseRuntimeSignIn.PlayerToken,tAccountSign,  tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeModify= RuntimeController.Post(tRequestRuntimeModify);
            Assert.That(tResponseRuntimeModify.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeOutSecond = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntimeModify.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeOutSecond = RuntimeController.Post(tRequestRuntimeOutSecond);
            Assert.That(tResponseRuntimeOutSecond.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeSignInFirst = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignInFirst= RuntimeController.Post(tRequestRuntimeSignInFirst);
            Assert.That(tResponseRuntimeSignInFirst.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
            
            NWDRequestRuntime tRequestRuntimeSignInSecond = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignInSecond = RuntimeController.Post(tRequestRuntimeSignInSecond);
            Assert.That(tResponseRuntimeSignInSecond.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            
            Assert.That(tResponseRuntimeSignInSecond.PlayerToken.PlayerReference, Is.EqualTo(tResponseRuntimeSignUp.PlayerToken.PlayerReference));
            
            NWDRequestRuntime tRequestRuntimeUseToken = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntimeSignInSecond.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseToken = RuntimeController.Post(tRequestRuntimeUseToken);
            Assert.That(tResponseRuntimeUseToken.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeSignDelete= NWDRequestRuntime.CreateRequestSignDelete(NWDHubConfiguration.KConfig,tResponseRuntimeUseToken.PlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignDelete = RuntimeController.Post(tRequestRuntimeSignDelete);
            Assert.That(tResponseRuntimeSignDelete.Status, Is.EqualTo(NWDRequestStatus.Error));
            
        }
        
         [Test]
        public void TestSignModify_AccountSignManager_LastOne_NotDelete()
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
            NWDRequestRuntime tRequestRuntimeModify = NWDRequestRuntime.CreateRequestSignModify(NWDHubConfiguration.KConfig,tResponseRuntimeSignIn.PlayerToken,tAccountSign,  tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeModify= RuntimeController.Post(tRequestRuntimeModify);
            Assert.That(tResponseRuntimeModify.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeOutSecond = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntimeModify.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeOutSecond = RuntimeController.Post(tRequestRuntimeOutSecond);
            Assert.That(tResponseRuntimeOutSecond.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeSignInFirst = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignInFirst= RuntimeController.Post(tRequestRuntimeSignInFirst);
            Assert.That(tResponseRuntimeSignInFirst.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
            
            NWDRequestRuntime tRequestRuntimeSignInSecond = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignInSecond = RuntimeController.Post(tRequestRuntimeSignInSecond);
            Assert.That(tResponseRuntimeSignInSecond.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            Assert.That(tResponseRuntimeSignInSecond.PlayerToken.PlayerReference, Is.EqualTo(tResponseRuntimeSignUp.PlayerToken.PlayerReference));
            
            NWDRequestRuntime tRequestRuntimeUseToken = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntimeSignInSecond.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseToken = RuntimeController.Post(tRequestRuntimeUseToken);
            Assert.That(tResponseRuntimeUseToken.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeSignDelete= NWDRequestRuntime.CreateRequestSignDelete(NWDHubConfiguration.KConfig,tResponseRuntimeUseToken.PlayerToken, tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignDelete = RuntimeController.Post(tRequestRuntimeSignDelete);
            Assert.That(tResponseRuntimeSignDelete.Status, Is.EqualTo(NWDRequestStatus.Error));
            
        }
        
        [OneTimeTearDown]
        public void TestClean()
        {
            
        }
    }
}