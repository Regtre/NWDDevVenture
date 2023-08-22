using NWDMaui.Views;
using Profile = Microsoft.Maui.Controls.Internals.Profile;

namespace NWDMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(SignUp), typeof(SignUp));
        Routing.RegisterRoute(nameof(NWDProfile), typeof(NWDProfile));
        Routing.RegisterRoute(nameof(SignModifyEmailPassword), typeof(SignModifyEmailPassword));
        Routing.RegisterRoute(nameof(SignModifyLoginPassword), typeof(SignModifyLoginPassword));
        Routing.RegisterRoute(nameof(SignInLoginPassword), typeof(SignInLoginPassword));
        Routing.RegisterRoute(nameof(SignInEmailPassword), typeof(SignInEmailPassword));
        Routing.RegisterRoute(nameof(SignAddLoginPassword),typeof(SignAddLoginPassword));
        Routing.RegisterRoute(nameof(SignAddEmailPassword),typeof(SignAddEmailPassword));
    }
}