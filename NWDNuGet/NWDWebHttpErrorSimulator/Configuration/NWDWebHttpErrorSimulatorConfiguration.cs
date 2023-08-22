using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebHttpErrorSimulator.Managers;
using NWDWebHttpErrorSimulator.Services;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebHttpErrorSimulator.Resources;
using NWDWebStandard.Managers;

namespace NWDWebHttpErrorSimulator.Configuration
{
    [Serializable]
    public class NWDWebHttpErrorSimulatorConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDWebHttpErrorSimulatorConfiguration KConfig = new NWDWebHttpErrorSimulatorConfiguration();
        public static readonly HttpClient HttpClientShared = new HttpClient();
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
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDWebHttpErrorSimulatorConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDWebHttpErrorSimulatorConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDWebHttpErrorSimulatorConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDWebHttpErrorSimulatorStartupService>();
                
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
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDWebHttpErrorSimulator), nameof(NWDWebHttpErrorSimulatorConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDWebHttpErrorSimulator)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDWebHttpErrorSimulator), nameof(NWDWebHttpErrorSimulatorConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDWebHttpErrorSimulator)));
                }
            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDWebHttpErrorSimulatorConfiguration? tConfig = sConfig.GetSection(nameof(NWDWebHttpErrorSimulatorConfiguration)).Get<NWDWebHttpErrorSimulatorConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDWebHttpErrorSimulatorConfiguration)));
                //NWDLogger.Trace(nameof(NWDWebHttpErrorSimulatorConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDWebHttpErrorSimulatorConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDWebHttpErrorSimulatorConfiguration)), NWDLogger.SplitObjectSerializable(new NWDWebHttpErrorSimulatorConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(this.GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);

            NWDWebRuntimeConfiguration.KConfig.NavBarInterfaceAdd(new NWDWebHttpErrorSimulatorNavBar());
            NWDWebRuntimeConfiguration.KConfig.SideBarInterfaceAdd(new NWDWebHttpErrorSimulatorSideBar());
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