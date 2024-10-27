using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SPSModels.Models;

namespace SparePartsStoreWeb.Data
{
	public class SparePartsStoreContext : DbContext
	{
		public SparePartsStoreContext(DbContextOptions<SparePartsStoreContext> options)
			: base(options)
		{
		}

		public DbSet<SparePart> SparePart { get; set; } = default!;

		public DbSet<Category> Category { get; set; } = default!;
	}
}
