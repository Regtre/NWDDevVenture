using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Exchanges;
using NWDFoundation.Models.Enums;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;

namespace NWDIdemobi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NWDHubLicenseController : ControllerBase
    {
        [HttpPost()]
        public NWDResponseLicense Post([FromBody] NWDRequestLicense sRequestLicense)
        {
            if (sRequestLicense.IsValid(NWDHubConfiguration.KConfig))
            {
                NWDUpPayloadLicense? tRequest = sRequestLicense.GetPayload<NWDUpPayloadLicense>(NWDHubConfiguration.KConfig);
                if (tRequest != null)
                {
                    NWDDownPayloadLicense tLicense = new NWDDownPayloadLicense();
                    // Check validity
                    // TODO : Valid license for this instance
                    // TODO ; Gte Project, project is ok, project is publish ... 
                    
                        tLicense.Success = true;
                        tLicense.LicenseValid = NWDLicenseStatus.Valid;
                        tLicense.NeedUpdate = NWDNeedUpdate.Upgrade;

                    // add version to check update
                    tLicense.Version = NWDVersionDll.Version;
                    return new NWDResponseLicense(NWDHubConfiguration.KConfig,sRequestLicense.ProjectId, sRequestLicense.Environment,NWDExchangeLicenseKind.CheckLicense, tLicense, NWDRequestStatus.Ok);
                }
                else
                {
                    return new NWDResponseLicense(NWDHubConfiguration.KConfig,sRequestLicense.ProjectId, sRequestLicense.Environment,NWDExchangeLicenseKind.CheckLicense, new NWDDownPayloadLicense(), NWDRequestStatus.Error);
                }
            }
            else
            {
                return new NWDResponseLicense(NWDHubConfiguration.KConfig,sRequestLicense.ProjectId, sRequestLicense.Environment,NWDExchangeLicenseKind.CheckLicense, new NWDDownPayloadLicense(), NWDRequestStatus.Error);
            }
        }
    }
}