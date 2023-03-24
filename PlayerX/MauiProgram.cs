using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PlayerX.Models;
using PlayerX.ViewModel;
using PlayerX.Views;

namespace PlayerX;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkitMediaElement()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        builder.Services.AddTransient<MainPage>(); 
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
