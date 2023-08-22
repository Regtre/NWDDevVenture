using NUnit.Framework;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using NWDUnityTests.Manager;
using System.Collections;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T1_SignUp_In_Out.Collisions
{
    public class t06_DiscordCollisionsTests
    {
        /// <summary>
        /// The legnth is volontary different for any credential method to avoid random collisions.
        /// 
        /// It also offers a little bit of leeway (up to +4) to make non colliding random values.
        /// </summary>
        private const int kLength = 80;

        public class SignInCollisionTests
        {
            NWDAsyncOperation AccountCreation = null;
            string SocialSignature;

            [OneTimeSetUp]
            public void OneTimeSetUp()
            {
                SocialSignature = NWDToolbox.RandomStringUnix(kLength);
                AccountCreation = NWDUnityEngine.Instance.AccountManager.DiscordSignUp(SocialSignature);
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
                    tDeviceManager.ChangeDevice(SocialSignature, SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn(SocialSignature, SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(SocialSignature, SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn(SocialSignature, SocialSignature, SocialSignature);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t05_Facebook()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignIn(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DiscordSignIn(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignIn(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AppleSignIn(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MicrosoftSignIn(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.TwitterSignIn(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LinkedInSignIn(SocialSignature);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }
        }

        public class SignUpCollisionTests
        {
            NWDAsyncOperation AccountCreation = null;
            string SocialSignature;

            [SetUp]
            public void SetUp()
            {
                SocialSignature = NWDToolbox.RandomStringUnix(kLength);
                AccountCreation = NWDUnityEngine.Instance.AccountManager.DiscordSignUp(SocialSignature);
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
                    tDeviceManager.ChangeDevice(SocialSignature, SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignUp(SocialSignature, SocialSignature);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t03_MailPassword()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(SocialSignature, SocialSignature);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t04_LoginMailPassword()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(SocialSignature, SocialSignature, SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DiscordSignUp(SocialSignature);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t07_Google()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AppleSignUp(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MicrosoftSignUp(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.TwitterSignUp(SocialSignature);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LinkedInSignUp(SocialSignature);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }
        }
    }
}