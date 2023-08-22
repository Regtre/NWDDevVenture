namespace NWDAppRuntime.Models;

public class NWDSignUpEmailPassword
{
    public string Email;
    public string Password;

    public NWDSignUpEmailPassword(string sEmail,string sPassword)
    {
        Email = sEmail;
        Password = sPassword;
    }

}