using Microsoft.Extensions.Logging;
using SPSMobile.Data.ApiClient;
using SPSMobile.Data.Repositories.CategoryRepository;
using SPSMobile.Data.Repositories.ClientRepository;
using SPSMobile.Data.Repositories.PurchaseOrderRepository;
using SPSMobile.Data.Repositories.SparePartRepository;
using SPSMobile.Data.UnitOfWork;
using SPSMobile.Pages;
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
			
			// API Client
			builder.Services.AddSingleton<IApiClient, ApiClient>();

			// Repositories for the app
			builder.Services.AddScoped<ISparePartRepository, SparePartRepository>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
			builder.Services.AddScoped<IClientRepository, ClientRepository>();

			// Unit of work
			builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
			builder.Services.AddTransient<ProductPage>();
			//builder.Services.AddTransient<CategoriesPage>();


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

			return builder.Build();
		}
	}
}
