using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDUnityShared.Engine;

namespace NWDUnityShared.Factories
{
    public static class NWDRequestRuntimeFactory
    {
        static public NWDRequestRuntime CreateGetSignaturesRequest(NWDRequestPlayerToken sPlayerToken)
        {
            return NWDRequestRuntime.CreateRequestGetAllSign(NWDUnityEngine.Instance.Config, sPlayerToken, NWDExchangeOrigin.Game, NWDUnityEngine.Instance.Config.GetDeviceOS());
        }

        static public NWDRequestRuntime CreateSignInRequest (NWDRequestPlayerToken sPlayerToken, NWDAccountSign sAccountSign)
        {
            return NWDRequestRuntime.CreateRequestSignIn(NWDUnityEngine.Instance.Config, sPlayerToken, sAccountSign, NWDExchangeOrigin.Game, NWDUnityEngine.Instance.Config.GetDeviceOS());
        }

        static public NWDRequestRuntime CreateSignUpRequest(NWDRequestPlayerToken sPlayerToken, NWDAccountSign sAccountSign)
        {
            return NWDRequestRuntime.CreateRequestSignUp(NWDUnityEngine.Instance.Config, sPlayerToken, sAccountSign, NWDExchangeOrigin.Game, NWDUnityEngine.Instance.Config.GetDeviceOS());
        }

        static public NWDRequestRuntime CreateSignOutRequest(NWDRequestPlayerToken sPlayerToken)
        {
            return NWDRequestRuntime.CreateRequestSignOut(NWDUnityEngine.Instance.Config, sPlayerToken, null, NWDExchangeOrigin.Game, NWDUnityEngine.Instance.Config.GetDeviceOS());
        }

        static public NWDRequestRuntime CreateDeleteSignatureRequest(NWDRequestPlayerToken sPlayerToken, NWDAccountSign sAccountSign)
        {
            return NWDRequestRuntime.CreateRequestSignDelete(NWDUnityEngine.Instance.Config, sPlayerToken, sAccountSign, NWDExchangeOrigin.Game, NWDUnityEngine.Instance.Config.GetDeviceOS());
        }

        static public NWDRequestRuntime CreateAddSignatureRequest(NWDRequestPlayerToken sPlayerToken, NWDAccountSign sAccountSign)
        {
            return NWDRequestRuntime.CreateRequestSignAdd(NWDUnityEngine.Instance.Config, sPlayerToken, sAccountSign, NWDExchangeOrigin.Game, NWDUnityEngine.Instance.Config.GetDeviceOS());
        }

        static public NWDRequestRuntime CreateEditSignatureRequest(NWDRequestPlayerToken sPlayerToken, NWDAccountSign sOldAccountSign, NWDAccountSign sNewAccountSign)
        {
            return NWDRequestRuntime.CreateRequestSignModify(NWDUnityEngine.Instance.Config, sPlayerToken, sOldAccountSign, sNewAccountSign, NWDExchangeOrigin.Game, NWDUnityEngine.Instance.Config.GetDeviceOS());
        }

        static public NWDRequestRuntime CreateSyncAllRequest(NWDRequestPlayerToken sPlayerToken, NWDUpPayloadDataSyncByIncrement sPayload)
        {
            return NWDRequestRuntime.CreateRequestSyncDataByIncrement(NWDUnityEngine.Instance.Config, sPlayerToken, sPayload, NWDExchangeOrigin.Game, NWDUnityEngine.Instance.Config.GetDeviceOS());
        }
    }
}