using System.Collections.Generic;
using NWDFoundation.Models;

namespace NWDCrucial.Exchanges.Payloads;

public class NWDDownPayloadGetSubServicesFromAccount : NWDDownPayloadCrucial
{
    public List<NWDAccountService> SubServices { get; set; } = new List<NWDAccountService>();
}