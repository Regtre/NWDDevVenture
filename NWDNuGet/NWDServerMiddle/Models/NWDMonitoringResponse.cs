using System;

namespace NWDServerMiddle.Models
{
    [Serializable]
    public class NWDMonitoringResponse
    {
        public string Bug { set; get; } = string.Empty;
        public NWDMonitoringStatus Request { set; get; } = NWDMonitoringStatus.Unknown;
        public NWDMonitoringStatus Service { set; get; } = NWDMonitoringStatus.Unknown;
        public NWDMonitoringStatus Databases { set; get; } = NWDMonitoringStatus.Unknown;

        public string Foundation { set; get; } = string.Empty;

        public string Server { set; get; } = string.Empty;
        public NWDMonitoringResponse()
        {

        }
    }
}