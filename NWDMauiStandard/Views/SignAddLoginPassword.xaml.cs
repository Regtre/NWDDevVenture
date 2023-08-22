using NWDAppRuntime.Middle;
using NWDAppRuntime.Middle.Facades;
using NWDAppRuntime.Models;
using NWDFoundation.Exchanges;

namespace NWDMauiDemo.Views;

public partial class SignAddLoginPassword : ContentPage
{
    private readonly INWDAccountManager _AccountManager; 
    public SignAddLoginPassword(/*INWDAccountManager sAccountManager*/)
    {
        InitializeComponent();
        _AccountManager = new NWDAccountManager(); 
    }

    private async void SignAdd(object sSender, EventArgs sE)
    {
        if (_AccountManager.AddLoginPassword(new NWDSignUpLoginPassword(Login.Text, Password.Text)).Status ==
            NWDRequestStatus.Ok)
        {
            await Shell.Current.GoToAsync(nameof(Profile));
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(SignInEmailPassword));
        }
    }
}