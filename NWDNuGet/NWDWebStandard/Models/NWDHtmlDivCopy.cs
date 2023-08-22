using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models.Enums;

namespace NWDWebStandard.Models
{
    [Serializable]
    public class NWDHtmlDivCopy
    {
        public string Data { set; get; } = string.Empty;
        public NWDBootstrapKindOfStyle BootstrapKindOfStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
        
        public bool SmallButton { set; get; }
        public NWDHtmlDivCopy(string sData, bool sSmallButton = true, NWDBootstrapKindOfStyle sBootstrapKindOfStyle = NWDBootstrapKindOfStyle.Primary)
        {
            Data = sData;
            BootstrapKindOfStyle = sBootstrapKindOfStyle;
            SmallButton = sSmallButton;
        }
    }
}