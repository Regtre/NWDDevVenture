using System;

namespace NWDServerMiddle.Models
{
    [Serializable]
    public class NWDOperationRequest
    {
        public string Key { set; get; } = string.Empty;

        public NWDOperationRequest()
        {

        }
    }
}
