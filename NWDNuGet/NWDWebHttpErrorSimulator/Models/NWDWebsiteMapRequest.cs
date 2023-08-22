using NWDFoundation.WebEdition.Enums;

namespace NWDWebHttpErrorSimulator.Models
{
[Serializable]
    public class NWDWebsiteMapRequest
    {
        public string ControllerName { set; get; } = string.Empty;
        public string ActionName { set; get; } = string.Empty;
        public string ReturnTypeName { set; get; } = string.Empty;
        public string GetParams { set; get; } = string.Empty;
        public string PostLinearized { set; get; } = string.Empty;
        public int Expected{ set; get; } = 200;
        public NWDPageStandardStatusTag StatusTag { set; get; } = NWDPageStandardStatusTag.None;
    }
}