using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NWDEditor.Exchanges.Payloads;
using NWDFoundation.Configuration;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;

namespace NWDEditor.Exchanges
{
    [Serializable]
    // TO DO REMOVE
    [Obsolete("TO REMOVE : Use Treat manager")]
    public class  NWDRequestEditor 
    {
        #region properties
        
        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Ori")]
        public NWDExchangeOrigin Origin { set; get; } = NWDExchangeOrigin.Unknown;

        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("Dvc")]
        public NWDExchangeDevice Device { set; get; } = NWDExchangeDevice.Unknown;

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
        //[JsonProperty("Sta")]
        public string StampStudio { set; get; } = string.Empty; //= DWN_Random.RandomStringToken(64);
        /// <summary>
        /// 
        /// </summary>
        //[JsonProperty("HSD")]
        public string HashStudio { set; get; } = string.Empty; //= DWN_Random.RandomStringToken(64);

        public string RolePublicKey { get; set; } = string.Empty;
        #endregion

        #region constructor

        /// <summary>
        /// Constructor for JSON
        /// </summary>
        public NWDRequestEditor()
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
        }

        /// <summary>
        /// Constructor of standard Request
        /// </summary>
        /// <param name="sKind"></param>
        /// <param name="sUpPayload"></param>
        /// <param name="sOrigin"></param>
        /// <param name="sDevice"></param>
        public NWDRequestEditor(string sRolePublicToken, NWDExchangeEditorKind sKind, NWDUpPayloadEditor? sUpPayload, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice, string sToken)
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
            RolePublicKey = sRolePublicToken;
            Kind = sKind;
            Origin = sOrigin;
            Device = sDevice;
            if (sUpPayload != null)
            {
                SetPayload(sUpPayload);
            }
            Secure(NWDRandom.RandomStringCypher(32),sToken);
        }

        /// <summary>
        /// Constructor of standard Request not secured
        /// </summary>
        /// <param name="sKind"></param>
        /// <param name="sUpPayload"></param>
        /// <param name="sOrigin"></param>
        /// <param name="sDevice"></param>
        private NWDRequestEditor(NWDExchangeEditorKind sTest, NWDUpPayloadEditor sUpPayload, NWDExchangeOrigin sOrigin, NWDExchangeDevice sDevice)
        {
            Timestamp = NWDTimestamp.GetTimestampUnix();
            Kind = sTest;
            Origin = sOrigin;
            Device = sDevice;
            SetPayload(sUpPayload);
        }

        #endregion
        
        #region methods
        
        protected void SetPayload(NWDUpPayloadEditor sUpPayload)
        {
            Payload = JsonConvert.SerializeObject(sUpPayload);
        }
        /// <summary>
        /// Return the payload as instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetPayload<T>() where T : NWDUpPayloadEditor
        { 
            T? rReturn = null;
            // if (IsValid( sCrucialKeyManager))
            {
                rReturn = JsonConvert.DeserializeObject<T>(Payload);
            }
            return rReturn;
        }
        
        public string GenerateHash(string sToken)
        {
            
                string tSaltKey = sToken;
                string tPayLoadPrint = NWDSecurityTools.GenerateSha(Payload);
                return  NWDSecurityTools.GenerateSha(tPayLoadPrint + StampStudio + tSaltKey + Kind.ToString() + Origin.ToString());

        }

        public void Secure(string sStamp, string sToken)
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
        
        #region static factories

        public static NWDRequestEditor CreateRequestTest( NWDExchangeOrigin sOrigin = NWDExchangeOrigin.Unknown, NWDExchangeDevice sDevice = NWDExchangeDevice.Unknown)
        {
            return new NWDRequestEditor(NWDExchangeEditorKind.Test,
                new NWDUpPayloadEditor(), sOrigin, sDevice);
        }
        
        #endregion
        
        
        public string Logger()
        { 
            string rReturn = NWDRandom.RandomString(24);
            NWDLogger.Information(nameof(NWDRequestEditor) + " ["+rReturn+"]", NWDLogger.SplitObjectSerializable(this));
            return rReturn;
        }
    }
}