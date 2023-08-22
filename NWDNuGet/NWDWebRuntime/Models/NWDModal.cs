using NWDFoundation.Models;

namespace NWDWebRuntime.Models;

public class NWDModal : NWDDatabaseWebBasicModel
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string ButtonFooterPositiveLabel { get; set; } = string.Empty;
    public string ButtonFooterPositiveURL { get; set; } = string.Empty;
    public string ButtonFooterNegativeLabel { get; set; } = string.Empty;
    public string ButtonFooterNegativeURL { get; set; } = string.Empty;
}