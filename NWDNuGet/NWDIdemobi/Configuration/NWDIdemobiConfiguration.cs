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
using NWDIdemobi.Services;
using NWDWebRuntime.Configuration;
using NWDIdemobi.Resources;

namespace NWDIdemobi.Configuration
{
    [Serializable]
    public class NWDIdemobiConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDIdemobiConfiguration KConfig = new NWDIdemobiConfiguration();
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
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDIdemobiConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDIdemobiConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDIdemobiConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDIdemobiStartupService>();
                
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
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDIdemobi), nameof(NWDIdemobiConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDIdemobi)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDIdemobi), nameof(NWDIdemobiConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDIdemobi)));
                }
            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDIdemobiConfiguration? tConfig = sConfig.GetSection(nameof(NWDIdemobiConfiguration)).Get<NWDIdemobiConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDIdemobiConfiguration)));
                //NWDLogger.Trace(nameof(NWDIdemobiConfiguration),  NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDIdemobiConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDIdemobiConfiguration)),  NWDLogger.SplitObjectSerializable(new NWDIdemobiConfiguration()));
            }
            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
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