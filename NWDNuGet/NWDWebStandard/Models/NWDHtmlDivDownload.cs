using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models.Enums;

namespace NWDWebStandard.Models
{
    [Serializable]
    public class NWDHtmlDivDownload
    {
        public string Filename { set; get; } = "filename.txt";
        public string Data { set; get; } = string.Empty;
        public NWDBootstrapKindOfStyle BootstrapKindOfStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
        public bool SmallButton { set; get; }

        public NWDHtmlDivDownload(string sFilename, string sData, bool sSmallButton = true, NWDBootstrapKindOfStyle sBootstrapKindOfStyle = NWDBootstrapKindOfStyle.Primary)
        {
            Filename = sFilename;
            Data = sData;
            BootstrapKindOfStyle = sBootstrapKindOfStyle;
            SmallButton = sSmallButton;
        }
    }
}