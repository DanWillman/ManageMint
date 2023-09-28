using ManageMint.DataAccess;
using ManageMint.Models;
using ManageMint.Models.Configuration;
using ManageMint.Pages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MudBlazor;
using MudBlazor.Services;

namespace ManageMint;

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
			});
		
		builder.Services.AddMauiBlazorWebView();

		builder.Configuration
			.AddJsonFile("appsettings.json", optional: true)
			.AddUserSecrets<App>(optional: true, reloadOnChange: true)
			.Build();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
		builder.Configuration.GetSection(nameof(Mongo)).Bind(new Mongo());
		
		builder.Services.AddMudServices();
		builder.Services.AddMudMarkdownServices();

		builder.Services.AddSingleton<IMongoService, MongoService>();
		
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddTransient<Diary>();

		BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
		return builder.Build();
	}
}
