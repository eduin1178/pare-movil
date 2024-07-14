using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace PAREMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("Jost-VariableFont_wght.ttf", "Jost");
                    fonts.AddFont("Jost-Italic-VariableFont_wght.ttf", "JostItalic");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddMudServices();

            return builder.Build();
        }
    }
}
