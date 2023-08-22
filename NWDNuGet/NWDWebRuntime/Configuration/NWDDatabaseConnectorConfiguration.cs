using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;

namespace NWDWebRuntime.Configuration
{
    [Serializable]
    public class NWDDatabaseConnectorConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDDatabaseConnectorConfiguration KConfig = new NWDDatabaseConnectorConfiguration();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instance properties

        public bool SetUpPage { get; set; }
        public bool IsActive = false;
        public NWDDatabaseCredentials Credentials { set; get; }

        #endregion


        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDDatabaseConnectorConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDDatabaseConnectorConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDDatabaseConnectorConfiguration? tConfig = sConfig.GetSection(nameof(NWDDatabaseConnectorConfiguration)).Get<NWDDatabaseConnectorConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                KConfig.IsActive = true;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDDatabaseConnectorConfiguration)));
                //NWDLogger.Trace(nameof(NWDDatabaseConnectorConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDDatabaseConnectorConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDDatabaseConnectorConfiguration)), NWDLogger.SplitObjectSerializable(new NWDDatabaseConnectorConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDConfigurationInstalled.AddConfiguration(KConfig);
        }

        public NWDDatabaseConnectorConfiguration()
        {
            Credentials = new NWDDatabaseCredentials()
            {
                TablePrefix = string.Empty,
                Server = "localhost",
                Kind = NWDDatabaseKind.MariaDb,
                User = "DevUser",
                Database = "LocalTestDataBase",
                Port = 3306,
                Password = NWDRandom.RandomStringNoMistake(8),
                Secure = NWDDatabaseConnectionSsl.None
            };
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