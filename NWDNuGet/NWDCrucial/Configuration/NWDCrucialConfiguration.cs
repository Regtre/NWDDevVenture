using System;
using Microsoft.Extensions.Configuration;
using NWDFoundation.Tools;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades;
using NWDFoundation.Logger;

namespace NWDCrucial.Configuration
{
    [Serializable]
    public class NWDCrucialConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDCrucialConfiguration KConfig = new NWDCrucialConfiguration();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instances properties

        public bool SetUpPage { set; get; } = true;
        public ulong PrivateProjectId { set; get; } = NWDConstants.K_FAKE_PROJECT_ID;
        public NWDEnvironmentKind PrivateEnvironment { set; get; } = NWDConstants.K_FAKE_PROJECT_ENVIRONMENT;
        public string PrivateCrucialKey { set; get; } = NWDConstants.K_FAKE_CRUCIAL_KEY;
        public string PrivateProjectKey { set; get; } = NWDConstants.K_FAKE_PROJECT_KEY;
        public string PrivateSecretKey { set; get; } = NWDConstants.K_FAKE_SECRET_KEY;

        #endregion

        #region static methods

        public static void LoadFromConfigurationManager(ConfigurationManager sConfiguration, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDCrucialConfiguration)));
            }
            else
            {
                try
                {
                    sConfiguration.AddJsonFile(nameof(NWDCrucialConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }

                KConfig.LoadConfig(sConfiguration);
            }
        }

        #endregion

        #region instance methods

        public void RandomFake()
        {
            PrivateProjectId = NWDConstants.K_RANDOM_PROJECT_ID;
            PrivateEnvironment = NWDConstants.K_RANDOM_PROJECT_ENVIRONMENT;
            PrivateCrucialKey = NWDConstants.K_RANDOM_CRUCIAL_KEY;
            PrivateProjectKey = NWDConstants.K_RANDOM_PROJECT_KEY;
            PrivateSecretKey = NWDConstants.K_RANDOM_SECRET_KEY;
        }

        /// <summary>
        /// Load the value from a file
        /// </summary>
        /// <param name="sConfiguration"></param>
        public void LoadConfig(IConfiguration sConfiguration)
        {
            NWDCrucialConfiguration? tConfig = sConfiguration.GetSection(nameof(NWDCrucialConfiguration)).Get<NWDCrucialConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDCrucialConfiguration)));
                //NWDLogger.Trace(nameof(NWDCrucialConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDCrucialConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDCrucialConfiguration)), NWDLogger.SplitObjectSerializable(new NWDCrucialConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(GetType());
            NWDConfigurationInstalled.AddConfiguration(KConfig);
        }

        public bool IsLoaded()
        {
            return Loaded;
        }

        #endregion
    }
}