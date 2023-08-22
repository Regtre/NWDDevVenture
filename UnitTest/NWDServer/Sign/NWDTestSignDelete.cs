using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;
using NWDServerMiddle.Managers.ModelManagers;

namespace UnitTest.NWDServer.Sign
{
    public class NWDTestSignDelete : NWDTestSign
    {
        [Test]
        public void TestSignDelete_AccountForSign()
        {
            Assert.IsNull(NWDAccountSignManager.AccountForSign(NewFakeAccountSign(), NWDSetup.GetPlayerToken()));
        }

        [Test]
        public void TestSignDelete_AccountForSign_WrongSign()
        {
            NWDResponseRuntime tResponseRuntime = RuntimeController.Post(NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web));
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
        }

        [Test]
        public void TestSignDelete_AccountSignDelete()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            NWDAccountSign tAccountSignSecond = NewFakeAccountSign();
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignAdd(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignDelete(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tResponseRuntime = RuntimeController.Post(NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web));
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tResponseRuntime = RuntimeController.Post(NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web));
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));

            tResponseRuntime = RuntimeController.Post(NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web));
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tResponseRuntime = RuntimeController.Post(NWDRequestRuntime.CreateRequestTest(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web));
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
        }

        [Test]
        public void TestSignDelete_AccountSignManager()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            NWDAccountSign tAccountSignSecond = NewFakeAccountSign();

            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            tRequestRuntime = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            NWDRequestPlayerToken tRequestPlayerTokenFirstSign = tResponseRuntime.PlayerToken;

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignAdd(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            NWDRequestPlayerToken tRequestPlayerTokenSecondSign = tResponseRuntime.PlayerToken;

            Assert.That(tRequestPlayerTokenFirstSign.PlayerReference, Is.EqualTo(tRequestPlayerTokenSecondSign.PlayerReference));

            tRequestRuntime = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tRequestRuntime = NWDRequestRuntime.CreateRequestSignDelete(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            Assert.IsNull(NWDAccountSignManager.AccountForSign(tAccountSign, tResponseRuntime.PlayerToken));

            tResponseRuntime = RuntimeController.Post(NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web));
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tResponseRuntime = RuntimeController.Post(NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web));
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
            
            tResponseRuntime = RuntimeController.Post(NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web));
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));

            tResponseRuntime = RuntimeController.Post(NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSignSecond, NWDExchangeOrigin.Web, NWDExchangeDevice.Web));
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));

            tResponseRuntime = RuntimeController.Post(NWDRequestRuntime.CreateRequestTest(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web));
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
        }

        [Test]
        public void TestSignelete_AccountSignManager_LastOne_NotDelete()
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

            NWDRequestRuntime tRequestRuntimeSignDelete = NWDRequestRuntime.CreateRequestSignDelete(NWDHubConfiguration.KConfig,tResponseRuntimeSignIn.PlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignDelete = RuntimeController.Post(tRequestRuntimeSignDelete);
            Assert.That(tResponseRuntimeSignDelete.Status, Is.EqualTo(NWDRequestStatus.Error));
        }

        [OneTimeTearDown]
        public void TestClean()
        {
        }
    }
}