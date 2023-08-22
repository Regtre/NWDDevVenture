using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDServerHumanInterface.Services;

namespace NWDServerHumanInterface.Configuration
{
    [Serializable]
    public class NWDServerHumanInterfaceConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDServerHumanInterfaceConfiguration KConfig = new NWDServerHumanInterfaceConfiguration();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instance properties

        public bool SetUpPage { set; get; } = true;

        #endregion

        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDServerHumanInterfaceConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDServerHumanInterfaceConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDServerHumanInterfaceConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDServerHumanInterfaceStartupService>();
            }
        }
        #endregion


        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDServerHumanInterfaceConfiguration? tConfig = sConfig.GetSection(nameof(NWDServerHumanInterfaceConfiguration)).Get<NWDServerHumanInterfaceConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess("" + nameof(NWDServerHumanInterfaceConfiguration) + " found in appsettings.json");
                //NWDLogger.TraceSuccess("Value in your appsettings.json file : \n \"" + nameof(NWDServerHumanInterfaceConfiguration) + "\":" + JsonSerializer.Serialize(tConfig).Replace("}", "}\n") + ",");
            }
            else
            {
                tConfig = new NWDServerHumanInterfaceConfiguration();
                NWDLogger.TraceFailed("⚠️ " + nameof(NWDServerHumanInterfaceConfiguration) + " not found in appsettings.json");
                NWDLogger.TraceFailed("⚠️ Insert this line in appsettings.json : \n \"" + nameof(NWDServerHumanInterfaceConfiguration) + "\":" + JsonSerializer.Serialize(tConfig).Replace("}", "}\n") + ",");
            }

            PrepareAfterConfiguration();
        }
        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(KConfig.GetType(), false);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
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