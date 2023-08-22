using System.Reflection;
using Microsoft.Extensions.Logging;
using NWDMauiRuntime.Configurations;

namespace NWDMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddScoped(sp => new HttpClient { });

#if DEBUG
        builder.Logging.AddDebug();
#endif
       NWDMauiRuntimeConfiguration.LoadFromBuilder(builder);

        return builder.Build();
    }
}