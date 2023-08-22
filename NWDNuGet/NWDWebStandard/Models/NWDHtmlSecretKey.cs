using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models.Enums;

namespace NWDWebStandard.Models
{
    [Serializable]
    public class NWDHtmlSecretKey
    {
        public string Data { set; get; } = string.Empty;
        public NWDBootstrapKindOfStyle BootstrapKindOfStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
        
        public bool SmallButton { set; get; }
        public NWDHtmlSecretKey(string sData, bool sSmallButton = true, NWDBootstrapKindOfStyle sBootstrapKindOfStyle = NWDBootstrapKindOfStyle.Primary)
        {
            Data = sData;
            BootstrapKindOfStyle = sBootstrapKindOfStyle;
            SmallButton = sSmallButton;
        }
    }
}