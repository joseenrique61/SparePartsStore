using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SPSAPI.Data;
using SPSAPI.DataSeeders;
using SPSAPI.Services;
using SPSAPI.Utilities.JWTResponseGenerator;
using SPSAPI.Utilities.JWTTokenGenerator;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDBContext>(
	options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationConnection")));

builder.Services.AddLogging();

// Configuration for JWT Bearers
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    // La API confía en Keycloak
    options.Authority = "http://localhost:8080/realms/FerreteriaRealm";
    options.Audience = "account"; // O el ClientID 'system-a-store'
    options.RequireHttpsMetadata = false;
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false, // Keycloak a veces pone audiencias dinámicas
        ValidateLifetime = false
    };
});

// Add the DBInitializer to the dependency injection
builder.Services.AddScoped<IAdministratorDataSeeder, AdministratorDataSeeder>();
builder.Services.AddScoped<ICategoryDataSeeder, CategoryDataSeeder>();
builder.Services.AddScoped<ISparePartDataSeeder, SparePartDataSeeder>();

// Add the token generator to the dependency injection
builder.Services.AddScoped<IJWTTokenGenerator, JWTTokenGenerator>();
builder.Services.AddScoped<IJWTResponseGenerator, JWTResponseGenerator>();

builder.Services.AddHttpClient<IVaultKmsService, VaultKmsService>();

// Add the controllers and prevent them from entering in an infinite loop when serializing recursive objects in JSON. Code made with ChatGPT.
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Nuevo generador oficial de OpenAPI para .NET 10
builder.Services.AddOpenApi(options =>
{
	options.AddDocumentTransformer((document, context, cancellationToken) =>
	{
		// Definimos el esquema de seguridad (JWT)
		var requirements = new Dictionary<string, string[]> {
						{ "Bearer", [] }
			};

		document.Components ??= new();
		document.Components.SecuritySchemes!.Add("Bearer", new OpenApiSecurityScheme
		{
			Type = SecuritySchemeType.Http,
			Scheme = "bearer",
			BearerFormat = "JWT",
			In = ParameterLocation.Header,
			Description = "Ingrese el token JWT"
		});

		document.Security!.Add(new OpenApiSecurityRequirement
		{
			[new OpenApiSecuritySchemeReference("bearer", document)] = []
		});

		return Task.CompletedTask;
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	// app.UseSwagger();
	// app.UseSwaggerUI();
}

// Seed data into the database
await DataSeeding(app);

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task DataSeeding(WebApplication app)
{
	using IServiceScope scoped = app.Services.CreateScope();

	await scoped.ServiceProvider.GetRequiredService<IAdministratorDataSeeder>().Initialize();
	await scoped.ServiceProvider.GetRequiredService<ICategoryDataSeeder>().Initialize();
	await scoped.ServiceProvider.GetRequiredService<ISparePartDataSeeder>().Initialize();
}