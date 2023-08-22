using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NWDWebUploader.Resources;
using Microsoft.Extensions.Localization;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebDownloader.Configuration;
using NWDWebUploader.Services;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebUploader.Managers;

namespace NWDWebUploader.Configuration
{
    [Serializable]
    public class NWDWebUploaderConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDWebUploaderConfiguration KConfig = new NWDWebUploaderConfiguration();
        private static bool Loaded { set; get; } = false;

        public const long MaxUploadSizeInBytes = 1000000000;

        #endregion

        #region instance properties

        public bool SetUpPage { set; get; } = true;

        #endregion

        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDWebUploaderConfiguration)));
            }
            else
            {
                NWDWebDownloaderConfiguration.LoadFromBuilder(sBuilder, sRuntimeCompileForDev);
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDWebUploaderConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDWebUploaderConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDWebUploaderStartupService>();
                
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
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDWebUploader), nameof(NWDWebUploaderConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDWebUploader)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDWebUploader), nameof(NWDWebUploaderConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDWebUploader)));
                }
            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDWebUploaderConfiguration? tConfig = sConfig.GetSection(nameof(NWDWebUploaderConfiguration)).Get<NWDWebUploaderConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDWebUploaderConfiguration)));
                //NWDLogger.Trace(nameof(NWDWebUploaderConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDWebUploaderConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDWebUploaderConfiguration)), NWDLogger.SplitObjectSerializable(new NWDWebUploaderConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            NWDWebRuntimeConfiguration.KConfig.NavBarInterfaceAdd(new NWDWebUploaderNavBar());
            NWDWebRuntimeConfiguration.KConfig.SideBarInterfaceAdd(new NWDWebUploaderSideBar());
            NWDWebRuntimeConfiguration.KConfig.NavFooterInterfaceAdd(new NWDWebUploaderNavFooter());
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