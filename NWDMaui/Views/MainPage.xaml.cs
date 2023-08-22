
using NWDAppRuntime.Middle;
using NWDAppRuntime.Middle.Facades;

namespace NWDMaui.Views;

public partial class MainPage : ContentPage
{
	private INWDAccountManager _AccountManager; 
	public MainPage(/*INWDAccountManager sAccountManager*/)
	{
		_AccountManager = new NWDAccountManager(); 
		InitializeComponent();
		if (!NWDAccountManager.IsConnected())
		{
			Profile.IsVisible = false;
		}
	}

	private void TestRequest(object sender, EventArgs e)
	{
		Status.Text = "Sending  Request... ";
		SemanticScreenReader.Announce(Status.Text);

		Status.Text = _AccountManager.TestRequest().Status.ToString();
		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private async void GoToSignInLoginPassword(object sSender, EventArgs sEventArgs)
	{
		await Shell.Current.GoToAsync(nameof(SignInLoginPassword));
	}
	private async void GoToSignInEmailPassword(object sSender, EventArgs sEventArgs)
	{
		await Shell.Current.GoToAsync(nameof(SignInEmailPassword));
	}

	private async void GoToSignUp(object sSender, EventArgs sEventArgs)
	{
		//await Navigation.PushModalAsync(new SignUpWithLoginPassword());
		await Shell.Current.GoToAsync(nameof(SignUp));

	}

	private void Clear_OnClicked(object sSender, EventArgs sE)
	{
		Preferences.Clear();
	}

	private async void GoToProfile(object sSender, EventArgs sE)
	{
		if (NWDAccountManager.IsConnected())
		{
			await Shell.Current.GoToAsync(nameof(NWDProfile));
		}
		else
		{
			await Shell.Current.GoToAsync(nameof(SignInEmailPassword));
		}
	}
}
	

