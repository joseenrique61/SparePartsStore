using SPSModels.Models;
using SPSAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace SPSAPI.DataSeeders
{
	public class CategoryDataSeeder : ICategoryDataSeeder
	{
		private readonly IServiceProvider _serviceProvider;

		public CategoryDataSeeder(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public async Task Initialize()
		{
			using var context = new ApplicationDBContext(_serviceProvider.GetRequiredService<DbContextOptions<ApplicationDBContext>>());
			try
			{
				if (context.Category.Any())
				{
					return;
				}

				context.Category.AddRange(
					new Category()
					{
						Name = "Motor",
					},
					new Category()
					{
						Name = "Refrigeración"
					},
					new Category()
					{
						Name = "Frenos"
					}
					);

				await context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}
	}
}
