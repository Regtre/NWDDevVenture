using NWDAppRuntime.Middle;
using NWDAppRuntime.Middle.Facades;

namespace NWDMauiDemo.Views;

public partial class SignModifyEmailPassword : ContentPage
{
    public INWDAccountManager _AccountManager; 
    public SignModifyEmailPassword(/*INWDAccountManager sAccountManager*/)
    {
        InitializeComponent();
        _AccountManager = new NWDAccountManager(); 
    }

    private async void SignModify(object sSender, EventArgs sE)
    {
        if (_AccountManager.SignModifyEmailPassword(OldEmail.Text, OldPassword.Text, NewEmail.Text, NewPassword.Text))
        {
            await Shell.Current.GoToAsync(nameof(Profile));

        }
    }
}