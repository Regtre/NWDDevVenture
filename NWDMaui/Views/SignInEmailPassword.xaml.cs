using NWDAppRuntime.Middle;
using NWDAppRuntime.Middle.Facades;
using NWDAppRuntime.Models;

namespace NWDMaui.Views;

public partial class SignInEmailPassword : ContentPage
{
    private readonly INWDAccountManager _AccountManager; 

    public SignInEmailPassword(/*INWDAccountManager sAccountManager*/)
    {
        InitializeComponent();
        _AccountManager = new NWDAccountManager();
    }

    private async void SignIn_Request(object sSender, EventArgs sE)
    {
        
        if (_AccountManager.SignInWithEmailPassword(new NWDSignInEmailPassword(UserName.Text, Password.Text)))
        {
            await Shell.Current.GoToAsync(nameof(NWDProfile));
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(SignInEmailPassword));
        }
        
    }
}