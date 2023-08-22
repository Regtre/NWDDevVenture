using System;
using Newtonsoft.Json;
using NWDTreat.Configuration;
using NWDTreat.Exchanges.Payloads;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
namespace NWDTreat.Exchanges
{
    public class NWDResponseTreat : NWDBasicResponse
    {
        #region properties
        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Kin")]
        public NWDExchangeTreatKind Kind { set; get; } = NWDExchangeTreatKind.Unknown;
        /// <summary>
        /// 
        /// </summary>
       // [JsonProperty("Dur")]
        public int Duration { set; get; }
        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Sta")]
        public string StampTreat { set; get; } = string.Empty; //= DWN_Random.RandomStringToken(64);
        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("HSD")]
        public string HashTreat { set; get; } = string.Empty; //= DWN_Random.RandomStringToken(64);
        #endregion
        #region constructor
        /// <summary>
        /// Constructor for JSON
        /// </summary>
        public NWDResponseTreat()
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
        }

        /// <summary>
        /// Constructor for standard Reponse
        /// </summary>
        /// <param name="sTreatKeyManager"></param>
        /// <param name="sEnvironment"></param>
        /// <param name="sKind"></param>
        /// <param name="sDownPayload"></param>
        /// <param name="sStatus"></param>
        /// <param name="sProjectId"></param>
        public NWDResponseTreat(INWDTreatKey sTreatKeyManager,ulong sProjectId, NWDEnvironmentKind sEnvironment, NWDExchangeTreatKind sKind, NWDDownPayloadTreat? sDownPayload, NWDRequestStatus sStatus)
        {
            ProjectId = sProjectId;
            Environment = sEnvironment;
            Timestamp = NWDTimestamp.GetTimestampUnix();
            Duration = 0;
            // if (NWDConfigRuntime.SharedInstance != null)
            // {
            //     ServerIdentity = NWDConfigRuntime.SharedInstance.GetServerIdentity();
            // }
            Kind = sKind;
            if (sDownPayload != null)
            {
                SetPayload(sDownPayload);
            }
            if (string.IsNullOrEmpty(Payload))
            {
                Payload = string.Empty;
            }
            Status = sStatus;
            Secure(sTreatKeyManager, NWDRandom.RandomStringCypher(32));
        }
        #endregion
        #region method
        protected void SetPayload(NWDDownPayloadTreat sDownPayload)
        {
            Payload = JsonConvert.SerializeObject(sDownPayload);
        }
        /// <summary>
        /// Return the payload as instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetPayload<T>(INWDTreatKey sTreatKeyManager) where T : NWDDownPayloadTreat
        {
            T? rReturn = null; 
            if (IsValid(sTreatKeyManager))
            {
                rReturn = JsonConvert.DeserializeObject<T>(Payload);
            }
            return rReturn;
        }
        #endregion
        #region methods
        private string GenerateHash(INWDTreatKey sTreatKeyManager)
        {
            if (Payload == null)
            {
                Payload = string.Empty;
            }
            string tSaltKey = sTreatKeyManager.GetTreatKey(ProjectId, Environment);
            if (string.IsNullOrEmpty(tSaltKey) == false)
            {
                string tPayLoadPrint = NWDSecurityTools.GenerateSha(Payload);
                return  NWDSecurityTools.GenerateSha(tPayLoadPrint + StampTreat + tSaltKey + Kind.ToString());
            }
            else
            {
                throw new Exception(sTreatKeyManager.GetTreatInstanceName()+" : "+nameof(INWDTreatKey) + "." + nameof(INWDTreatKey.GetTreatKey) + " return string empty or null!");
            }
        }
        private void Secure(INWDTreatKey sTreatKeyManager,string sStamp)
        {
            StampTreat = sStamp;
            HashTreat = GenerateHash(sTreatKeyManager);
        }
        /// <summary>
        /// Test if Request is secured with the good hash print
        /// </summary>
        /// <returns></returns>
        public  bool IsValid(INWDTreatKey sTreatKeyManager)
        {
            bool rReturn = false;
            if (string.IsNullOrEmpty(HashTreat) == false)
            {
                if (GenerateHash(sTreatKeyManager) == HashTreat)
                {
                    rReturn = true;
                }
            }
            return rReturn;
        }
        #endregion
        
        public void Logger(string sId = "from request logger")
        {
            switch (Status)
            {
                case NWDRequestStatus.Ok:
                    NWDLogger.Information(nameof(NWDResponseTreat) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
                case NWDRequestStatus.Error:
                case NWDRequestStatus.None:
                case NWDRequestStatus.NoNetwork:
                case NWDRequestStatus.DaoError:
                    NWDLogger.Error(nameof(NWDResponseTreat) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
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
                    NWDLogger.Warning(nameof(NWDResponseTreat) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
            }
        }
    }
}