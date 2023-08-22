using NUnit.Framework;
using NWDFoundation.Exchanges;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using System.Collections;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T1_SignUp_In_Out.Process
{
    public class t03_MailPasswordProcessTests
    {
        /// <summary>
        /// The legnth is volontary different for any credential method to avoid random collisions.
        /// 
        /// It also offers a little bit of leeway (up to +4) to make non colliding random values.
        /// </summary>
        private const int kLength = 65;

        [UnityTest]
        public IEnumerator t1_SignUpWithMailPassword()
        {
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t2_CannotSignUpWithSameMailPassword()
        {
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t3_SignUpWhenConnectedWithMailPassword()
        {
            string tMail1 = NWDToolbox.RandomStringUnix(kLength);
            string tMail2 = NWDToolbox.RandomStringUnix(kLength + 1);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail1, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail2, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t4_CanSignUpWithDifferentMailCase()
        {
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail.ToUpper(), tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail.ToLower(), tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t5_CannotSignInWithDifferentMailCase()
        {
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail.ToUpper(), tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail.ToLower(), tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t6_CannotSignInWithDifferentPassword()
        {
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword1 = NWDToolbox.RandomStringUnix(kLength);
            string tPassword2 = NWDToolbox.RandomStringUnix(kLength + 1);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail, tPassword1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail, tPassword2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
        }

        [UnityTest]
        public IEnumerator t7_CannotSignUpWithDifferentPasswordCase()
        {
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail, tPassword.ToUpper());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail, tPassword.ToLower());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
        }

        [UnityTest]
        public IEnumerator t8_CannotSignInWithDifferentPasswordCase()
        {
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(tMail, tPassword.ToUpper());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(tMail, tPassword.ToLower());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
        }
    }
}