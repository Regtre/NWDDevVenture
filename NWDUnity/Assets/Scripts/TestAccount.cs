using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAccount : MonoBehaviour
{
    public Sprite Test1;
    public Texture Test2;

    List<NWDAccountSign> Signatures = new List<NWDAccountSign>();

    string Login;
    string Email;
    string Password;
    string SocialSignature;
    Vector2 ScrollView;

    string OperationMessage = "";
    string ErrorMessage = "";

    private void OnGUI()
    {
        NWDRequestPlayerToken tToken = NWDUnityEngine.Instance.AccountManager.GetPlayerToken();
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.Label(nameof(tToken.PlayerReference));
        GUILayout.Label(tToken.PlayerReference.ToString());
        if (GUILayout.Button("Copy"))
        {
            GUIUtility.systemCopyBuffer = tToken.PlayerReference.ToString();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.Label(nameof(tToken.Token));
        GUILayout.Label(tToken.Token);
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.MinWidth(600));
        Login = TestGUILayout.TextField(nameof(Login), Login);
        Email = TestGUILayout.TextField(nameof(Email), Email);
        Password = TestGUILayout.TextField(nameof(Password), Password);
        GUILayout.EndHorizontal();

        SocialSignature = TestGUILayout.TextField(nameof(SocialSignature), SocialSignature, GUI.skin.box);

        GUILayout.BeginHorizontal();

        SignInGUI();
        SignUpGUI();
        AddSignatureGUI();
        GetAndDeleteSignatureGUI();

        GUILayout.EndHorizontal();

        GetServicesGUI();

        GUILayout.Label(OperationMessage, GUI.skin.box);

        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            GUILayout.Label(ErrorMessage, GUI.skin.box);
        }

        GUILayout.EndVertical();
    }

    private void SignInGUI ()
    {
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.MinWidth(200));

        GUILayout.Label("Sign In");

        if (GUILayout.Button("Device"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignIn();
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.DeviceSignIn)));
        }

        if (GUILayout.Button("Login/Password"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn(Login, Password);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.LoginPasswordSignIn)));
        }

        if (GUILayout.Button("Email/Password"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn(Email, Password);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.MailPasswordSignIn)));
        }

        if (GUILayout.Button("Login/Email/Password"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn(Login, Email, Password);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignIn)));
        }

        if (GUILayout.Button("Facebook"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignIn(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.FacebookSignIn)));
        }

        if (GUILayout.Button("Discord"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DiscordSignIn(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.DiscordSignIn)));
        }

        if (GUILayout.Button("Google"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignIn(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.GoogleSignIn)));
        }

        if (GUILayout.Button("Apple"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AppleSignIn(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AppleSignIn)));
        }

        if (GUILayout.Button("Microsoft"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MicrosoftSignIn(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.MicrosoftSignIn)));
        }

        if (GUILayout.Button("Twitter"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.TwitterSignIn(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.TwitterSignIn)));
        }

        if (GUILayout.Button("LinkedIn"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LinkedInSignIn(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.LinkedInSignIn)));
        }

        GUILayout.EndVertical();
    }

    private void SignUpGUI ()
    {
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.MinWidth(200));

        GUILayout.Label("Sign Up");

        if (GUILayout.Button("Device"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DeviceSignUp();
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.DeviceSignUp)));
        }

        if (GUILayout.Button("Login/Password"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginPasswordSignUp(Login, Password);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.LoginPasswordSignUp)));
        }

        if (GUILayout.Button("Email/Password"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp(Email, Password);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.MailPasswordSignUp)));
        }

        if (GUILayout.Button("Login/Email/Password"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp(Login, Email, Password);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.LoginMailPasswordSignUp)));
        }

        if (GUILayout.Button("Facebook"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.FacebookSignUp(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.FacebookSignUp)));
        }

        if (GUILayout.Button("Discord"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.DiscordSignUp(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.DiscordSignUp)));
        }

        if (GUILayout.Button("Google"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.GoogleSignUp(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.GoogleSignUp)));
        }

        if (GUILayout.Button("Apple"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AppleSignUp(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AppleSignUp)));
        }

        if (GUILayout.Button("Microsoft"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.MicrosoftSignUp(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.MicrosoftSignUp)));
        }

        if (GUILayout.Button("Twitter"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.TwitterSignUp(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.TwitterSignUp)));
        }

        if (GUILayout.Button("LinkedIn"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.LinkedInSignUp(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.LinkedInSignUp)));
        }

        GUILayout.EndVertical();
    }

    private void AddSignatureGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.MinWidth(200));

        GUILayout.Label("Add signature");

        if (GUILayout.Button("Device"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddDeviceSignature();
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddDeviceSignature)));
        }

        if (GUILayout.Button("Login/Password"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginPasswordSignature(Login, Password);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddLoginPasswordSignature)));
        }

        if (GUILayout.Button("Email/Password"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddMailPasswordSignature(Email, Password);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddMailPasswordSignature)));
        }

        if (GUILayout.Button("Login/Email/Password"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddLoginMailPasswordSignature(Login, Email, Password);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddLoginMailPasswordSignature)));
        }

        if (GUILayout.Button("Facebook"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddFacebookSignature(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddFacebookSignature)));
        }

        if (GUILayout.Button("Discord"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddDiscordSignature(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddDiscordSignature)));
        }

        if (GUILayout.Button("Google"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddGoogleSignature(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddGoogleSignature)));
        }

        if (GUILayout.Button("Apple"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddAppleSignature(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddAppleSignature)));
        }

        if (GUILayout.Button("Microsoft"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddMicrosoftSignature(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddMicrosoftSignature)));
        }

        if (GUILayout.Button("Twitter"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddTwitterSignature(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddTwitterSignature)));
        }

        if (GUILayout.Button("LinkedIn"))
        {
            NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.AddLinkedInSignature(SocialSignature);
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.AddLinkedInSignature)));
        }

        GUILayout.EndVertical();
    }

    private void GetAndDeleteSignatureGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.MinWidth(400));

        GUILayout.Label("Delete signatures");

        if (GUILayout.Button("Refresh"))
        {
            NWDAsyncOperation<List<NWDAccountSign>> tOperation = NWDUnityEngine.Instance.AccountManager.GetAccountSignatures();
            tOperation.OnDone += (_) =>
            {
                if (tOperation.State == NWDAsyncOperationState.Success)
                {
                    Signatures = tOperation.Result;
                }
                else
                {
                    Signatures.Clear();
                }
            };
            StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.GetAccountSignatures)));
        }

        ScrollView = GUILayout.BeginScrollView(ScrollView, GUILayout.MaxHeight(250));

        foreach (NWDAccountSign tSign in Signatures)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label (tSign.SignType.ToString());
            GUILayout.Label (tSign.LoginHash);
            GUI.enabled = tSign.SignType == NWDAccountSignType.EmailPassword || tSign.SignType == NWDAccountSignType.LoginPassword || tSign.SignType == NWDAccountSignType.LoginEmailPassword;
            if (GUILayout.Button("Edit"))
            {
                NWDAsyncOperation tOperation;
                switch (tSign.SignType)
                {
                    case NWDAccountSignType.LoginPassword:
                        tOperation = NWDUnityEngine.Instance.AccountManager.EditLoginPasswordSignature(tSign, Login, Password);
                        StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.EditLoginPasswordSignature)));
                        break;
                    case NWDAccountSignType.EmailPassword:
                        tOperation = NWDUnityEngine.Instance.AccountManager.EditMailPasswordSignature(tSign, Email, Password);
                        StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.EditMailPasswordSignature)));
                        break;
                    case NWDAccountSignType.LoginEmailPassword:
                        tOperation = NWDUnityEngine.Instance.AccountManager.EditLoginMailPasswordSignature(tSign, Login, Email, Password);
                        StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.EditLoginMailPasswordSignature)));
                        break;
                }
            }
            GUI.enabled = true;
            if (GUILayout.Button("Remove"))
            {
                NWDAsyncOperation tOperation = NWDUnityEngine.Instance.AccountManager.RemoveSignature(tSign);
                StartCoroutine(RequestProcessing(tOperation, nameof(NWDUnityEngine.Instance.AccountManager.RemoveSignature)));
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();

        GUILayout.EndVertical();
    }

    private void GetServicesGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.MinWidth(400));

        GUILayout.Label("Services");
        GUILayout.BeginHorizontal();
        GUILayout.Label(nameof(NWDAccountService.Name));
        GUILayout.Label(nameof(NWDAccountService.Service));
        GUILayout.Label(nameof(NWDAccountService.Start));
        GUILayout.Label(nameof(NWDAccountService.End));
        GUILayout.Label(nameof(NWDAccountService.Duration));
        GUILayout.Label(nameof(NWDAccountService.OfflineCounterDown));
        GUILayout.Label(nameof(NWDAccountService.Active));
        GUILayout.EndHorizontal();

        foreach (NWDAccountService tService in NWDUnityEngine.Instance.AccountManager.GetAccountServices())
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(tService.Name);
            GUILayout.Label(tService.Service.ToString());
            GUILayout.Label(tService.Start.ToString());
            GUILayout.Label(tService.End.ToString());
            GUILayout.Label(tService.Duration.ToString());
            GUILayout.Label(tService.OfflineCounterDown.ToString());
            GUILayout.Label(tService.Active.ToString());
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }

    //private string ShortenedString (string sValue, uint maxChar)
    //{

    //}

    private IEnumerator RequestProcessing (NWDAsyncOperation sOperation, string sName)
    {
        ErrorMessage = "";

        while (!sOperation.IsDone)
        {
            OperationMessage = "Operation `" + sName + "` progress: " + sOperation.Progress*100 + "%";
            yield return null;
        }

        OperationMessage = "Operation `" + sName + "` finished with state: " + sOperation.State;
        if (sOperation.State == NWDAsyncOperationState.Failure)
        {
            ErrorMessage = sOperation.Error.Message;
            Debug.LogException(sOperation.Error);
        }
    }
}
