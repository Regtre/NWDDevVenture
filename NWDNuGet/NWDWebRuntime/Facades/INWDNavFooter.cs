using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Facades;

public interface INWDNavFooter
{
    public NWDNavBarCategory[]? AddNavFooterMenu(HttpContext sHttpContext)
    {
        return null;
    }
}