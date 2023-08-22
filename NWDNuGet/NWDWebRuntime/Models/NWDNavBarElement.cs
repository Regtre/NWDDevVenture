using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Models;

public class NWDNavBarElement
{
    public string Name { set; get; } = "Element";
    public string Icon { set; get; } = "";
    public string ActionName { set; get; } = "#";
    public string ControllerName { set; get; } = "#";
    public string UrlParameter { set; get; } = "#";
    public NWDBootstrapKindOfStyle BadgeStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
    public string BadgeText { set; get; } = "";

    public string Url()
    {
        string rReturn = "";
        string tParam = "";
        if (string.IsNullOrEmpty(ControllerName) == false)
        {
            rReturn = "/" + ControllerName;
            tParam = "?";
        }
        if (string.IsNullOrEmpty(ActionName) == false)
        {
            rReturn = rReturn + "/" + ActionName + "/";
            tParam = "?";
        }
        if (string.IsNullOrEmpty(UrlParameter) == false)
        {
            rReturn = rReturn + tParam + UrlParameter;
        }
        return rReturn;
    }
    
}