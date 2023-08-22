using System;
using Newtonsoft.Json;
using NWDCrucial.Configuration;
using NWDCrucial.Exchanges.Payloads;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;

namespace NWDCrucial.Exchanges
{
    [Serializable]
    public class NWDRequestCrucial : NWDBasicRequest
    {
        #region properties

        public const int K_SIZE_OF_STAMP = 64;

        #endregion

        #region properties

        /// <summary>
        /// Origin of request
        /// </summary>
        // [JsonProperty(PropertyName ="ORI")]
        public NWDExchangeOrigin Origin { set; get; } = NWDExchangeOrigin.Unknown;

        /// <summary>
        /// Device of request
        /// </summary>
        // [JsonProperty(PropertyName ="DVC")]
        public NWDExchangeDevice Device { set; get; } = NWDExchangeDevice.Unknown;

        /// <summary>
        /// Kind of request
        /// </summary>
        // [JsonProperty(PropertyName ="KIN")]
        public NWDExchangeCrucialKind Kind { set; get; } = NWDExchangeCrucialKind.Unknown;

        /// <summary>
        /// Payload in request
        /// </summary>
        // [JsonProperty(PropertyName ="PAY")]
        public string Payload { set; get; } = string.Empty;

        /// <summary>
        /// Timestamp of request
        /// </summary>
        // [JsonProperty(PropertyName ="TIM")]
        public int Timestamp { set; get; }

        /// <summary>
        /// Random stamp of request
        /// </summary>
        // [JsonProperty(PropertyName ="STA")]
        public string StampCrucial { set; get; } = string.Empty; //= DWN_Random.RandomStringToken(64);

        /// <summary>
        /// Hash sign of request
        /// </summary>
        // [JsonProperty(PropertyName ="HAH")]
        public string HashCrucial { set; get; } = string.Empty; //= DWN_Random.RandomStringToken(64);

        #endregion

        #region constructor

        /// <summary>
        /// Constructor for JSON
        /// </summary>
        public NWDRequestCrucial()
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
        }

        /// <summary>
        /// Constructor of standard Request
        /// </summary>
        /// <param name="sCrucialKeyManager"></param>
        /// <param name="sKind"></param>
        /// <param name="sUpPayload"></param>
        /// <param name="sOrigin"></param>
        /// <param name="sDevice"></param>
        public NWDRequestCrucial(INWDCrucialKey sCrucialKeyManager, NWDExchangeCrucialKind sKind, NWDUpPayloadCrucial? sUpPayload, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice)
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
            ProjectId = sCrucialKeyManager.GetCrucialProjectId();
            Environment = sCrucialKeyManager.GetCrucialEnvironment();
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

            Secure(sCrucialKeyManager, NWDRandom.RandomStringCypher(K_SIZE_OF_STAMP));
        }

        #endregion

        #region methods

        protected void SetPayload(NWDUpPayloadCrucial sUpPayload)
        {
            Payload = JsonConvert.SerializeObject(sUpPayload);
        }

        /// <summary>
        /// Return the payload as instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetPayload<T>(INWDCrucialKey sCrucialKeyManager) where T : NWDUpPayloadCrucial
        {
            T? rReturn = null;
            if (IsValid( sCrucialKeyManager))
            {
                rReturn = JsonConvert.DeserializeObject<T>(Payload);
            }
            return rReturn;
        }

        public string GenerateHash(INWDCrucialKey sCrucialKeyManager)
        {
            string tSaltKey = sCrucialKeyManager.GetCrucialKey();
            if (string.IsNullOrEmpty(tSaltKey) == false)
            {
                string tPayLoadPrint = NWDSecurityTools.GenerateSha(Payload);
                string tHash  = NWDSecurityTools.GenerateSha(tPayLoadPrint + StampCrucial + tSaltKey + Kind.ToString() + Origin.ToString());
                return tHash;
            }
            else
            {
                throw new Exception(nameof(NWDCrucialConfiguration) + "." + nameof(sCrucialKeyManager.GetCrucialKey) + " return string empty or null!");
            }
        }

        public void Secure(INWDCrucialKey sCrucialKeyManager, string sStamp)
        {
            StampCrucial = sStamp;
            HashCrucial = GenerateHash(sCrucialKeyManager);
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
            else
            {
                NWDLogger.Warning(nameof(HashCrucial) + " is empty");
            }

            return rReturn;
        }

        #endregion

        #region static factories

        public static NWDRequestCrucial CreateRequestTest(INWDCrucialKey sCrucialKeyManager, NWDExchangeOrigin sOrigin = NWDExchangeOrigin.Unknown, NWDExchangeDevice sDevice = NWDExchangeDevice.Unknown)
        {
            return new NWDRequestCrucial(sCrucialKeyManager, NWDExchangeCrucialKind.Test, new NWDUpPayloadCrucial(), sOrigin, sDevice);
        }

        #endregion
        
        public string Logger()
        { 
            string rReturn = NWDRandom.RandomString(24);
            //NWDLogger.Information(nameof(NWDRequestCrucial) + " ["+rReturn+"]", NWDLogger.SplitObjectSerializable(this));
            return rReturn;
        }
    }
}