using SPSModels.Models;
using SPSAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace SPSAPI.DataSeeders
{
	public class SparePartDataSeeder : ISparePartDataSeeder
	{
		private readonly IServiceProvider _serviceProvider;

		public SparePartDataSeeder(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public async Task Initialize()
		{
			using var context = new ApplicationDBContext(_serviceProvider.GetRequiredService<DbContextOptions<ApplicationDBContext>>());
			try
			{
				if (context.SparePart.Any())
				{
					return;
				}

				// Code for spare part seeding (not yet defined default elements)

				//await context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}
	}
}
