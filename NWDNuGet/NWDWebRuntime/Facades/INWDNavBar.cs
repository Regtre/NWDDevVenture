using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Facades;

public interface INWDNavBar
{
    public NWDNavBarMenu[]? AddNavBarMenu(NWDNavBarKind sNavBarKind, HttpContext sHttpContext);
    public NWDNavBarMenu[]? AddNavBarAccount(HttpContext sHttpContext);
    public NWDNavBarCategory[]? AddNavBarAdmin(HttpContext sHttpContext);
    public NWDNavBarCategory[]? AddNavBarApp(HttpContext sHttpContext);
    public NWDNavBarCategory[]? AddNavBarDebug(HttpContext sHttpContext);
}