using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Managers
{
    public static class NWDAuthorizeByOneOfService
    {
        public const string K_SERVICE_ONLY = "/Home/ServiceOnly/";
        public const string K_SERVICE_ONLY_WITHOUT_LAYOUT = "/Home/ServiceOnlyWithoutLayout/";
        public static bool ValidFor(HttpContext sContext, NWDGenericServiceEnum[] sServices )
        {
            List<long> tServices = new List<long>();
            foreach (NWDGenericServiceEnum tEnum in sServices)
            {
                tServices.Add(tEnum.Value);
            }
            return ValidFor(sContext, tServices);
        }
        
        public static bool ValidFor(HttpContext sContext, NWDGenericServiceEnum sService )
        {
            List<long> tServices = new List<long>();
            if (sService != null)
            {
                tServices.Add(sService.Value);
            }
            return ValidFor(sContext, tServices);
        }
        
        private static bool ValidFor(HttpContext sContext, List<long>? sRequiredServices)
        {
            bool rReturn = false;
            if (NWDAccountWebManager.AccountIsConnected(sContext) == true)
            {
                bool tServiceOk = false;
                if (sRequiredServices != null)
                {
                    int tTimeStamp = NWDTimestamp.GetTimestampUnix();
                    List<NWDAccountService>? tList = NWDAccountServiceWebManager.GetServices(sContext);
                    foreach (long tServiceReference in sRequiredServices)
                    {
                        bool tPartial = false;
                        if (tList != null)
                        {
                            foreach (NWDAccountService tService in tList)
                            {
                                // NWDLogger.WriteLine(" is " + tService.Service + " ==  " + tServiceReference + "?");
                                if (tServiceReference == tService.Service)
                                {
                                    if (tService.Status == NWDAccountServiceStatus.IsActive)
                                    {
                                        if (tTimeStamp < tService.End && tTimeStamp > tService.Start)
                                        {
                                            //NWDLogger.Information( tService.Service + " is valid for this user!");
                                            tPartial = true;
                                            break;
                                        }
                                        else
                                        {
                                            // NWDLogger.WriteLine("  " + tService.Service + " is invalid!   "+tTimeStamp+" < "+tService.End+" && "+tTimeStamp+" > "+tService.Start);
                                        }
                                    }
                                }
                            }
                        }

                        if (tPartial == true)
                        {
                            tServiceOk = true;
                            break;
                        }
                    }
                }
                if (tServiceOk == true)
                {
                    rReturn = true;
                }
            }
            else
            {
                rReturn = false;
            }
            return rReturn;
        }
    }
}