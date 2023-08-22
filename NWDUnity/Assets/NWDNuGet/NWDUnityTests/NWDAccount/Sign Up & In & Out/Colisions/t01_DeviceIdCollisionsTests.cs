using NUnit.Framework;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using NWDUnityTests.Manager;
using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine.TestTools;

namespace NWDUnityTests.F1_Accounts.T1_SignUp_In_Out.Collisions
{
    public class t01_DeviceIdCollisionsTests
    {
        public class SignInCollisionTests
        {
            NWDAsyncOperation AccountCreation = null;
            string DeviceId;

            [OneTimeSetUp]
            public void OneTimeSetUp()
            {
                AccountCreation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
                DeviceId = NWDUnityEngine.Instance.DeviceManager.GetDeviceId();
            }

            [UnityTest]
            public IEnumerator t01_DeviceId()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn(DeviceId, DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(DeviceId, DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn(DeviceId, DeviceId, DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignIn(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DiscordSignIn(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignIn(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AppleSignIn(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MicrosoftSignIn(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.TwitterSignIn(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LinkedInSignIn(DeviceId);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Warning, tOperation.State);
            }
        }

        public class SignUpCollisionTests
        {
            NWDAsyncOperation AccountCreation = null;
            string DeviceId;

            [SetUp]
            public void SetUp()
            {
                NWDUnityTestsDeviceManager tDeviceManager = NWDUnityEngine.Instance.DeviceManager as NWDUnityTestsDeviceManager;
                if (tDeviceManager != null)
                {
                    tDeviceManager.RegenerateDevice();
                }

                AccountCreation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
                DeviceId = NWDUnityEngine.Instance.DeviceManager.GetDeviceId();
            }

            [UnityTest]
            public IEnumerator t01_DeviceId()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Failure, tOperation.State);
            }

            [UnityTest]
            public IEnumerator t02_LoginPassword()
            {
                if (!AccountCreation.IsDone)
                {
                    yield return AccountCreation;
                }

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignUp(DeviceId, DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(DeviceId, DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(DeviceId, DeviceId, DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DiscordSignUp(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AppleSignUp(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MicrosoftSignUp(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.TwitterSignUp(DeviceId);
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

                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LinkedInSignUp(DeviceId);
                yield return tOperation;
                Assert.AreEqual(NWDAsyncOperationState.Success, tOperation.State);
            }
        }
    }
}