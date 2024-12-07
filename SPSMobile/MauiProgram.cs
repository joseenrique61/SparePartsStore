using Microsoft.Extensions.Logging;
using SPSMobile.Data.ApiClient;
using SPSMobile.Data.Repositories.CategoryRepository;
using SPSMobile.Data.Repositories.ClientRepository;
using SPSMobile.Data.Repositories.PurchaseOrderRepository;
using SPSMobile.Data.Repositories.SparePartRepository;
using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using SPSMobile.Utilities.AlertService;
using SPSMobile.Utilities.Authenticator;
using SPSMobile.Utilities.ClientUIManager;
using SPSMobile.Utilities.DataSeeder;
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

			// Authentication
			builder.Services.AddSingleton<IAuthenticator, Authenticator>();

			// Inject the repositories based on the configuration
			InjectRepositories(builder);

			// Unit of work
			builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

			// DisplayAlert Service
			builder.Services.AddScoped<IAlertService, AlertService>();

			// Client UI manager
			builder.Services.AddScoped<IClientUIManager, ClientUIManager>();

			// ViewModels
			RegisterViewModels(builder);
			
			// Pages
			RegisterPages(builder);

#if DEBUG
			builder.Logging.AddDebug();
#endif

			MauiApp mauiApp = builder.Build();

#if NO_API
			using (IServiceScope serviceScope = mauiApp.Services.CreateScope())
			{
				serviceScope.ServiceProvider.GetRequiredService<IDataSeeder>().Seed();
			}
#endif

			return mauiApp;
		}

		private static void InjectRepositories(MauiAppBuilder builder)
		{
#if NO_API
			// Repositories for the app
			builder.Services.AddScoped<ISparePartRepository, LocalSparePartRepository>();
			builder.Services.AddScoped<ICategoryRepository, LocalCategoryRepository>();
			builder.Services.AddScoped<IPurchaseOrderRepository, LocalPurchaseOrderRepository>();
			builder.Services.AddScoped<IClientRepository, LocalClientRepository>();

			// Data seeder
			builder.Services.AddScoped<IDataSeeder, DataSeeder>();
#else
				// HttpClient
				builder.Services.AddTransient<HttpClient>();

				// API Client
				builder.Services.AddSingleton<IApiClient, ApiClient>();

				// Repositories for the app
				builder.Services.AddScoped<ISparePartRepository, SparePartRepository>();
				builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
				builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
				builder.Services.AddScoped<IClientRepository, ClientRepository>();
#endif
		}

		private static void RegisterViewModels(MauiAppBuilder builder)
		{
			builder.Services.AddSingleton<AppShellViewModel>();
			builder.Services.AddSingleton<ClientViewModel>();
			builder.Services.AddSingleton<PurchaseOrderViewModel>();
			builder.Services.AddTransient<LoginViewModel>();
			builder.Services.AddTransient<RegisterViewModel>();
			builder.Services.AddTransient<SparePartsViewModel>();
			builder.Services.AddTransient<InformationViewModel>();
		}

		private static void RegisterPages(MauiAppBuilder builder)
		{
			IEnumerable<Type> classes = from type in Assembly.GetExecutingAssembly().GetTypes()
									  where type.IsClass && type.Namespace == "SPSMobile.Pages"
									  select type;
			foreach (Type type in classes)
			{
				builder.Services.AddTransient(type);
			}
		}
	}
}
