using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Exchanges;
using NWDFoundation.Models.Enums;
using NWDHub.Configuration;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;

namespace NWDHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NWDProjectLicenseController : ControllerBase
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
                    if (tRequest.HttpsDns == "https://www.net-worked-data.com" || tRequest.HttpsDns == "https://localhost:2051")
                    {
                        tLicense.Success = true;
                        tLicense.LicenseValid = NWDLicenseStatus.Valid;
                        tLicense.NeedUpdate = NWDNeedUpdate.Update;
                    }
                    else if (tRequest.HttpsDns == "https://community.ooabab.com")
                    {
                        tLicense.Success = true;
                        tLicense.LicenseValid = NWDLicenseStatus.Valid;
                        tLicense.NeedUpdate = NWDNeedUpdate.Update;
                    }
                    else if (tRequest.HttpsDns == "https://nwd.ooabab.com")
                    {
                        tLicense.Success = true;
                        tLicense.LicenseValid = NWDLicenseStatus.Invalid;
                        tLicense.NeedUpdate = NWDNeedUpdate.Update;
                    }
                    else
                    {
                        tLicense.Success = true;
                        tLicense.LicenseValid = NWDLicenseStatus.Invalid;
                        tLicense.NeedUpdate = NWDNeedUpdate.Update;
                    }

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