using NUnit.Framework;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using NWDUnityTests.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T2_SocialSignature.Remove
{
    public class t1_DeleteSocialSignatureTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp ()
        {
            NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;

            if (tDeviceManager != null)
            {
                tDeviceManager.RegenerateDevice();
            }
        }

        [UnityTest]
        public IEnumerator t01_DeleteDeviceIdSocialSignature()
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;
            if (tDeviceManager != null)
            {
                tDeviceManager.RegenerateDevice();
            }

            tOperation = NWDUnityEngine.Instance.AccountManager.AddDeviceSignature();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t02_DeleteLoginPasswordSocialSignature()
        {
            string tLogin1 = NWDToolbox.RandomStringUnix(60);
            string tLogin2 = NWDToolbox.RandomStringUnix(61);
            string tPassword = NWDToolbox.RandomStringUnix(60);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignUp(tLogin1, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginPasswordSignature(tLogin2, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t03_DeleteMailPasswordSocialSignature()
        {
            string tMail1 = NWDToolbox.RandomStringUnix(65);
            string tMail2 = NWDToolbox.RandomStringUnix(66);
            string tPassword = NWDToolbox.RandomStringUnix(65);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail1, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddMailPasswordSignature(tMail2, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t04_DeleteLoginMailPasswordSocialSignature()
        {
            string tLogin1 = NWDToolbox.RandomStringUnix(70);
            string tLogin2 = NWDToolbox.RandomStringUnix(71);
            string tMail = NWDToolbox.RandomStringUnix(70);
            string tPassword = NWDToolbox.RandomStringUnix(70);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin1, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginMailPasswordSignature(tLogin2, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t05_DeleteFacebookSocialSignature()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(75);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(75);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddFacebookSignature(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t06_DeleteDiscordSocialSignature()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(80);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(80);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DiscordSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddDiscordSignature(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t07_DeleteGoogleSocialSignature()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(85);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(85);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddGoogleSignature(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t08_DeleteAppleSocialSignature()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(90);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(90);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AppleSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddAppleSignature(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t09_DeleteMicrosoftSocialSignature()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(95);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(95);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MicrosoftSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddMicrosoftSignature(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t10_DeleteTwitterSocialSignature()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(100);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(100);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.TwitterSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddTwitterSignature(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t11_DeleteLinkedInSocialSignature()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(105);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(105);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LinkedInSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddLinkedInSignature(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }
    }
}
