namespace NWDWebRuntime.Models
{
    [Serializable]
    public class NWDSocialShareableUrl
    {
        public string Title { set; get; } = string.Empty;
        public string UrlEncoded { set; get; } = string.Empty;
        public NWDSocialShareableKind Kind { set; get; } = NWDSocialShareableKind.Html;
        
        public NWDSocialShareableStyle Style { set; get; } = NWDSocialShareableStyle.Inline;

        public NWDSocialShareableUrl()
        {
            
        }
        public NWDSocialShareableUrl(string sTitle, string sUrlEncoded, NWDSocialShareableKind sKind, NWDSocialShareableStyle sStyle = NWDSocialShareableStyle.Inherited)
        {
            Title = sTitle;
            UrlEncoded = sUrlEncoded;
            Kind = sKind;
            Style = sStyle;
        }
    }
}