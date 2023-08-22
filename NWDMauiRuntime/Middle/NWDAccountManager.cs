using Newtonsoft.Json;
using NWDAppRuntime.Middle.Facades;
using NWDAppRuntime.Models;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDMauiRuntime.Back;
using NWDMauiRuntime.Configurations;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;

namespace NWDAppRuntime.Middle;

public class NWDAccountManager : INWDAccountManager
{
    public NWDAccountManager()
    {
    }

    public NWDResponseRuntime TestRequest()
    {
        NWDRequestRuntime tRequestRuntime =
            NWDRequestRuntime.CreateRequestTest(NWDMauiRuntimeConfiguration.KConfig, NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(), NWDExchangeOrigin.App, NWDExchangeDevice.Unknown);
        NWDResponseRuntime tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(tRequestRuntime).Result;
        return tResponseRuntime;
    }


    #region LoginPassword

    public bool SignInWithLoginPassword(NWDSignInLoginPassword sSignInLoginPassword)
    {
        NWDResponseRuntime tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(NWDRequestRuntime.CreateRequestSignIn(NWDMauiRuntimeConfiguration.KConfig, 
            NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),
            NWDAccountSign.CreateLoginPassword(sSignInLoginPassword.Login, sSignInLoginPassword.Password,
                NWDMauiRuntimeConfiguration.KConfig.ProjectId), NWDExchangeOrigin.App, NWDExchangeDevice.Unknown)).Result;
        
        //NWDDataAppManager.FastSync();
        return tResponseRuntime.IsValid(NWDMauiRuntimeConfiguration.KConfig) && tResponseRuntime.Status == NWDRequestStatus.Ok;
    }

    public bool SignUpWithLoginPassword(NWDSignUpLoginPassword sSignUpLoginPassword)
    {
        NWDResponseRuntime tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(NWDRequestRuntime.CreateRequestSignUp(NWDMauiRuntimeConfiguration.KConfig,
            NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),
            NWDAccountSign.CreateLoginPassword(sSignUpLoginPassword.Login, sSignUpLoginPassword.Password,
                NWDMauiRuntimeConfiguration.KConfig.ProjectId), NWDExchangeOrigin.App, NWDExchangeDevice.Unknown)).Result;
        return tResponseRuntime.IsValid(NWDMauiRuntimeConfiguration.KConfig) && tResponseRuntime.Status == NWDRequestStatus.Ok;
    }

    public NWDResponseRuntime AddLoginPassword(NWDSignUpLoginPassword sSignUpLoginPassword)
    {
        NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignAdd(
            NWDMauiRuntimeConfiguration.KConfig, NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),
            NWDAccountSign.CreateLoginPassword(sSignUpLoginPassword.Login, sSignUpLoginPassword.Password,
                NWDMauiRuntimeConfiguration.KConfig.ProjectId), NWDExchangeOrigin.App, NWDExchangeDevice.Unknown);
        NWDResponseRuntime tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(tRequestRuntime).Result;

        return tResponseRuntime;
    }
    public bool SignModifyLoginPassword(NWDSignUpLoginPassword sOldSign, NWDSignUpLoginPassword sNewSign)
    {
        NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignModify(
            NWDMauiRuntimeConfiguration.KConfig, NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),
            NWDAccountSign.CreateEmailPassword(sOldSign.Login, sOldSign.Password,
                NWDMauiRuntimeConfiguration.KConfig.ProjectId),
            NWDAccountSign.CreateEmailPassword(sNewSign.Login, sNewSign.Password,
                NWDMauiRuntimeConfiguration.KConfig.ProjectId), NWDExchangeOrigin.App, NWDExchangeDevice.Unknown);
        return NWDRuntimeCallback.SharedInstance().PostRequest(tRequestRuntime).Result.Status == NWDRequestStatus.Ok;
    }

   

    public bool SignDeleteLoginPassword(NWDSignUpLoginPassword sSign)
    {
        NWDResponseRuntime tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(NWDRequestRuntime.CreateRequestSignDelete(NWDMauiRuntimeConfiguration.KConfig ,NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),
            NWDAccountSign.CreateLoginPassword(sSign.Login, sSign.Password,
                NWDMauiRuntimeConfiguration.KConfig.ProjectId)
            , NWDExchangeOrigin.App, NWDExchangeDevice.Unknown)).Result;
        
        return tResponseRuntime.Status == NWDRequestStatus.Ok;
    }

    #endregion

    #region EmailPassword

    public bool SignInWithEmailPassword(NWDSignInEmailPassword sSignInEmailPassword)
    {
        NWDResponseRuntime tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(NWDRequestRuntime.CreateRequestSignIn(
            NWDMauiRuntimeConfiguration.KConfig,NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),
            NWDAccountSign.CreateEmailPassword(sSignInEmailPassword.Email, sSignInEmailPassword.Password,
                NWDMauiRuntimeConfiguration.KConfig.ProjectId)
            , NWDExchangeOrigin.App, NWDExchangeDevice.Unknown)).Result;
       
        
        /*NWDDataAppManager.FastSync();*/

        return tResponseRuntime.IsValid(NWDMauiRuntimeConfiguration.KConfig) && tResponseRuntime.Status == NWDRequestStatus.Ok;
    }

    public bool SignUpWithEmailPassword(NWDSignUpEmailPassword sSignUpEmailPassword)
    {
        NWDResponseRuntime tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(NWDRequestRuntime.CreateRequestSignUp(
            NWDMauiRuntimeConfiguration.KConfig,NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),
            NWDAccountSign.CreateEmailPassword(sSignUpEmailPassword.Email, sSignUpEmailPassword.Password,
                NWDMauiRuntimeConfiguration.KConfig.ProjectId)
            , NWDExchangeOrigin.App, NWDExchangeDevice.Unknown)).Result;
        return tResponseRuntime.IsValid(NWDMauiRuntimeConfiguration.KConfig) && tResponseRuntime.Status == NWDRequestStatus.Ok;
    }

    public NWDResponseRuntime AddEmailPasswordSign(NWDSignUpEmailPassword sSignUpEmailPassword)
    {
        NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignAdd(NWDMauiRuntimeConfiguration.KConfig,
            NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),
            NWDAccountSign.CreateEmailPassword(sSignUpEmailPassword.Email, sSignUpEmailPassword.Password,
                NWDMauiRuntimeConfiguration.KConfig.ProjectId)
            , NWDExchangeOrigin.App, NWDExchangeDevice.Unknown);
        NWDResponseRuntime tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(tRequestRuntime).Result;

        return tResponseRuntime;
    }
    public bool SignModifyEmailPassword(string sOldEmail, string sOldPassword, string sNewEmail,
        string sNewPassword)
    {
        NWDRequestRuntime tRequestRuntime = NWDRequestRuntime.CreateRequestSignModify(
            NWDMauiRuntimeConfiguration.KConfig, NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),
            NWDAccountSign.CreateEmailPassword(sOldEmail, sOldPassword, NWDMauiRuntimeConfiguration.KConfig.ProjectId),
            NWDAccountSign.CreateEmailPassword(sNewEmail, sNewPassword, NWDMauiRuntimeConfiguration.KConfig.ProjectId),
            NWDExchangeOrigin.App, NWDExchangeDevice.Unknown);
        return NWDRuntimeCallback.SharedInstance().PostRequest(tRequestRuntime).Result.Status == NWDRequestStatus.Ok;
    }
    
    public bool SignDeleteyEmailPassword(NWDSignInEmailPassword sSign)
    {
        NWDResponseRuntime tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(NWDRequestRuntime.CreateRequestSignDelete(NWDMauiRuntimeConfiguration.KConfig ,NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken(),
            NWDAccountSign.CreateEmailPassword(sSign.Email, sSign.Password,
                NWDMauiRuntimeConfiguration.KConfig.ProjectId)
            , NWDExchangeOrigin.App, NWDExchangeDevice.Unknown)).Result;
        
        return tResponseRuntime.Status == NWDRequestStatus.Ok;
    }

    

    public List<NWDAccountSign> GetSigns()
    {
        List<NWDAccountSign> rResult = new List<NWDAccountSign>();
        NWDRequestRuntime tRequestRuntime =
            NWDRequestRuntime.CreateRequestGetAllSign(NWDMauiRuntimeConfiguration.KConfig, NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken()
                , NWDExchangeOrigin.App, NWDExchangeDevice.Unknown);
        NWDResponseRuntime tResponseRuntime = NWDRuntimeCallback.SharedInstance().PostRequest(tRequestRuntime).Result;
        if (tResponseRuntime.Status == NWDRequestStatus.Ok)
        {
            NWDDownPayloadAccountSignAll tPayloadAccountSignAll =
                tResponseRuntime.GetPayload<NWDDownPayloadAccountSignAll>(NWDMauiRuntimeConfiguration.KConfig);
            rResult = tPayloadAccountSignAll.AccountSignList;
        }
        
        return rResult;
    }

    #endregion

    #region Service

    public List<NWDAccountService> GetServices()
    {
        return JsonConvert.DeserializeObject<List<NWDAccountService>>(Preferences.Get(nameof(NWDAccountService), ""));
    }

    /*public List<NWDAccountInfo> GetAccountInfos()
    {
        return NWDDataAppManager.GetDataForPlayerByClass<NWDAccountInfo>(); 
    }*/

    #endregion
    
    public static ushort GetDataTrack() // TODO : Where get Data Track ? 
    {
        return 0 ;
    }
    public static NWDAccount GetAccount()
    {
        return new NWDAccount() { Reference = NWDRuntimeCallback.SharedInstance().GetRequestPlayerToken().PlayerReference };
    }
    
    
    public static bool IsConnected()
    {
        return NWDRuntimeCallback.IsKeyPresentAndNotEmpty(nameof(NWDRequestPlayerToken.PlayerReference)) &&
               NWDRuntimeCallback.IsKeyPresentAndNotEmpty(nameof(NWDRequestPlayerToken.AccountRange)) &&
               NWDRuntimeCallback.IsKeyPresentAndNotEmpty(nameof(NWDRequestPlayerToken.Token));
    }
    
    public void SignOut()
    {
        SecureStorage.Remove(nameof(NWDRequestPlayerToken.PlayerReference));
        SecureStorage.Remove(nameof(NWDRequestPlayerToken.AccountRange));
        SecureStorage.Remove(nameof(NWDRequestPlayerToken.Token));
    }

    
}