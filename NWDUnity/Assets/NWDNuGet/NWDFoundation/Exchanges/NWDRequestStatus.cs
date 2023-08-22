using System;
namespace NWDFoundation.Exchanges
{
    [Serializable]
    public enum NWDRequestStatus
    {
        /// <summary>
        /// Answer returned if Server is Disabled 
        /// </summary>
        ServerIsDisabled = -3,
        /// <summary>
        /// Answer returned if Server is overflow 
        /// </summary>
        PleaseChangeServer = -2,
        /// <summary>
        /// Just Test answer : ok or ko
        /// </summary>
        Test = -1,
        /// <summary>
        /// No Response
        /// </summary>
        None = 0,
        /// <summary>
        /// Answer Unknow 
        /// </summary>
        Unknown = 98,
        /// <summary>
        /// Answer Error 
        /// </summary>
        Error = 99,
        
        Ok = 1,
        
        ProjectIsPublishing = 9,
        
        AccountUnknown = 89,
        
        AccountError = 90,
        
        AccountNotUnique = 91,
        
        AccountBan = 12,
        
        AccountTrashed = 13,
        
        NoNetwork = 700,
        
        DaoError = 800,
        
        TokenError = 900,
        
        HashInvalid = 901,
        
        TokenNull = 902,
        
        TokenEmpty = 903,
        
        
    }
}

