using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;
using NWDServerMiddle.Managers.ModelManagers;
using UnitTest.NWDServer.Sign;

namespace UnitTest.NWDServer.PlayerToken
{
    public class NWDTestPlayerToken : NWDTestSign
    {
        [Test]
        public void TestTokenWebIsEqualInSession()
        {
            // Create sign
            NWDAccountSign tAccounSign = NewFakeAccountSign();
            
            NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignUp = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUp);
            Assert.That(tResponseRuntimeSignUp.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            string tTokenWeb = tResponseRuntimeSignUp.PlayerToken.Token;
            
            NWDRequestRuntime tRequestRuntimeUseToken = NWDRequestRuntime.CreateRequestGetAllSign(NWDHubConfiguration.KConfig,tResponseRuntimeSignUp.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseToken = RuntimeController.Post(tRequestRuntimeUseToken);
            
            Assert.That(tResponseRuntimeUseToken.PlayerToken.Token, Is.EqualTo(tTokenWeb));
        }
        
         [Test]
        public void TestTokenWebIsEqualInSessionSignin()
        {
            // Create sign
            NWDAccountSign tAccounSign = NewFakeAccountSign();
            
            NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignUp = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUp);
            Assert.That(tResponseRuntimeSignUp.Status, Is.EqualTo(NWDRequestStatus.Ok));
            
            NWDRequestRuntime tRequestRuntimeOut = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntimeSignUp.PlayerToken, null, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeOut = RuntimeManager.Process(tRequestRuntimeOut);
            
            NWDRequestRuntime tRequestRuntimeSignIn = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(), tAccounSign, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeSignIn = RuntimeController.Post(tRequestRuntimeSignIn);
            Assert.That(tResponseRuntimeSignIn.Status, Is.EqualTo(NWDRequestStatus.Ok));
            string tTokenWeb = tResponseRuntimeSignIn.PlayerToken.Token;
            
            NWDRequestRuntime tRequestRuntimeUseTokenB = NWDRequestRuntime.CreateGetAllPlayerData(NWDHubConfiguration.KConfig,tResponseRuntimeSignIn.PlayerToken, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseRuntime tResponseRuntimeUseTokenB = RuntimeController.Post(tRequestRuntimeUseTokenB);
            
            Assert.That(tResponseRuntimeUseTokenB.PlayerToken.Token, Is.EqualTo(tTokenWeb));
            
            tResponseRuntimeOut = RuntimeManager.Process(tRequestRuntimeOut);
            tResponseRuntimeSignIn = RuntimeController.Post(tRequestRuntimeSignIn);
            Assert.AreNotEqual(tResponseRuntimeSignIn.PlayerToken.Token, tTokenWeb);
            
        }
        
        
        [Test]
        [TestCase(NWDExchangeOrigin.Unknown)]
        [TestCase(NWDExchangeOrigin.Game)]
        [TestCase(NWDExchangeOrigin.App)]
        [TestCase(NWDExchangeOrigin.UnityEditor)]
        public void TestTokenGamebIsNOTEqualInSessionSignin(NWDExchangeOrigin sExchangeOrigin)
        {
            
            // Create sign
            NWDAccountSign tAccounSign = NewFakeAccountSign();
            
            NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(sExchangeOrigin), tAccounSign, sExchangeOrigin, NWDExchangeDevice.Macos);
            NWDResponseRuntime tResponseRuntimeSignUp = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUp);
            Assert.That(tResponseRuntimeSignUp.Status, Is.EqualTo(NWDRequestStatus.Ok));
            string tTokenGameSignUp = tResponseRuntimeSignUp.PlayerToken.Token;
            
            NWDRequestRuntime tRequestRuntimeOut = NWDRequestRuntime.CreateRequestSignOut(NWDHubConfiguration.KConfig,tResponseRuntimeSignUp.PlayerToken, null, sExchangeOrigin, NWDExchangeDevice.Macos);
            NWDResponseRuntime tResponseRuntimeOut = RuntimeManager.Process(tRequestRuntimeOut);
            Assert.That(tResponseRuntimeOut.Status, Is.EqualTo(NWDRequestStatus.Ok));
            string tTokenGameSignOut = tResponseRuntimeOut.PlayerToken.Token;
            
            NWDRequestRuntime tRequestRuntimeSignIn = NWDRequestRuntime.CreateRequestSignIn(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(sExchangeOrigin), tAccounSign, sExchangeOrigin, NWDExchangeDevice.Macos);
            NWDResponseRuntime tResponseRuntimeSignIn = RuntimeController.Post(tRequestRuntimeSignIn);
            Assert.That(tResponseRuntimeSignIn.Status, Is.EqualTo(NWDRequestStatus.Ok));
            string tTokenGameSignIn = tResponseRuntimeSignIn.PlayerToken.Token;
            
            NWDRequestRuntime tRequestRuntimeUseTokenRequest = NWDRequestRuntime.CreateGetAllPlayerData(NWDHubConfiguration.KConfig,tResponseRuntimeSignIn.PlayerToken, sExchangeOrigin, NWDExchangeDevice.Macos);
            NWDResponseRuntime tResponseRuntimeUseTokenRequest = RuntimeController.Post(tRequestRuntimeUseTokenRequest);
            Assert.That(tResponseRuntimeUseTokenRequest.Status, Is.EqualTo(NWDRequestStatus.Ok));
            string tTokenGameRequest = tResponseRuntimeUseTokenRequest.PlayerToken.Token;
            
            Assert.AreNotEqual(tTokenGameRequest, tTokenGameSignIn);
            
            NWDRequestRuntime tRequestRuntimeUseTokenRequestSecond = NWDRequestRuntime.CreateGetAllPlayerData(NWDHubConfiguration.KConfig,tResponseRuntimeUseTokenRequest.PlayerToken, sExchangeOrigin, NWDExchangeDevice.Macos);
            NWDResponseRuntime tResponseRuntimeUseTokenRequestSecond = RuntimeController.Post(tRequestRuntimeUseTokenRequestSecond);
            Assert.That(tResponseRuntimeUseTokenRequestSecond.Status, Is.EqualTo(NWDRequestStatus.Ok));
            string tTokenGameRequestSecond = tResponseRuntimeUseTokenRequestSecond.PlayerToken.Token;
            
            Assert.AreNotEqual(tTokenGameRequestSecond, tTokenGameRequest);
            Assert.AreNotEqual(tTokenGameRequestSecond, tTokenGameSignIn);
            
        }
        
        
        
        [Test]
        [TestCase(NWDExchangeOrigin.Unknown)]
        [TestCase(NWDExchangeOrigin.Game)]
        [TestCase(NWDExchangeOrigin.App)]
        [TestCase(NWDExchangeOrigin.UnityEditor)]
        public void TestTokenHack(NWDExchangeOrigin sExchangeOrigin)
        {
            // Create sign
            NWDAccountSign tAccounSign = NewFakeAccountSign();
            
            NWDRequestRuntime tRequestRuntimeSignUp = NWDRequestRuntime.CreateRequestSignUp(NWDHubConfiguration.KConfig,NWDSetup.GetPlayerToken(sExchangeOrigin), tAccounSign, sExchangeOrigin, NWDExchangeDevice.Macos);
            NWDResponseRuntime tResponseRuntimeSignUp = NWDAccountSignManager.ProcessSignUp(tRequestRuntimeSignUp);
            Assert.That(tResponseRuntimeSignUp.Status, Is.EqualTo(NWDRequestStatus.Ok));
            string tTokenGameSignUp = tResponseRuntimeSignUp.PlayerToken.Token;

            tResponseRuntimeSignUp.PlayerToken.Token += "j"; 
            NWDRequestRuntime tRequestRuntimeUseTokenRequest = NWDRequestRuntime.CreateGetAllPlayerData(NWDHubConfiguration.KConfig,tResponseRuntimeSignUp.PlayerToken, sExchangeOrigin, NWDExchangeDevice.Macos);
            NWDResponseRuntime tResponseRuntimeUseTokenRequest = RuntimeController.Post(tRequestRuntimeUseTokenRequest);
            
            Assert.That(tResponseRuntimeUseTokenRequest.Status, Is.EqualTo(NWDRequestStatus.Error));
            string tTokenGameRequest = tResponseRuntimeUseTokenRequest.PlayerToken.Token;
            
            Assert.AreNotEqual(tTokenGameRequest, tTokenGameSignUp);
            
        }
    }
}