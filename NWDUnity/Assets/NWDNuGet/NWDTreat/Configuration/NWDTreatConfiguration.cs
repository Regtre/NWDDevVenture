using System;
using System.IO;
#if !UNITY_EDITOR
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
#endif
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;

namespace NWDTreat.Configuration
{
    [Serializable]
    public class NWDTreatConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDTreatConfiguration KConfig = new NWDTreatConfiguration();
        public static bool Loaded { set; get; } = false;

        #endregion

        #region instance properties
        
        public bool SetUpPage { get; set; }

        public bool IsDevelopment { set; get; } = false;

        #endregion

        #region static methods

#if !UNITY_EDITOR
        public static void LoadFromConfigurationManager(ConfigurationManager sConfiguration, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDTreatConfiguration)));
            }
            else
            {
                try
                {
                    sConfiguration.AddJsonFile(nameof(NWDTreatConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sConfiguration);
            }
        }
#endif

        #endregion

        #region instance methods

#if !UNITY_EDITOR
        public void LoadConfig(IConfiguration sConfig)
        {
            NWDTreatConfiguration? tConfig = sConfig.GetSection(nameof(NWDTreatConfiguration)).Get<NWDTreatConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDTreatConfiguration)));
                NWDLogger.Trace(nameof(NWDTreatConfiguration),  NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDTreatConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDTreatConfiguration)),  NWDLogger.SplitObjectSerializable(new NWDTreatConfiguration()));
            }

            PrepareAfterConfiguration();
        }
#endif

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(this.GetType());
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