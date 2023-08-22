using System;
using Newtonsoft.Json;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDRuntime.Exchanges.Payloads;

namespace NWDRuntime.Exchanges
{
    [Serializable]
    public class NWDRequestLicense : NWDBasicRequest
    {
        #region properties


        /// <summary>
        /// Kind of request
        /// </summary>
        // [JsonProperty(PropertyName ="KIN")]
        public NWDExchangeLicenseKind Kind { set; get; } = NWDExchangeLicenseKind.Unknown;

        #endregion

        #region constructor

        /// <summary>
        /// Constructor for JSON
        /// </summary>
        public NWDRequestLicense()
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
        }

        /// <summary>
        /// Constructor of standard Request
        /// </summary>
        /// <param name="sEnvironment"></param>
        /// <param name="sKind"></param>
        /// <param name="sUpPayload"></param>
        /// <param name="sOrigin"></param>
        /// <param name="sDevice"></param>
        /// <param name="sProjectKeyManager"></param>
        /// <param name="sProjectId"></param>
        public NWDRequestLicense(INWDProjectKey sProjectKeyManager, ulong sProjectId, NWDEnvironmentKind sEnvironment, NWDExchangeLicenseKind sKind, NWDUpPayloadLicense? sUpPayload, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice)
        {
            ProjectId = sProjectId;
            Environment = sEnvironment;
            Timestamp = NWDTimestamp.GetTimestampUnix();
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

            Secure(sProjectKeyManager, NWDRandom.RandomStringCypher(32));
        }

        #endregion

        #region methods

        protected void SetPayload(NWDUpPayloadLicense sUpPayload)
        {
            Payload = JsonConvert.SerializeObject(sUpPayload);
        }

        /// <summary>
        /// Return the payload as instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetPayload<T>(INWDProjectKey sProjectKeyManager) where T : NWDUpPayloadLicense
        {
            T? rReturn = null;
            if (IsValid(sProjectKeyManager))
            {
                rReturn = JsonConvert.DeserializeObject<T>(Payload);
            }
            return rReturn;
        }

        public string GenerateHash(INWDProjectKey sProjectKeyManager)
        {
            string tSaltKey = sProjectKeyManager.GetProjectKey(ProjectId, Environment);
            if (string.IsNullOrEmpty(tSaltKey) == false)
            {
                string tPayLoadPrint = NWDSecurityTools.GenerateSha(Payload);
                string tHash = NWDSecurityTools.GenerateSha(tPayLoadPrint + Stamp + tSaltKey + Kind.ToString() + Origin.ToString());
                return tHash;
            }
            else
            {
                throw new Exception(sProjectKeyManager.GetProjectKeyInstanceName()+" : "+ nameof(INWDProjectKey) + "." + nameof(INWDProjectKey.GetProjectKey) + " return string empty or null!");
            }
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
            if (string.IsNullOrEmpty(Hash) == false)
            {
                if (GenerateHash(sProjectKeyManager) == Hash)
                {
                    rReturn = true;
                }
            }
            else
            {
                NWDLogger.Warning(nameof(Hash) + " is empty");
            }

            return rReturn;
        }

        #endregion

        #region static factories

        public static NWDRequestLicense CreateRequestTest(INWDProjectKey sProjectKeyManager, ulong sProjectId, NWDEnvironmentKind sEnvironment, NWDExchangeOrigin sOrigin = NWDExchangeOrigin.Unknown, NWDExchangeDevice sDevice = NWDExchangeDevice.Unknown)
        {
            return new NWDRequestLicense(sProjectKeyManager,  sProjectId,  sEnvironment, NWDExchangeLicenseKind.Test,new NWDUpPayloadLicense(), sOrigin, sDevice);
        }

        #endregion
        
        public string Logger()
        { 
            string rReturn = NWDRandom.RandomString(24);
            NWDLogger.Information(nameof(NWDRequestLicense) + " ["+rReturn+"]", NWDLogger.SplitObjectSerializable(this));
            return rReturn;
        }
    }
}