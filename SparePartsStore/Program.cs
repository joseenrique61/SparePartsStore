using SparePartsStoreWeb.Data.ApiClient;
using SparePartsStoreWeb.Data.Repositories.CategoryRepository;
using SparePartsStoreWeb.Data.Repositories.ClientRepository;
using SparePartsStoreWeb.Data.Repositories.PurchaseOrderRepository;
using SparePartsStoreWeb.Data.Repositories.SparePartRepository;
using SparePartsStoreWeb.Data.UnitOfWork;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

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

app.Run();
