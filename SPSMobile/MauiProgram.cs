using Microsoft.Extensions.Logging;
using SPSMobile.Data.ApiClient;
using SPSMobile.Data.FileManager;
using SPSMobile.Data.Repositories.CategoryRepository;
using SPSMobile.Data.Repositories.ClientRepository;
using SPSMobile.Data.Repositories.PurchaseOrderRepository;
using SPSMobile.Data.Repositories.SparePartRepository;
using SPSMobile.Data.UnitOfWork;
using SPSMobile.Utilities;
using System.Reflection;

namespace SPSMobile
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
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			builder.Services.AddTransient<HttpClient>();

			// Authentication
			builder.Services.AddSingleton<IAuthenticator, Authenticator>();

			//// File manager
			//builder.Services.AddScoped<IFileManager, FileManager>();
			
			//// API Client
			//builder.Services.AddSingleton<IApiClient, ApiClient>();

			// Repositories for the app
			builder.Services.AddScoped<ISparePartRepository, LocalSparePartRepository>();
			builder.Services.AddScoped<ICategoryRepository, LocalCategoryRepository>();
			builder.Services.AddScoped<IPurchaseOrderRepository, LocalPurchaseOrderRepository>();
			builder.Services.AddScoped<IClientRepository, LocalClientRepository>();

			// Unit of work
			builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

			// Data seeder
			builder.Services.AddScoped<IDataSeeder, DataSeeder>();

			// Pages
			string pagesNamespace = "SPSMobile.Pages";
			IEnumerable<Type> pages = from type in Assembly.GetExecutingAssembly().GetTypes()
					where type.IsClass && type.Namespace == pagesNamespace
					select type;
			foreach (Type type in pages)
			{
				builder.Services.AddTransient(type);
			}

#if DEBUG
			builder.Logging.AddDebug();
#endif

			MauiApp mauiApp = builder.Build();
			using (IServiceScope serviceScope = mauiApp.Services.CreateScope())
			{
				serviceScope.ServiceProvider.GetRequiredService<IDataSeeder>().Seed();
			}

			return mauiApp;
		}
	}
}
