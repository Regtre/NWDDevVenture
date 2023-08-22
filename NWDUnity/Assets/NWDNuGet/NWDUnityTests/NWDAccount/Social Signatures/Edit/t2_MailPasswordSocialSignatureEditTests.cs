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
    public class t2_MailPasswordSocialSignatureEditTests
    {
        /// <summary>
        /// The legnth is volontary different for any credential method to avoid random collisions.
        /// 
        /// It also offers a little bit of leeway (up to +4) to make non colliding random values.
        /// </summary>
        private const int kLength = 65;

        [UnityTest]
        public IEnumerator t1_CanEditMail()
        {
            string tMail1 = NWDToolbox.RandomStringUnix(kLength);
            string tMail2 = NWDToolbox.RandomStringUnix(kLength+1);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail1, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.EditMailPasswordSignature(tSignOperation.Result[0], tMail2, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail1, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail2, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t2_CanEditPassword()
        {
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword1 = NWDToolbox.RandomStringUnix(kLength);
            string tPassword2 = NWDToolbox.RandomStringUnix(kLength+1);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail, tPassword1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.EditMailPasswordSignature(tSignOperation.Result[0], tMail, tPassword2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail, tPassword1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail, tPassword2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t3_CanEditMailAndPassword()
        {
            string tMail1 = NWDToolbox.RandomStringUnix(kLength);
            string tMail2 = NWDToolbox.RandomStringUnix(kLength+1);
            string tPassword1 = NWDToolbox.RandomStringUnix(kLength);
            string tPassword2 = NWDToolbox.RandomStringUnix(kLength + 1);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail1, tPassword1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            NWDAsyncOperation<List<NWDAccountSign>> tSignOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            yield return tSignOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tSignOperation.State);
            Assert.AreEqual(1, tSignOperation.Result.Count);

            tOperation = NWDUnityEngine.Instance.AccountManager.EditMailPasswordSignature(tSignOperation.Result[0], tMail2, tPassword2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail1, tPassword1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail2, tPassword2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }
    }
}