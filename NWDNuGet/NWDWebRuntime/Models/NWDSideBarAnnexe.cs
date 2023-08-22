using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Models;

public class NWDSideBarAnnexe
{
    public string Name { set; get; } = "Tree";
    public string ImagePath { set; get; } = "Tree";
    public NWDBootstrapKindOfStyle Style { set; get; } = NWDBootstrapKindOfStyle.Primary;
    public string Description { set; get; } = "";
    public NWDBootstrapKindOfStyle BadgeStyle { set; get; } = NWDBootstrapKindOfStyle.Primary;
    public string BadgeText { set; get; } = "";
    
}