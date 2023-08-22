using System;
using NWDFoundation.Exchanges;

namespace NWDRuntime.Exchanges
{
    [Serializable]
    public enum NWDExchangeRuntimeKind 
    {
        /// <summary>
        /// No exchange kind (!)
        /// </summary>
        None = NWDExchangeKind.None,
        /// <summary>
        /// Use to test connection : ok or ko
        /// </summary>
        Test = NWDExchangeKind.Test,
        /// <summary>
        /// Not yet specified
        /// </summary>
        Unknown = NWDExchangeKind.Unknown,
        
        AccountDelete = 2, //=>Payload NWDUpPayloadAccountDelete, response NWDDownPayloadAccountDelete
        AccountChangeRange = 3, //=>Payload NWDUpPayloadAccountChangeRange, response NWDDownPayloadAccountChangeRange

        SignOut = 10, //=>Payload NWDUpPayloadAccountSignOut, response NWDDownPayloadAccountSignOut
        SignIn = 11, //=>Payload NWDUpPayloadAccountSignIn, response NWDDownPayloadAccountSignIn
        SignUp = 12, //=>Payload NWDUpPayloadAccountSignUp, response NWDDownPayloadAccountSignUp
        SignLost = 13, //=>Payload NWDUpPayloadAccountSignLost, response NWDDownPayloadAccountSignLost
        SignAdd = 14, //=>Payload NWDUpPayloadAccountSignAdd, response NWDDownPayloadAccountSignAdd
        SignModify = 15, //=>Payload NWDUpPayloadAccountSignModify, response NWDDownPayloadAccountSignModify
        SignDelete = 16, //=>Payload NWDUpPayloadAccountSignDelete, response NWDDownPayloadAccountSignDelete
        GetAllSign = 17, //=>Payload NWDUpPayloadAccountSignAll, response NWDDownPayloadAccountSignAll
        SignRescue = 18, //=>Payload NWDUpPayloadAccountSignRescue, response NWDDownPayloadAccountSignRescue

        SyncDataByIncrement = 21, //=>Payload NWDUpPayloadDataSyncByIncrement, response NWDDownPayloadDataSyncByIncrement
        GetAllData = 22, //=>Payload NWDUpPayloadAllData, response NWDDownPayloadAllData

        GetAllPlayerData = 31, //=>Payload NWDUpPayloadAllPlayerData, response NWDPayloadAllPlayerDataRespons

        GetPlayerDataByReferences = 32, //=>Payload NWDUpPayloadPlayerDataByReferences, response PayloadPlayerDataByReferencesResponse

        GetPlayerDataByBundle = 33, //=> Payload NWDUpPayloadPlayerDataByBundle, response NWDDownPayloadPlayerDataByBundle

        GetAllStudioData = 41, //=>Payload NWDUpPayloadAllStudioData, response PayloadAllStudioDataResponse

        GetStudioDataByReferences = 42, //=> Payload NWDUpPayloadStudioDataByReferences, response NWDDownPayloadStudioDataByReferences

        GetStudioDataByBundle = 43, //=> Payload NWDUpPayloadStudioDataByBundle, response NWDDownPayloadStudioDataByBundle
     
        CreateRelationship = 50, 
        LinkRelationship = 51, 
        FinalizeRelationship = 52,
        GetRelationship = 53,
        
    }
}