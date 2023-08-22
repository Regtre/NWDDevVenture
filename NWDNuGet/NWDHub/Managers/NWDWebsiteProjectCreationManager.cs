using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Tools;
using NWDHub.Models;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Tools;
using NWDWebStandard.Configuration;
using NWDWebTreat.Configuration;

namespace NWDHub.Managers;

public static class NWDWebsiteProjectCreationManager
{
    private const int KPortHttp = 3210;
    private const int KPortHttps = 3211;

    public static string PreExtension(NWDProjectTreatStorage sProjectTreatStorage)
    {
        string tEnvExtension = "." + sProjectTreatStorage.Environment.ToString();
        if (sProjectTreatStorage.Environment == NWDEnvironmentKind.Production)
        {
            tEnvExtension = "";
        }
        else if (sProjectTreatStorage.Environment == NWDEnvironmentKind.Dev)
        {
            tEnvExtension = ".Development";
        }

        return tEnvExtension;
    }

    public static string WebStandardConfigPathNameFor(NWDProject sProject, NWDProjectTreatStorage sProjectTreatStorage)
    {
        return nameof(NWDWebStandardConfiguration) + PreExtension(sProjectTreatStorage) + ".json";
    }

    public static string WebStandardConfigFor(NWDProject sProject, NWDProjectTreatStorage sProjectTreatStorage, bool sInAppSetting)
    {
        NWDWebStandardConfiguration tConfiguration = new NWDWebStandardConfiguration()
        {
            WebSiteName = sProject.Name,
            SocietyName = sProject.SocietyName,
            SocietyAddress = sProject.SocietyAddress,
            SocietyTown = sProject.SocietyTown,
            SocietyZipCode = sProject.SocietyZipCode,
            SocietyCountry = sProject.SocietyCountry,
            SocietySiret = sProject.SocietySiret,
            SocietyApe = sProject.SocietyApe,
            SocietyRcs = sProject.SocietyRcs,
            SocietyTva = sProject.SocietyTva,
            
        };
        if (sInAppSetting == true)
        {
            return "\"" + nameof(NWDWebStandardConfiguration) + "\": " + JsonConvert.SerializeObject(tConfiguration, Formatting.Indented) + "";
        }
        else
        {
            return "{\n\"" + nameof(NWDWebStandardConfiguration) + "\": " + JsonConvert.SerializeObject(tConfiguration, Formatting.Indented) + "\n}\n";
        }
    }

    public static string WebTreatConfigPathNameFor(NWDProject sProject, NWDProjectTreatStorage sProjectTreatStorage)
    {
        return nameof(NWDWebTreatConfiguration) + PreExtension(sProjectTreatStorage) + ".json";
    }

    public static string WebTreatConfigFor(NWDProject sProject, NWDProjectTreatStorage sProjectTreatStorage, bool sInAppSetting)
    {
        NWDWebTreatConfiguration tConfiguration = new NWDWebTreatConfiguration()
        {
            MyTreatKey = sProjectTreatStorage.TreatKey,
        };
        if (sInAppSetting == true)
        {
            return "\"" + nameof(NWDWebTreatConfiguration) + "\": " + JsonConvert.SerializeObject(tConfiguration, Formatting.Indented) + "";
        }
        else
        {
            return "{\n\"" + nameof(NWDWebTreatConfiguration) + "\": " + JsonConvert.SerializeObject(tConfiguration, Formatting.Indented) + "\n}\n";
        }
    }

    public static string WebRuntimeConfigPathNameFor(NWDProject sProject, NWDProjectTreatStorage sProjectTreatStorage)
    {
        return nameof(NWDWebRuntimeConfiguration) + PreExtension(sProjectTreatStorage) + ".json";
    }

    public static string WebRuntimeConfigFor(NWDProject sProject, NWDProjectTreatStorage sProjectTreatStorage, bool sInAppSetting)
    {
       
        NWDWebRuntimeConfiguration tConfiguration = new NWDWebRuntimeConfiguration()
        {
            MyProjectKey = sProjectTreatStorage.ProjectKey,
            MyProjectId = sProjectTreatStorage.ProjectUniqueId,
            MyEnvironment = sProjectTreatStorage.Environment,
            MySecretKey = sProjectTreatStorage.SecretKey,
            HubDns = NWDWebRuntimeConfiguration.KConfig.GetHubDnsClean(),
            ServerFormatDns = NWDWebRuntimeConfiguration.KConfig.GetServerFormatDnsClean(),
            BaseLanguage = sProject.BaseLanguage,
            SupportLanguages = sProject.SupportLanguages,
        };
        
        if (string.IsNullOrEmpty(sProject.WebsiteDns) == false )
        {
            UriBuilder tUriBuilder = new UriBuilder(sProject.WebsiteDns);
            if (tUriBuilder != null)
            {
                tConfiguration.Dns = tUriBuilder.Host;
            }
        }
        if (sInAppSetting == true)
        {
            return "\"" + nameof(NWDWebRuntimeConfiguration) + "\": " + JsonConvert.SerializeObject(tConfiguration, Formatting.Indented) + "";
        }
        else
        {
            return "{\n\"" + nameof(NWDWebRuntimeConfiguration) + "\": " + JsonConvert.SerializeObject(tConfiguration, Formatting.Indented) + "\n}\n";
        }
    }

    private static void AddFile(ZipArchive sZipArchive, string sPath, string sContent)
    {
        ZipArchiveEntry tFile = sZipArchive.CreateEntry(sPath);
        using (Stream tEntryStream = tFile.Open())
        {
            using (StreamWriter tStreamWriter = new StreamWriter(tEntryStream))
            {
                tStreamWriter.Write(sContent);
            }
        }
    }

    public static MemoryStream GenerateWebsiteProject(NWDWebsiteProjectCreationOption sOptions, NWDProject sProject)
    {
        string tNameOfProject = "NWDMyGameWebsite";
        tNameOfProject = NWDStringCleaner.UnixCleaner(sProject.Name);
        MemoryStream tMemoryStream = new MemoryStream();
        Guid tSolutionGuid = Guid.NewGuid();
        Guid tProjGuid = Guid.NewGuid();
        Guid tConfGuid = Guid.NewGuid();
        using (ZipArchive tZipArchive = new ZipArchive(tMemoryStream, ZipArchiveMode.Create, true))
        {

            AddFile(tZipArchive, "License.txt", "Generated by https://www.Net-Worked-Data.com for project " + sProject.Reference);
            AddFile(tZipArchive, "" + tNameOfProject + ".sln", string.Join(Environment.NewLine,
                "Microsoft Visual Studio Solution File, Format Version 12.00",
                "# Visual Studio Version 16",
                "VisualStudioVersion = 25.0.1704.2",
                "MinimumVisualStudioVersion = 10.0.40219.1",
                "Project(\"{" + tProjGuid + "}\") = \"" + tNameOfProject + "\", \"" + tNameOfProject + "\\" + tNameOfProject + ".csproj\", \"{" + tConfGuid + "}\"",
                "EndProject",
                "Global",
                "GlobalSection(SolutionConfigurationPlatforms) = preSolution",
                "Debug|Any CPU = Debug|Any CPU",
                "Release|Any CPU = Release|Any CPU",
                "EndGlobalSection",
                "GlobalSection(ProjectConfigurationPlatforms) = postSolution",
                "{" + tConfGuid + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU",
                "{" + tConfGuid + "}.Debug|Any CPU.Build.0 = Debug|Any CPU",
                "{" + tConfGuid + "}.Release|Any CPU.ActiveCfg = Release|Any CPU",
                "{" + tConfGuid + "}.Release|Any CPU.Build.0 = Release|Any CPU",
                "EndGlobalSection",
                "GlobalSection(SolutionProperties) = preSolution",
                "HideSolutionNode = FALSE",
                "EndGlobalSection",
                "GlobalSection(ExtensibilityGlobals) = postSolution",
                "SolutionGuid = {" + tSolutionGuid + "}",
                "EndGlobalSection",
                "EndGlobal"
            ));
            AddFile(tZipArchive, "NuGet.Config", string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                "   <configuration>",
                "   <packageSources>",
                "       <add key=\"nuget.org\" value=\"https://api.nuget.org/v3/index.json\" protocolVersion=\"3\" />",
                "       <add key=\"NetWorkedData\" value=\"https://gitlab.hephaiscode.com/api/v4/projects/281/packages/nuget/index.json\" />",
                "   </packageSources>",
                "</configuration>"
            ));
            AddFile(tZipArchive, "" + tNameOfProject + "/Properties/launchSettings.json", string.Join(Environment.NewLine,
                "",
                "{",
                "\"profiles\": {",
                "    \"" + tNameOfProject + "\": {",
                "      \"commandName\": \"Project\",",
                "      \"launchBrowser\": true,",
                "      \"applicationUrl\": \"https://localhost:" + KPortHttps + ";http://localhost:" + KPortHttp + "\",",
                "      \"dotnetRunMessages\": true",
                "    },",
                "    \"" + tNameOfProject + "-Development\": {",
                "      \"commandName\": \"Project\",",
                "      \"launchBrowser\": true,",
                "      \"environmentVariables\": {",
                "        \"ASPNETCORE_ENVIRONMENT\": \"Development\",",
                "        \"ASPNETCORE_HOSTINGSTARTUPASSEMBLIES\": \"Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation\"",
                "      },",
                "      \"applicationUrl\": \"https://localhost:" + KPortHttps + ";http://localhost:" + KPortHttp + "\",",
                "      \"dotnetRunMessages\": true",
                "    },",
                "    \"" + tNameOfProject + "-" + NWDEnvironmentKind.PlayTest + "\": {",
                "      \"commandName\": \"Project\",",
                "      \"launchBrowser\": true,",
                "      \"environmentVariables\": {",
                "        \"ASPNETCORE_ENVIRONMENT\": \"" + NWDEnvironmentKind.PlayTest + "\"",
                "      },",
                "      \"applicationUrl\": \"https://localhost:" + KPortHttps + ";http://localhost:" + KPortHttp + "\",",
                "      \"dotnetRunMessages\": true",
                "    },",
                "    \"" + tNameOfProject + "-" + NWDEnvironmentKind.Qualification + "\": {",
                "      \"commandName\": \"Project\",",
                "      \"launchBrowser\": true,",
                "      \"environmentVariables\": {",
                "        \"ASPNETCORE_ENVIRONMENT\": \"" + NWDEnvironmentKind.Qualification + "\"",
                "      },",
                "      \"applicationUrl\": \"https://localhost:" + KPortHttps + ";http://localhost:" + KPortHttp + "\",",
                "      \"dotnetRunMessages\": true",
                "    },",
                "    \"" + tNameOfProject + "-" + NWDEnvironmentKind.PreProduction + "\": {",
                "      \"commandName\": \"Project\",",
                "      \"launchBrowser\": true,",
                "      \"environmentVariables\": {",
                "        \"ASPNETCORE_ENVIRONMENT\": \"" + NWDEnvironmentKind.PreProduction + "\"",
                "      },",
                "      \"applicationUrl\": \"https://localhost:" + KPortHttps + ";http://localhost:" + KPortHttp + "\",",
                "      \"dotnetRunMessages\": true",
                "    },",
                "    \"" + tNameOfProject + "-" + NWDEnvironmentKind.PostProduction + "\": {",
                "      \"commandName\": \"Project\",",
                "      \"launchBrowser\": true,",
                "      \"environmentVariables\": {",
                "        \"ASPNETCORE_ENVIRONMENT\": \"" + NWDEnvironmentKind.PostProduction + "\"",
                "      },",
                "      \"applicationUrl\": \"https://localhost:" + KPortHttps + ";http://localhost:" + KPortHttp + "\",",
                "      \"dotnetRunMessages\": true",
                "    },",
                "    \"IIS Express\": {",
                "      \"commandName\": \"IISExpress\",",
                "      \"launchBrowser\": true,",
                "      \"environmentVariables\": {",
                "        \"ASPNETCORE_ENVIRONMENT\": \"Development\",",
                "        \"ASPNETCORE_HOSTINGSTARTUPASSEMBLIES\": \"Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation\"",
                "      }",
                "    }",
                "  },",
                "  \"iisSettings\": {",
                "    \"windowsAuthentication\": false,",
                "    \"anonymousAuthentication\": true,",
                "    \"iisExpress\": {",
                "      \"applicationUrl\": \"https://localhost:" + KPortHttps + ";http://localhost:" + KPortHttp + "\",",
                "      \"sslPort\": 44314",
                "    }",
                "  }",
                "}"));
            AddFile(tZipArchive, "" + tNameOfProject + "/" + tNameOfProject + ".csproj", string.Join(Environment.NewLine,
                "<Project Sdk=\"Microsoft.NET.Sdk.Web\">",
                "   <PropertyGroup>",
                "       <TargetFramework>net7.0</TargetFramework>",
                "       <Nullable>enable</Nullable>",
                "       <ImplicitUsings>enable</ImplicitUsings>",
                "   </PropertyGroup>",
                "   <ItemGroup>",
                "       <PackageReference Include=\"NWDWebStandard\" Version=\"" + NWDVersionDll.Version + "\" />",
                "       <PackageReference Include=\"NWDWebTreat\" Version=\"" + NWDVersionDll.Version + "\" />",
                "       <PackageReference Include=\"NWDWebUploader\" Version=\"" + NWDVersionDll.Version + "\" />",
                "   </ItemGroup>",
                "   <ItemGroup>",
                "       <Content Update=\"wwwroot\\css\\site.css\">",
                "           <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>",
                "       </Content>",
                "   </ItemGroup>",
                "</Project>"
            ));
            AddFile(tZipArchive, "" + tNameOfProject + "/Program.cs", string.Join(Environment.NewLine,
                "using NWDWebRuntime.Configuration;",
                "using NWDWebStandard.Configuration;",
                "using NWDWebTreat.Configuration;",
                "using NWDWebUploader.Configuration;",
                "var builder = WebApplication.CreateBuilder(args);",
                "// Add services to the container.",
                "builder.Services.AddControllersWithViews();",
                "/*add*/",
                "NWDWebRuntimeConfiguration.LoadFromBuilder(builder); // MANDATORY",
                "NWDWebStandardConfiguration.LoadFromBuilder(builder); // MANDATORY",
                sOptions.TreatModule == true ? "NWDWebTreatConfiguration.LoadFromBuilder(builder); // MANDATORY" : "",
                "/*---*/",
                "var app = builder.Build();",
                "// Configure the HTTP request pipeline.",
                "if (!app.Environment.IsDevelopment())",
                "{",
                "app.UseExceptionHandler(\"/Home/Error\");",
                "}",
                "app.UseStaticFiles();",
                "app.UseRouting();",
                "/*add*/",
                "NWDWebRuntimeConfiguration.UseApp(app);",
                "/*---*/",
                "app.UseAuthorization();",
                "app.MapControllerRoute(",
                "name: \"default\",",
                "pattern: \"{controller=Home}/{action=Index}/{id?}\");",
                "app.Run();"
            ));
            AddFile(tZipArchive, "" + tNameOfProject + "/appsettings.json", string.Join(Environment.NewLine,
                "{",
                "\"Logging\": {",
                "   \"LogLevel\": {",
                "       \"Default\": \"Information\",",
                "       \"Microsoft.AspNetCore\": \"Warning\"",
                "       }",
                "   },",
                "\"AllowedHosts\": \"*\",",
                "\"Kestrel\": {",
                "   \"EndPoints\": {",
                "       \"Http\": {",
                "           \"Url\": \"http://localhost:" + KPortHttp + "\"",
                "           },",
                "       \"Https\": {",
                "           \"Url\": \"https://localhost:" + KPortHttps + "\"",
                "           }",
                "       }",
                "   },",
                "\"NWDDatabaseConnectorConfiguration\": {",
                "   \"IsActive\": true,",
                "   \"SetUpPage\": false,",
                "   \"Credentials\": {",
                "       \"Name\": \"ReflexionMariaDB Database for local website\",",
                "       \"Range\": 1,",
                "       \"Kind\": 21,",
                "       \"Server\": \"cj2443-002.eu.clouddb.ovh.net\",",
                "       \"User\": \"TesterTwo\",",
                "       \"Database\": \"NWDTestTwo\",",
                "       \"TablePrefix\": \"DEV_L\",",
                "       \"Port\": 35938,",
                "       \"Password\": \"Y8ZErDjXE7MMuXUA29GV\",",
                "       \"Secure\": 0",
                "       }",
                "   }",
                "}"
            ));
            AddFile(tZipArchive, "" + tNameOfProject + "/appsettings.Development.json", string.Join(Environment.NewLine,
                "{",
                "\"Logging\": {",
                "   \"LogLevel\": {",
                "       \"Default\": \"Information\",",
                "       \"Microsoft.AspNetCore\": \"Warning\"",
                "       }",
                "   },",
                "\"AllowedHosts\": \"*\",",
                "\"Kestrel\": {",
                "   \"EndPoints\": {",
                "       \"Http\": {",
                "           \"Url\": \"http://localhost:" + KPortHttp + "\"",
                "           },",
                "       \"Https\": {",
                "           \"Url\": \"https://localhost:" + KPortHttps + "\"",
                "           }",
                "       }",
                "   }",
                "}"
            ));
            AddFile(tZipArchive, "" + tNameOfProject + "/Controllers/HomeController.cs", string.Join(Environment.NewLine,
                "using Microsoft.AspNetCore.Mvc;",
                "using NWDWebStandard.Controllers;",
                "using NWDWebStandard.Models;",
                "namespace NWDWeb.Controllers;",
                "public class HomeController : NWDBasicController<HomeController>",
                "{",
                "private readonly ILogger<HomeController> _logger;",
                "public HomeController(ILogger<HomeController> logger)",
                "{",
                "_logger = logger;",
                "}",
                "public IActionResult Index()",
                "{",
                "NWDStatisticsConsolidated.IncrementForValue(\"Home\", \"HomePage\", \"Home\", \"\", HttpContext);",
                "return View();",
                "}",
                "}"
            ));
            AddFile(tZipArchive, "" + tNameOfProject + "/wwwroot/css/site.css", string.Join(Environment.NewLine,
                "#your ccs file"
            ));
            AddFile(tZipArchive, "" + tNameOfProject + "/wwwroot/js/site.js", string.Join(Environment.NewLine,
                "//your js file"
            ));
            AddFile(tZipArchive, "" + tNameOfProject + "/Views/Home/Index.cshtml", string.Join(Environment.NewLine,
                "@{Layout = \"~/Views/Shared/_Layout.cshtml\"; }",
                "<div class=\"text-center\">",
                "<h1 class=\"display-4\">Welcome to ...</h1>",
                "<p>Learn about this project ... <a href=\"https://www.net-worked-data.com\">building inter-connected Website, Unity Game and MAUI app with www.net-worked-data.com</a>.</p>",
                "</div>"
            ));
            foreach (NWDProjectTreatStorage tKeys in NWDProjectTreatManager.GetAllByProjectUniqueId(sProject.ProjectUniqueId).OrderBy(x => x.Environment).ToList())
            {
                AddFile(tZipArchive, tNameOfProject + "/" + WebRuntimeConfigPathNameFor(sProject, tKeys), WebRuntimeConfigFor(sProject, tKeys, false));
                AddFile(tZipArchive, tNameOfProject + "/" + WebStandardConfigPathNameFor(sProject, tKeys), WebStandardConfigFor(sProject, tKeys, false));
                AddFile(tZipArchive, tNameOfProject + "/" + WebTreatConfigPathNameFor(sProject, tKeys), WebTreatConfigFor(sProject, tKeys, false));
            }
            tZipArchive.Dispose();
        }
        tMemoryStream.Seek(0, SeekOrigin.Begin);
        return tMemoryStream;
    }
}