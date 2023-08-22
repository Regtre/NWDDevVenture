using System;
using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDServerMiddle.Models.Enum;

namespace NWDServerMiddle.Models
{
    [Serializable]
    public class NWDAccountInformation
    {
        public NWDAccountStatus Status { set; get; }
        public NWDAccount? Account { set; get; } = null;
        public NWDRequestPlayerToken? PlayerToken { set; get; } = null;

        public NWDRequestStatus RequestStatus { set; get; } = NWDRequestStatus.None;
    }
}