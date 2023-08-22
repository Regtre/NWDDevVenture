using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDUnityShared.Tools;
using System.Collections.Generic;

namespace NWDUnityShared.Facades
{
    public interface INWDUnityAccountManager
    {
        public NWDRequestPlayerToken GetPlayerToken();
        public List<NWDAccountService> GetAccountServices();

        #region Get Signatures
        public NWDAsyncOperation<List<NWDAccountSign>> GetAccountSignatures();
        #endregion

        #region Sign In
        public NWDAsyncOperation AutoSignIn();
        public NWDAsyncOperation DeviceSignIn();
        public NWDAsyncOperation LoginPasswordSignIn(string sLogin, string sPassword);
        public NWDAsyncOperation MailPasswordSignIn(string sEmail, string sPassword);
        public NWDAsyncOperation LoginMailPasswordSignIn(string sLogin, string sEmail, string sPassword);
        public NWDAsyncOperation FacebookSignIn(string sFacebookId);
        public NWDAsyncOperation DiscordSignIn(string sDiscordId);
        public NWDAsyncOperation GoogleSignIn(string sGoogleId);
        public NWDAsyncOperation AppleSignIn(string sAppleId);
        public NWDAsyncOperation MicrosoftSignIn(string sMicrosoftId);
        public NWDAsyncOperation TwitterSignIn(string sTwitterId);
        public NWDAsyncOperation LinkedInSignIn(string sLinkedInId);
        #endregion

        #region Sign Up
        public NWDAsyncOperation DeviceSignUp();
        public NWDAsyncOperation LoginPasswordSignUp(string sLogin, string sPassword);
        public NWDAsyncOperation MailPasswordSignUp(string sEmail, string sPassword);
        public NWDAsyncOperation LoginMailPasswordSignUp(string sLogin, string sEmail, string sPassword);
        public NWDAsyncOperation FacebookSignUp(string sFacebookId);
        public NWDAsyncOperation DiscordSignUp(string sDiscordId);
        public NWDAsyncOperation GoogleSignUp(string sGoogleId);
        public NWDAsyncOperation AppleSignUp(string sAppleId);
        public NWDAsyncOperation MicrosoftSignUp(string sMicrosoftId);
        public NWDAsyncOperation TwitterSignUp(string sTwitterId);
        public NWDAsyncOperation LinkedInSignUp(string sLinkedInId);
        #endregion

        #region Sign Out
        public NWDAsyncOperation SignOut();
        #endregion

        #region Add Signature
        public NWDAsyncOperation AddDeviceSignature();
        public NWDAsyncOperation AddLoginPasswordSignature(string sLogin, string sPassword);
        public NWDAsyncOperation AddMailPasswordSignature(string sEmail, string sPassword);
        public NWDAsyncOperation AddLoginMailPasswordSignature(string sLogin, string sEmail, string sPassword);
        public NWDAsyncOperation AddFacebookSignature(string sFacebookId);
        public NWDAsyncOperation AddDiscordSignature(string sDiscordId);
        public NWDAsyncOperation AddGoogleSignature(string sGoogleId);
        public NWDAsyncOperation AddAppleSignature(string sAppleId);
        public NWDAsyncOperation AddMicrosoftSignature(string sMicrosoftId);
        public NWDAsyncOperation AddTwitterSignature(string sTwitterId);
        public NWDAsyncOperation AddLinkedInSignature(string sLinkedInId);
        #endregion

        #region Edit Signature
        public NWDAsyncOperation EditLoginPasswordSignature(NWDAccountSign sAccountSign, string sNewLogin, string sNewPassword);
        public NWDAsyncOperation EditMailPasswordSignature(NWDAccountSign sAccountSign, string sNewEmail, string sNewPassword);
        public NWDAsyncOperation EditLoginMailPasswordSignature(NWDAccountSign sAccountSign, string sNewLogin, string sNewEmail, string sNewPassword);
        #endregion

        #region Remove Signature
        public NWDAsyncOperation RemoveSignature(NWDAccountSign sAccountSign);
        #endregion
    }
}
