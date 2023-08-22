using System.Reflection;
using System.Text.Json;
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
using NWDWebPlayerDemo.Services;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebPlayerDemo.Resources;

namespace NWDWebPlayerDemo.Configuration
{
    [Serializable]
    public class NWDWebPlayerDemoConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDWebPlayerDemoConfiguration KConfig = new NWDWebPlayerDemoConfiguration();
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
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDWebPlayerDemoConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDWebPlayerDemoConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDWebPlayerDemoConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDWebPlayerDemoStartupService>();
                
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
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDWebPlayerDemo), nameof(NWDWebPlayerDemoConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDWebPlayerDemo)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDWebPlayerDemo), nameof(NWDWebPlayerDemoConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDWebPlayerDemo)));
                }

            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDWebPlayerDemoConfiguration? tConfig = sConfig.GetSection(nameof(NWDWebPlayerDemoConfiguration)).Get<NWDWebPlayerDemoConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDWebPlayerDemoConfiguration)));
                //NWDLogger.Trace(nameof(NWDWebPlayerDemoConfiguration),  NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDWebPlayerDemoConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDWebPlayerDemoConfiguration)),  NWDLogger.SplitObjectSerializable(new NWDWebPlayerDemoConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            
            //Do something once
            // NWDWebRuntimeConfiguration.KConfig.AppNavBarPartialList.Add(new NWDPartialView() { PartialPath = "_PlayerRightNavbar.cshtml" });
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