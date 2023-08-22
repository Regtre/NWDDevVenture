using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Models;

public class NWDSideBarBlock
{
    public string Name { set; get; } = "Tree";
    public NWDBootstrapKindOfStyle BadgeStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
    public string BadgeText { set; get; } = "";
    public List<NWDSideBarCategory> Categories { set; get; } = new List<NWDSideBarCategory>();
}