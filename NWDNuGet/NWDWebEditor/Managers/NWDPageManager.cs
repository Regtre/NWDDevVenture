using NWDWebRuntime.Models;
using NWDWebStandard.Models;

namespace NWDWebEditor.Managers;

public static class NWDPageManager
{
    public static List<NWDPage> GetPageUseAsMenu()
    {
        return NWDWebDBDataManager.GetBy<NWDPage>(new Dictionary<string, string>(){{nameof(NWDPage.UseAsNavBarMenu), "1" }});
    }
}