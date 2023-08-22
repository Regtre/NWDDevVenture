using System;

namespace NWDServerMiddle.Models;

[Serializable]
public enum NWDMonitoringStatus : int
{
    Ko = -1,
    Unknown = 0,
    Ok = 1,
}