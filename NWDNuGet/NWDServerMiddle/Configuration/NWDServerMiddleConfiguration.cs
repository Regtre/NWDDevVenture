using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NWDCrucial.Configuration;
using NWDCrucial.Models;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDServerMiddle.Managers.ModelManagers;

namespace NWDServerMiddle.Configuration
{
    [Serializable]
    public class NWDServerMiddleConfiguration : INWDConfiguration, INWDCrucialKey, INWDProjectKey, INWDSecretKey
    {
        #region static properties

        public static NWDServerMiddleConfiguration KConfig = new NWDServerMiddleConfiguration();
        private static bool Loaded { set; get; }
        public static readonly HttpClient HttpClientShared = new HttpClient();

        #endregion

        #region instance properties

        public bool SetUpPage { get; set; }
        public string SlackWebHook { set; get; } = string.Empty;
        
        #region NWDRelationship
        public int RelationshipCodeLength { set; get; } = 5;
        public int RelationshipCodeValidationLengthInSeconds { set; get; } = 300;
        public int GraceTimeInSeconds { set; get; } = 60;

        #endregion

        #endregion

        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDServerMiddleConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDServerMiddleConfiguration) + ".json", true, true);
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
            NWDServerMiddleConfiguration? tConfig = sConfig.GetSection(nameof(NWDServerMiddleConfiguration)).Get<NWDServerMiddleConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDServerMiddleConfiguration)));
                //NWDLogger.Trace(nameof(NWDServerHookSlackConfiguration),  NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDServerMiddleConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDServerMiddleConfiguration)), NWDLogger.SplitObjectSerializable(new NWDServerMiddleConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(this.GetType());
            NWDLibrariesInstalled.AddAssemblyByType(typeof(NWDServerBack.NWDVersionDll), false);
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

        #region interfaces

        public string GetCrucialInstanceName()
        {
            return nameof(NWDServerMiddleConfiguration);
        }

        public NWDEnvironmentKind GetCrucialEnvironment()
        {
            return NWDCrucialConfiguration.KConfig.PrivateEnvironment;
        }

        public string GetCrucialKey()
        {
            return NWDCrucialConfiguration.KConfig.PrivateCrucialKey;
        }

        public ulong GetCrucialProjectId()
        {
            return NWDCrucialConfiguration.KConfig.PrivateProjectId;
        }

        public string GetCrucialProjectKey()
        {
            return NWDCrucialConfiguration.KConfig.PrivateProjectKey;
        }

        public string GetCrucialSecretKey()
        {
            return NWDCrucialConfiguration.KConfig.PrivateSecretKey;
        }

        public string GetSecretKeyInstanceName()
        {
            return nameof(NWDServerMiddleConfiguration);
        }

        public string GetProjectKeyInstanceName()
        {
            return nameof(NWDServerMiddleConfiguration);
        }

        public string GetProjectKey(ulong sProjectId, NWDEnvironmentKind sEnvironment)
        {
            if (sProjectId == GetCrucialProjectId())
            {
                return GetCrucialProjectKey();
            }
            else
            {
                NWDProjectCredentials? tProjectCredentials = NWDProjectCredentialManager.GetOneByProjectIdAndEnvironment(sProjectId, sEnvironment);
                if (tProjectCredentials != null)
                {
                    return tProjectCredentials.ProjectKey;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string GetSecretKey(ulong sProjectId, NWDEnvironmentKind sEnvironment)
        {
            if (sProjectId == GetCrucialProjectId())
            {
                return GetCrucialSecretKey();
            }
            else
            {
                NWDProjectCredentials? tProjectCredentials = NWDProjectCredentialManager.GetOneByProjectIdAndEnvironment(sProjectId, sEnvironment);
                if (tProjectCredentials != null)
                {
                    return tProjectCredentials.SecretKey;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #endregion
    }
}