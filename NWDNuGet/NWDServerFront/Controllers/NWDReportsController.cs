using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Configuration;
using NWDServerMiddle.Managers.ModelManagers;
using NWDServerMiddle.Models;
using NWDServerShared.Configuration;

namespace NWDServerFront.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NWDReportController : ControllerBase
    {
        [HttpPost(Name = "PostReport")]
        public NWDReportResponse Post([FromBody] NWDReportRequest sRequest)
        {
            NWDReportResponse rResponse = new NWDReportResponse
            {
                Foundation = NWDLibrariesInstalled.GetFileVersionInfo(typeof(NWDFoundation.Models.NWDAccount)).Information.FileVersion,
                Server = NWDLibrariesInstalled.GetFileVersionInfo(typeof(NWDAccountInformation)).Information.FileVersion
            };
            // test the configuration
            if (sRequest.Key != NWDServerConfiguration.KConfig?.MonitoringSecretKey)
            {
                rResponse.Request = NWDReportStatus.Ko;
            }
            else
            {
                
            }

            return rResponse;
        }
    }
}