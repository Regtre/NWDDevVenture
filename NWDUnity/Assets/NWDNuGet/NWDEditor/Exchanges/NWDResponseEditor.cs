using System;
using Newtonsoft.Json;
using NWDEditor.Exchanges.Payloads;
using NWDFoundation.Configuration;
using NWDFoundation.Exchanges;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Logger;
using NWDFoundation.Tools;

namespace NWDEditor.Exchanges
{
    // TO DO REMOVE
    [Obsolete("TO REMOVE : Use Treat manager")]
    public class NWDResponseEditor
    {
        #region properties

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Stt")]
        public NWDRequestStatus Status { set; get; } = NWDRequestStatus.None;

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Kin")]
        public NWDExchangeEditorKind Kind { set; get; } = NWDExchangeEditorKind.Unknown;

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Pay")]
        public string Payload { set; get; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Tim")]
        public int Timestamp { set; get; }

        /// <summary>
        /// 
        /// </summary>
       // [JsonProperty("Dur")]
        public int Duration { set; get; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Ser")]
        public string ServerIdentity { set; get; } = string.Empty;
        
        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Sta")]
        public string StampStudio { set; get; } = string.Empty; //= DWN_Random.RandomStringToken(64);
        
        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("HSD")]
        public string HashStudio { set; get; } = string.Empty; //= DWN_Random.RandomStringToken(64);
        
        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("DBG")]
        public string Debug { set; get; } = string.Empty;
        
        public string RolePublicKey { get; set; } = string.Empty;
        #endregion

        #region constructor

        /// <summary>
        /// Constructor for JSON
        /// </summary>
        public NWDResponseEditor()
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
        }


        /// <summary>
        /// Constructor for standard Response secure by secret Token
        /// </summary>
        /// <param name="sKind"></param>
        /// <param name="sDownPayload"></param>
        /// <param name="sStatus"></param>
        /// <param name="sToken"></param>

        public NWDResponseEditor(NWDExchangeEditorKind sKind, NWDDownPayloadEditor? sDownPayload, NWDRequestStatus sStatus, string sToken)
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
            Duration = 0;
            Kind = sKind;
            if (sDownPayload != null)
            {
                SetPayload(sDownPayload);
            }
            Status = sStatus;
            Secure(NWDRandom.RandomStringCypher(32),sToken);
        }
        
        // <summary>
        /// Constructor for standard Response non secure
        /// </summary>
        /// <param name="sKind"></param>
        /// <param name="sDownPayload"></param>
        /// <param name="sStatus"></param>
        public NWDResponseEditor(NWDExchangeEditorKind sKind, NWDDownPayloadEditor? sDownPayload, NWDRequestStatus sStatus)
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
            Duration = 0;
            Kind = sKind;
            if (sDownPayload != null)
            {
                SetPayload(sDownPayload);
            }
            Status = sStatus; 
        }
        #endregion

        #region method
        protected void SetPayload(NWDDownPayloadEditor sDownPayload)
        {
            Payload = JsonConvert.SerializeObject(sDownPayload);
        }
        /// <summary>
        /// Return the payload as instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetPayload<T>(string sToken) where T : NWDDownPayloadEditor
        {
            T? rReturn = null;
            if (IsValid(sToken))
            {
                rReturn = JsonConvert.DeserializeObject<T>(Payload);
            }
            return rReturn;
        }
        #endregion
        
        #region methods
        private string GenerateHash(string sToken)
        {
            
                string tSaltKey = sToken;
                string tPayLoadPrint = NWDSecurityTools.GenerateSha(Payload);
                return  NWDSecurityTools.GenerateSha(tPayLoadPrint + StampStudio + tSaltKey + Kind.ToString());
            
        }
        private void Secure(string sStamp, string sToken)
        {
            StampStudio = sStamp;
            HashStudio = GenerateHash(sToken);
        }

        /// <summary>
        /// Test if Request is secured with the good hash print
        /// </summary>
        /// <returns></returns>
        public  bool IsValid(string sToken)
        {
            bool rReturn = false;
            if (string.IsNullOrEmpty(HashStudio) == false)
            {
                if (GenerateHash(sToken) == HashStudio)
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
                    NWDLogger.Information(nameof(NWDResponseEditor) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
                case NWDRequestStatus.Error:
                case NWDRequestStatus.None:
                case NWDRequestStatus.NoNetwork:
                case NWDRequestStatus.DaoError:
                    NWDLogger.Error(nameof(NWDResponseEditor) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
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
                    NWDLogger.Warning(nameof(NWDResponseEditor) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
            }
        }
    }
}