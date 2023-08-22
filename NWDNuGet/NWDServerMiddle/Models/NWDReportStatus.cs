using System;

namespace NWDServerMiddle.Models
{
    [Serializable]
    public enum NWDReportStatus : int
    {
        Ko = -1,
        Unknown = 0,
        Ok = 1,
    }
}