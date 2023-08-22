using NUnit.Framework;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using NWDUnityTests.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T2_SocialSignature.Add
{
    public class t01_DeviceIdSocialSignatureAddTests
    {
        [SetUp]
        public void SetUp()
        {
            NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;
            if (tDeviceManager != null)
            {
                tDeviceManager.RegenerateDevice();
            }
        }

        [UnityTest]
        public IEnumerator t1_CanAddSameSocialSignautreType()
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;
            if (tDeviceManager != null)
            {
                tDeviceManager.RegenerateDevice();
            }

            tOperation = NWDUnityEngine.Instance.AccountManager.AddDeviceSignature();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t2_CannotAddSameSocialSignautreOnSameAccount()
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.AddDeviceSignature();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t3_CannotAddSameSocialSignautreOnDifferentAccount()
        {
            // Create First Sign (Sign1)
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            string tDeviceId = NWDUnityEngine.Instance.DeviceManager.GetDeviceId();
            string tDeviceName = NWDUnityEngine.Instance.DeviceManager.GetDeviceName();

            NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;
            if (tDeviceManager != null)
            {
                tDeviceManager.RegenerateDevice();
            }

            // Create new account with different sign (Sign2)
            tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            if (tDeviceManager != null)
            {
                tDeviceManager.ChangeDevice(tDeviceId, tDeviceName);
            }

            // Add Try to add (Sign1) to (Sign2)
            tOperation = NWDUnityEngine.Instance.AccountManager.AddDeviceSignature();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t4_CanMoveSocialSignautreOnDifferentAccount()
        {
            // Create First Sign (Sign1)
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            string tDeviceId = NWDUnityEngine.Instance.DeviceManager.GetDeviceId();
            string tDeviceName = NWDUnityEngine.Instance.DeviceManager.GetDeviceName();

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;
            if (tDeviceManager != null)
            {
                tDeviceManager.RegenerateDevice();
            }

            // Add Try to add (Sign2) to (Sign1)
            tOperation = NWDUnityEngine.Instance.AccountManager.AddDeviceSignature();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            if (tDeviceManager != null)
            {
                tDeviceManager.RegenerateDevice();
            }

            // Create new account with different sign (Sign3)
            tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            if (tDeviceManager != null)
            {
                tDeviceManager.ChangeDevice(tDeviceId, tDeviceName);
            }

            // Add Try to add (Sign1) to (Sign3)
            tOperation = NWDUnityEngine.Instance.AccountManager.AddDeviceSignature();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }
    }
}
