using Newtonsoft.Json;
using NWDFoundation.Configuration;
using NWDFoundation.Exchanges;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDRuntime.Exchanges.Payloads;
using System;
using NWDFoundation.Facades;
using NWDFoundation.Logger;

namespace NWDRuntime.Exchanges
{
    [Serializable]
    public class NWDRequestRuntime : NWDBasicRequest
    {
        #region properties

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Kin")]
        public NWDExchangeRuntimeKind Kind { set; get; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Pla")]
        public NWDRequestPlayerToken PlayerToken { set; get; }

        #endregion

        #region constructor

        /// <summary>
        /// Constructor for JSON
        /// </summary>
        public NWDRequestRuntime()
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
        }

        /// <summary>
        /// Constructor of standard Request
        /// </summary>
        /// <param name="sProjectKeyManager"></param>
        /// <param name="sProjectId"></param>
        /// <param name="sPlayerToken"></param>
        /// <param name="sKind"></param>
        /// <param name="sUpPayload"></param>
        /// <param name="sOrigin"></param>
        /// <param name="sDevice"></param>
        public NWDRequestRuntime(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken, NWDExchangeRuntimeKind sKind,
            NWDUpPayload sUpPayload, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice)
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
            ProjectId = sPlayerToken.ProjectId;
            Environment = sPlayerToken.EnvironmentKind;
            PlayerToken = sPlayerToken;
            Kind = sKind;
            Origin = sOrigin;
            Device = sDevice;
            if (sUpPayload != null)
            {
                SetPayload(sUpPayload);
            }

            if (string.IsNullOrEmpty(Payload))
            {
                Payload = string.Empty;
            }
            if (ProjectId != 0)
            {
                Secure(sProjectKeyManager, NWDRandom.RandomStringCypher(32));
            }
        }

        #endregion

        #region static factories

        public static NWDRequestRuntime CreateRequestTest(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken,
            NWDExchangeOrigin sOrigin,
            NWDExchangeDevice sDevice)
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.Test,
                new NWDUpPayload(), sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestNone(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken,
            NWDExchangeOrigin sOrigin,
            NWDExchangeDevice sDevice)
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.None,
                new NWDUpPayload(), sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestSignIn(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken,
            NWDAccountSign sAccountSign, NWDExchangeOrigin sOrigin,
            NWDExchangeDevice sDevice)
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.SignIn,
                new NWDUpPayloadAccountSignIn() { AccountSign = sAccountSign }, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestSignUp(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken,
            NWDAccountSign sAccountSign, NWDExchangeOrigin sOrigin,
            NWDExchangeDevice sDevice)
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.SignUp,
                new NWDUpPayloadAccountSignUp() { AccountSign = sAccountSign }, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestSignOut(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken,
            NWDAccountSign sDeviceSign, NWDExchangeOrigin sOrigin,
            NWDExchangeDevice sDevice)
        {
            return new NWDRequestRuntime( sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.SignOut,
                new NWDUpPayloadAccountSignOut() { DeviceSign = sDeviceSign }, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestSignLost(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken, string sRescueEmail,
            NWDExchangeOrigin sOrigin,
            NWDExchangeDevice sDevice)
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.SignLost,
                new NWDUpPayloadAccountSignLost() { RescueEmail = sRescueEmail }, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestSignRescue(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken, NWDAccountSign sAccountSign, NWDExchangeOrigin sOrigin , NWDExchangeDevice sDevice)
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.SignRescue, new NWDUpPayloadAccountSignRescue() { AccountSign=sAccountSign}, sOrigin, sDevice);
        }
        public static NWDRequestRuntime CreateRequestSignAdd(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken, NWDAccountSign sAccountSign, NWDExchangeOrigin sOrigin , NWDExchangeDevice sDevice )
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.SignAdd, new NWDUpPayloadAccountSignAdd() { AccountSign = sAccountSign }, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestSignModify(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken,
            NWDAccountSign sOldAccountSign, NWDAccountSign sNewAccountSign,
            NWDExchangeOrigin sOrigin ,
            NWDExchangeDevice sDevice )
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.SignModify,
                new NWDUpPayloadAccountSignModify()
                    { OldAccountSign = sOldAccountSign, NewAccountSign = sNewAccountSign }, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestSignDelete(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken,
            NWDAccountSign sAccountSign, NWDExchangeOrigin sOrigin,
            NWDExchangeDevice sDevice )
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.SignDelete,
                new NWDUpPayloadAccountSignDelete() { AccountSign = sAccountSign }, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestGetAllSign(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken,
            NWDExchangeOrigin sOrigin ,
            NWDExchangeDevice sDevice )
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.GetAllSign,
                new NWDUpPayloadAccountSignAll() { }, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestSyncDataByIncrement(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken,
            NWDUpPayloadDataSyncByIncrement sUpPayloadDataSyncByIncrement
            ,NWDExchangeOrigin sOrigin ,
            NWDExchangeDevice sDevice )
        {
            return new NWDRequestRuntime(sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.SyncDataByIncrement,
                sUpPayloadDataSyncByIncrement, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateGetAllPlayerData(INWDProjectKey sProjectKeyManager,NWDRequestPlayerToken sPlayerToken,
            NWDExchangeOrigin sOrigin ,
            NWDExchangeDevice sDevice )
        {
            return new NWDRequestRuntime( sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.GetAllPlayerData,
                new NWDUpPayloadDataSyncByIncrement() { }, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestCreateRelationship(INWDProjectKey sProjectKeyManager,NWDRequestPlayerToken sPlayerToken,
            NWDExchangeOrigin sOrigin ,
            NWDExchangeDevice sDevice )
        {

            return new NWDRequestRuntime( sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.CreateRelationship,
                new NWDUpPayloadDataSyncByIncrement() { }, sOrigin, sDevice);
        }

        public static NWDRequestRuntime CreateRequestLinkRelationship(INWDProjectKey sProjectKeyManager,NWDRequestPlayerToken sPlayerToken,
            NWDExchangeOrigin sOrigin ,
            NWDExchangeDevice sDevice, 
            string sCode
        )
        {

            return new NWDRequestRuntime( sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.LinkRelationship,
                new NWDUpPayloadLinkRelationship(sCode) { }, sOrigin, sDevice);
        }
        public static NWDRequestRuntime CreateRequestFinalizeRelationship(INWDProjectKey sProjectKeyManager,NWDRequestPlayerToken sPlayerToken,
            NWDExchangeOrigin sOrigin ,
            NWDExchangeDevice sDevice, 
            NWDRelationship sRelationship,
            bool sIsAccepted
        )
        {

            return new NWDRequestRuntime( sProjectKeyManager, sPlayerToken, NWDExchangeRuntimeKind.FinalizeRelationship,
                new NWDUpPayloadFinalizeRelationship(sRelationship.Reference, sIsAccepted) { }, sOrigin, sDevice);
        }
        #endregion

        #region methods

        public void SetPayload(NWDUpPayload sUpPayload)
        {
            Payload = JsonConvert.SerializeObject(sUpPayload);
        }

        /// <summary>
        /// Return the payload as instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPayload<T>(INWDProjectKey sProjectKeyManager) where T : NWDUpPayload
        {
            T? rReturn = null;
            if (IsValid(sProjectKeyManager))
            {
                rReturn = JsonConvert.DeserializeObject<T>(Payload);
            }
            else
            {
                //NWDLogger.Warning("["+IdName+ "] " +nameof (NWDRequestRuntime) +" "+nameof(GetPayload)+" NOT POSSIBLE TO GET OBJECT! for "+ProjectId+" "+Environment.ToString()+" (player token "+ProjectId+" "+Environment.ToString()+") with salt result  '" +sProjectKeyManager.GetProjectKey(PlayerToken.ProjectId, PlayerToken.EnvironmentKind)+"' " + Payload);
            }
            return rReturn;
        }

        protected string GenerateHash(INWDProjectKey sProjectKeyManager)
        {
            string rReturn;
            string tPayLoadPrint;
            if (PlayerToken == null)
            {
                PlayerToken = new NWDRequestPlayerToken(ProjectId, Environment);
            }

            if (Payload == null)
            {
                Payload = string.Empty;
            }

            // string tSaltKey = sProjectKeyManager.GetProjectKey(PlayerToken.ProjectId, PlayerToken.EnvironmentKind);
            string tSaltKey = sProjectKeyManager.GetProjectKey(ProjectId, Environment);
            if (string.IsNullOrEmpty(tSaltKey) == false)
            {
                tPayLoadPrint = NWDSecurityTools.GenerateSha(Payload);
                rReturn = NWDSecurityTools.GenerateSha(
                    tPayLoadPrint
                    + Stamp
                    + tSaltKey
                    + Kind.ToString()
                    + PlayerToken.Token
                    + PlayerToken.AccountRange.ToString()
                    + PlayerToken.EnvironmentKind.ToString()
                    + Origin.ToString()
                    // + PlayerToken.DataTrack.ToString())
                    );
            }
            else
            {
                return string.Empty;
                //throw new Exception(sProjectKeyManager.GetProjectKeyInstanceName()+" : "+ nameof(INWDProjectKey) + "." + nameof(INWDProjectKey.GetProjectKey) + " return string empty or null!");
            }
            
            //NWDLogger.Information("["+IdName+ "] " +nameof (NWDRequestRuntime) +"  hash "+ProjectId+" "+Environment.ToString()+" (player token "+ProjectId+" "+Environment.ToString()+") with salt result  '" +sProjectKeyManager.GetProjectKey(PlayerToken.ProjectId, PlayerToken.EnvironmentKind)+"', rReturn = '"+rReturn+"' for message '" + Payload);
            return rReturn;
        }

        public void Secure(INWDProjectKey sProjectKeyManager, string sStamp)
        {
            Stamp = sStamp;
            Hash = GenerateHash(sProjectKeyManager);
        }

        /// <summary>
        /// Test if Request is secured with the good hash print
        /// </summary>
        /// <returns></returns>
        public bool IsValid(INWDProjectKey sProjectKeyManager)
        {
            bool rReturn = false;
            string tHashActual = GenerateHash(sProjectKeyManager);
            if (string.IsNullOrEmpty(Hash) == false)
            {
                if (tHashActual == Hash)
                {
                    if (PlayerToken.ProjectId == ProjectId)
                    {
                        rReturn = true;
                    }
                }
            }
            if (rReturn == false)
            {
                //NWDLogger.Warning("["+IdName+ "] " +nameof (NWDRequestRuntime) +"." +nameof (IsValid) +" Error  : hash is not valid for "+ProjectId+" "+Environment.ToString()+" (player token "+ProjectId+" "+Environment.ToString()+") with salt result  '" +sProjectKeyManager.GetProjectKey(PlayerToken.ProjectId, PlayerToken.EnvironmentKind)+"' Hash is '"+Hash+"' and actual generate return '"+tHashActual+"'");
            }
            else
            {
                //NWDLogger.TraceSuccess("["+IdName+ "] " +nameof (NWDRequestRuntime) +"." +nameof (IsValid) +" hash is valid for "+ProjectId+" "+Environment.ToString()+" (player token "+ProjectId+" "+Environment.ToString()+") with salt result  '" +sProjectKeyManager.GetProjectKey(PlayerToken.ProjectId, PlayerToken.EnvironmentKind)+"' Hash is '"+Hash+"' and actual generate return '"+tHashActual+"'");
            }
            return rReturn;
        }

        #endregion
        public string Logger(string sUrl)
        {
            string rReturn = NWDRandom.RandomString(24);
            //NWDLogger.Information(nameof(NWDRequestRuntime) + " ["+rReturn+"] ", sUrl);
            //NWDLogger.Information(nameof(NWDRequestRuntime) + " ["+rReturn+"] ", NWDLogger.SplitObjectSerializable(this));
            return rReturn;
        }
    }
}