using Microsoft.AspNetCore.Http;
using NWDFoundation.Exchanges.Payloads;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDRuntime.Exchanges;
using Newtonsoft.Json;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models.Enums;
using NWDWebRuntime.Tools.Sessions;

namespace NWDWebRuntime.Managers
{
    public class NWDAccountServiceWebManager
    {
        public static void FlushAllService(HttpContext? sHttpContext)
        {
            NWDWebRuntimeStartupService.SessionServices.SetValue(sHttpContext, string.Empty);
        }

        public static void RegisterService(NWDResponseRuntime sResponse, HttpContext? sHttpContext)
        {
            if (sResponse.Status == NWDRequestStatus.Ok)
            {
                NWDDownPayload tDownPayload = sResponse.GetPayload<NWDDownPayload>(NWDWebRuntimeConfiguration.KConfig);
                if (tDownPayload != null)
                {
                    List<NWDAccountService> tServices = tDownPayload.AccountServiceList;
                    foreach (NWDAccountService tService in tServices)
                    {
                    }
                    // TODO Add service for this account from configuration 
                    {
                        // TODO remove this fake service
                        // NWDAccountService tService = new NWDAccountService(sResponse.PlayerToken.ProjectId, sResponse.PlayerToken.EnvironmentKind, sResponse.PlayerToken.PlayerReference, 698745123, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddYears(10));
                        // tServices.Add(tService);
                        
                        if (NWDWebRuntimeConfiguration.KConfig.AdminAccountsId.Contains(sResponse.PlayerToken.PlayerReference))
                        {
                            /*NWDAccountService tServiceAdmin = new NWDAccountService(NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), sResponse.PlayerToken.PlayerReference, NWDWebRuntimeConfiguration.KConfig.AdminService, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddYears(10));*/
                            NWDAccountService tServiceAdmin = new NWDAccountService(NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(), sResponse.PlayerToken.PlayerReference, NWDGenericServiceEnum.Admin.Value, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddYears(10));

                            tServiceAdmin.Name = "Administrator";
                            tServices.Add(tServiceAdmin);
                        }
                    }
                    string tServicesString = JsonConvert.SerializeObject(tServices);
                    NWDWebRuntimeStartupService.SessionServices.SetValue(sHttpContext, tServicesString);
                }
            }
            else
            {
                //TODO : delete services for this session ?
                NWDWebRuntimeStartupService.SessionServices.SetValue(sHttpContext, string.Empty);
            }
        }

        public static List<NWDAccountService>? GetServices(HttpContext? sHttpContext)
        {
            List<NWDAccountService>? rReturn = new List<NWDAccountService>();
            string tServicesString = NWDWebRuntimeStartupService.SessionServices.GetValue(sHttpContext);
            if (string.IsNullOrEmpty(tServicesString) == false)
            {
                rReturn = JsonConvert.DeserializeObject<List<NWDAccountService>>(tServicesString);
            }
            return rReturn;
        }

        public static string GetServiceName(NWDAccountService sAccountService)
        {
            //TODO: faire e sort que l'on obtienne le nom du service via la memoire ou via une requête en ligne ...
            if (string.IsNullOrEmpty(sAccountService.Name))
            {
                return sAccountService.Service.ToString();
                
            }
            else
            {
                return sAccountService.Name;
            }
        }

        public static string GetEndDateString(NWDAccountService sAccountService)
        {
           DateTime tReturn = NWDTimestamp.TimestampUnixToDatetime(sAccountService.End);
           return tReturn.ToLongDateString();// + tReturn.ToLongTimeString();
        }
        public static string GetStartDateString(NWDAccountService sAccountService)
        {
            DateTime tReturn = NWDTimestamp.TimestampUnixToDatetime(sAccountService.Start);
            return tReturn.ToLongDateString();//+ tReturn.ToLongTimeString();
        }
    }
}