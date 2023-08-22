using System;

namespace NWDServerMiddle.Models
{
    [Serializable]
    public class NWDReportRequest
    {
        public string Key { set; get; } = string.Empty;

        public NWDReportRequest()
        {

        }
    }
}
