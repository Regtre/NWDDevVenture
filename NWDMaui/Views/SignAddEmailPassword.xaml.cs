using NWDAppRuntime.Middle;
using NWDAppRuntime.Middle.Facades;
using NWDAppRuntime.Models;
using NWDFoundation.Exchanges;

namespace NWDMaui.Views;

public partial class SignAddEmailPassword : ContentPage
{
    private INWDAccountManager _AccountManager; 
    public SignAddEmailPassword(/*INWDAccountManager sAccountManager*/)
    {
        InitializeComponent();
        _AccountManager = new NWDAccountManager(); 
    }

    
    private async void SignAdd(object sSender, EventArgs sE)
    {
        if (_AccountManager.AddEmailPasswordSign(new NWDSignUpEmailPassword(Email.Text, Password.Text)).Status == NWDRequestStatus.Ok)
        {
            await Shell.Current.GoToAsync(nameof(NWDProfile));
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(SignInEmailPassword));
        }
    }
}