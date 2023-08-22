using NWDFoundation.Models;

namespace NWDHub.Models
{
    //TODO Remove? 
    [Serializable]
    public class NWDAccountProjects
    {
        public List<NWDProject> ProjectList { set; get; } = new List<NWDProject>();
    }
}