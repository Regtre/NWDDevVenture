namespace NWDWebGitLabReport.Models
{
    [Serializable]
    public class NWDGitLabCommitModel
    {
        public string SecretToken { set; get; } = string.Empty;
        public string Hash { set; get; }= string.Empty;
        public string Label { set; get; }= string.Empty;
    }
}
