using System;

namespace NWDFoundation.Models.Enums
{
    [Serializable]
    public enum NWDServiceKind
    {
        Session = 0,
        Cookie = 1,
        IpUnique = 2,
        
        IpClassD = 10,
        IpClassC = 11,
        IpClassB = 12,
        IpClassA = 13,
    }
}