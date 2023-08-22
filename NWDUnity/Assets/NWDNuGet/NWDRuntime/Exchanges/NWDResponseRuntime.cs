using System;
using Newtonsoft.Json;
using NWDFoundation.Configuration;
using NWDFoundation.Exchanges;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;

namespace NWDRuntime.Exchanges
{
    [Serializable]
    public class NWDResponseRuntime : NWDBasicResponse
    {
        #region properties

        public string Debug { set; get; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Kin")]
        public NWDExchangeRuntimeKind RuntimeKind { set; get; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Pla")]
        public NWDRequestPlayerToken PlayerToken { set; get; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Dur")]
        public int Duration { set; get; }

        #endregion

        #region constructor

        /// <summary>
        /// Constructor for JSON
        /// </summary>
        public NWDResponseRuntime()
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
        }

        public NWDResponseRuntime(NWDRequestStatus sStatus)
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
            Status = sStatus;
        }
        /// <summary>
        /// Constructor for standard Reponse
        /// </summary>
        /// <param name="sProjectKeyManager"></param>
        /// <param name="sProjectId"></param>
        /// <param name="sPlayerToken"></param>
        /// <param name="sRuntimeKind"></param>
        /// <param name="sPayload"></param>
        /// <param name="sStatus"></param>
        public NWDResponseRuntime(INWDProjectKey sProjectKeyManager, NWDRequestPlayerToken sPlayerToken, NWDExchangeRuntimeKind sRuntimeKind, NWDDownPayload sPayload, NWDRequestStatus sStatus, string sDebug="")
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
            Duration = 0;
            // if (NWDConfigRuntime.SharedInstance != null)
            // {
            //     ServerIdentity = NWDConfigRuntime.SharedInstance.GetServerIdentity();
            // }

            ProjectId = sPlayerToken.ProjectId;
            Environment = sPlayerToken.EnvironmentKind;
            PlayerToken = sPlayerToken;
            RuntimeKind = sRuntimeKind;
            if (sPayload != null)
            {
                SetPayload(sPayload);
            }

            if (string.IsNullOrEmpty(Payload))
            {
                Payload = string.Empty;
            }
#if DEBUG
            Debug = sDebug;
#endif
            Status = sStatus;
            if (ProjectId != 0)
            {
                Secure( sProjectKeyManager, NWDRandom.RandomStringCypher(32));
            }
        }

        #endregion

        #region method

        protected void SetPayload(NWDDownPayload sPayload)
        {
            Payload = JsonConvert.SerializeObject(sPayload);
        }

        /// <summary>
        /// Return the payload as instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPayload<T>(INWDProjectKey sProjectKeyManager) where T : NWDDownPayload
        {
            T? rReturn = null;
            if (IsValid( sProjectKeyManager))
            {
                rReturn = JsonConvert.DeserializeObject<T>(Payload);
            }
            else
            {
                //NWDLogger.Warning("["+IdName+ "] " +nameof (NWDResponseRuntime) +" "+nameof(GetPayload)+" NOT POSSIBLE TO GET OBJECT! for "+ProjectId+" "+Environment.ToString()+" (player token "+ProjectId+" "+Environment.ToString()+") with salt result  '" +sProjectKeyManager.GetProjectKey(PlayerToken.ProjectId, PlayerToken.EnvironmentKind)+"' " + Payload);
            }
            return rReturn;
        }

        private string GenerateHash(INWDProjectKey sProjectKeyManager)
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
            string tSaltKey = sProjectKeyManager.GetProjectKey(ProjectId, Environment);
            if (string.IsNullOrEmpty(tSaltKey) == false )
            {
                tPayLoadPrint = NWDSecurityTools.GenerateSha(Payload);
                rReturn = NWDSecurityTools.GenerateSha(
                    tPayLoadPrint 
                    + Stamp 
                    + tSaltKey 
                    + Status.ToString() 
                    + RuntimeKind.ToString() 
                    + PlayerToken.Token 
                    + PlayerToken.AccountRange.ToString() 
                    + PlayerToken.EnvironmentKind.ToString() 
                    // + PlayerToken.DataTrack
                    );
            }
            else
            {
                rReturn = string.Empty;
                //throw new Exception(sProjectKeyManager.GetProjectKeyInstanceName()+" : "+ nameof(INWDProjectKey) + "." + nameof(INWDProjectKey.GetProjectKey) + " return string empty or null!");
            }
            //NWDLogger.Information("["+IdName+ "] " +nameof (NWDResponseRuntime) +"  hash "+ProjectId+" "+Environment.ToString()+" (player token "+ProjectId+" "+Environment.ToString()+") with salt result  '" +sProjectKeyManager.GetProjectKey(PlayerToken.ProjectId, PlayerToken.EnvironmentKind)+"', rReturn = '"+rReturn+"' for message '" + Payload+"'");

            return rReturn;
        }

        protected void Secure(INWDProjectKey sProjectKeyManager, string sStamp)
        {
            Stamp = sStamp;
            Hash = GenerateHash(sProjectKeyManager);
        }

        /// <summary>
        /// Test if Response is secured with the good hash print
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
                    rReturn = true;
                }
            }
            if (rReturn == false)
            {
                //NWDLogger.Warning("["+IdName+ "] " +nameof (NWDResponseRuntime) +"." +nameof (IsValid) +" Error  : hash is not valid for "+ProjectId+" "+Environment.ToString()+" (player token "+ProjectId+" "+Environment.ToString()+") with salt result  '" +sProjectKeyManager.GetProjectKey(PlayerToken.ProjectId, PlayerToken.EnvironmentKind)+"' Hash is '"+Hash+"' and actual generate return '"+tHashActual+"'");
            }
            else
            {
                //NWDLogger.TraceSuccess("["+IdName+ "] " +nameof (NWDResponseRuntime) +"." +nameof (IsValid) +" hash is valid for "+ProjectId+" "+Environment.ToString()+" (player token "+ProjectId+" "+Environment.ToString()+") with salt result  '" +sProjectKeyManager.GetProjectKey(PlayerToken.ProjectId, PlayerToken.EnvironmentKind)+"' Hash is '"+Hash+"' and actual generate return '"+tHashActual+"'");
            }
            return rReturn;
        }

        #endregion
        public void Logger(string sId = "from request logger")
        {
            switch (Status)
            {
                case NWDRequestStatus.Ok:
                    //NWDLogger.Information(nameof(NWDResponseRuntime) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
                case NWDRequestStatus.Error:
                case NWDRequestStatus.None:
                case NWDRequestStatus.NoNetwork:
                case NWDRequestStatus.DaoError:
                    //NWDLogger.Error(nameof(NWDResponseRuntime) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
                case NWDRequestStatus.ServerIsDisabled:
                case NWDRequestStatus.PleaseChangeServer:
                case NWDRequestStatus.Test:
                case NWDRequestStatus.Unknown:
                case NWDRequestStatus.ProjectIsPublishing:
                case NWDRequestStatus.AccountUnknown:
                case NWDRequestStatus.AccountError:
                case NWDRequestStatus.AccountNotUnique:
                case NWDRequestStatus.AccountBan:
                case NWDRequestStatus.AccountTrashed:
                case NWDRequestStatus.TokenError:
                    //NWDLogger.Warning(nameof(NWDResponseRuntime) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
            }
        }
    }
}