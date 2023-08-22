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
using NWDHub.Configuration;
using NWDIdemobi.Configuration;
using NWDWebTrackException.Configuration;

var tBuilder = WebApplication.CreateBuilder(args);

/*add for dev mode ... with Active Razor Runtime Compilation */
bool tDevMode = false;
#if DEBUG
tDevMode = true;
#endif
/*---*/
/*add for services*/
NWDServerFrontConfiguration.LoadFromBuilder(tBuilder); // MANDATORY
/*---*/
/*add for website*/
//NWDWebRuntimeConfiguration.LoadFromBuilder(tBuilder,tDevMode); // MANDATORY
NWDWebStandardConfiguration.LoadFromBuilder(tBuilder,tDevMode); // MANDATORY
NWDWebEditorConfiguration.LoadFromBuilder(tBuilder,tDevMode); // MANDATORY
NWDWebGitLabReportConfiguration.LoadFromBuilder(tBuilder,tDevMode); // MANDATORY
NWDWebHttpErrorSimulatorConfiguration.LoadFromBuilder(tBuilder,tDevMode); // MANDATORY
NWDWebTrackExceptionConfiguration.LoadFromBuilder(tBuilder,tDevMode); // MANDATORY
NWDHubConfiguration.LoadFromBuilder(tBuilder,tDevMode); // MANDATORY
// for ideMobi only
NWDIdemobiConfiguration.LoadFromBuilder(tBuilder,tDevMode); // MANDATORY
// for internal development only
NWDWebDevelopmentConfiguration.LoadFromBuilder(tBuilder,tDevMode); // MANDATORY
/*---*/

// Add services to the container.
tBuilder.Services.AddControllers();
tBuilder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
tBuilder.Services.AddEndpointsApiExplorer();
tBuilder.Services.AddSwaggerGen();
tBuilder.Services.AddHostedService<NWDServerStartupService>();
tBuilder.Services.AddHttpClient();

#if RELEASE
NWDLogger.Trace("⚠️ RELEASE MODE! ⚠️"); 
#elif DEBUG
NWDLogger.Trace("⚠️ DEBUG IS ACTIVE! ⚠️");
#endif

var tApp = tBuilder.Build();

if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment)
{
    NWDLogger.TraceAttention("-------------------------------- ⚠️ Is Development ⚠️-------------------------------- ");
}
else
{
    NWDLogger.TraceAttention("-------------------------------- ⚠️ Is NOT Development ⚠️-------------------------------- ");
}

tApp.UseStaticFiles();

tApp.UseSwagger();
tApp.UseSwaggerUI();

tApp.UseRouting();

/*add*/
NWDServerFrontConfiguration.UseApp(tApp);
NWDWebRuntimeConfiguration.UseApp(tApp);
/*---*/

tApp.UseAuthorization();

NWDServerConfiguration.KConfig.Status = NWDServerStatus.Active;
NWDLogger.TraceSuccess("Website is active!");

tApp.MapControllers();

tApp.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

tApp.Run();
