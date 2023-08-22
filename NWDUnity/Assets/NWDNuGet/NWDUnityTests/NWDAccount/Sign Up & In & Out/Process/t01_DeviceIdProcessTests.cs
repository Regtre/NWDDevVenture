using NUnit.Framework;
using NWDFoundation.Exchanges;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using NWDUnityTests.Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T1_SignUp_In_Out.Process
{
    public class t01_DeviceIdProcessTests
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
        public IEnumerator t1_SignUpWithDeviceId()
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignIn();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t2_CannotSignUpWithSameDeviceId()
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t3_SignUpWhenConnectedWithDeviceId()
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;
            tDeviceManager.RegenerateDevice();

            tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t4_SignUpOnInvalidSignIn ()
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignIn();
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, 0);
        }
    }
}