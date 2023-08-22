namespace NWDWebRuntime.Models
{
    public enum NWDSocialShareableStyle : int
    {
        Inherited,
        Inline,
        Carded,
        MenuDropdown,
        Toolbar,
    }
    public enum NWDSocialShareableGlobalStyle : int
    {
        Inline = NWDSocialShareableStyle.Inline,
        Carded = NWDSocialShareableStyle.Carded,
        MenuDropdown = NWDSocialShareableStyle.MenuDropdown,
        Toolbar = NWDSocialShareableStyle.Toolbar,
    }
}