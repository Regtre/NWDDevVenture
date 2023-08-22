using System;
using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades;
using NWDFoundation.Tools;
using NWDPlayerStatistic.Exchanges.Payloads;

namespace NWDPlayerStatistic.Exchanges;

public class NWDPlayerStatisticResponse: NWDBasicResponse
    {
        #region properties

        /// <summary>
        /// Kind of request
        /// </summary>
        // [JsonProperty(PropertyName ="KIN")]
        public NWDExchangePlayerStatisticKind Kind { set; get; } = NWDExchangePlayerStatisticKind.Unknown;

        /// <summary>
        /// Payload in response
        /// </summary>
        // [JsonProperty(PropertyName ="PAY")]
        public string Payload { set; get; } = string.Empty;

        /// <summary>
        /// Timestamp of repsonse
        /// </summary>
        // [JsonProperty(PropertyName ="TIM")]
        public int Timestamp { set; get; }

        /// <summary>
        /// Duration of treatment
        /// </summary>
        // [JsonProperty(PropertyName ="SER")]
        public string ServerIdentity { set; get; } = string.Empty;

        /// <summary>
        /// Random stamp of response
        /// </summary>
        // [JsonProperty(PropertyName ="STA")]
        public string StampCrucial { set; get; } = string.Empty; //= DWN_Random.RandomStringToken(64);

        /// <summary>
        /// Hash sign of response
        /// </summary>
        // [JsonProperty(PropertyName ="HAH")]
        public string HashCrucial { set; get; } = string.Empty; //= DWN_Random.RandomStringToken(64);

        /// <summary>
        /// Debug information if it is development
        /// </summary>
        // [JsonProperty(PropertyName ="DBG")]
        public string Debug { set; get; } = string.Empty;

        #endregion

        #region constructor

        /// <summary>
        /// Constructor for JSON
        /// </summary>
        public NWDPlayerStatisticResponse()
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
        }

        /// <summary>
        /// Constructor for standard Reponse
        /// </summary>
        /// <param name="sKind"></param>
        /// <param name="sDownPayload"></param>
        /// <param name="sStatus"></param>
        public NWDPlayerStatisticResponse(INWDCrucialKey sCrucialKeyManager,NWDExchangePlayerStatisticKind sKind, NWDPlayerStatisticDownPayload? sDownPayload, NWDRequestStatus sStatus)
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
            Kind = sKind;
            ProjectId = sCrucialKeyManager.GetCrucialProjectId();
            Environment = sCrucialKeyManager.GetCrucialEnvironment();
            if (sDownPayload != null)
            {
                SetPayload(sDownPayload);
            }

            if (string.IsNullOrEmpty(Payload))
            {
                Payload = string.Empty;
            }

            Status = sStatus;
            Secure( sCrucialKeyManager,NWDRandom.RandomStringCypher(NWDPlayerStatisticRequest.K_SIZE_OF_STAMP));
        }

        #endregion

        #region method

        protected void SetPayload(NWDPlayerStatisticDownPayload sDownPayload)
        {
            Payload = JsonConvert.SerializeObject(sDownPayload);
        }

        /// <summary>
        /// Return the payload as instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetPayload<T>(INWDCrucialKey sCrucialKeyManager) where T : NWDPlayerStatisticDownPayload
        {
            T? rReturn = null;
            if (IsValid( sCrucialKeyManager))
            {
                rReturn = JsonConvert.DeserializeObject<T>(Payload);
            }
            return rReturn;
        }

        #endregion

        #region methods

        private string GenerateHash(INWDCrucialKey sCrucialKeyManager)
        {
            string tSaltKey = sCrucialKeyManager.GetCrucialKey();
            if (string.IsNullOrEmpty(tSaltKey) == false)
            {
                string tPayLoadPrint = NWDSecurityTools.GenerateSha(Payload);
                return NWDSecurityTools.GenerateSha(tPayLoadPrint + StampCrucial + tSaltKey + Kind.ToString());
            }
            else
            {
                throw new Exception(sCrucialKeyManager.GetCrucialInstanceName()+" : "+ nameof(sCrucialKeyManager) + "." + nameof(sCrucialKeyManager.GetCrucialKey) + " return string empty or null!");
            }
        }

        private void Secure(INWDCrucialKey sCrucialKeyManager, string sStamp)
        {
            StampCrucial = sStamp;
            HashCrucial = GenerateHash( sCrucialKeyManager);
        }

        /// <summary>
        /// Test if Request is secured with the good hash print
        /// </summary>
        /// <returns></returns>
        public bool IsValid(INWDCrucialKey sCrucialKeyManager)
        {
            bool rReturn = false;
            if (string.IsNullOrEmpty(HashCrucial) == false)
            {
                if (GenerateHash(sCrucialKeyManager) == HashCrucial)
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
                    //NWDLogger.Information(nameof(NWDResponseCrucial) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
                case NWDRequestStatus.Error:
                case NWDRequestStatus.None:
                case NWDRequestStatus.NoNetwork:
                case NWDRequestStatus.DaoError:
                    //NWDLogger.Error(nameof(NWDResponseCrucial) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
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
                    //NWDLogger.Warning(nameof(NWDResponseCrucial) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
            }
        }
    }
