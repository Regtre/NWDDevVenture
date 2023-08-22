using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NWDFoundation.Models;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using NWDUnityTests.Manager;
using UnityEngine;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T2_SocialSignature.Remove
{
    public class t2_DeleteAllSocialSignatureTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;

            if (tDeviceManager != null)
            {
                tDeviceManager.RegenerateDevice();
            }
        }

        [UnityTest]
        public IEnumerator t1_DeleteAllSocialSignatureTests()
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            string tLogin1 = NWDToolbox.RandomStringUnix(60);
            string tLogin2 = NWDToolbox.RandomStringUnix(70);
            string tMail = NWDToolbox.RandomStringUnix(65);
            string tPassword = NWDToolbox.RandomStringUnix(60);
            string tSocialSignature = NWDToolbox.RandomStringUnix(75);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginPasswordSignature(tLogin1, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddMailPasswordSignature(tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginMailPasswordSignature(tLogin2, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddFacebookSignature(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddDiscordSignature(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddGoogleSignature(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddAppleSignature(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddMicrosoftSignature(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddTwitterSignature(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.AddLinkedInSignature(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);

            for (int i = 0; i < tSignOperation.Result.Count; i++)
            {
                tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[i]);
                yield return tOperation;
            }

            tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);
        }
    }
}
