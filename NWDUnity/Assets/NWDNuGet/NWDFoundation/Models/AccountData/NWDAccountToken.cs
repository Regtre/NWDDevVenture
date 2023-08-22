using System;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDAccountToken : NWDAccountData
    {
        #region properties
        public NWDExchangeOrigin ExchangeOrigin { set; get; }
        public string? Token { set; get; }
        
        #endregion
        
        #region constructor
        public NWDAccountToken()
        {
            ProjectId = 0;
            Account = 0;
            Range = 0;
            ExchangeOrigin = NWDExchangeOrigin.Unknown;
            Token = string.Empty;
        }
        
        public NWDAccountToken(NWDRequestPlayerToken sToCopy)
        {
            ProjectId = sToCopy.ProjectId;
            Account = sToCopy.PlayerReference;
            Range = sToCopy.AccountRange;
            ExchangeOrigin = sToCopy.ExchangeOrigin;
            Token = sToCopy.Token;
        }

        #endregion
    }
}