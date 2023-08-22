using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDServerMiddle.Managers.ModelManagers;
using NWDServerMiddle.Models;

namespace UnitTest.NWDServer.Sign
{
    public class NWDTestSignUp : NWDTestSign
    {
        [Test]
        public void TestSignUp_AccountSign_SignHash_empty()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            // NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.SignHash = string.Empty;
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*,tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_SignHash_Percent()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.SignHash = "%";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_SignHash_Star()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.SignHash = "*";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_SignHash_Underscore()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.SignHash = "_";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_RescueHash_empty()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.RescueHash = string.Empty;
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_RescueHash_Percent()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.RescueHash = "%";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_RescueHash_Star()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.RescueHash = "*";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_RescueHash_Underscore()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.RescueHash = "_";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_RescueHash_MixteHack()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.RescueHash = "%_*%";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_LoginHash_empty()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.LoginHash = string.Empty;
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_LoginHash_Percent()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.LoginHash = "%";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_LoginHash_Star()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.LoginHash = "*";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_LoginHash_Underscore()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.LoginHash = "_";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSign_LoginHash_MixteHack()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            tAccountSign.LoginHash = "%_*%";
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_HackPayload()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tRequestRuntime.SetPayload(new NWDUpPayloadAccountSignUp() { AccountSign = NewFakeAccountSign() });
            
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_HackProject()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tRequestRuntime.ProjectId = NWDSetup.FalseProjectId;
            
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_HackProject_Second()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tRequestRuntime.PlayerToken.ProjectId = NWDSetup.FalseProjectId;
            
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_HackProject_Third()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            tRequestRuntime.ProjectId = NWDSetup.FalseProjectId;
            tRequestRuntime.PlayerToken.ProjectId = NWDSetup.FalseProjectId;
            
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
            tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_AccountSignManager()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
        }
        
        [Test]
        public void TestSignUp_AccountSignManager_NotTwice()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,tRequestPlayerToken, NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            NWDResponseRuntime tResponseRuntimeSecond = NWDAccountSignManager.ProcessSignUp(tRequestRuntime/*, tAccountInformation*/);
            Assert.That(tResponseRuntimeSecond.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_RuntimeManager()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
        }
        
        [Test]
        public void TestSignUp_RuntimeManager_NotTwice()
        {
            NWDRequestPlayerToken tRequestPlayerToken = NWDSetup.GetPlayerToken();
            NWDAccountInformation tAccountInformation = NWDAccountManager.CheckRequest(tRequestPlayerToken);
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            NWDResponseRuntime tResponseRuntimeSecond = RuntimeManager.Process(tRequestRuntime);
            Assert.That(tResponseRuntimeSecond.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [Test]
        public void TestSignUp_RuntimeController()
        {
            NWDAccountSign tAccountSign = NewFakeAccountSign();
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccountSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            NWDDownPayloadAccountSignUp tDownPayloadAccountSignUp = tResponseRuntime.GetPayload<NWDDownPayloadAccountSignUp>(NWDHubConfiguration.KConfig);
            NWDAccountSign tAccountSignReturn = tDownPayloadAccountSignUp.AccountSignList[0];
            NWDLogger.Trace("tResponseRuntime.PlayerToken.PlayerReference = " +tResponseRuntime.PlayerToken.PlayerReference );
            NWDLogger.Trace("tResponseRuntime.PlayerToken.AccountRange = " +tResponseRuntime.PlayerToken.AccountRange );
            
            NWDLogger.Trace("tAccountSignReturn.Account = " + tAccountSignReturn.Account );
            NWDLogger.Trace("tAccountSignReturn.Range = " + tAccountSignReturn.Range );
            Assert.IsNotEmpty(tResponseRuntime.PlayerToken.Token);
            Assert.IsTrue(tResponseRuntime.PlayerToken.PlayerReference>0);
            Assert.IsTrue(tResponseRuntime.PlayerToken.AccountRange>0);

            Assert.IsTrue(tDownPayloadAccountSignUp.AccountSignList.Count == 1);
            Assert.That(tDownPayloadAccountSignUp.AccountSignList[0].Account, Is.EqualTo(tResponseRuntime.PlayerToken.PlayerReference));
            Assert.That(tDownPayloadAccountSignUp.AccountSignList[0].LoginHash, Is.EqualTo(tAccountSign.LoginHash));
            Assert.That(tDownPayloadAccountSignUp.AccountSignList[0].RescueHash, Is.EqualTo(tAccountSign.RescueHash));
            Assert.That(tDownPayloadAccountSignUp.AccountSignList[0].SignHash, Is.EqualTo(tAccountSign.SignHash));
            
            Assert.That(tDownPayloadAccountSignUp.AccountSignList[0].SignStatus, Is.EqualTo(NWDAccountSignAction.Associated));
            
            NWDLogger.Trace(nameof(NWDTestSignUp) + "." + nameof(TestSignUp_RuntimeController) + "() => tResponseRuntime.PlayerToken.AccountRange = " + tResponseRuntime.PlayerToken.AccountRange);
            NWDLogger.Trace(nameof(NWDTestSignUp) + "." + nameof(TestSignUp_RuntimeController) + "() => tResponseRuntime.PlayerToken.AccountRange = " + JsonConvert.SerializeObject(tResponseRuntime.PlayerToken));
            
            // add test to use token 
            
            NWDLogger.Trace(nameof(NWDTestSignUp) + "." + nameof(TestSignUp_RuntimeController) + " ------ ADD TEST TO USE TOKEN ONCE------- ");
            
            NWDRequestRuntime tRequestRuntimeUseToken = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntime.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseToken = RuntimeController.Post(tRequestRuntimeUseToken);
            Assert.That(tResponseRuntimeUseToken.Status, Is.EqualTo(NWDRequestStatus.Ok));
            Assert.That(tResponseRuntimeUseToken.PlayerToken.Token, Is.EqualTo(tResponseRuntime.PlayerToken.Token));
            Assert.That(tResponseRuntimeUseToken.PlayerToken.PlayerReference, Is.EqualTo(tResponseRuntime.PlayerToken.PlayerReference));
            Assert.That(tResponseRuntimeUseToken.PlayerToken.ProjectId, Is.EqualTo(tResponseRuntime.PlayerToken.ProjectId));
            
            // add test to re-use token 
            
            NWDLogger.Trace(nameof(NWDTestSignUp) + "." + nameof(TestSignUp_RuntimeController) + " ------ ADD TEST TO USE TOKEN ONCE MORE------- ");
            
            NWDRequestRuntime tRequestRuntimeUseTokenSecond = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntimeUseToken.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseTokenSecond = RuntimeController.Post(tRequestRuntimeUseTokenSecond);
            Assert.That(tResponseRuntimeUseTokenSecond.Status, Is.EqualTo(NWDRequestStatus.Ok));
            Assert.That(tResponseRuntimeUseTokenSecond.PlayerToken.Token, Is.EqualTo(tResponseRuntimeUseToken.PlayerToken.Token));
            Assert.That(tResponseRuntimeUseTokenSecond.PlayerToken.PlayerReference, Is.EqualTo(tResponseRuntimeUseToken.PlayerToken.PlayerReference));
            Assert.That(tResponseRuntimeUseTokenSecond.PlayerToken.ProjectId, Is.EqualTo(tResponseRuntimeUseToken.PlayerToken.ProjectId));
            
        }
        
        [Test]
        public void TestSignUp_RuntimeController_NotTwice()
        {
            NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), NewFakeAccountSign(), NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntime = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntime.Status, Is.EqualTo(NWDRequestStatus.Ok));
            NWDResponseRuntime tResponseRuntimeSecond = RuntimeController.Post(tRequestRuntime);
            Assert.That(tResponseRuntimeSecond.Status, Is.EqualTo(NWDRequestStatus.Error));
        }
        
        [OneTimeTearDown]
        public void TestClean()
        {
            
        }
    }
}