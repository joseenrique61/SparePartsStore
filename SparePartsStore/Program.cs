using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using SparePartsStoreWeb.Data.ApiClient;
using SparePartsStoreWeb.Data.Repositories.CategoryRepository;
using SparePartsStoreWeb.Data.Repositories.ClientRepository;
using SparePartsStoreWeb.Data.Repositories.PurchaseOrderRepository;
using SparePartsStoreWeb.Data.Repositories.SparePartRepository;
using SparePartsStoreWeb.Data.UnitOfWork;
using SparePartsStoreWeb.Utilities;
using SPSModels.Models;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IApiClient, ApiClient>();
builder.Services.AddSession();

// Repositories for the app
builder.Services.AddScoped<ISparePartRepository, SparePartRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

// Unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Authentication
builder.Services.AddScoped<IAuthenticator, Authenticator>();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
	// Configuraciones para HTTP local
	options.Cookie.SameSite = SameSiteMode.Lax;
	options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
})
.AddOpenIdConnect(options =>
{
	options.Authority = "http://localhost:8080/realms/FerreteriaRealm";
	options.ClientId = "system-a-sps";
	options.ClientSecret = "iLbbnOqDFd0UkWMKBMsa2YdMamBrSycp"; // El de la pestaña Credentials
	options.ResponseType = "code";

	// IMPORTANTE: Como Keycloak está en Docker y tu app en HTTP
	options.RequireHttpsMetadata = false;

	options.SaveTokens = true;
	options.CallbackPath = "/signin-oidc"; // Esta ruta la maneja el middleware automáticamente

	// Ajustes de cookies para evitar errores de correlación en HTTP
	options.NonceCookie.SameSite = SameSiteMode.Lax;
	options.CorrelationCookie.SameSite = SameSiteMode.Lax;
	options.NonceCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
	options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

	options.TokenValidationParameters = new TokenValidationParameters
	{
		// Forzamos a que use "role" como el tipo de claim para roles
		NameClaimType = "preferred_username",
		RoleClaimType = "role"
	};

	options.Events = new OpenIdConnectEvents
	{
		OnTokenValidated = async context =>
		{
			// 1. Obtener el ID de Keycloak (sub) y datos del token
			var claims = context.Principal?.Claims;
			var keycloakId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			var email = claims?.First(c => 
                c.Type == ClaimTypes.Email || c.Type == "email" || c.Type.EndsWith("/email")).Value;
			var name = claims?.FirstOrDefault(c => c.Type == "preferred_username")?.Value;

			var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

			if (string.IsNullOrEmpty(keycloakId)) return;

			var newUser = new Client
			{
				User = new User
				{
					KeyCloakId = keycloakId,
					Email = email ?? "no-email@keycloak.com",
					// Como la contraseña la maneja Keycloak, puedes poner un hash aleatorio 
					// o dejarlo vacío si tu modelo lo permite.
					Password = "EXTERNAL_AUTH"
				},
				Name = name ?? "Nuevo Usuario",
				City = "Quito",
				Country = "Ecuador",
				Address = "Any"
			};

			var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

			ClaimsIdentity? identity = null;

			var userId = await unitOfWork.Client.Login(keycloakId);
			if (userId != -1)
			{
				identity = context.Principal?.Identity as ClaimsIdentity;
				identity?.AddClaim(new Claim("LocalId", userId.ToString()));
				return;
			}

			userId = await unitOfWork.Client.Register(newUser);

			if (userId == -1)
			{
				logger.LogError("Failed to create user.");
				return;
			}

			// 5. Inyectar el ID LOCAL en los claims para que tus controladores sigan funcionando con el Id entero
			identity = context.Principal?.Identity as ClaimsIdentity;
			identity?.AddClaim(new Claim("LocalId", userId.ToString()));
		}
	};
});


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseSession();

app.UseRequestLocalization("en-US", "es-EC");

app.Run();
