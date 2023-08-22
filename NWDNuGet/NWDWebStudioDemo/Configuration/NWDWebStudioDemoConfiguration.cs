using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NWDWebStudioDemo.Resources;
using Microsoft.Extensions.Localization;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebStudioDemo.Controllers;
using NWDWebStudioDemo.Services;

namespace NWDWebStudioDemo.Configuration
{
    [Serializable]
    public class NWDWebStudioDemoConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDWebStudioDemoConfiguration KConfig = new NWDWebStudioDemoConfiguration();
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
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDWebStudioDemoConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDWebStudioDemoConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDWebStudioDemoConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDWebStudioDemoStartupService>();
                
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
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDWebStudioDemo), nameof(NWDWebStudioDemoConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDWebStudioDemo)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDWebStudioDemo), nameof(NWDWebStudioDemoConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDWebStudioDemo)));
                }
            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDWebStudioDemoConfiguration? tConfig = sConfig.GetSection(nameof(NWDWebStudioDemoConfiguration)).Get<NWDWebStudioDemoConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDWebStudioDemoConfiguration)));
                //NWDLogger.Trace(nameof(NWDWebStudioDemoConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDWebStudioDemoConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDWebStudioDemoConfiguration)), NWDLogger.SplitObjectSerializable(new NWDWebStudioDemoConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            //Do something once
            // NWDWebRuntimeConfiguration.KConfig.AdminNavBarPartialList.Add(new NWDPartialView() { PartialPath = "_StudioRightNavbar.cshtml" });
            
            NWDWebRuntimeConfiguration.KConfig.NavBarInterfaceAdd(new NWDProfessionalVoucherNavBar());
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