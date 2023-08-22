using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDTreat.Exchanges.Payloads
{
    public class NWDUpPayloadTreatGetSubServicesFromAccount : NWDUpPayloadTreat
    {
        public NWDAccount Account { get; set; }
        public NWDEnvironmentKind Environment { get; set; }

        public NWDUpPayloadTreatGetSubServicesFromAccount(NWDAccount sAccount, NWDEnvironmentKind sEnvironment)
        {
            Account = sAccount;
            Environment = sEnvironment;
        }
    }
}
