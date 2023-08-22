namespace NWDWebDownloader.Models;

public class NWDBuild
{
    public string FolderName { set; get; } = string.Empty;
    public string Name { set; get; } = string.Empty;
    public string Path { set; get; } = string.Empty;
    public DateTime Date { set; get; } = DateTime.Now;
    public string Size { set; get; } = string.Empty;
    public string Extension { set; get; }= string.Empty;
    
    public NWDBuild(){}
    public NWDBuild(string sFolderName, string sName, string sPath, DateTime sDate, string sSize,string sExtension)
    {
        FolderName = sFolderName;
        Name = sName;
        Path = sPath;
        Date = sDate;
        Size = sSize;
        Extension = sExtension;
    }

    public override bool Equals(object? obj)
    {
        return obj is NWDBuild build &&
               FolderName == build.FolderName &&
               Name == build.Name &&
               Path == build.Path;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(FolderName, Name, Path);
    }
}