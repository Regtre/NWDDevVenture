using System;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NWDCrucial.Configuration;
using NWDCrucial.Models;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;

namespace NWDServerShared.Configuration
{
    [Serializable]
    public class NWDServerConfiguration : INWDConfiguration //, INWDCrucialKey
    {
        #region static properties

        public static NWDServerConfiguration KConfig = new NWDServerConfiguration();
        private static bool Loaded { set; get; } = false;

        [NonSerialized] public NWDServerStatus Status = NWDServerStatus.Inactive;
        [NonSerialized] public NWDServerStatus ClusterStatus = NWDServerStatus.Active;
        [NonSerialized] public float Overflow = 0.0F;

        public bool IsOverFlow()
        {
            //TODO analyse real charge of server
            return false;
        }
        #endregion

        #region instance properties

        public string Name { set; get; } = "NWD_SERVER_NAME";

        public string GetServerIdentity()
        {
            return Name + " (" + Ip + ")";
        }
            
        public string Ip { set; get; } = "NWD_SERVER_IP";

        public string CommitHash { set; get; } = "CI_COMMIT_SHA";

        public string Job { set; get; } = "CI_JOB_ID";

        public string PipelineDate { set; get; } = "CI_PIPELINE_DATE";
        
        public int RescueTokenDuration { set; get; } = 14400; //one day
        
        public float OverflowLimit { set; get; } = 0.8F;
        public bool SetUpPage { get; set; }
        public bool Debug { set; get; } = false;
        public string MonitoringSecretKey { set; get; } = NWDRandom.RandomNetWorkedDataToken();

        #endregion

        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDServerConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDServerConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                NWDCrucialConfiguration.LoadFromConfigurationManager(sBuilder.Configuration);
            }
        }
        
        #endregion

        #region instance methods
        /// <summary>
        /// Load the value from a section from  ConfigurationerverEmail.json
        /// </summary>
        /// <param name="sConfig"></param>
        public void LoadConfig(IConfigurationRoot sConfig)
        {
            NWDServerConfiguration? tConfig = sConfig.GetSection(nameof(NWDServerConfiguration)).Get<NWDServerConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDServerConfiguration)));
                //NWDLogger.Trace(nameof(NWDAdminConfiguration),  NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDServerConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDServerConfiguration)),  NWDLogger.SplitObjectSerializable(new NWDServerConfiguration()));
            }

            PrepareAfterConfiguration();
        }
        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), false);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            NWDConfigurationInstalled.AddConfiguration(NWDConfigurationDatabase.KConfig);
        }

        public bool IsLoaded()
        {
            return Loaded;
        }
        public void RandomFake()
        {}

        #endregion
        
        
        // #region interfaces
        // public string GetCrucialInstanceName()
        // {
        //     return nameof(NWDServerConfiguration);
        // }
        //
        // public string GetCrucialKey()
        // {
        //     return NWDCrucialConfiguration.KConfig.PrivateCrucialKey;
        // }
        // public ulong GetCrucialProjectId()
        // {
        //     return NWDCrucialConfiguration.KConfig.PrivateProjectId;
        // }
        // public string GetCrucialProjectKey()
        // {
        //     return NWDCrucialConfiguration.KConfig.PrivateProjectKey;
        // }
        // public string GetCrucialSecretKey()
        // {
        //     return NWDCrucialConfiguration.KConfig.PrivateSecretKey;
        // }
        // public NWDEnvironmentKind GetCrucialEnvironment()
        // {
        //     return NWDCrucialConfiguration.KConfig.PrivateEnvironment;
        // }
        //
        // #endregion
        
    }
}

