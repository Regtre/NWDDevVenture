namespace NWDWebHttpErrorSimulator.Models
{
    [Serializable]
    public class NWDWebsiteMapAction
    {
        public string ActionName { set; get; } = string.Empty;
        public List<NWDWebsiteMapRequest> RequestList { set; get; } = new List<NWDWebsiteMapRequest>();
    }
}