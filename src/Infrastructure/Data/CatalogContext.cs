using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Entities.ShowcaseAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<ShowcaseBlock> ShowcaseBlocks { get; set; }
        public DbSet<ShowcaseTemplate> Showcases { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Dimensions> Dimensions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ImageObj> Images { get; set; }
    }
}
