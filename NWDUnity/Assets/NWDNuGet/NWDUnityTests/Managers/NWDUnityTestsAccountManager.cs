using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDUnityShared.Facades;
using NWDUnityShared.Tools;
using System.Collections.Generic;

namespace NWDUnityTests.Manager
{
    public class NWDUnityTestsAccountManager : INWDUnityAccountManager
    {
        public NWDAsyncOperation AddAppleSignature(string sAppleId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AddDeviceSignature()
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AddDiscordSignature(string sDiscordId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AddFacebookSignature(string sFacebookId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AddGoogleSignature(string sGoogleId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AddLinkedInSignature(string sLinkedInId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AddLoginMailPasswordSignature(string sLogin, string sEmail, string sPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AddLoginPasswordSignature(string sLogin, string sPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AddMailPasswordSignature(string sEmail, string sPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AddMicrosoftSignature(string sMicrosoftId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AddTwitterSignature(string sTwitterId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AppleSignIn(string sAppleId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AppleSignUp(string sAppleId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation AutoSignIn()
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation DeviceSignIn()
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation DeviceSignUp()
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation DiscordSignIn(string sDiscordId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation DiscordSignUp(string sDiscordId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation EditLoginMailPasswordSignature(NWDAccountSign sAccountSign, string sNewLogin, string sNewEmail, string sNewPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation EditLoginPasswordSignature(NWDAccountSign sAccountSign, string sNewLogin, string sNewPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation EditMailPasswordSignature(NWDAccountSign sAccountSign, string sNewEmail, string sNewPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation FacebookSignIn(string sFacebookId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation FacebookSignUp(string sFacebookId)
        {
            throw new System.NotImplementedException();
        }

        public List<NWDAccountService> GetAccountServices()
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation<List<NWDAccountSign>> GetAccountSignatures()
        {
            throw new System.NotImplementedException();
        }

        public NWDRequestPlayerToken GetPlayerToken()
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation GoogleSignIn(string sGoogleId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation GoogleSignUp(string sGoogleId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation LinkedInSignIn(string sLinkedInId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation LinkedInSignUp(string sLinkedInId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation LoginMailPasswordSignIn(string sLogin, string sEmail, string sPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation LoginMailPasswordSignUp(string sLogin, string sEmail, string sPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation LoginPasswordSignIn(string sLogin, string sPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation LoginPasswordSignUp(string sLogin, string sPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation MailPasswordSignIn(string sEmail, string sPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation MailPasswordSignUp(string sEmail, string sPassword)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation MicrosoftSignIn(string sMicrosoftId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation MicrosoftSignUp(string sMicrosoftId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation RemoveSignature(NWDAccountSign sAccountSign)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation SignOut()
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation TwitterSignIn(string sTwitterId)
        {
            throw new System.NotImplementedException();
        }

        public NWDAsyncOperation TwitterSignUp(string sTwitterId)
        {
            throw new System.NotImplementedException();
        }
    }
}
