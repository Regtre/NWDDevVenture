using NWDFoundation.Models;

namespace NWDWebGitLabReport.Models;

public class NWDSprintInfo : NWDPlayerData
{
    public string MilestoneName { set; get; } = "";
    public double Capacity { set; get; }
    public double Velocity { set; get; }
}