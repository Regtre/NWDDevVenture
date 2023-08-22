using System.Collections.Generic;
using NWDFoundation.Models;

namespace NWDTreat.Exchanges.Payloads
{
    public class NWDDownPayloadTreatGetSubServicesFromAccount : NWDDownPayloadTreat
    {
        public List<NWDAccountService> AccountServiceList { get; set; } = new List<NWDAccountService>();
    }
}
