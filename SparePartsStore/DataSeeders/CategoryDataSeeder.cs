using Microsoft.EntityFrameworkCore;
using SparePartsStore.Data;
using SparePartsStore.Models;

namespace SparePartsStore.DataSeeders
{
	public class CategoryDataSeeder
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			using (var context = new SparePartsStoreContext(serviceProvider.GetRequiredService<DbContextOptions<SparePartsStoreContext>>()))
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
			}
		}
	}
}
