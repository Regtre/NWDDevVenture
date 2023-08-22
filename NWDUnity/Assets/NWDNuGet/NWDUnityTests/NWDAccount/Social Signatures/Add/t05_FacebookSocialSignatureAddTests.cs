using NUnit.Framework;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using NWDUnityTests.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T2_SocialSignature.Add
{
    public class t05_FacebookSocialSignatureAddTests
    {
        /// <summary>
        /// The legnth is volontary different for any credential method to avoid random collisions.
        /// 
        /// It also offers a little bit of leeway (up to +4) to make non colliding random values.
        /// </summary>
        private const int kLength = 75;

        [UnityTest]
        public IEnumerator t1_CanAddSameSocialSignautreType()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(kLength);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(kLength + 1);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.AddFacebookSignature(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t2_CannotAddSameSocialSignautreOnSameAccount()
        {
            string tSocialSignature = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.AddFacebookSignature(tSocialSignature);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t3_CannotAddSameSocialSignautreOnDifferentAccount()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(kLength);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(kLength + 1);

            // Create First Sign (Sign1)
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Create new account with different sign (Sign2)
            tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            // Add Try to add (Sign1) to (Sign2)
            tOperation = NWDUnityEngine.Instance.AccountManager.AddFacebookSignature(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t4_CanMoveSocialSignautreOnDifferentAccount()
        {
            string tSocialSignature1 = NWDToolbox.RandomStringUnix(kLength);
            string tSocialSignature2 = NWDToolbox.RandomStringUnix(kLength + 1);
            string tSocialSignature3 = NWDToolbox.RandomStringUnix(kLength + 2);

            // Create First Sign (Sign1)
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            // Add Try to add (Sign2) to (Sign1)
            tOperation = NWDUnityEngine.Instance.AccountManager.AddFacebookSignature(tSocialSignature2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Create new account with different sign (Sign3)
            tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(tSocialSignature3);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            // Add Try to add (Sign1) to (Sign3)
            tOperation = NWDUnityEngine.Instance.AccountManager.AddFacebookSignature(tSocialSignature1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }
    }
}
