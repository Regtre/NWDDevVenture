using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Models;

public class NWDNavBarMenu
{
    public string Name { set; get; } = "MenuName";
    public string Description { set; get; } = "";
    public int Annotation { set; get; } = 0;
    public bool NavBar { set; get; } = true; 
    public string IconStyle { set; get; } = "";
    public string TokenId  { set; get; } = NWDRandom.RandomStringToken(16);
    public int MaxLines { set; get; } = 5;
    public string ActionName { set; get; } = "";
    public string ControllerName { set; get; } = "";
    public string UrlParameter { set; get; } = "";
    
    public NWDBootstrapKindOfStyle BadgeStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
    public string BadgeText { set; get; } = "";
    public List<NWDNavBarCategory> Categories { set; get; } = new List<NWDNavBarCategory>();
    
    public string Url()
    {
        if (string.IsNullOrEmpty(ControllerName) && string.IsNullOrEmpty(ActionName))
        {
            return string.Empty;
        }
        return "/" + ControllerName + "/" + ActionName + "?" + UrlParameter;
    }
}