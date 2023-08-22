using NWDAppRuntime.Middle;
using NWDAppRuntime.Middle.Facades;
using NWDAppRuntime.Models;
using NWDFoundation.Models.Enums;

namespace NWDMauiDemo.Views;

public partial class SignUp : ContentPage
{
    private readonly NWDAccountSignType _Type;
    private INWDAccountManager _AccountManager; 

    public SignUp(/*INWDAccountManager sAccountManager*/)
    {
        InitializeComponent();
        _Type = NWDAccountSignType.EmailPassword;

        _AccountManager = new NWDAccountManager(); 
    }
   

    private async void SignUp_Request(object sSender, EventArgs sEventArgs)
    {
        bool result = false;  
        switch (_Type)
        {
            case NWDAccountSignType.EmailPassword:
                result = _AccountManager.SignUpWithEmailPassword(new NWDSignUpEmailPassword(UserName.Text, Password.Text));
                break; 
            case NWDAccountSignType.LoginPassword:
                result = _AccountManager.SignUpWithLoginPassword(new NWDSignUpLoginPassword(UserName.Text, Password.Text));
                break;
        }

        if (result)
        {
            await Shell.Current.GoToAsync(nameof(SignUp));
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(SignInEmailPassword));
        }

    }
}