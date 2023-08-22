using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Configuration;
using NWDFoundation.Facades.Back;
using NWDServerMiddle.Managers.ModelManagers;
using NWDServerMiddle.Models;
using NWDServerShared.Configuration;

namespace NWDServerFront.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NWDMonitoringController : ControllerBase
    {
        [HttpPost(Name = "PostMonitoring")]
        public NWDMonitoringResponse Post([FromBody] NWDMonitoringRequest sRequest)
        {
            NWDMonitoringResponse rResponse = new NWDMonitoringResponse
            {
                Foundation = NWDLibrariesInstalled.GetFileVersionInfo(typeof(NWDFoundation.Models.NWDAccount)).Information.FileVersion,
                Server = NWDLibrariesInstalled.GetFileVersionInfo(typeof(NWDAccountInformation)).Information.FileVersion
            };
            // test the configuration
            if (sRequest.Key != NWDServerConfiguration.KConfig?.MonitoringSecretKey)
            {
                rResponse.Request = NWDMonitoringStatus.Ko;
            }
            else
            {
                rResponse.Request = NWDMonitoringStatus.Ok;
                // so ... it's running : 
                rResponse.Service = NWDMonitoringStatus.Ok;
                // test database connexion
                bool tAllDatabaseTest = true;
                // test databases for player
                List<INWDDao> tDaoList = new List<INWDDao>();
                tDaoList.AddRange(NWDAccountManager.DaoList);
                tDaoList.AddRange(NWDAccountServiceManager.DaoList);
                tDaoList.AddRange(NWDAccountSignManager.DaoList);
                tDaoList.AddRange(NWDAccountTokenManager.DaoList);
                tDaoList.AddRange(NWDPlayerDataManager.DaoList);
                tDaoList.AddRange(NWDStudioDataManager.DaoList);
                tDaoList.AddRange(NWDProjectServiceKeyManager.DaoList);
                tDaoList.AddRange(NWDProjectCredentialManager.DaoList);
                foreach (INWDDao tDao in tDaoList)
                {
                    if (tDao.TestConnexion() == false)
                    {
                        rResponse.Bug = rResponse.Bug + " Error in database " + tDao.GetInfos() + "! ";
                        tAllDatabaseTest = false;
                    }
                }
                // if only one database is KO ... all database test is false! see bug to find error! 
                if (tAllDatabaseTest == true)
                {
                    rResponse.Databases = NWDMonitoringStatus.Ok;
                }
                else
                {
                    rResponse.Databases = NWDMonitoringStatus.Ko;
                }
            }

            return rResponse;
        }
    }
}