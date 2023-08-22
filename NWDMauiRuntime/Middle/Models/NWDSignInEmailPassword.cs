namespace NWDAppRuntime.Models;

public class NWDSignInEmailPassword
{
    public string Email { get; set; }
    public string Password{ get; set; }

    public NWDSignInEmailPassword(string sEmail, string sPassword)
    {
        Email = sEmail;
        Password = sPassword;
    }
}