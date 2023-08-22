using NUnit.Framework;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T2_SocialSignature.Edit
{
    public class t1_LoginPasswordSocialSignatureEditTests
    {
        /// <summary>
        /// The legnth is volontary different for any credential method to avoid random collisions.
        /// 
        /// It also offers a little bit of leeway (up to +4) to make non colliding random values.
        /// </summary>
        private const int kLength = 60;

        [UnityTest]
        public IEnumerator t1_CanEditLogin()
        {
            string tLogin1 = NWDToolbox.RandomStringUnix(kLength);
            string tLogin2 = NWDToolbox.RandomStringUnix(kLength+1);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignUp(tLogin1, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.EditLoginPasswordSignature(tSignOperation.Result[0], tLogin2, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn(tLogin1, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn(tLogin2, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t2_CanEditPassword()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tPassword1 = NWDToolbox.RandomStringUnix(kLength);
            string tPassword2 = NWDToolbox.RandomStringUnix(kLength+1);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignUp(tLogin, tPassword1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.EditLoginPasswordSignature(tSignOperation.Result[0], tLogin, tPassword2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn(tLogin, tPassword1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn(tLogin, tPassword2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t3_CanEditLoginAndPassword()
        {
            string tLogin1 = NWDToolbox.RandomStringUnix(kLength);
            string tLogin2 = NWDToolbox.RandomStringUnix(kLength+1);
            string tPassword1 = NWDToolbox.RandomStringUnix(kLength);
            string tPassword2 = NWDToolbox.RandomStringUnix(kLength + 1);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignUp(tLogin1, tPassword1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.EditLoginPasswordSignature(tSignOperation.Result[0], tLogin2, tPassword2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn(tLogin1, tPassword1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn(tLogin2, tPassword2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }
    }
}