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
    public class t04_LoginMailPasswordSocialSignatureAddTests
    {
        /// <summary>
        /// The legnth is volontary different for any credential method to avoid random collisions.
        /// 
        /// It also offers a little bit of leeway (up to +4) to make non colliding random values.
        /// </summary>
        private const int kLength = 70;

        [UnityTest]
        public IEnumerator t1_CanAddSameSocialSignautreType()
        {
            string tLogin1 = NWDToolbox.RandomStringUnix(kLength);
            string tLogin2 = NWDToolbox.RandomStringUnix(kLength + 1);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin1, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginMailPasswordSignature(tLogin2, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t2_CannotAddSameSocialSignautreOnSameAccount()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginMailPasswordSignature(tLogin, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t3_CannotAddSameSocialSignautreOnDifferentAccount()
        {
            string tLogin1 = NWDToolbox.RandomStringUnix(kLength);
            string tLogin2 = NWDToolbox.RandomStringUnix(kLength + 1);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            // Create First Sign (Sign1)
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin1, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Create new account with different sign (Sign2)
            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin2, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            // Add Try to add (Sign1) to (Sign2)
            tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginMailPasswordSignature(tLogin1, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t4_CanMoveSocialSignautreOnDifferentAccount()
        {
            string tLogin1 = NWDToolbox.RandomStringUnix(kLength);
            string tLogin2 = NWDToolbox.RandomStringUnix(kLength + 1);
            string tLogin3 = NWDToolbox.RandomStringUnix(kLength + 2);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            // Create First Sign (Sign1)
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin1, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            // Add Try to add (Sign2) to (Sign1)
            tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginMailPasswordSignature(tLogin2, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Remove Sign1 from Account
            tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSignOperation.Result[0]);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            // Create new account with different sign (Sign3)
            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin3, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            // Add Try to add (Sign1) to (Sign3)
            tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginMailPasswordSignature(tLogin1, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }
    }
}
