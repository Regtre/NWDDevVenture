using NUnit.Framework;
using NWDFoundation.Exchanges;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using System.Collections;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T1_SignUp_In_Out.Process
{
    public class t07_GoogleProcessTests
    {
        /// <summary>
        /// The legnth is volontary different for any credential method to avoid random collisions.
        /// 
        /// It also offers a little bit of leeway (up to +4) to make non colliding random values.
        /// </summary>
        private const int kLength = 85;

        [UnityTest]
        public IEnumerator t1_SignUpWithGoogle()
        {
            string tSocialSignature = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignIn(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t2_CannotSignUpWithSameGoogle()
        {
            string tSocialSignature = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t3_SignUpWhenConnectedWithGoogle()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(kLength);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(kLength + 1);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t4_CanSignUpWithDifferentCase()
        {
            string tSocialSignature = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(tSocialSignature.ToLower());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(tSocialSignature.ToUpper());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t5_CannotSignInWithDifferentCase()
        {
            string tSocialSignature = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(tSocialSignature.ToLower());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignIn(tSocialSignature.ToUpper());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }
    }
}