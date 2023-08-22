using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using NWDCrucial.Configuration;
using NWDEditor.Facades;
using NWDFoundation.Models.Enums;
using NWDCrucial.Exchanges.Payloads;
using NWDCrucial.Models;
using NWDFoundation.Configuration;
using NWDHub.Managers;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDHub.Services;
using NWDTreat.Configuration;
using NWDTreat.Facades;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebTreat.Configuration;

namespace NWDHub.Configuration
{
    [Serializable]
    public class NWDHubConfiguration : INWDConfiguration, INWDTreatKey, INWDCrucialKey, INWDProjectKey, INWDSecretKey
        
    {
        #region static properties

        public static NWDHubConfiguration KConfig = new NWDHubConfiguration();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instance properties

        public bool SetUpPage { set; get; } = true;
        [JsonIgnore]
        public NWDLicenseStatus HubLicenseValid = NWDLicenseStatus.Unknow;
        [JsonIgnore]
        public NWDNeedUpdate NeedUpdate = NWDNeedUpdate.Unknow;
        [JsonIgnore]
        public string NewVersion = string.Empty;
        
        #endregion

        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDHubConfiguration)));
            }
            else
            {
                NWDCrucialConfiguration.LoadFromConfigurationManager(sBuilder.Configuration);
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDHubConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }

                // load config 
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDHubConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDHubStartupService>();
                sBuilder.Services.AddSingleton<INWDEditorManager, NWDEditorManager>();
                sBuilder.Services.AddSingleton<INWDTreatManager, NWDTreatManager>();
                
                // add localizations
                // sBuilder.Services.AddLocalization(sOptions => sOptions.ResourcesPath = NWDConstants.K_RESOURCES);
                // sBuilder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization(sOo =>
                // {
                //     Type tType = typeof(SharedResource);
                //     string? tFullname = tType.GetTypeInfo().Assembly.FullName;
                //     if (string.IsNullOrEmpty(tFullname) == false)
                //     {
                //         AssemblyName? tAssemblyName = new AssemblyName(tFullname);
                //         if (tAssemblyName.Name != null)
                //         {
                //             var tFactory = sBuilder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                //             if (tFactory != null)
                //             {
                //                 var tLocalizer = tFactory.Create(nameof(SharedResource), tAssemblyName.Name);
                //                 sOo.DataAnnotationLocalizerProvider = (sType, sFactory) => tLocalizer;
                //             }
                //         }
                //     }
                // });
                
                // add razor
                if (sRuntimeCompileForDev)
                {
                    if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment)
                    {
                        var tMvcBuilder = sBuilder.Services.AddRazorPages();
                        tMvcBuilder.AddRazorRuntimeCompilation();
                        sBuilder.Services.Configure<MvcRazorRuntimeCompilationOptions>(sOptions =>
                        {
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDHub), nameof(NWDHubConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDHub)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDHub), nameof(NWDHubConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDHub)));
                }
            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDHubConfiguration? tConfig = sConfig.GetSection(nameof(NWDHubConfiguration)).Get<NWDHubConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDHubConfiguration)));
                //NWDLogger.Trace(nameof(NWDHubConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDHubConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDHubConfiguration)), NWDLogger.SplitObjectSerializable(new NWDHubConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            NWDWebDBDataManager.CreateAllTables();
            NWDWebRuntimeConfiguration.KConfig.NavBarInterfaceAdd(new NWDHubNavBar());
            NWDWebRuntimeConfiguration.KConfig.SideBarInterfaceAdd(new NWDHubSideBar());
            NWDWebRuntimeConfiguration.KConfig.NavFooterInterfaceAdd(new NWDHubNavFooter());
        }

        // public void CreateDefaultProjectAndAccount(NWDEnvironmentKind sEnvironmentKind)
        // {
        //     NWDUpPayloadProjectPublish tPublish = new NWDUpPayloadProjectPublish()
        //     {
        //         ProjectCredentialsList = new List<NWDProjectCredentials>()
        //         {
        //             new NWDProjectCredentials()
        //             {
        //                 ProjectId = NWDCrucialConfiguration.KConfig.CrucialProjectId,
        //                 EnvironmentKind = sEnvironmentKind,
        //                 TreatKey = NWDCrucialConfiguration.KConfig.CrucialTreatKey,
        //                 ProjectKey = NWDCrucialConfiguration.KConfig.CrucialProjectKey,
        //             },
        //         },
        //         ProjectServiceKeyList = new List<NWDProjectServiceKey>()
        //         {
        //             new NWDProjectServiceKey()
        //             {
        //                 ProjectId = NWDCrucialConfiguration.KConfig.CrucialProjectId,
        //                 EnvironmentKind = sEnvironmentKind,
        //                 Name = "Crucial Admin",
        //                 ServiceId = NWDCrucialConfiguration.KConfig.CrucialServiceId,
        //             }
        //         }
        //     };
        //     NWDResponseCrucial? tCrucialPublishResponse = NWDWebCrucialCallbackServers.PostRequest(new NWDRequestCrucial(NWDExchangeCrucialKind.ProjectPublish, tPublish, NWDExchangeOrigin.Web, NWDExchangeDevice.Web)).Result;
        // }
        public bool IsLoaded()
        {
            return Loaded;
        }
        
        public void RandomFake()
        {
        }
        
        public string GetTreatKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            if (sProjectId == NWDCrucialConfiguration.KConfig.PrivateProjectId && sEnvironmentKind == NWDCrucialConfiguration.KConfig.PrivateEnvironment )
            {
                return NWDWebTreatConfiguration.KConfig.MyTreatKey;
            }
            return NWDProjectTreatManager.GetByProjectUniqueId(sProjectId, sEnvironmentKind).TreatKey;
        }

        public string GetTreatInstanceName()
        {
            return nameof(NWDHubConfiguration);
        }
        
        public string GetSecretKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            if (sProjectId == NWDCrucialConfiguration.KConfig.PrivateProjectId && sEnvironmentKind == NWDCrucialConfiguration.KConfig.PrivateEnvironment )
            {
                return NWDCrucialConfiguration.KConfig.PrivateSecretKey;
            }
            return NWDProjectTreatManager.GetByProjectUniqueId(sProjectId, sEnvironmentKind).SecretKey;
        }
        public string GetProjectKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            if (sProjectId == NWDCrucialConfiguration.KConfig.PrivateProjectId && sEnvironmentKind == NWDCrucialConfiguration.KConfig.PrivateEnvironment )
            {
                return NWDCrucialConfiguration.KConfig.PrivateProjectKey;
            }
            return NWDProjectTreatManager.GetByProjectUniqueId(sProjectId, sEnvironmentKind).ProjectKey;
        }
        public string GetProjectKeyInstanceName()
        {
            return nameof(NWDHubConfiguration);
        }
        #endregion
        
        
        #region interfaces

        public string GetSecretKeyInstanceName()
        {
            return nameof(NWDHubConfiguration);
        }
        public string GetCrucialInstanceName()
        {
            return nameof(NWDHubConfiguration);
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
        public NWDEnvironmentKind GetCrucialEnvironment()
        {
            return NWDCrucialConfiguration.KConfig.PrivateEnvironment;
        }

        #endregion
    }
}