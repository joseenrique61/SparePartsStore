using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSAPI.Utilities;
using SPSModels.Models;
using System.Diagnostics;

namespace SPSAPI.DataSeeders
{
	public class AdministratorDataSeeder : IAdministratorDataSeeder
	{
		private readonly IServiceProvider _serviceProvider;

		public AdministratorDataSeeder(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public async Task Initialize()
		{
			using var context = new ApplicationDBContext(_serviceProvider.GetRequiredService<DbContextOptions<ApplicationDBContext>>());
			try
			{
				if (context.Administrator.Any())
				{
					return;
				}

				User user = new()
				{
					Email = "admin@admin.com",
					PasswordHash = PasswordHasher.Hash("admin")
				};
				await context.User.AddAsync(user);
				await context.SaveChangesAsync();

				Administrator admin = new()
				{
					User = user,
					UserId = user.Id
				};

				await context.Administrator.AddAsync(admin);
				await context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}
	}
}
