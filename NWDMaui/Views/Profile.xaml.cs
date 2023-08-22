using NWDAppRuntime.Middle;
using NWDMaui.ViewModels;

namespace NWDMaui.Views;

public partial class NWDProfile 
{
    public NWDProfile(/*ProfileViewModel sProfileViewModel*/)
    {
        InitializeComponent();
        BindingContext = new ProfileViewModel(new NWDAccountManager());
    }
    
    private async void GoToRegisterLoginPassword(object sSender, EventArgs sEventArgs)
    {
        //await Navigation.PushModalAsync(new SignInWithLoginPassword());
        await Shell.Current.GoToAsync("RegisterLoginPassword");
    }

    private async void SignModifyEmailPassword(object sSender, EventArgs sEventArgs)
    {
        await Shell.Current.GoToAsync(nameof(SignModifyEmailPassword));

    }
    private async void GoToAddSignEmailPassword(object sSender, EventArgs sEventArgs)
    {
        await Shell.Current.GoToAsync(nameof(SignAddEmailPassword));
    }
    private async void GoToAddSignLoginPassword(object sSender, EventArgs sEventArgs)
    {
        await Shell.Current.GoToAsync(nameof(SignAddLoginPassword));
    }

    private async void SignOut(object sSender, EventArgs sEventArgs)
    {
        Preferences.Clear();
        await Shell.Current.GoToAsync(nameof(SignInEmailPassword));
    }

    private async void SignModifyLoginPassword(object sSender, EventArgs sE)
    {
        await Shell.Current.GoToAsync(nameof(SignModifyLoginPassword));
    }
    
    private async void SignModify(object sSender, EventArgs sE)
    {
        await Shell.Current.GoToAsync(nameof(SignModifyLoginPassword));
    }
}