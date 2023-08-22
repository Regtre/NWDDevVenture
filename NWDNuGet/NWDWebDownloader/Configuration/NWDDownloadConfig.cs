using Newtonsoft.Json;
using NWDFoundation.Tools;
using NWDWebRuntime.Models.Enums;

namespace NWDWebDownloader.Configuration;

public class NWDDownloadConfig
{
    public string PageName { set; get; } = "Test Application";
    public string RootPath { set; get; } = string.Empty;
    public string HeaderFileName { set; get; } = "header.html";
    public string FooterFileName { set; get; } = "footer.html";
    public string? DescriptionFileName { set; get; } = "description.html";
    public int BuildListSize { set; get; }
    public string LayoutPath { set; get; } = string.Empty;
    public List<string> MimeTypeAuthorized { set; get; } = new List<string>();
    [JsonProperty]
    private long ServiceId { set; get; }

    [JsonIgnore]
    public NWDGenericServiceEnum Service
    {
        get
        {
            return NWDGenericServiceEnum.GetForValue(ServiceId);
        }
    }

    public bool AllowAnonymous { set; get; }
    public bool AllowConnected { set; get; }
}