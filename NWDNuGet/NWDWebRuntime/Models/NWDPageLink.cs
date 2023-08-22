namespace NWDWebRuntime.Models;

[Serializable]
public class NWDPageLink
{
    public string CssStyle;
    public bool Active;
    public string Text;
    public int Page;

    public NWDPageLink(string sCssStyle, bool sActive, string sText, int sPage)
    {
        CssStyle = sCssStyle;
        Active = sActive;
        Text = sText;
        Page = sPage;
    }
}
