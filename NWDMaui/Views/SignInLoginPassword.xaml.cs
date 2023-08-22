using NWDAppRuntime.Middle;
using NWDAppRuntime.Middle.Facades;
using NWDAppRuntime.Models;

namespace NWDMaui.Views;

public partial class SignInLoginPassword : ContentPage
{
    private readonly INWDAccountManager _AccountManager; 

    public SignInLoginPassword(/*INWDAccountManager sAccountManager*/)
    {
        InitializeComponent();
        _AccountManager = new NWDAccountManager(); 
    }
    
    private async void SignIn_Request(object sSender, EventArgs sE)
    {
        
        if (_AccountManager.SignInWithLoginPassword(new NWDSignInLoginPassword(Login.Text, Password.Text)))
        {
            await Shell.Current.GoToAsync(nameof(NWDProfile));
        }
        
        
    }
}