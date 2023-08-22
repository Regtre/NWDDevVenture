using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDCrucial.Exchanges.Payloads;

public class NWDUpPayloadGetSubServicesFromAccount : NWDUpPayloadCrucial
{
    public NWDAccount Account { get; set; } = new NWDAccount();
    public NWDEnvironmentKind Environment { get; set; }
}