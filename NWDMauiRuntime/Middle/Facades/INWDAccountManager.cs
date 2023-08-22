using NWDAppRuntime.Models;
using NWDFoundation.Models;
using NWDRuntime.Exchanges;

namespace NWDAppRuntime.Middle.Facades;

public interface INWDAccountManager
{
    public NWDResponseRuntime TestRequest();
    public  bool SignInWithLoginPassword(NWDSignInLoginPassword sSignInLoginPassword);
    public bool SignUpWithLoginPassword(NWDSignUpLoginPassword sSignUpLoginPassword);
    public  NWDResponseRuntime AddLoginPassword(NWDSignUpLoginPassword sSignUpLoginPassword);
    public bool SignInWithEmailPassword(NWDSignInEmailPassword sSignInEmailPassword);
    public  bool SignUpWithEmailPassword(NWDSignUpEmailPassword sSignUpEmailPassword);
    public NWDResponseRuntime AddEmailPasswordSign(NWDSignUpEmailPassword sSignUpEmailPassword);

    public bool SignModifyEmailPassword(string sOldEmail, string sOldPassword, string sNewEmail,
        string sNewPassword);

    public bool SignModifyLoginPassword(NWDSignUpLoginPassword sOldSign, NWDSignUpLoginPassword sNewSign);

    public void SignOut();

    public List<NWDAccountSign> GetSigns();
    public List<NWDAccountService> GetServices();
}