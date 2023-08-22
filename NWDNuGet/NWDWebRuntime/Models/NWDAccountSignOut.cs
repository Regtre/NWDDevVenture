
using NWDWebRuntime.Facades;
using NWDWebRuntime.Models.Enums;
namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountSignOut : INWDTempData
{
    public string AccountSignOutController { set; get; } = string.Empty;
    public string AccountSignOutAction { set; get; } = string.Empty;
}