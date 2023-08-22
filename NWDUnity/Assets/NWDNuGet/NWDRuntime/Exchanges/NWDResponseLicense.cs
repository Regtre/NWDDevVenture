using System;
using Newtonsoft.Json;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades;
using NWDFoundation.Tools;
using NWDRuntime.Exchanges.Payloads;

namespace NWDRuntime.Exchanges
{
    public class NWDResponseLicense : NWDBasicResponse
    {
        #region properties

        public NWDExchangeLicenseKind Kind { set; get; } = NWDExchangeLicenseKind.Unknown;

        #endregion

        #region constructor

        /// <summary>
        /// Constructor for JSON
        /// </summary>
        public NWDResponseLicense()
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
        }

        /// <summary>
        /// Constructor for standard Reponse
        /// </summary>
        /// <param name="sEnvironment"></param>
        /// <param name="sKind"></param>
        /// <param name="sDownPayload"></param>
        /// <param name="sStatus"></param>
        /// <param name="sProjectKeyManager"></param>
        /// <param name="sProjectId"></param>
        public NWDResponseLicense(INWDProjectKey sProjectKeyManager, ulong sProjectId, NWDEnvironmentKind sEnvironment,NWDExchangeLicenseKind sKind, NWDDownPayloadLicense? sDownPayload, NWDRequestStatus sStatus)
        {
            ProjectId = sProjectId;
            Environment = sEnvironment;
            Timestamp = NWDTimestamp.GetTimestampUnix();
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
            Secure(sProjectKeyManager, NWDRandom.RandomStringCypher(32));
        }

        #endregion

        #region method

        protected void SetPayload(NWDDownPayloadLicense sDownPayload)
        {
            Payload = JsonConvert.SerializeObject(sDownPayload);
        }

        /// <summary>
        /// Return the payload as instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetPayload<T>(INWDProjectKey sProjectKeyManager) where T : NWDDownPayloadLicense
        {
            T? rReturn = null;
            if (IsValid( sProjectKeyManager))
            {
                rReturn = JsonConvert.DeserializeObject<T>(Payload);
            }
            return rReturn;
        }

        #endregion

        #region methods

        private string GenerateHash(INWDProjectKey sProjectKeyManager)
        {
            string tSaltKey = sProjectKeyManager.GetProjectKey(ProjectId, Environment);
            if (string.IsNullOrEmpty(tSaltKey) == false)
            {
                string tPayLoadPrint = NWDSecurityTools.GenerateSha(Payload);
                return NWDSecurityTools.GenerateSha(tPayLoadPrint + Stamp + tSaltKey + Kind.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        private void Secure(INWDProjectKey sProjectKeyManager,string sStamp)
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

            return rReturn;
        }

        #endregion
        
        public void Logger(string sId = "from request logger")
        {
            switch (Status)
            {
                case NWDRequestStatus.Ok:
                    //NWDLogger.Information(nameof(NWDResponseLicense) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
                case NWDRequestStatus.Error:
                case NWDRequestStatus.None:
                case NWDRequestStatus.NoNetwork:
                case NWDRequestStatus.DaoError:
                    //NWDLogger.Error(nameof(NWDResponseLicense) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
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
                    //NWDLogger.Warning(nameof(NWDResponseLicense) + " [" + sId + "] " + this.Status.ToString(), NWDLogger.SplitObjectSerializable(this));
                    break;
            }
        }
    }
}