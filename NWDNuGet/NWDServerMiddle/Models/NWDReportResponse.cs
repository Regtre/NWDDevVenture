using System;

namespace NWDServerMiddle.Models
{
    [Serializable]
    public class NWDReportResponse
    {
        public string Bug { set; get; } = string.Empty;
        public NWDReportStatus Request { set; get; } = NWDReportStatus.Unknown;
        public NWDReportStatus Service { set; get; } = NWDReportStatus.Unknown;
        public NWDReportStatus Databases { set; get; } = NWDReportStatus.Unknown;

        public string Foundation { set; get; } = string.Empty;

        public string Server { set; get; } = string.Empty;
        public NWDReportResponse()
        {

        }
    }
}