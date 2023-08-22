using NWDFoundation.Configuration;
using NWDFoundation.Logger;
using NWDFoundation.Models.Enums;
using NWDServerFront.Configuration;
using NWDServerShared.Configuration;
using NWDServerShared.Models;
using NWDWebDevelopment.Configuration;
using NWDWebRuntime.Configuration;
using NWDWebStandard.Configuration;
using NWDWebEditor.Configuration;
using NWDWebGitLabReport.Configuration;
using NWDWebHttpErrorSimulator.Configuration;
using NWDWebPlayerDemo.Configuration;
using NWDHub.Configuration;
using NWDIdemobi.Configuration;
using NWDServerHumanInterface.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Tools;
using NWDWebStudioDemo.Configuration;
using NWDWebTrackException.Configuration;
using NWDWebTreat.Configuration;
using NWDWebUploader.Configuration;

//NWDLogger.TestLayout();

var tBuilder = WebApplication.CreateBuilder(args);

/*add for dev mode ... with Active Razor Runtime Compilation */
bool tDevMode = true;
#if DEBUG
tDevMode = true;
#endif
/*---*/
/*add for services*/
NWDServerFrontConfiguration.LoadFromBuilder(tBuilder); // MANDATORY
NWDServerHumanInterfaceConfiguration.LoadFromBuilder(tBuilder);
/*---*/
/*add for website*/
//NWDWebRuntimeConfiguration.LoadFromBuilder(tBuilder,tDevMode); // MANDATORY
NWDWebStandardConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
NWDWebEditorConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
NWDWebPlayerDemoConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
NWDWebStudioDemoConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
NWDWebUploaderConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
NWDWebGitLabReportConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
NWDWebHttpErrorSimulatorConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
NWDWebTrackExceptionConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
NWDHubConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
NWDWebTreatConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
// for ideMobi only
NWDIdemobiConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
// for internal development only
NWDWebDevelopmentConfiguration.LoadFromBuilder(tBuilder, tDevMode); // MANDATORY
/*---*/

// Add services to the container.
tBuilder.Services.AddControllers();
tBuilder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
tBuilder.Services.AddEndpointsApiExplorer();
tBuilder.Services.AddSwaggerGen();
tBuilder.Services.AddHostedService<NWDServerStartupService>();
tBuilder.Services.AddHttpClient();

NWDComplexLogger tComplexLogger = new NWDComplexLogger(NWDLogLevel.Trace);
tComplexLogger.AddDefaultWritter(new NWDConsoleLogger());
tComplexLogger.AddSpecificWriter(new NWDTeamsWebHookLogger("https://babaoo.webhook.office.com/webhookb2/cc07a20e-83ed-4998-a6be-014ff55d2f17@cbf2c732-9574-4b2d-b22c-21cb996914c1/IncomingWebhook/17c5352a96c54fe897bbbd460701f1c1/aa2b5696-1ea7-4999-9c3d-728886b3b160"), NWDLogLevel.Critical);
tComplexLogger.AddSpecificWriter(new NWDSlackWebHookLogger("https://hooks.slack.com/services/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"), NWDLogLevel.Critical);
NWDLogger.SetWriter(tComplexLogger);


#if RELEASE
NWDLogger.Information("RELEASE mode is true");
#elif DEBUG
NWDLogger.Information("DEBUG mode is true");
#endif

var tApp = tBuilder.Build();

if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment)
{
    NWDLogger.Information(nameof(NWDWebRuntimeConfiguration) + "." + nameof(NWDWebRuntimeConfiguration.KConfig) + "." + nameof(NWDWebRuntimeConfiguration.KConfig.IsDevelopment) + " is true");
}
else
{
    NWDLogger.Information(nameof(NWDWebRuntimeConfiguration) + "." + nameof(NWDWebRuntimeConfiguration.KConfig) + "." + nameof(NWDWebRuntimeConfiguration.KConfig.IsDevelopment) + " is false");
}

tApp.UseStaticFiles();

tApp.UseSwagger();
tApp.UseSwaggerUI();

tApp.UseRouting();

/*add*/
NWDServerFrontConfiguration.UseApp(tApp);
NWDWebRuntimeConfiguration.UseApp(tApp);

NWDEmailTemplateManager.Default.Subject = "•••"+NWDEmailTemplate.K_SUBJECT_TAG;
NWDEmailTemplateManager.Default.Message = "<h1>Hi guys!</h1><img src='https://localhost:2051/favicons/favicon-80x80.png'/><br/><div>"+NWDEmailTemplate.K_MESSAGE_TAG+"</div>";
NWDEmailTemplateManager.Default.Header = "<div><b>MON HEADER</b></div>";
NWDEmailTemplateManager.Default.Footer = "<div><b>MON FOOTER</b></div>";
/*---*/

tApp.UseAuthorization();

NWDServerConfiguration.KConfig.Status = NWDServerStatus.Active;
NWDLogger.Information("Website is active!");

tApp.MapControllers();

tApp.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

tApp.Run();
