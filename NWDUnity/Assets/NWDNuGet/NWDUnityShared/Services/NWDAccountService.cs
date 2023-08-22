using NWDFoundation.Exchanges;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDUnityShared.Engine;
using NWDUnityShared.Factories;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;

namespace NWDUnityShared.Services
{
    public class NWDAccountService : NWDRuntimeService
    {
        static private object _lock = new object();

        static private NWDRequestPlayerToken playerToken = null;
        static public NWDRequestPlayerToken PlayerToken
        {
            get
            {
                lock (_lock)
                {
                    if (playerToken == null)
                    {
                        playerToken = new NWDRequestPlayerToken();
                        playerToken.ProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                        playerToken.EnvironmentKind = NWDUnityEngine.Instance.EnvironmentManager.GetCurrentEnvironment();
                        playerToken.ExchangeOrigin = NWDExchangeOrigin.Game;
                    }
                    return playerToken;
                }
            }
            private set
            {
                lock (_lock)
                {
                    playerToken = value;
                }
            }
        }

        static public List<NWDFoundation.Models.NWDAccountService> accountServices = new List<NWDFoundation.Models.NWDAccountService>();
        static public List<NWDFoundation.Models.NWDAccountService> AccountServices
        {
            get
            {
                lock (_lock)
                {
                    return accountServices;
                }
            }
            private set
            {
                lock (_lock)
                {
                    accountServices = value;
                }
            }
        }

        static private NWDAccountSign accountSign = null;
        static public NWDAccountSign AccountSign
        {
            get
            {
                lock (_lock)
                {
                    return accountSign;
                }
            }
            private set
            {
                lock (_lock)
                {
                    accountSign = value;
                }
            }
        }

        static private void ProcessServerResponse(NWDResponseRuntime sResponse, ref NWDAsyncHandler sAsyncHandler)
        {
            ProcessServerResponse<NWDDownPayload>(sResponse, ref sAsyncHandler);
        }

        static public T ProcessServerResponse<T> (NWDResponseRuntime sResponse, ref NWDAsyncHandler sAsyncHandler) where T : NWDDownPayload
        {
            PlayerToken = sResponse.PlayerToken;

            T tPayload = sResponse.GetPayload<T>(NWDUnityEngine.Instance.Config);

            if (tPayload == null || tPayload.AccountServiceList == null)
            {
                AccountServices = new List<NWDFoundation.Models.NWDAccountService> ();
            }
            else
            {
                AccountServices = tPayload.AccountServiceList;
            }

            try
            {
                CheckServerResponse(sResponse);
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(PlayerToken.Token))
                {
                    throw e;
                }
            }

            if (string.IsNullOrEmpty(PlayerToken.Token))
            {
                sAsyncHandler.AddWarning(new Exception("Player token is empty!\nSign in with DeviceId..."));

                string tDeviceId = NWDUnityEngine.Instance.DeviceManager.GetDeviceId();
                string tDeviceName = NWDUnityEngine.Instance.DeviceManager.GetDeviceName();
                ulong tProjectId = NWDUnityEngine.Instance.Config.GetProjectId();
                NWDAccountSign tAccountSign = NWDAccountSign.CreateDeviceId(tDeviceId, tProjectId, tDeviceName);
                SignIn(tAccountSign, ref sAsyncHandler);
            }

            return tPayload;
        }

        static public List<NWDAccountSign> GetSignatures(ref NWDAsyncHandler sAsyncHandler)
        {
            NWDRequestRuntime tRequest = NWDRequestRuntimeFactory.CreateGetSignaturesRequest(PlayerToken);
            NWDResponseRuntime tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            NWDDownPayloadAccountSignAll tDownPayload = ProcessServerResponse<NWDDownPayloadAccountSignAll>(tResponse, ref sAsyncHandler);
            if (tDownPayload == null || tDownPayload.AccountSignList == null)
            {
                throw new ArgumentNullException("No Account sign list was found!");
            }

            return tDownPayload.AccountSignList;
        }

        /// <summary>
        /// NWD account creation method.
        /// </summary>
        /// <param name="sSign"></param>
        static public void SignUp(NWDAccountSign sSign, ref NWDAsyncHandler sAsyncHandler)
        {
            NWDRequestRuntime tRequest = NWDRequestRuntimeFactory.CreateSignUpRequest(PlayerToken, sSign);
            NWDResponseRuntime tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            ProcessServerResponse(tResponse, ref sAsyncHandler);

            AccountSign = sSign;
        }

        /// <summary>
        /// Login method for NWD accounts.
        /// </summary>
        /// <param name="sSign"></param>
        static public void SignIn(NWDAccountSign sSign, ref NWDAsyncHandler sAsyncHandler)
        {
            NWDRequestRuntime tRequest = NWDRequestRuntimeFactory.CreateSignInRequest(PlayerToken, sSign);
            NWDResponseRuntime tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            if (tResponse.Status == NWDRequestStatus.AccountUnknown && sSign.SignType == NWDFoundation.Models.Enums.NWDAccountSignType.DeviceId)
            {
                SignUp(sSign, ref sAsyncHandler);
            }
            else
            {
                ProcessServerResponse(tResponse, ref sAsyncHandler);
                AccountSign = sSign;
            }
        }

        /// <summary>
        /// Logout method for NWD accounts.
        /// </summary>
        static public void SignOut (ref NWDAsyncHandler sAsyncHandler)
        {
            NWDRequestRuntime tRequest = NWDRequestRuntimeFactory.CreateSignOutRequest(PlayerToken);
            NWDResponseRuntime tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            ProcessServerResponse(tResponse, ref sAsyncHandler);
        }
        static public void AddSignature (NWDAccountSign sSign, ref NWDAsyncHandler sAsyncHandler)
        {
            NWDRequestRuntime tRequest = NWDRequestRuntimeFactory.CreateAddSignatureRequest(PlayerToken, sSign);
            NWDResponseRuntime tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            ProcessServerResponse(tResponse, ref sAsyncHandler);
        }
        static public void EditSignature(NWDAccountSign sOldSign, NWDAccountSign sNewSign, ref NWDAsyncHandler sAsyncHandler)
        {
            NWDRequestRuntime tRequest = NWDRequestRuntimeFactory.CreateEditSignatureRequest(PlayerToken, sOldSign, sNewSign);
            NWDResponseRuntime tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            ProcessServerResponse(tResponse, ref sAsyncHandler);
        }
        static public void RemoveSignature(NWDAccountSign sSign, ref NWDAsyncHandler sAsyncHandler)
        {
            NWDRequestRuntime tRequest = NWDRequestRuntimeFactory.CreateDeleteSignatureRequest(PlayerToken, sSign);
            NWDResponseRuntime tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            ProcessServerResponse(tResponse, ref sAsyncHandler);
        }
    }
}
