namespace NWDWebDownloader.Models;

public class DownloadPageResource
{
    public string Header { set; get; }= string.Empty;
    public string Footer { set; get; }= string.Empty;
    public Dictionary<string, string> DescriptionByCategory { set; get; } = new Dictionary<string, string>();
    
    public void Clear()
    {
        Header = string.Empty;
        Footer = string.Empty;
        DescriptionByCategory.Clear();
    }
}