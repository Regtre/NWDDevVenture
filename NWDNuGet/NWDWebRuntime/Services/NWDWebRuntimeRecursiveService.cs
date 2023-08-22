using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models.Enums;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDWebRuntime.CallBacks;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;

namespace NWDWebRuntime.Services
{
    /// <summary>
    /// use to clean account with RGPD rules ... afeter 3 years without billing : bye bye
    /// </summary>
    public static class NWDWebRuntimeRecursiveService
    {
        private static Timer? _TestTimer;
        private static int _TestCounter;
#if DEBUG
        private const int K_TestTimeSpan = 30;
#else
         private static int K_TestTimeSpan = 3600;
#endif
        //private static bool _ActivePurge;

        public static void StartTimer()
        {
            TestConfiguration();
            TimeSpan tStartTimeSpan = TimeSpan.Zero;
            TimeSpan tPeriodTimeSpan = TimeSpan.FromSeconds(K_TestTimeSpan);
            _TestTimer = new Timer((sE) => { RecursiveService(); }, null, tStartTimeSpan, tPeriodTimeSpan);
        }

        //static public string GetTestCounter()
        //{
        //    return TestCounter.ToString("0000000");
        //}

        private static void RecursiveService()
        {
            //NWDLogger.WriteLine("TicTac ... ... ... " + (TestCounter * TestTimeSpan) + " seconds passed, this method is recall! ( NWDWebRuntimeStartupService.RecursiveService(); )");

            // if (ActivePurge == true)
            // {
            //     //NWDLogger.WriteLine("Deleta old account => find very old service and delete account ( more than" + AccountModel.AccountRetention + " years)");
            //     foreach (string tAccount in AccountService.GetOldServicesAccount())
            //     {
            //         AccountModel.Delete(tAccount);
            //         //NWDLogger.WriteLine(" Account " + tAccount + " was deleted");
            //     }
            //     // BillingModel.DeleteOld();
            //     // IndentModel.DeleteOld();
            // }
            _TestCounter++;
            TestConfiguration();
            
            // Test licence 
            NWDUpPayloadLicense tUpPayloadLicense = new NWDUpPayloadLicense();
            tUpPayloadLicense.HttpsDns = NWDWebRuntimeConfiguration.KConfig.GetDnsHttps();
            NWDRequestLicense tRequestLicense = new NWDRequestLicense(NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.GetProjectEnvironment(),NWDExchangeLicenseKind.CheckLicense, tUpPayloadLicense, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseLicense? tResponseLicense = NWDProjectLicenseCallbackServers.PostRequest(tRequestLicense).Result;
            NWDDownPayloadLicense? tResult = null;
            if (tResponseLicense!=null && tResponseLicense.Status == NWDRequestStatus.Ok)
            {
                tResult = tResponseLicense.GetPayload<NWDDownPayloadLicense>(NWDWebRuntimeConfiguration.KConfig);
            }
            if (tResult != null)
            {
                NWDWebRuntimeConfiguration.KConfig.LicenseValid = tResult.LicenseValid;
                NWDWebRuntimeConfiguration.KConfig.NewVersion = tResult.Version;
                if (tResult.Version != NWDVersionDll.Version)
                {
                    NWDWebRuntimeConfiguration.KConfig.NeedUpdate = tResult.NeedUpdate;
                }
                else
                {
                    NWDWebRuntimeConfiguration.KConfig.NeedUpdate = NWDNeedUpdate.Update;
                }
            }
            else
            {
                NWDWebRuntimeConfiguration.KConfig.LicenseValid = NWDLicenseStatus.Unknow;
                NWDWebRuntimeConfiguration.KConfig.NewVersion = "-";
                NWDWebRuntimeConfiguration.KConfig.NeedUpdate = NWDNeedUpdate.Unknow;
            }
            
            
        }

        public static void TestConfiguration()
        {
            // TODO CA DECONNE?  CAR SI LE PLAYER TOKEN EST VIDE CA VA DECONNECTER LE PLAYER EN COURS! .... LA MERDE? 
            // TODO : NON CAR LE PLAYER TOKEN N'A PAS DE SESSION DONC CA DETRUIT PAS LE PLAYER TOKEN EN COURS ...  
            NWDWebRuntimeConfiguration.KConfig.ConfigurationStatus.TryAdd(nameof(NWDWebRuntimeRecursiveService), NWDRequestStatus.Unknown);
            NWDResponseRuntime? tResponse = NWDWebRuntimeCallbackServers.PostRequest(NWDRequestRuntime.CreateRequestTest(NWDWebRuntimeConfiguration.KConfig, NWDWebRuntimeCallbackServers.GetRequestPlayerToken(null), NWDExchangeOrigin.Web, NWDExchangeDevice.Web), null).Result;
            NWDWebRuntimeConfiguration.KConfig.ConfigurationStatus[nameof(NWDWebRuntimeRecursiveService)] = tResponse.Status;
            NWDLogger.TraceSuccess(nameof(NWDWebRuntimeRecursiveService) + " configuration test return " + tResponse.Status);
        }
    }
}