using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Hosting;
using NWDFoundation.Exchanges;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Services;
using NWDWebRuntime.Tools;
using NWDWebRuntime.Tools.Cookies;
using NWDWebRuntime.Tools.Sessions;

namespace NWDWebRuntime
{
    public class NWDWebRuntimeStartupService : IHostedService
    {
        private IServiceProvider Services;
        
        // Cookies
        public static NWDCookieString SessionCookie = new NWDCookieString(NWDWebRuntimeConfiguration.KConfig.SessionCookieName, "Session", "Session usage", NWDCookieDefinitionGroup.Functional, string.Empty, false, false);
        public static NWDCookieULong CookieAccount = new NWDCookieULong("Account", "Account", "Account reference", NWDCookieDefinitionGroup.Functional, 0, false, false);
        public static NWDCookieUShort CookieAccountRange = new NWDCookieUShort("AccountRange", "AccountRange", "Account range", NWDCookieDefinitionGroup.Functional, 0, false, false);
        public static NWDCookieString CookieAccountToken = new NWDCookieString("AccountToken", "Account Token", "Account Token", NWDCookieDefinitionGroup.Functional, string.Empty, false, false);
        public static NWDCookieBool CookieAccountRemember = new NWDCookieBool("AccountRemember", "Account Remember", "Account remember", NWDCookieDefinitionGroup.Functional, false, false, false);
        public static NWDCookieString CookieLanguage = new NWDCookieString(CookieRequestCultureProvider.DefaultCookieName, "Language", "Language", NWDCookieDefinitionGroup.Functional, "en_EN", false, false);
        public static NWDCookieBool CookieConsent = new NWDCookieBool("Consent", "", "", NWDCookieDefinitionGroup.Consent, false);
        public static NWDCookieBool CookiePub = new NWDCookieBool("Pub", "", "", NWDCookieDefinitionGroup.Advertisement, true);
        public static NWDCookieBool CookieAnalytics = new NWDCookieBool("Analytics", "", "", NWDCookieDefinitionGroup.Analytics, true);
        #if DEBUG
            public static NWDCookieBool CookieDebugInformation = new NWDCookieBool("DebugInformation", "Debug Information", "Show or not Debug Information dropdown", NWDCookieDefinitionGroup.Optional, false,true, true);
        #else
            public static NWDCookieBool CookieDebugInformation = new NWDCookieBool("DebugInformation", "Debug Information", "Show or not Debug Information dropdown", NWDCookieDefinitionGroup.Optional, false, true, true);
        #endif
        // Session
        public static NWDSessionULong SessionAccount = new NWDSessionULong("Account", "Account", "Account", NWDSessionDefinitionGroup.Functional, 0);
        
        public static NWDSessionSerializable<NWDSyncInformation> SessionPlayerSync = new NWDSessionSerializable<NWDSyncInformation> ("PlayerSync", "Playe Sync", "R-Sync Player Data", NWDSessionDefinitionGroup.Functional, new NWDSyncInformation());
        public static NWDSessionSerializable<NWDSyncInformation> SessionStudioSync = new NWDSessionSerializable<NWDSyncInformation> ("StudioSync", "Studio Sync", "R-Sync Studio Data", NWDSessionDefinitionGroup.Functional, new NWDSyncInformation());
        
        public static NWDSessionUShort SessionAccountRange = new NWDSessionUShort("AccountRange", "AccountRange", "AccountRange", NWDSessionDefinitionGroup.Functional, 0);
        public static NWDSessionString SessionAccountToken = new NWDSessionString("AccountToken", "Account Token", "Account token", NWDSessionDefinitionGroup.Functional, string.Empty);
        public static NWDSessionBool SessionFromCookie = new NWDSessionBool("AccountFromCookie", "Account from cookies", "Account from cookies", NWDSessionDefinitionGroup.Functional, false);
        public static NWDSessionString SessionServices = new NWDSessionString("Account services", "Account services", "Account services", NWDSessionDefinitionGroup.Functional, "");

        public static List<NWDNotification> Notifications = new List<NWDNotification>(); 

        public NWDWebRuntimeStartupService(IServiceProvider sServices)
        {
            Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            // test server connexion 
            NWDWebRuntimeRecursiveService.StartTimer();
            NWDToolBox.CountryList();
            // just waiting more time ... 
            await Task.Delay(25);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(25);
        }
        
    }
}
