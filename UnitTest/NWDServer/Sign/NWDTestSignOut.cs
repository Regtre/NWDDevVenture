using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;
using NWDServerMiddle.Managers.ModelManagers;
using NWDServerMiddle.Models;

namespace UnitTest.NWDServer.Sign
{
    public class NWDTestSignOut : NWDTestSign
    {
        [Test]
        public void TestSignOut_AccountSignManager()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccounSign = NewFakeAccountSign();
            NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignUp = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUp/*, tAccountInformation*/);
            Assert.That(tResponseRuntimeSignUp.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeOut = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeOut = NWDAccountSignManager.ProcessSignOut(tRequestRuntimeOut, tAccountInformation);
            Assert.That(tResponseRuntimeOut.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            Assert.That(tResponseRuntimeOut.PlayerToken.PlayerReference, Is.Not.EqualTo(tResponseRuntimeSignUp.PlayerToken.PlayerReference));
            
            NWDLogger.Trace(nameof(NWDTestSignOut) + "." + nameof(TestSignOut_RuntimeController) + " ------ ADD TEST TO USE TOKEN ONCE------- ");
            
            NWDRequestRuntime tRequestRuntimeUseToken = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntimeOut.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseToken = RuntimeController.Post(tRequestRuntimeUseToken);
            Assert.That(tResponseRuntimeUseToken.Status, Is.EqualTo(NWDRequestStatus.TokenEmpty));
        }
        
        [Test]
        public void TestSignOut_RuntimeManager()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccounSign = NewFakeAccountSign();
            NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignUp = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUp/*, tAccountInformation*/);
            Assert.That(tResponseRuntimeSignUp.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeOut = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeOut = RuntimeManager.Process(tRequestRuntimeOut);
            Assert.That(tResponseRuntimeOut.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            Assert.That(tResponseRuntimeOut.PlayerToken.PlayerReference, Is.Not.EqualTo(tResponseRuntimeSignUp.PlayerToken.PlayerReference));
            
            NWDLogger.Trace(nameof(NWDTestSignOut) + "." + nameof(TestSignOut_RuntimeController) + " ------ ADD TEST TO USE TOKEN ONCE------- ");
            
            NWDRequestRuntime tRequestRuntimeUseToken = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntimeOut.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseToken = RuntimeManager.Process(tRequestRuntimeUseToken);
            Assert.That(tResponseRuntimeUseToken.Status, Is.EqualTo(NWDRequestStatus.TokenEmpty));
        }
        
        [Test]
        public void TestSignOut_RuntimeController()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccounSign = NewFakeAccountSign();
            NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignUp = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUp/*, tAccountInformation*/);
            Assert.That(tResponseRuntimeSignUp.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeOut = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeOut = RuntimeController.Post(tRequestRuntimeOut);
            Assert.That(tResponseRuntimeOut.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            Assert.That(tResponseRuntimeOut.PlayerToken.PlayerReference, Is.Not.EqualTo(tResponseRuntimeSignUp.PlayerToken.PlayerReference));
            
            NWDLogger.Trace(nameof(NWDTestSignOut) + "." + nameof(TestSignOut_RuntimeController) + " ------ ADD TEST TO USE TOKEN ONCE------- ");
            
            NWDRequestRuntime tRequestRuntimeUseToken = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntimeOut.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseToken = RuntimeController.Post(tRequestRuntimeUseToken);
            Assert.That(tResponseRuntimeUseToken.Status, Is.EqualTo(NWDRequestStatus.TokenEmpty));
        }
        
        [Test]
        public void TestSignOut_RuntimeController_WithDeviceSign()
        {
            NWDRequestPlayerToken tRequestPlayerTokenDevice = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformationDevice = NWDAccountManager.CheckRequest(tRequestPlayerTokenDevice);
            NWDAccountSign tAccounSignDevice = NewFakeAccountSign();
            tAccounSignDevice.SignType = NWDAccountSignType.DeviceId;
            NWDRequestRuntime tRequestRuntimeSignUpDevice = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerTokenDevice, tAccounSignDevice, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignUpDevice = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUpDevice/*, tAccountInformationDevice*/);
            Assert.That(tResponseRuntimeSignUpDevice.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccounSign = NewFakeAccountSign();
            NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignUp = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUp/*, tAccountInformation*/);
            Assert.That(tResponseRuntimeSignUp.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeOut = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, tAccounSignDevice, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeOut = RuntimeController.Post(tRequestRuntimeOut);
            Assert.That(tResponseRuntimeOut.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            Assert.That(tResponseRuntimeOut.PlayerToken.PlayerReference, Is.EqualTo(tResponseRuntimeSignUpDevice.PlayerToken.PlayerReference));
            
            NWDLogger.Trace(nameof(NWDTestSignOut) + "." + nameof(TestSignOut_RuntimeController) + " ------ ADD TEST TO USE TOKEN ONCE------- ");
            
            NWDRequestRuntime tRequestRuntimeUseToken = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntimeOut.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseToken = RuntimeController.Post(tRequestRuntimeUseToken);
            Assert.That(tResponseRuntimeUseToken.Status, Is.EqualTo(NWDRequestStatus.Ok));
        }
        
        [OneTimeTearDown]
        public void TestClean()
        {
            
        }
    }
}