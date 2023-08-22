using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDUnityShared.Engine;
using NWDUnityShared.Facades;
using NWDUnityShared.TaskSchedulers;
using NWDUnityShared.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace NWDUnityShared.Managers
{
    public class NWDUnityAccountManager : INWDUnityAccountManager
    {
        public NWDAccountTaskScheduler Scheduler = new NWDAccountTaskScheduler();

        public NWDRequestPlayerToken GetPlayerToken()
        {
            return Services.NWDAccountService.PlayerToken;
        }

        public List<NWDAccountService> GetAccountServices()
        {
            return Services.NWDAccountService.AccountServices;
        }

        #region Get Signatures
        public NWDAsyncOperation<List<NWDAccountSign>> GetAccountSignatures()
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                return Services.NWDAccountService.GetSignatures(ref sHandler);
            });
        }
        #endregion

        #region Sign In
        /// <summary>
        /// Automatically sign in with the last used sign data.
        /// </summary>
        /// <returns>The async operation.</returns>
        public NWDAsyncOperation AutoSignIn()
        {
            return DeviceSignIn();
        }

        public NWDAsyncOperation DeviceSignIn()
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                string tDeviceId = NWDUnityEngine.Instance.DeviceManager.GetDeviceId();
                string tDeviceName = NWDUnityEngine.Instance.DeviceManager.GetDeviceName();
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateDeviceId(tDeviceId, tProjectId, tDeviceName);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation LoginPasswordSignIn(string sLogin, string sPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateLoginPassword(sLogin, sPassword, tProjectId);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation MailPasswordSignIn(string sEmail, string sPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateEmailPassword(sEmail, sPassword, tProjectId);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation LoginMailPasswordSignIn(string sLogin, string sEmail, string sPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateLoginEmailPassword(sLogin, sEmail, sPassword, tProjectId);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation FacebookSignIn(string sFacebookId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateFacebook(sFacebookId, tProjectId);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation DiscordSignIn(string sDiscordId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateDiscord(sDiscordId, tProjectId);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation GoogleSignIn(string sGoogleId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateGoogle(sGoogleId, tProjectId);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AppleSignIn(string sAppleId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateApple(sAppleId, tProjectId);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation MicrosoftSignIn(string sMicrosoftId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateMicrosoft(sMicrosoftId, tProjectId);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation TwitterSignIn(string sTwitterId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateTwitter(sTwitterId, tProjectId);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation LinkedInSignIn(string sLinkedInId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateLinkedIn(sLinkedInId, tProjectId);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        #endregion

        #region Sign Up
        public NWDAsyncOperation DeviceSignUp()
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                string tDeviceId = NWDUnityEngine.Instance.DeviceManager.GetDeviceId();
                string tDeviceName = NWDUnityEngine.Instance.DeviceManager.GetDeviceName();
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateDeviceId(tDeviceId, tProjectId, tDeviceName);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation LoginPasswordSignUp(string sLogin, string sPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateLoginPassword(sLogin, sPassword, tProjectId);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation MailPasswordSignUp(string sEmail, string sPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateEmailPassword(sEmail, sPassword, tProjectId);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation LoginMailPasswordSignUp(string sLogin, string sEmail, string sPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateLoginEmailPassword(sLogin, sEmail, sPassword, tProjectId);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation FacebookSignUp(string sFacebookId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateFacebook(sFacebookId, tProjectId);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation DiscordSignUp(string sDiscordId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateDiscord(sDiscordId, tProjectId);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation GoogleSignUp(string sGoogleId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateGoogle(sGoogleId, tProjectId);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AppleSignUp(string sAppleId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateApple(sAppleId, tProjectId);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation MicrosoftSignUp(string sMicrosoftId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateMicrosoft(sMicrosoftId, tProjectId);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation TwitterSignUp(string sTwitterId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateTwitter(sTwitterId, tProjectId);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation LinkedInSignUp(string sLinkedInId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateLinkedIn(sLinkedInId, tProjectId);
                Services.NWDAccountService.SignUp(tAccountSign, ref sHandler);
            });
        }
        #endregion

        #region Sign Out
        public NWDAsyncOperation SignOut()
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                string tDeviceId = NWDUnityEngine.Instance.DeviceManager.GetDeviceId();
                string tDeviceName = NWDUnityEngine.Instance.DeviceManager.GetDeviceName();
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateDeviceId(tDeviceId, tProjectId, tDeviceName);
                Services.NWDAccountService.SignIn(tAccountSign, ref sHandler);
            });
        }
        #endregion

        #region Add Signature
        public NWDAsyncOperation AddDeviceSignature()
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                string tDeviceId = NWDUnityEngine.Instance.DeviceManager.GetDeviceId();
                string tDeviceName = NWDUnityEngine.Instance.DeviceManager.GetDeviceName();
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateDeviceId(tDeviceId, tProjectId, tDeviceName);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AddLoginPasswordSignature(string sLogin, string sPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateLoginPassword(sLogin, sPassword, tProjectId);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AddMailPasswordSignature(string sEmail, string sPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateEmailPassword(sEmail, sPassword, tProjectId);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AddLoginMailPasswordSignature(string sLogin, string sEmail, string sPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateLoginEmailPassword(sLogin, sEmail, sPassword, tProjectId);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AddFacebookSignature(string sFacebookId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateFacebook(sFacebookId, tProjectId);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AddDiscordSignature(string sDiscordId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateDiscord(sDiscordId, tProjectId);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AddGoogleSignature(string sGoogleId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateGoogle(sGoogleId, tProjectId);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AddAppleSignature(string sAppleId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateApple(sAppleId, tProjectId);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AddMicrosoftSignature(string sMicrosoftId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateMicrosoft(sMicrosoftId, tProjectId);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AddTwitterSignature(string sTwitterId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateTwitter(sTwitterId, tProjectId);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation AddLinkedInSignature(string sLinkedInId)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateLinkedIn(sLinkedInId, tProjectId);
                Services.NWDAccountService.AddSignature(tAccountSign, ref sHandler);
            });
        }
        #endregion

        #region Edit Signature
        public NWDAsyncOperation EditLoginPasswordSignature(NWDAccountSign sAccountSign, string sNewLogin, string sNewPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tNewAccountSign = NWDAccountSign.CreateLoginPassword(sNewLogin, sNewPassword, tProjectId);
                Services.NWDAccountService.EditSignature(sAccountSign, tNewAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation EditMailPasswordSignature(NWDAccountSign sAccountSign, string sNewEmail, string sNewPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tNewAccountSign = NWDAccountSign.CreateEmailPassword(sNewEmail, sNewPassword, tProjectId);
                Services.NWDAccountService.EditSignature(sAccountSign, tNewAccountSign, ref sHandler);
            });
        }
        public NWDAsyncOperation EditLoginMailPasswordSignature(NWDAccountSign sAccountSign, string sNewLogin, string sNewEmail, string sNewPassword)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tNewAccountSign = NWDAccountSign.CreateLoginEmailPassword(sNewLogin, sNewEmail, sNewPassword, tProjectId);
                Services.NWDAccountService.EditSignature(sAccountSign, tNewAccountSign, ref sHandler);
            });
        }
        #endregion

        #region Remove Signature
        public NWDAsyncOperation RemoveSignature(NWDAccountSign sAccountSign)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                Services.NWDAccountService.RemoveSignature(sAccountSign, ref sHandler);
            });
        }
        #endregion
    }
}
