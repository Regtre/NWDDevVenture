using NWDAppRuntime.Middle;
using NWDAppRuntime.Middle.Facades;
using NWDAppRuntime.Models;

namespace NWDMauiDemo.Views;

public partial class SignModifyLoginPassword : ContentPage
{
    private INWDAccountManager _AccountManager; 
    public SignModifyLoginPassword(/*INWDAccountManager sAccountManager*/)
    {
        InitializeComponent();
        _AccountManager = new NWDAccountManager(); 
    }

    private async void SignModify(object sSender, EventArgs sE)
    {
        if (_AccountManager.SignModifyLoginPassword(new NWDSignUpLoginPassword(OldLogin.Text, OldPassword.Text),
                new NWDSignUpLoginPassword(NewLogin.Text, NewPassword.Text)))
        {
            await Shell.Current.GoToAsync(nameof(Profile));

        }
            
    }
}