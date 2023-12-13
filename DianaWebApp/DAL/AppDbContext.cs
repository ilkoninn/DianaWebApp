
namespace DianaWebApp.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){ }

        // Additional Models Section
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }  
        public DbSet<Material> Materials { get; set; }
        public DbSet<Category> Categories { get; set; }

        // Product Model Section
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductMaterial> ProductMaterials { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        

    }
}
