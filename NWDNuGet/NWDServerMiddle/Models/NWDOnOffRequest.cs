using System;

namespace NWDServerMiddle.Models
{
    [Serializable]
    public class NWDOnOffRequest
    {
        public string Key { set; get; } = string.Empty;
        public NWDOnOffStatus OnOff { set; get; } = NWDOnOffStatus.On;

        public NWDOnOffRequest()
        {

        }
    }
}
