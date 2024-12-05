using Microsoft.Extensions.Logging;
using SPSMobile.Data.ApiClient;
using SPSMobile.Data.Repositories.CategoryRepository;
using SPSMobile.Data.Repositories.ClientRepository;
using SPSMobile.Data.Repositories.PurchaseOrderRepository;
using SPSMobile.Data.Repositories.SparePartRepository;
using SPSMobile.Data.UnitOfWork;
using SPSMobile.Utilities.Authenticator;
using SPSMobile.Utilities.DataSeeder;
using System.Reflection;

namespace SPSMobile
{
	public static class MauiProgram
	{
		private static readonly bool UseAPI = true;

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

			// Authentication
			builder.Services.AddSingleton<IAuthenticator, Authenticator>();

			// Inject the repositories based on the configuration
			InjectRepositories(builder, UseAPI);

			// Unit of work
			builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

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

			if (!UseAPI)
			{
				using (IServiceScope serviceScope = mauiApp.Services.CreateScope())
				{
					serviceScope.ServiceProvider.GetRequiredService<IDataSeeder>().Seed();
				}
			}

			return mauiApp;
		}

		private static void InjectRepositories(MauiAppBuilder builder, bool api)
		{
			if (api)
			{
				// HttpClient
				builder.Services.AddTransient<HttpClient>();

				// API Client
				builder.Services.AddSingleton<IApiClient, ApiClient>();

				// Repositories for the app
				builder.Services.AddScoped<ISparePartRepository, SparePartRepository>();
				builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
				builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
				builder.Services.AddScoped<IClientRepository, ClientRepository>();
			}
			else
			{
				// Repositories for the app
				builder.Services.AddScoped<ISparePartRepository, LocalSparePartRepository>();
				builder.Services.AddScoped<ICategoryRepository, LocalCategoryRepository>();
				builder.Services.AddScoped<IPurchaseOrderRepository, LocalPurchaseOrderRepository>();
				builder.Services.AddScoped<IClientRepository, LocalClientRepository>();

				// Data seeder
				builder.Services.AddScoped<IDataSeeder, DataSeeder>();
			}
		}
	}
}
