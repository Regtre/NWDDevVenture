using System;

namespace NWDServerMiddle.Models
{
    [Serializable]
    public class NWDOperationResponse
    {
        public string Bug { set; get; } = string.Empty;
        public NWDOperationStatus Request { set; get; } = NWDOperationStatus.Unknown;
        public NWDOperationStatus Service { set; get; } = NWDOperationStatus.Unknown;
        public NWDOperationStatus Databases { set; get; } = NWDOperationStatus.Unknown;

        public string Foundation { set; get; } = string.Empty;

        public string Server { set; get; } = string.Empty;
        public NWDOperationResponse()
        {

        }
    }
}