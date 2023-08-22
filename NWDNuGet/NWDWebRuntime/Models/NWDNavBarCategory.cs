using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;

namespace NWDWebRuntime.Models;

public class NWDNavBarCategory
{
 public string Name { set; get; } = "";
 public string Description { set; get; } = "";
 public string IconStyle { set; get; } = "";
 public NWDBootstrapKindOfStyle BadgeStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
 public string BadgeText { set; get; } = "";
 public List<NWDNavBarElement> Elements { set; get; } = new List<NWDNavBarElement>();
}