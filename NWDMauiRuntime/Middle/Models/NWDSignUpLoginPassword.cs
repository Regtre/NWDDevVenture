namespace NWDAppRuntime.Models;

public class NWDSignUpLoginPassword
{
    public string Login { get; set; }
    public string Password{ get; set; }

    public NWDSignUpLoginPassword(string sLogin, string sPassword)
    {
        Login = sLogin;
        Password = sPassword; 
    }

}