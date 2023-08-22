namespace NWDWebHttpErrorSimulator.Models
{
    [Serializable]
    public class NWDWebsiteMapLibrary
    {
        public string LibraryName { set; get; } = string.Empty;
        public List<NWDWebsiteMapClass> ClassList { set; get; } = new List<NWDWebsiteMapClass>();
    }
}