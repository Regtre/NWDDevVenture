namespace NWDWebRuntime.Models
{
    [Serializable]
    public class NWDModelModalEdit
    {
        public string Title { set; get; } = string.Empty;
        public string Description { set; get; } = string.Empty;
        public Type ClassType { set; get; }
        //public string Classname { set; get; } = string.Empty;
        public ulong Reference { set; get; }
        
        public Dictionary<string,string>? DefaultValues  { set; get; }
        
        public string? UrlReload { set; get; }
    }
}