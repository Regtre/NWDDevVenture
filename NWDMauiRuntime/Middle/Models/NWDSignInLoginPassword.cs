namespace NWDAppRuntime.Models;

public class NWDSignInLoginPassword
{
    public string Login { get; set; }
    public string Password{ get; set; }

    public NWDSignInLoginPassword(string sLogin, string sPassword)
    {
        Login = sLogin;
        Password = sPassword;
    }
}