using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;
using NWDWebEditor;

namespace NWDWebPlayerDemo.Models;

public class Test2  : NWDDatabaseWebBasicModel,WebEditionListedObject
{
    [NWDWebPropertyDescription("Title", NWDWebEditionStyle.Text,false, "", "my Title", "my Placeholder", sUseAsSortBy: true, sUseAsColumn: false, sIsPrimaryColumn: true)]
    public string Name2 { set; get; } = string.Empty;
    [NWDWebPropertyDescription("Color", NWDWebEditionStyle.Color,false, "", "my Description", "my Description Placeholder")]
    public string Color2 { set; get; } = string.Empty;
    [NWDWebPropertyDescription("Date", NWDWebEditionStyle.Date,false, "", "my Description", "my Description Placeholder")]
    public DateTime Date2 { set; get; } = DateTime.Now;
    
    public string DisplayString()
    {
        return Name2 + "-" + Color2 + "-" + Date2;
    }
}