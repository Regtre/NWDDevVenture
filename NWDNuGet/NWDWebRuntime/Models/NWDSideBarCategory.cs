using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools;

namespace NWDWebRuntime.Models;

public class NWDSideBarCategory
{
 public string Name { set; get; } = "Category";
 public string IconStyle { set; get; } = "";
 public bool AlwaysShow { set; get; } = false;
 public string TokenId = NWDRandom.RandomStringToken(16);
 public NWDBootstrapKindOfStyle BadgeStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
 public string BadgeText { set; get; } = "";
 public List<NWDSideBarElement> Elements { set; get; } = new List<NWDSideBarElement>();
}