using NWDCrucial.Configuration;
using NWDFoundation.Exchanges;
using NWDFoundation.Models.Enums;
using NWDHub.Configuration;
using NWDHub.Managers;
using NWDRuntime.Exchanges;
using NWDRuntime.Exchanges.Payloads;
using NWDWebRuntime.Configuration;

namespace NWDHub.Services
{
    public static class NWDHubRecursiveService
    {
        private static Timer? TestTimer;
#if DEBUG
        private const int _TIME_SPAN = 600;
#else
        private const int _TIME_SPAN = 3600;
#endif
        private static double Counter = 0;

        public static void StartTimer()
        {
            TimeSpan tStartTimeSpan = TimeSpan.Zero;
            TimeSpan tPeriodTimeSpan = TimeSpan.FromSeconds(_TIME_SPAN);
            TestTimer = new Timer((sE) => { RecursiveService(); }, null, tStartTimeSpan, tPeriodTimeSpan);
        }

        private static void RecursiveService()
        {
            // Create Admin account
            if (Counter < 2)
            {
                Counter++;
                //NWDHubConfiguration.KConfig.CreateDefaultProjectAndAccount(NWDCrucialConfiguration.KConfig.CrucialEnvironmentKind);
            }

            // Test update of nuget ...
            // TODO   Test update of nuget (with licence?)

            // Test licence 
            NWDUpPayloadLicense tUpPayloadLicense = new NWDUpPayloadLicense();
            tUpPayloadLicense.HttpsDns = NWDWebRuntimeConfiguration.KConfig.GetDnsHttps();
            NWDRequestLicense tRequestLicense = new NWDRequestLicense(NWDHubConfiguration.KConfig, NWDHubConfiguration.KConfig.GetCrucialProjectId(), NWDHubConfiguration.KConfig.GetCrucialEnvironment(),NWDExchangeLicenseKind.CheckLicense, tUpPayloadLicense, NWDExchangeOrigin.Web, NWDExchangeDevice.Web);
            NWDResponseLicense? tResponseLicense = NWDHubLicenseCallbackServers.PostRequest(tRequestLicense).Result;
            NWDDownPayloadLicense? tResult = null;
            if (tResponseLicense!=null && tResponseLicense.Status == NWDRequestStatus.Ok)
            {
                tResult = tResponseLicense.GetPayload<NWDDownPayloadLicense>(NWDHubConfiguration.KConfig);
            }
            if (tResult != null)
            {
                NWDHubConfiguration.KConfig.HubLicenseValid = tResult.LicenseValid;
                NWDHubConfiguration.KConfig.NewVersion = tResult.Version;
                if (tResult.Version != NWDVersionDll.Version)
                {
                    NWDHubConfiguration.KConfig.NeedUpdate = tResult.NeedUpdate;
                }
                else
                {
                    NWDHubConfiguration.KConfig.NeedUpdate = NWDNeedUpdate.Update;
                }
            }
            else
            {
                NWDHubConfiguration.KConfig.HubLicenseValid = NWDLicenseStatus.Invalid;
                NWDHubConfiguration.KConfig.NewVersion = "-";
                NWDHubConfiguration.KConfig.NeedUpdate = NWDNeedUpdate.Unknow;
            }
        }
    }
}