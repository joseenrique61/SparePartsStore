using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SPSAPI.Data;
using SPSAPI.DataSeeders;
using SPSAPI.Utilities.JWTResponseGenerator;
using SPSAPI.Utilities.JWTTokenGenerator;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDBContext>(
	options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationConnection")));

// Configuration for JWT Bearers
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JWTSettings:Issuer"]!,
		ValidAudience = builder.Configuration["JWTSettings:Audience"]!,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"]!))
	};
});

// Add the DBInitializer to the dependency injection
builder.Services.AddScoped<IAdministratorDataSeeder, AdministratorDataSeeder>();
builder.Services.AddScoped<ICategoryDataSeeder, CategoryDataSeeder>();
builder.Services.AddScoped<ISparePartDataSeeder, SparePartDataSeeder>();

// Add the token generator to the dependency injection
builder.Services.AddScoped<IJWTTokenGenerator, JWTTokenGenerator>();
builder.Services.AddScoped<IJWTResponseGenerator, JWTResponseGenerator>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger for use with JWT. Code obtained from https://medium.com/@deidra108/oauth-bearer-token-with-swagger-ui-net-6-0-86835e616deb and modified.
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "SPSAPI", Version = "v1" });
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Enter token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "bearer"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
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