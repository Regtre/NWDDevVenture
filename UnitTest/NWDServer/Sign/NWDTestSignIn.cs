using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDServerMiddle.Managers.ModelManagers;
using NWDServerMiddle.Models;

namespace UnitTest.NWDServer.Sign
{
    public class NWDTestSignIn : NWDTestSign
    {
        [Test]
        public void TestSignIn_AccountSign_SignHash_empty()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.SignHash = string.Empty;
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*,tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_SignHash_Percent()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.SignHash = "%";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_SignHash_Star()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.SignHash = "*";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_SignHash_Underscore()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.SignHash = "_";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_RescueHash_empty()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.RescueHash = string.Empty;
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_RescueHash_Percent()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.RescueHash = "%";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_RescueHash_Star()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.RescueHash = "*";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_RescueHash_Underscore()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.RescueHash = "_";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_RescueHash_MixteHack()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.RescueHash = "%_*%";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_LoginHash_empty()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.LoginHash = string.Empty;
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_LoginHash_Percent()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.LoginHash = "%";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_LoginHash_Star()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.LoginHash = "*";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_LoginHash_Underscore()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.LoginHash = "_";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSign_LoginHash_MixteHack()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.LoginHash = "%_*%";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_HackPayload()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tRequestRuntime.SetPayload(new NWDUpPayloadAccountSignIn() { AccountSign = NewFakeAccountSign() });
            
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
            tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_HackPayload_2()
        {
            
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccounSign = NewFakeAccountSign();
            NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignUp = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUp/*, tAccountInformation*/);
            Assert.That(tResponseRuntimeSignUp.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tRequestRuntime.SetPayload(new NWDUpPayloadAccountSignIn() { AccountSign = tAccounSign });
            
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
            tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_HackProject()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tRequestRuntime.ProjectId = NWDSetup.FalseProjectId;
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
            tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_HackProject_Second()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tRequestRuntime.PlayerToken.ProjectId = NWDSetup.FalseProjectId;
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
            tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_HackProject_Third()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tRequestRuntime.ProjectId = NWDSetup.FalseProjectId;
            tRequestRuntime.PlayerToken.ProjectId = NWDSetup.FalseProjectId;
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
            tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignIn_AccountSignManager()
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
        }
        
        [Test]
        public void TestSignIn_AccountSignManager_NotExist()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignIn(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.AccountUnknown));
        }
        
        [Test]
        public void TestSignIn_RuntimeManager()
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
        }
        
        [Test]
        public void TestSignIn_RuntimeController()
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
            NWDDownPayloadAccountSignIn tDownPayloadAccountSignIn = tResponseRuntime.GetPayload<NWDDownPayloadAccountSignIn>(NWDHubConfiguration.KConfig);
            NWDLogger.Trace("tResponseRuntime.PlayerToken.PlayerReference = " +tResponseRuntime.PlayerToken.PlayerReference );
            NWDLogger.Trace("tResponseRuntime.PlayerToken.AccountRange = " +tResponseRuntime.PlayerToken.AccountRange );
            Assert.IsNotEmpty(tResponseRuntime.PlayerToken.Token);
            Assert.IsTrue(tResponseRuntime.PlayerToken.PlayerReference>0);
            Assert.IsTrue(tResponseRuntime.PlayerToken.AccountRange>0);

            NWDLogger.Trace(nameof(NWDTestSignIn) + "." + nameof(TestSignIn_RuntimeController) + "() => tResponseRuntime.PlayerToken.AccountRange = " + tResponseRuntime.PlayerToken.AccountRange);
            NWDLogger.Trace(nameof(NWDTestSignIn) + "." + nameof(TestSignIn_RuntimeController) + "() => tResponseRuntime.PlayerToken.AccountRange = " + JsonConvert.SerializeObject(tResponseRuntime.PlayerToken));
            
            // add test to use token 
            
            NWDLogger.Trace(nameof(NWDTestSignIn) + "." + nameof(TestSignIn_RuntimeController) + " ------ ADD TEST TO USE TOKEN ONCE------- ");
            
            NWDRequestRuntime tRequestRuntimeUseToken = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseToken = RuntimeController.Post(tRequestRuntimeUseToken);
            Assert.That(tResponseRuntimeUseToken.Status, Is.EqualTo(NWDRequestStatus.Ok));
            Assert.That(tResponseRuntimeUseToken.PlayerToken.Token, Is.EqualTo(tResponseRuntime.PlayerToken.Token));
            Assert.That(tResponseRuntimeUseToken.PlayerToken.PlayerReference, Is.EqualTo(tResponseRuntime.PlayerToken.PlayerReference));
            Assert.That(tResponseRuntimeUseToken.PlayerToken.ProjectId, Is.EqualTo(tResponseRuntime.PlayerToken.ProjectId));
            
            // add test to re-use token 
            
            NWDLogger.Trace(nameof(NWDTestSignIn) + "." + nameof(TestSignIn_RuntimeController) + " ------ ADD TEST TO USE TOKEN ONCE MORE------- ");
            
            NWDRequestRuntime tRequestRuntimeUseTokenSecond = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntimeUseToken.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseTokenSecond = RuntimeController.Post(tRequestRuntimeUseTokenSecond);
            Assert.That(tResponseRuntimeUseTokenSecond.Status, Is.EqualTo(NWDRequestStatus.Ok));
            Assert.That(tResponseRuntimeUseTokenSecond.PlayerToken.Token, Is.EqualTo(tResponseRuntimeUseToken.PlayerToken.Token));
            Assert.That(tResponseRuntimeUseTokenSecond.PlayerToken.PlayerReference, Is.EqualTo(tResponseRuntimeUseToken.PlayerToken.PlayerReference));
            Assert.That(tResponseRuntimeUseTokenSecond.PlayerToken.ProjectId, Is.EqualTo(tResponseRuntimeUseToken.PlayerToken.ProjectId));
            
        }
        
        [OneTimeTearDown]
        public void TestClean()
        {
            
        }
    }
}