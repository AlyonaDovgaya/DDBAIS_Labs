using System.Data.Entity;
using WebApplication.Models;

namespace WebApplication.Data
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext() 
            : base("name=SqlConnection") 
        { }

        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
    }
}
