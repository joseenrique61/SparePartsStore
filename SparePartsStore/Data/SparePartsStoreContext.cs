using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SparePartsStore.Models;

namespace SparePartsStore.Data
{
    public class SparePartsStoreContext : DbContext
    {
        public SparePartsStoreContext (DbContextOptions<SparePartsStoreContext> options)
            : base(options)
        {
        }

        public DbSet<SparePartsStore.Models.SparePart> SparePart { get; set; } = default!;

        public DbSet<SparePartsStore.Models.Category> Category { get; set; }
    }
}
