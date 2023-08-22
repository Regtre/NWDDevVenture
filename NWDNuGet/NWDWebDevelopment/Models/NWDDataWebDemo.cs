using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDWebDevelopment.Models;
[Serializable]
public class NWDDataWebDemo : NWDDatabaseWebBasicModel
{
   [NWDWebPropertyDescription("ExampleText", NWDWebEditionStyle.Text, true, "", "", "")]
   public string ExampleText { set; get; } = string.Empty;
   [NWDWebPropertyDescription("ExampleTextOptional", NWDWebEditionStyle.Text, false, "", "", "")]
   public string ExampleTextOptional { set; get; } = string.Empty;
   [NWDWebPropertyDescription("ExampleUnixText", NWDWebEditionStyle.UnixText, false, "", "", "")]
   public string ExampleUnixText { set; get; } = string.Empty;
   [NWDWebPropertyDescription("ExampleAsciiText", NWDWebEditionStyle.AsciiText, false, "", "", "")]
   public string ExampleAsciiText { set; get; } = string.Empty;
}