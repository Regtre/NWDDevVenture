using NWDFoundation.Tools;

namespace NWDWebGitLabReport.Models
{
    [Serializable]
    public class NWDProjectGitConnection
    {
        public string Description { set; get; } = string.Empty;
        public string LocalTokenReport { set; get; } = NWDRandom.RandomStringToken(24);
        public string Name { set; get; } = string.Empty;
        public string GitUrl { set; get; } = "https://gitlab.xxxxxx.com";
        public string GitBadge { set; get; } = "https://gitlab.xxxxxx.com";
        public string GitProject { set; get; } = "x_Group/x_project"; 
        
        public bool GitPublic { set; get; } = false;
        public string GitToken { set; get; } = "create token in your GitLab project settings, Private Token";
        public bool ShowProject { set; get; } = true;
        public bool ShowInformation { set; get; } = true;
        public bool ShowBurnDownChart { set; get; } = true;
        public bool ShowBurnUpChart { set; get; } = true;
        public bool ShowTaskChart { set; get; } = true;
        public bool ShowTaskStateChart { set; get; } = true;
        public bool ShowTask { set; get; } = true;
    }
}
