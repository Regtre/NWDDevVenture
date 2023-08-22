using System;

namespace NWDServerMiddle.Models
{
    [Serializable]
    public class NWDMonitoringRequest
    {
        public string Key { set; get; } = string.Empty;

        public NWDMonitoringRequest()
        {

        }
    }
}
