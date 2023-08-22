using System;

namespace NWDServerMiddle.Models.Enum
{
    [Serializable]
    public enum NWDAccountStatus
    {
        Unknown = 0,
        Valid = 1,
        TokenError = 2,
        AccountTrashed = 3,
        AccountBan = 4,
        
        DaoError = 9,
        AccountUnknown = 10,
        AccountNotUnique = 11,
    }
}