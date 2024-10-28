using Microsoft.EntityFrameworkCore;
using SPSModels.Models;

namespace SPSAPI.Data
{
	public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
	{
		public DbSet<Category> Category { get; set; }

		public DbSet<SparePart> SparePart { get; set; }

		public DbSet<User> User { get; set; }

		public DbSet<Client> Client { get; set; }

		public DbSet<Administrator> Administrator { get; set; }
	}
}
