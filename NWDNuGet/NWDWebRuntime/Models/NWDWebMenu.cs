using NWDWebRuntime.Facades;

namespace NWDWebRuntime.Models;

public class NWDWebMenu : INWDWebMenu
{
    public string PartialViewName = string.Empty;
    
    public string AspPartial()
    {
        return PartialViewName;
    }
}