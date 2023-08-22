namespace NWDWebHttpErrorSimulator.Models
{
    [Serializable]
    public class NWDWebsiteMapClass
    {
        public string ControllerName { set; get; } = string.Empty;
        public List<NWDWebsiteMapAction> ActionList { set; get; } = new List<NWDWebsiteMapAction>();
    }
}