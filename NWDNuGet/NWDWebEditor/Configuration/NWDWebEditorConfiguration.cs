using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebEditor.Managers;
using NWDWebEditor.Services;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebEditor.Resources;

namespace NWDWebEditor.Configuration
{
    [Serializable]
    public class NWDWebEditorConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDWebEditorConfiguration KConfig = new NWDWebEditorConfiguration();
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
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDWebEditorConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDWebEditorConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDWebEditorConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDWebEditorStartupService>();
                
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
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDWebEditor), nameof(NWDWebRuntimeConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDWebEditor)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDWebEditor), nameof(NWDWebRuntimeConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDWebEditor)));
                }
            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDWebEditorConfiguration? tConfig = sConfig.GetSection(nameof(NWDWebEditorConfiguration)).Get<NWDWebEditorConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDWebEditorConfiguration)));
                //NWDLogger.Trace(nameof(NWDWebEditorConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDWebEditorConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDWebEditorConfiguration)), NWDLogger.SplitObjectSerializable(new NWDWebEditorConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;

            NWDLibrariesInstalled.AddAssemblyByType(typeof(NWDEditor.Facades.INWDEditorManager));
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            
            NWDWebRuntimeConfiguration.KConfig.SideBarInterfaceAdd(new NWDWebEditorSideBar());
            NWDWebRuntimeConfiguration.KConfig.NavBarInterfaceAdd(new NWDWebEditorNavBar());
            NWDWebRuntimeConfiguration.KConfig.NavFooterInterfaceAdd(new NWDWebEditorNavFooter());
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