using NWDFoundation.Configuration.Environments;

namespace NWDPlayerStatistic.Exchanges.Payloads;

public class NWDPlayerStatisticUpPayload
{
    public ulong ProjectId { get; set; }
    public NWDEnvironmentKind Environment{ get; set; }

    public NWDPlayerStatisticUpPayload(ulong sProjectId, NWDEnvironmentKind sEnvironment)
    {
        ProjectId = sProjectId;
        Environment = sEnvironment;
    }
    public NWDPlayerStatisticUpPayload() { }
}