using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NWDEditor.Exchanges;
using NWDEditor.Exchanges.Payloads;
using NWDTreat.Configuration;
using NWDTreat.Exchanges.Payloads;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;

namespace NWDTreat.Exchanges
{
    [Serializable]
    public class NWDRequestTreat : NWDBasicRequest
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
        public NWDRequestTreat()
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
        }

        /// <summary>
        /// Constructor of standard Request
        /// </summary>
        /// <param name="sTreatKeyManager"></param>
        /// <param name="sEnvironment"></param>
        /// <param name="sKind"></param>
        /// <param name="sUpPayload"></param>
        /// <param name="sOrigin"></param>
        /// <param name="sDevice"></param>
        /// <param name="sProjectId"></param>
        public NWDRequestTreat(INWDTreatKey sTreatKeyManager, ulong sProjectId, NWDEnvironmentKind sEnvironment, NWDExchangeTreatKind sKind, NWDUpPayloadTreat? sUpPayload, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice)
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

            Secure(sTreatKeyManager, NWDRandom.RandomStringCypher(32));
        }

        #endregion

        #region methods

        protected void SetPayload(NWDUpPayloadTreat sUpPayload)
        {
            Payload = JsonConvert.SerializeObject(sUpPayload);
        }

        /// <summary>
        /// Return the payload as instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetPayload<T>(INWDTreatKey sTreatKeyManager) where T : NWDUpPayloadTreat
        {
            T? rReturn = null;
            if (IsValid(sTreatKeyManager))
            {
                rReturn = JsonConvert.DeserializeObject<T>(Payload);
            }

            return rReturn;
        }

        public string GenerateHash(INWDTreatKey sTreatKeyManager)
        {

            if (Payload == null)
            {
                Payload = string.Empty;
            }
            string tSaltKey = sTreatKeyManager.GetTreatKey(ProjectId, Environment);
            if (string.IsNullOrEmpty(tSaltKey) == false)
            {
                string tPayLoadPrint = NWDSecurityTools.GenerateSha(Payload);
                return NWDSecurityTools.GenerateSha(tPayLoadPrint + StampTreat + tSaltKey + Kind.ToString() + Origin.ToString());
            }
            else
            {
                throw new Exception(sTreatKeyManager.GetTreatInstanceName()+" : "+ nameof(INWDTreatKey) + "." + nameof(INWDTreatKey.GetTreatKey) + " return string empty or null!");
            }
        }

        public void Secure(INWDTreatKey sTreatKeyManager, string sStamp)
        {
            StampTreat = sStamp;
            HashTreat = GenerateHash(sTreatKeyManager);
        }

        /// <summary>
        /// Test if Request is secured with the good hash print
        /// </summary>
        /// <returns></returns>
        public bool IsValid(INWDTreatKey sTreatKeyManager)
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

        #region static factories

        public static NWDRequestTreat CreateRequestTest(INWDTreatKey sTreatKeyManager, ulong sProjectId, NWDEnvironmentKind sEnvironment, NWDExchangeOrigin sOrigin = NWDExchangeOrigin.Unknown, NWDExchangeDevice sDevice = NWDExchangeDevice.Unknown)
        {
            return new NWDRequestTreat(sTreatKeyManager, sProjectId, sEnvironment, NWDExchangeTreatKind.Test,
                new NWDUpPayloadTreat(), sOrigin, sDevice);
        }

        public static NWDRequestTreat CreateRequestServiceCreate(INWDTreatKey sTreatKeyManager, ulong sProjectId, NWDEnvironmentKind sEnvironment, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice, List<NWDProjectServiceKey> sServiceList)
        {
            return new NWDRequestTreat(sTreatKeyManager, sProjectId, sEnvironment, NWDExchangeTreatKind.ServiceCreate,
                new NWDUpPayloadServiceCreate(sServiceList), sOrigin, sDevice);
        }

        public static NWDRequestTreat CreateRequestAssociateService(INWDTreatKey sTreatKeyManager, ulong sProjectId, NWDEnvironmentKind sEnvironment, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice, NWDAccountService sAccountService /*, NWDRequestPlayerToken sPlayerToken*/)
        {
            return new NWDRequestTreat(sTreatKeyManager, sProjectId, sEnvironment, NWDExchangeTreatKind.AssociateService,
                new NWDUpPayloadTreatService( /*sPlayerToken,*/sAccountService), sOrigin, sDevice);
        }
        public static NWDRequestTreat CreateRequestDissociateServiceAndSubServices(INWDTreatKey sTreatKeyManager, ulong sProjectId, NWDEnvironmentKind sEnvironment, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice, NWDReference<NWDAccountService> sService,  NWDReference<NWDAccount> sAccount)
        {
            return new NWDRequestTreat(sTreatKeyManager, sProjectId, sEnvironment, NWDExchangeTreatKind.DissociateService,
                new NWDUpPayloadTreatDissociateServiceAndSubServices(sService,sAccount), sOrigin, sDevice);
        }
        public static NWDRequestTreat CreateRequestAssociateSubService(INWDTreatKey sTreatKeyManager, ulong sProjectId, NWDEnvironmentKind sEnvironment, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice, NWDAccountService sOfferByService, NWDAccountService sOfferToService, NWDAccount sOfferByAccount, NWDAccount sOfferToAccount)
        {
            return new NWDRequestTreat(sTreatKeyManager, sProjectId, sEnvironment, NWDExchangeTreatKind.AssociateSubService,
                new NWDUpPayloadTreatAssociateSubService(sOfferByService,sOfferToService,sOfferByAccount,sOfferToAccount), sOrigin, sDevice);
        }
        public static NWDRequestTreat CreateRequestGetSubServicesFromAccount(INWDTreatKey sTreatKeyManager, ulong sProjectId, NWDEnvironmentKind sEnvironment, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice, NWDAccount sAccount)
        {
            return new NWDRequestTreat(sTreatKeyManager, sProjectId, sEnvironment, NWDExchangeTreatKind.GetSubServicesFromAccount,
                new NWDUpPayloadTreatGetSubServicesFromAccount(sAccount,sEnvironment), sOrigin, sDevice);
        }

        #endregion

        public string Logger()
        {
            string rReturn = NWDRandom.RandomString(24);
            NWDLogger.Information(nameof(NWDRequestTreat) + " [" + rReturn + "]", NWDLogger.SplitObjectSerializable(this));
            return rReturn;
        }

       
    }
}