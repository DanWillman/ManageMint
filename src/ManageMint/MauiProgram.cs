using ManageMint.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
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
			.AddJsonFile("appsettings.json")
			.AddUserSecrets<App>(optional: false, reloadOnChange: true)
			.Build();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		builder.Services.AddMudServices();

		BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
		builder.Services.AddSingleton<IMongoCollection<Person>>(s => new MongoClient(builder.Configuration["Mongo:ConnectionString"])
			.GetDatabase(builder.Configuration["Mongo:Database"])
			.GetCollection<Person>(builder.Configuration["Mongo:PersonCollectionName"]));
		return builder.Build();
	}
}
