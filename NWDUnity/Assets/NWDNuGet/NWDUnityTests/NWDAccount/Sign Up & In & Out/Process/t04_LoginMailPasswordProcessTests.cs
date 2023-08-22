using NUnit.Framework;
using NWDFoundation.Exchanges;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using System.Collections;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T1_SignUp_In_Out.Process
{
    public class t04_LoginMailPasswordProcessTests
    {
        /// <summary>
        /// The legnth is volontary different for any credential method to avoid random collisions.
        /// 
        /// It also offers a little bit of leeway (up to +4) to make non colliding random values.
        /// </summary>
        private const int kLength = 70;

        [UnityTest]
        public IEnumerator t01_SignUpWithLoginMailPassword()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn(tLogin, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t02_CannotSignUpWithSameLoginMailPassword()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t03_SignUpWhenConnectedWithLoginMailPassword()
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

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin2, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t04_CanSignUpWithDifferentLoginCase()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin.ToUpper(), tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin.ToLower(), tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t05_CannotSignInWithDifferentLoginCase()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin.ToUpper(), tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignUpPlayerReference = tToken.PlayerReference;

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn(tLogin.ToLower(), tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }

        [UnityTest]
        public IEnumerator t06_CannotSignInWithDifferentPassword()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword1 = NWDToolbox.RandomStringUnix(kLength);
            string tPassword2 = NWDToolbox.RandomStringUnix(kLength + 1);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin, tMail, tPassword1);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn(tLogin, tMail, tPassword2);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
        }

        [UnityTest]
        public IEnumerator t07_CannotSignUpWithDifferentPasswordCase()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin, tMail, tPassword.ToUpper());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn(tLogin, tMail, tPassword.ToLower());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
        }

        [UnityTest]
        public IEnumerator t08_CannotSignInWithDifferentPasswordCase()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin, tMail, tPassword.ToUpper());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn(tLogin, tMail, tPassword.ToLower());
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
        }

        [UnityTest]
        public IEnumerator t09_CannotSignUpWithDifferentMailCase()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin, tMail.ToUpper(), tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin, tMail.ToLower(), tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);
        }

        [UnityTest]
        public IEnumerator t10_CannotSignInWithDifferentMailCase()
        {
            string tLogin = NWDToolbox.RandomStringUnix(kLength);
            string tMail = NWDToolbox.RandomStringUnix(kLength);
            string tPassword = NWDToolbox.RandomStringUnix(kLength);

            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin, tMail.ToUpper(), tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn(tLogin, tMail.ToLower(), tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
        }

        [UnityTest]
        public IEnumerator t11_CanSignInWithDifferentLoginAndSameMail()
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

            tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(tLogin2, tMail, tPassword);
            yield return tOperation;
            Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);

            tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
            ulong tSignInPlayerReference = tToken.PlayerReference;

            Assert.AreNotEqual(tSignUpPlayerReference, tSignInPlayerReference);
        }
    }
}