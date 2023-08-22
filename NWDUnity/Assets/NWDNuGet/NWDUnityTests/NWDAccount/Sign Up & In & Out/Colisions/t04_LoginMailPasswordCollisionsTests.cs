using NUnit.Framework;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using NWDUnityTests.Manager;
using System.Collections;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T1_SignUp_In_Out.Collisions
{
    public class t04_LoginMailPasswordCollisionsTests
    {
        /// <summary>
        /// The legnth is volontary different for any credential method to avoid random collisions.
        /// 
        /// It also offers a little bit of leeway (up to +4) to make non colliding random values.
        /// </summary>
        private const int kLength = 70;

        public class SignInCollisionTests
        {
            NWDAsyncOperation AccountCreation = null;
            string Login;
            string Mail;
            string Password;

            [OneTimeSetUp]
            public void OneTimeSetUp()
            {
                Login = NWDToolbox.RandomStringUnix(kLength);
                Mail = NWDToolbox.RandomStringUnix(kLength);
                Password = NWDToolbox.RandomStringUnix(kLength);
                AccountCreation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(Login, Mail, Password);
            }

            [UnityTest]
            public IEnumerator t01_DeviceId()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;
                if (tDeviceManager != null)
                {
                    tDeviceManager.ChangeDevice(Login, Password);
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignIn();
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t02_LoginPassword()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn(Login, Password);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t03_MailPassword()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(Login, Password);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t04_LoginMailPassword()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn(Login, Mail, Password);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t05_Facebook()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignIn(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t06_Discord()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DiscordSignIn(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t07_Google()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignIn(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t08_Apple()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AppleSignIn(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t09_Microsoft()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MicrosoftSignIn(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t10_Twitter()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.TwitterSignIn(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t11_LinkedIn()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LinkedInSignIn(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }
        }

        public class SignUpCollisionTests
        {
            NWDAsyncOperation AccountCreation = null;
            string Login;
            string Mail;
            string Password;

            [SetUp]
            public void SetUp()
            {
                Login = NWDToolbox.RandomStringUnix(kLength);
                Mail = NWDToolbox.RandomStringUnix(kLength);
                Password = NWDToolbox.RandomStringUnix(kLength);
                AccountCreation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(Login, Mail, Password);
            }

            [UnityTest]
            public IEnumerator t01_DeviceId()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;
                if (tDeviceManager != null)
                {
                    tDeviceManager.ChangeDevice(Login, Password);
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t02_LoginPassword()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignUp(Login, Password);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t03_MailPassword()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(Login, Password);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t04_LoginMailPassword()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(Login, Mail, Password);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t05_Facebook()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t06_Discord()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DiscordSignUp(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t07_Google()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t08_Apple()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AppleSignUp(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t09_Microsoft()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MicrosoftSignUp(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t10_Twitter()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.TwitterSignUp(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t11_LinkedIn()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LinkedInSignUp(Login);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t12_MailPasswordWithMailAsLogin()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(Mail, Password);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }
        }
    }
}