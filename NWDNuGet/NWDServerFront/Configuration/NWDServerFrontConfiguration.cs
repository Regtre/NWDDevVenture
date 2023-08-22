using System;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDServerMiddle.Configuration;
using NWDServerMiddle.Managers;
using NWDServerMiddle.Managers.ModelManagers;
using NWDServerShared.Configuration;

namespace NWDServerFront.Configuration
{
    public class NWDServerFrontConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDServerFrontConfiguration KConfig { get; set; } = new NWDServerFrontConfiguration();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instance properties
        public bool SetUpPage { get; set; }

        #endregion

        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDServerFrontConfiguration)));
            }
            else
            {
                // load config before
                NWDConfigurationDatabase.KConfig.LoadConfiguration(sBuilder.Configuration);
                NWDServerConfiguration.LoadFromBuilder(sBuilder, sRuntimeCompileForDev);
                NWDServerMiddleConfiguration.LoadFromBuilder(sBuilder, sRuntimeCompileForDev);
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.AddSingleton<NWDRuntimeManager>();
                sBuilder.Services.AddSingleton<NWDCrucialManager>();
                sBuilder.Services.AddSingleton<NWDPlayerStatisticManager>();
            }
        }
        public static void UseApp(WebApplication sApp)
        {
            sApp.UseExceptionHandler(sErrorApp =>
            {
                sErrorApp.Run(async sContext =>
                {
                    var tExceptionHandlerPathFeature = sContext.Features.Get<IExceptionHandlerPathFeature>();
                    if (tExceptionHandlerPathFeature?.Error != null)
                    {
                        NWDServerHookSlack tWebhook = new NWDServerHookSlack();
                        await tWebhook.SendAsyncNotificationException(tExceptionHandlerPathFeature.Error, sContext);
                    }
                });
            });
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfiguration)
        {
            NWDServerFrontConfiguration tConfig = sConfiguration.GetSection(nameof(NWDServerFrontConfiguration)).Get<NWDServerFrontConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDServerFrontConfiguration)));
                //NWDLogger.Trace(nameof(NWDServerFrontConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDServerFrontConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDServerFrontConfiguration)), NWDLogger.SplitObjectSerializable(new NWDServerFrontConfiguration()));
            }
            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), false);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            
            NWDCrucialInformationManager.Prepare(); // MUST BE THE FIRST

            NWDProjectCredentialManager.Prepare();
            NWDProjectServiceKeyManager.Prepare();
            NWDAccountManager.Prepare();
            NWDAccountSignManager.Prepare();
            NWDAccountTokenManager.Prepare();
            NWDAccountServiceManager.Prepare();
            NWDAccountOrderManager.Prepare();
            NWDAccountInvoiceManager.Prepare();
            NWDPlayerDataManager.Prepare();
            NWDStudioDataManager.Prepare();
            NWDRelationshipManager.Prepare();
        }


        public bool IsLoaded()
        {
            return Loaded;
        }
        
        public void RandomFake()
        {
        }

        #endregion
    }
}